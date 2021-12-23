# application/kst_api/__init__.py
from flask import Blueprint

kst_api_blueprint = Blueprint('kst_api', __name__)

from . import routes