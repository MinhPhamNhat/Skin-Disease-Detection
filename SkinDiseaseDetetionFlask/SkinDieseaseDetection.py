import base64
import os
import warnings
from io import BytesIO

import numpy as np
import tensorflow as tf
from PIL import Image
from tensorflow.keras import Model
from tensorflow.keras.applications import vgg16, resnet50, efficientnet
from tensorflow.keras.layers import Dense, Input, Dropout, average

warnings.filterwarnings('ignore')
os.environ['TF_CPP_MIN_LOG_LEVEL'] = '3'
tf.get_logger().setLevel('ERROR')


# Save model weights and create folder if folder is not existed
def load_model_weights(model):
    folder_path = "./model_weights"

    model_weights_path = f'{folder_path}/{model.name}.weights.h5'
    model.load_weights(model_weights_path)


def base64_to_rgb_image(base64_string):
    image_data = base64.b64decode(base64_string)

    image = Image.open(BytesIO(image_data))

    if image.mode != 'RGB':
        image = image.convert('RGB')

    resized_image = image.resize(IMAGE_SIZE[::-1], Image.LANCZOS)

    return np.array(resized_image)


# Function build a VGG16 transfer learning model
def build_vgg16_tl(name='vgg16_tl'):
    input_layer = Input(shape=INPUT_SHAPE)

    vgg16_model = vgg16.VGG16(weights='imagenet', include_top=False, pooling='avg', input_shape=INPUT_SHAPE)
    vgg16_model.trainable = False

    vgg16_tl = vgg16.preprocess_input(input_layer)
    vgg16_tl = vgg16_model(vgg16_tl)
    vgg16_tl = Dense(4096, activation='relu')(vgg16_tl)
    vgg16_tl = Dense(1072, activation='relu')(vgg16_tl)
    vgg16_tl = Dropout(0.2)(vgg16_tl)
    vgg16_tl = Dense(len(CLASSES), activation='softmax')(vgg16_tl)

    vgg16_tl_model = Model(inputs=input_layer, outputs=vgg16_tl, name=name)

    return vgg16_tl_model


# Function build a ResNet50 transfer learning model
def build_resnet50_tl(name='resnet50_tl'):
    input_layer = Input(shape=INPUT_SHAPE)

    resnet50_model = resnet50.ResNet50(weights='imagenet', include_top=False, pooling='avg', input_shape=INPUT_SHAPE)
    resnet50_model.trainable = False

    resnet50_tl = resnet50.preprocess_input(input_layer)
    resnet50_tl = resnet50_model(resnet50_tl)
    resnet50_tl = Dense(4096, activation='relu')(resnet50_tl)
    resnet50_tl = Dense(1072, activation='relu')(resnet50_tl)
    resnet50_tl = Dropout(0.2)(resnet50_tl)
    resnet50_tl = Dense(len(CLASSES), activation='softmax')(resnet50_tl)

    resnet50_tl_model = Model(inputs=input_layer, outputs=resnet50_tl, name=name)

    return resnet50_tl_model


def build_efficientnetb0_tl(name='efficientnetb0_tl'):
    input_layer = Input(shape=INPUT_SHAPE)

    efficientnetb0_model = efficientnet.EfficientNetB0(weights='imagenet', include_top=False, pooling='avg',
                                                       input_shape=INPUT_SHAPE)
    efficientnetb0_model.trainable = False

    efficientnetb0_tl = efficientnet.preprocess_input(input_layer)
    efficientnetb0_tl = efficientnetb0_model(efficientnetb0_tl)
    efficientnetb0_tl = Dense(4096, activation='relu')(efficientnetb0_tl)
    efficientnetb0_tl = Dense(1072, activation='relu')(efficientnetb0_tl)
    efficientnetb0_tl = Dropout(0.2)(efficientnetb0_tl)
    efficientnetb0_tl = Dense(len(CLASSES), activation='softmax')(efficientnetb0_tl)

    efficientnetb0_tl_model = Model(inputs=input_layer, outputs=efficientnetb0_tl, name=name)

    return efficientnetb0_tl_model


def build_stacking_model():
    input_layer = Input(shape=INPUT_SHAPE)

    vgg16_tl_model = build_vgg16_tl()
    vgg16_tl_model.compile(optimizer='adam', loss='categorical_crossentropy',
                           metrics=['accuracy', 'precision', 'recall'])
    vgg16_tl_model_x = vgg16_tl_model(input_layer)

    resnet50_tl_model = build_resnet50_tl()
    resnet50_tl_model.compile(optimizer='adam', loss='categorical_crossentropy',
                              metrics=['accuracy', 'precision', 'recall'])
    resnet50_tl_model_x = resnet50_tl_model(input_layer)

    efficientnetb0_tl_model = build_efficientnetb0_tl()
    efficientnetb0_tl_model.compile(optimizer='adam', loss='categorical_crossentropy',
                                    metrics=['accuracy', 'precision', 'recall'])
    efficientnetb0_tl_model_x = efficientnetb0_tl_model(input_layer)

    vgg16_resnet50_efficientnetb0_stacking = average([vgg16_tl_model_x, resnet50_tl_model_x, efficientnetb0_tl_model_x])
    vgg16_resnet50_efficientnetb0_stacking = Dense(4096, activation='relu')(vgg16_resnet50_efficientnetb0_stacking)
    vgg16_resnet50_efficientnetb0_stacking = Dense(1072, activation='relu')(vgg16_resnet50_efficientnetb0_stacking)
    vgg16_resnet50_efficientnetb0_stacking = Dropout(0.2)(vgg16_resnet50_efficientnetb0_stacking)
    vgg16_resnet50_efficientnetb0_stacking = Dense(len(CLASSES), activation='softmax')(
        vgg16_resnet50_efficientnetb0_stacking)

    vgg16_resnet50_efficientnetb0_stacking_model = Model(inputs=input_layer,
                                                         outputs=vgg16_resnet50_efficientnetb0_stacking,
                                                         name='vgg16_resnet50_efficientnetb0_stacking')

    return vgg16_resnet50_efficientnetb0_stacking_model


BATCH_SIZE = 64
IMAGE_SIZE = (150, 200)  # height, width
TRAIN_PATH = "/kaggle/input/basedir/base_dir/train_dir"
TEST_PATH = "/kaggle/input/basedir/base_dir/val_dir"
INPUT_SHAPE = (150, 200, 3)
CLASSES = [
    'akiec',
    'bcc',
    'bkl',
    'df',
    'mel',
    'nv',
    'vasc',
]


def save_model_weights(model):
    folder_path = "./model_weights"

    if not os.path.exists(folder_path):
        os.makedirs(folder_path)

    model_weights_path = f'{folder_path}/{model.name}.weights.h5'
    model.save_weights(model_weights_path, overwrite=True)
    print(f'Saved model to: {model_weights_path}')


def predict_skin_disease(image_base64, model_name):
    model = None
    if model_name == 'vgg16':
        model = build_vgg16_tl()

    if model_name == 'resnet50':
        model = build_resnet50_tl()

    if model_name == 'efficientnetb0':
        model = build_efficientnetb0_tl()

    if model_name == 'vgg16_resnet50_efficientnetb0':
        model = build_stacking_model()

    load_model_weights(model)

    scaled_img = base64_to_rgb_image(image_base64)
    prediction = model.predict(np.array([scaled_img]))

    return (prediction[0], CLASSES, CLASSES[np.argmax(prediction)])
