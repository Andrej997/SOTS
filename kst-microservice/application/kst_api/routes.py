# application/kst_api/routes.py
from flask import jsonify, request, make_response
from . import kst_api_blueprint


@kst_api_blueprint.route('/api/message', methods=['GET'])
def hello():
    return 'Hello'
