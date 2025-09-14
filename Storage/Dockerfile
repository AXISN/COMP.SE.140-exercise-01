FROM python:3.9
WORKDIR /app

COPY . /app

RUN pip install fastapi[standard] mysql-connector-python

EXPOSE 1234

CMD ["fastapi", "run", "main.py", "--port", "1234"]