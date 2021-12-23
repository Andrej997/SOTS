# application/kst_api/routes.py
from flask import jsonify, request, make_response
from . import kst_api_blueprint
import pandas as pd
from learning_spaces.kst import iita

#{'a': [1, 0, 1], 'b': [0, 1, 0], 'c': [0, 1, 1]}
@kst_api_blueprint.route('/api/calculate', methods=['POST'])
def calculate():
    matrix = request.json
    data_frame = pd.DataFrame(matrix)
    try:
        response = iita(data_frame, v=1)
    except Exception:
        return 'error'
    
    response['diff'] = response['diff'].tolist()
    return jsonify(response)
