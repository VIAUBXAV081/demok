import tensorflow as tf
import keras

@keras.saving.register_keras_serializable()
def f2(y_true, y_pred):
    mse = keras.metrics.mean_squared_error(y_true, y_pred)

    c1 = tf.constant(1, dtype=mse.dtype)
    c50 = tf.constant(50, dtype=mse.dtype)
    c100 = tf.constant(100, dtype=mse.dtype)

    return c50 * log10(c100 / tf.sqrt(c1 + mse))

@keras.saving.register_keras_serializable()
def log10(x):
    numerator = tf.math.log(x)
    denominator = tf.math.log(tf.constant(10, dtype=numerator.dtype))
    return numerator / denominator
