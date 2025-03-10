import os

import numpy as np
import onnx
import tensorflow as tf
import pandas as pd
from sklearn.preprocessing import OneHotEncoder, Normalizer
from sklearn.metrics import confusion_matrix, ConfusionMatrixDisplay
from matplotlib import pyplot as plt
import tf2onnx

def encode_categorical_values(df, columns):
	encoder = OneHotEncoder(sparse_output=False)
	one_hot_encoded = encoder.fit_transform(df[columns])
	one_hot_df = pd.DataFrame(one_hot_encoded, columns=encoder.get_feature_names_out(columns), dtype=np.uint8)
	return pd.concat([df.drop(columns,axis=1), one_hot_df], axis=1)

def normalize_numerical_values(df, columns):
	normalizer = Normalizer()
	normalized = normalizer.transform(df[columns])
	normalized_df = pd.DataFrame(normalized, columns=columns, dtype=np.float64)
	return pd.concat([df.drop(columns,axis=1), normalized_df], axis=1)

def main():
	# Download dataset
	dataset_url = 'http://storage.googleapis.com/download.tensorflow.org/data/petfinder-mini.zip'
	dataser_folder = tf.keras.utils.get_file('petfinder_mini.zip', dataset_url, extract=True, cache_dir='./data')
	csv_file = os.path.join(dataser_folder, 'petfinder-mini/petfinder-mini.csv')
	df = pd.read_csv(csv_file)

	# Preprocess dataset
	## Drop unnecessary description column
	df.drop(columns=['Description'], inplace=True)

	## Encode categorical values
	df = encode_categorical_values(df, ['Breed1', 'Color1', 'Color2', 'FurLength', 'Gender', 'Health',
											   'MaturitySize', 'Sterilized', 'Type', 'Vaccinated'])

	## Normalize numerical values
	df = normalize_numerical_values(df, ['Age', 'Fee', 'PhotoAmt'])

	## Convert to tensors
	labels = df.pop('AdoptionSpeed')
	_,stat = np.unique(labels, return_counts=True)
	print(stat)
	ds = tf.data.Dataset.from_tensor_slices((df, labels)).shuffle(buffer_size=1000)

	# Split dataset to 80%-10%-10%
	number_of_samples = len(df)
	number_of_train_samples = int(number_of_samples * 0.8)
	number_of_val_samples = int(number_of_samples * 0.1) + 1
	number_of_test_samples = number_of_samples - number_of_train_samples - number_of_val_samples

	print(number_of_train_samples, 'training examples')
	print(number_of_val_samples, 'validation examples')
	print(number_of_test_samples, 'test examples')

	number_of_inputs = len(df.columns)
	number_of_outputs = labels.max() + 1
	batch_size = 256
	steps_per_epochs = number_of_train_samples // batch_size
	print(number_of_inputs, 'input features')
	print(number_of_outputs, 'output features')

	ds_train = ds.take(number_of_train_samples).batch(batch_size)
	ds_val = ds.skip(number_of_train_samples).take(number_of_val_samples).batch(batch_size)
	ds_test = ds.skip(number_of_train_samples + number_of_val_samples)

	# Create model
	model = tf.keras.models.Sequential([
		tf.keras.layers.Input(shape=(number_of_inputs,)),

		tf.keras.layers.Dense(256, activation='relu'),
		tf.keras.layers.BatchNormalization(),
		tf.keras.layers.Dropout(0.3),

		tf.keras.layers.Dense(128, activation='relu'),
		tf.keras.layers.BatchNormalization(),
		tf.keras.layers.Dropout(0.3),

		tf.keras.layers.Dense(64, activation='relu'),
		tf.keras.layers.BatchNormalization(),
		tf.keras.layers.Dropout(0.3),

		tf.keras.layers.Dense(32, activation='relu'),
		tf.keras.layers.BatchNormalization(),
		tf.keras.layers.Dropout(0.3),

		tf.keras.layers.Dense(16, activation='relu'),
		tf.keras.layers.BatchNormalization(),
		tf.keras.layers.Dropout(0.3),

		tf.keras.layers.Dense(number_of_outputs),
	])

	model.compile(
		optimizer='adam',
		loss=tf.keras.losses.SparseCategoricalCrossentropy(),
		metrics=[tf.keras.metrics.SparseCategoricalAccuracy()]
	)

	model.summary()
	tf.keras.utils.plot_model(model, to_file='data/model.png', show_shapes=True, dpi=64)

	# Train model
	model_path = 'data/model.keras'
	epochs = 1000
	early_stop_callback = tf.keras.callbacks.EarlyStopping(monitor='val_loss', mode="min", patience=15)
	checkpoint_callback = tf.keras.callbacks.ModelCheckpoint(model_path, monitor="val_loss", mode="min",
															 save_best_only=True,
															 verbose=1)
	train_history = model.fit(ds_train,
							  epochs=epochs,
							  callbacks=[checkpoint_callback],# early_stop_callback],
							  validation_data=ds_val,
							  )

	# Plot training procedure
	fig = plt.figure()
	plt.plot(train_history.epoch, train_history.history['loss'], 'r', label='Training loss')
	plt.plot(train_history.epoch, train_history.history['val_loss'], 'b', label='Validation loss')
	plt.title('Training and Validation Loss')
	plt.xlabel('Epoch')
	plt.ylabel('Loss Value')
	plt.legend()
	plt.show()
	fig.savefig('data/train.png')

	# Load the best model back
	model = tf.keras.models.load_model(model_path)

	# Evaluate model
	results = model.evaluate(ds_test.batch(batch_size))
	print(f'Accuracy: {results[1]*100:.2f}%')

	# Make predictions
	features = np.array([feature.numpy() for feature, _ in ds_test])
	true_labels = np.array([label.numpy() for _, label in ds_test])
	pred_dist = model.predict(features)

	# Evaluate predictions
	# th = 0.5
	# pred_labels = (pred_dist > th).reshape(-1)
	pred_labels = np.argmax(pred_dist, axis=1)
	cm = confusion_matrix(true_labels, pred_labels)

	disp = ConfusionMatrixDisplay(confusion_matrix=cm)
	disp.plot(cmap='Blues')
	plt.title(f'Evaluation (Accuracy: {results[1]*100:.2f}%)')
	plt.show()
	fig.savefig('data/results.png')

	# Export model to ONNX
	input = model.inputs[0]
	model.output_names = ['output']
	input_signature = [tf.TensorSpec(input.shape, tf.float32, name='input')]
	onnx_model, _ = tf2onnx.convert.from_keras(model, input_signature)
	onnx.save(onnx_model, 'data/model.onnx')

if __name__ == '__main__':
	main()
