import json

import numpy as np
import onnx
import tensorflow as tf
import pandas as pd
from sklearn.metrics import confusion_matrix, ConfusionMatrixDisplay, accuracy_score
from matplotlib import pyplot as plt
import tf2onnx

def main():
	# Download dataset
	dataset_url = 'https://raw.githubusercontent.com/mwaskom/seaborn-data/refs/heads/master/penguins.csv'
	csv_file = tf.keras.utils.get_file('penguins.csv', dataset_url, cache_dir='./data')
	df = pd.read_csv(csv_file)

	# Remove missing data
	df.dropna(inplace=True, axis=0)

	# Remove unnecessary columns
	df.drop(columns=['island', 'sex'], inplace=True)

	# Encode categorical columns
	categorical_columns = ['species']
	replacement_values = {
		column: {
			value: index for index, value in enumerate(df[column].unique())
		}
		for column in categorical_columns
	}
	df.replace(replacement_values, inplace=True)

	# Extract labels
	target_column = 'species'
	labels = df.pop(target_column)

	# Persist labels
	with open('data/datasets/labels.json', 'w') as f:
		json.dump(replacement_values, f, indent=4)

	# Convert to tensors
	ds = tf.data.Dataset.from_tensor_slices((df, labels))
	ds = ds.shuffle(500, seed=42)

	# Split dataset to 80%-10%-10%
	number_of_samples = len(df)
	number_of_train_samples = int(number_of_samples * 0.8)
	number_of_val_samples = int(number_of_samples * 0.1)+1
	number_of_test_samples = number_of_samples - number_of_train_samples - number_of_val_samples
	batch_size = 16

	print('Training examples:', number_of_train_samples)
	print('Validation examples:', number_of_val_samples)
	print('Test examples:', number_of_test_samples)
	print('Batch size:', batch_size)

	ds_train = ds.take(number_of_train_samples).batch(batch_size)
	ds_val = ds.skip(number_of_train_samples).take(number_of_val_samples).batch(batch_size)
	ds_test = ds.skip(number_of_train_samples + number_of_val_samples).batch(batch_size)

	# Define number of inputs and outputs
	number_of_inputs = len(df.columns)
	number_of_outputs = int(labels.max() + 1)
	print('Input features:',number_of_inputs)
	print('Output features:', number_of_outputs)

	# Create preprocessor layer
	train_features = np.array([feature.numpy() for feature, _ in ds_train.unbatch()])
	normalization_layer = tf.keras.layers.Normalization()
	normalization_layer.adapt(train_features)

	# Create model
	model = tf.keras.models.Sequential([
		tf.keras.layers.Input(shape=(number_of_inputs,)),
		normalization_layer,
		tf.keras.layers.Dense(8, activation='relu'),
		tf.keras.layers.Dropout(0.3),
		tf.keras.layers.Dense(number_of_outputs, activation='softmax'),
	])

	model.compile(
		optimizer=tf.keras.optimizers.SGD(momentum=0.9),
		loss=tf.keras.losses.SparseCategoricalCrossentropy(),
		metrics=[tf.keras.metrics.SparseCategoricalAccuracy()]
	)

	model.summary()
	tf.keras.utils.plot_model(model, to_file='data/model.png', show_shapes=True, dpi=64)

	# Train model
	model_path = 'data/model.keras'
	epochs = 150
	early_stopping = tf.keras.callbacks.EarlyStopping(monitor='val_loss', mode="min", patience=10)
	checkpoint_callback = tf.keras.callbacks.ModelCheckpoint(model_path, monitor="val_loss", mode="min",
															 save_best_only=True,
															 verbose=1)
	train_history = model.fit(ds_train,
							  epochs=epochs,
							  callbacks=[checkpoint_callback, early_stopping],
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

	# Evaluate
	results = model.evaluate(ds_test, verbose=1)
	print(f'Accuracy: {results[1]*100:.2f}%')

	# Make predictions
	features, true_labels = zip(*[(feature.numpy(), label.numpy()) for feature, label in ds_test.unbatch()])
	features = np.array(features)
	true_labels = np.array(true_labels)
	pred_dist = model.predict(features)

	# Evaluate predictions
	pred_labels = np.argmax(pred_dist, axis=1)
	cm = confusion_matrix(true_labels, pred_labels)
	acc = accuracy_score(true_labels, pred_labels)

	label_names = replacement_values[target_column].keys()
	disp = ConfusionMatrixDisplay(confusion_matrix=cm, display_labels=label_names)
	disp.plot(cmap='Blues')
	plt.title(f'Evaluation (Accuracy: {acc*100:.2f}%)')
	fig = plt.gcf()
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
