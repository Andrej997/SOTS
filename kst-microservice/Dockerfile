# Dockerfile
FROM python:3.7
COPY requirements.txt /kstapp/requirements.txt
WORKDIR /kstapp
RUN pip install -r requirements.txt
COPY . /kstapp
ENTRYPOINT ["python"]
CMD ["run.py"]