# Train and save example TensorFlow model

This example script trains a basic CNN model and saves to `keras` and `onnx` file format

## How to run

- Put your data to `data` folder
    ```bash
    data
    ├───models
    ├───test
    │   ├───curve
    │   │   └─── *.crv
    │   └───raman
    │       └─── *.rmf
    └───train
        ├───curve
        │   └─── *.crv
        └───raman
            └─── *.rmf
    ```
- Create virtual environment
  ```bash
  python -m venv <virtual-environment-name>
  .\<virtual-environment-name>\Scripts\activate
  ```

- Install requirements
  ```bash
  pip install -r requirements.txt
  ```
  
- Run script
  ```bash
  python main.py
  ```
  
