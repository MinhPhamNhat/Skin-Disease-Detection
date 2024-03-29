from flask import Flask, jsonify, request
from flask_cors import CORS

import SkinDieseaseDetection

app = Flask(__name__)
cors = CORS(app)
app.config['CORS_HEADERS'] = 'Content-Type'

@app.route('/predict/<model>', methods=['POST'])
def get_employee_by_id(model):
    image = request.form["Image"]
    predictions, labels, pred = SkinDieseaseDetection.predict_skin_disease(image, model)
    result = dict()
    result['predictions'] = {labels[i]: float(predictions[i]) for i in range(len(labels))}
    result['result'] = pred
    
    return jsonify(result)


if __name__ == '__main__':
    app.run(port=5000)