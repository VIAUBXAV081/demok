import matplotlib.pyplot as plt
import tensorflow as tf
import onnx
import tf2onnx

from helpers.metrics import f2
from helpers.datasets import load_dataset, load_dataset_objects

# Data parameters
data_folder = 'data'

train_input_path = f'{data_folder}/train/raman/*.rmf'
train_output_path = f'{data_folder}/train/curve/*.crv'

test_input_path = f'{data_folder}/test/raman/*.rmf'
test_output_path = f'{data_folder}/test/curve/*.crv'

# Load dataset
(train_input, train_output) = load_dataset(train_input_path, train_output_path)
for i in range(3):
    plt.figure()
    plt.subplot(1, 2, 1)
    plt.imshow(train_input[i])
    plt.title('Raman map')
    plt.subplot(1, 2, 2)
    plt.plot(train_output[i])
    plt.grid(True)
    plt.title(f'Dissolution curve')
    plt.show()

# Model parameters
model_input_name = 'raman_map'
model_input_shape = (31, 31, 1)
model_output_name = 'dissolution_curve'
model_output_shape = 37

train_epochs = 100
validation_split = 0.2
learning_rate = 0.1

model_save_path = f'{data_folder}/models/example_cnn_model'
example_prediction_name = 'DR_V01A'

# Create models
model = tf.keras.Sequential([
    tf.keras.layers.Conv2D(16, (3, 3), activation='relu', input_shape=model_input_shape, name=model_input_name),
    tf.keras.layers.MaxPooling2D(),
    tf.keras.layers.Conv2D(32, (3, 3), activation='relu'),
    tf.keras.layers.MaxPooling2D(),
    tf.keras.layers.Flatten(),
    tf.keras.layers.Dense(64, activation='relu'),
    tf.keras.layers.Dense(model_output_shape, name=model_output_name),
])

model.compile(optimizer=tf.keras.optimizers.Adam(learning_rate=learning_rate),
              loss='mean_absolute_error',
              metrics=[f2]
              )

# Train models
history = model.fit(train_input, train_output, epochs=train_epochs, validation_split=validation_split)

# Plot training procedure
plt.figure()
plt.plot(history.history['f2'], label='f2')
plt.plot(history.history['val_f2'], label='val_f2')
plt.xlabel('Epoch')
plt.ylabel('f2')
plt.legend()
plt.grid(True)
plt.title('Training of model')
plt.show()

# Evaluate models
(test_input, test_output) = load_dataset(test_input_path, test_output_path)

(evaluation_f2, _) = model.evaluate(test_input, test_output)
print(f'Evaluation f2 result: {evaluation_f2}')

# Create example prediction
test_dataset_object = load_dataset_objects(test_input_path, test_output_path)
example_input = test_dataset_object.get(example_prediction_name).get('Map').components.get('HPMC')
example_output = test_dataset_object.get(example_prediction_name).get('Curve')
prediction = model.predict(example_input.reshape((1,) + model_input_shape), verbose=0).reshape(model_output_shape)

# Calculate example prediction f2 value
prediction_f2 = f2(example_output.values, prediction)
print(f'{example_prediction_name} prediction f2 result: {prediction_f2}')

# Plot prediction to visual inspection
plt.figure()
plt.plot(example_output.time, example_output.values, label='label')
plt.plot(example_output.time, prediction, label='prediction')
plt.xlabel('Time')
plt.ylabel('Dissolved API')
plt.legend()
plt.title(f'Example prediction of {example_prediction_name}')
plt.grid(True)
plt.show()

# Save models to keras
model.save(f'{model_save_path}.keras')

# Convert models to onnx and save
model.output_names = [model_output_name]
input_signature = [tf.TensorSpec((1,) + model_input_shape, tf.float32, name=model_input_name)]
onnx_model, _ = tf2onnx.convert.from_keras(model, input_signature)
onnx.save(onnx_model, f'{model_save_path}.onnx')

# Validate models with https://netron.app/
