from pydantic import BaseModel
from fastapi import FastAPI, HTTPException
import mysql.connector
import time
import sys

class LogEntry(BaseModel):
  id: int
  message: str
  service: str
  created_at: str

class AddLog(BaseModel):
  message: str
  service: str

app = FastAPI()

def connect_to_database(max_retries=5):
    for attempt in range(max_retries):
        try:
            connection = mysql.connector.connect(
                host="storage-db",
                user="StorageAPI",
                password="SuperSecurePassw0rd",
                database="StorageDB",
                consume_results=True
            )

            return connection
        except mysql.connector.Error as err:
            print(f"Database connection failed: {err}")
            if attempt < max_retries - 1:
                time.sleep(5)
            else:
                sys.exit(1)
    return None

# Connect to database with retry logic
mydb = connect_to_database()

@app.get("/log")
def get_logs():
  try:
    cursor = mydb.cursor()
    cursor.execute("SELECT id, message, service, created_at FROM Logs")
    logs = []

    for (id, message, service, created_at) in cursor:
      logs.append(LogEntry(id=id, message=message, service=service, created_at=str(created_at)))

    print(mydb.is_connected)
    return {"logs": logs}
  except mysql.connector.Error as err:
    raise HTTPException(status_code=500, detail=f"Database error: {err}")
  except Exception as e:
    raise HTTPException(status_code=500, detail=f"Unexpected error: {e}")

@app.post("/log", status_code=201)
def append_log(entry: AddLog):
  try:
    if entry.service != "Service1" and entry.service != "Service2":
      raise HTTPException(status_code=400, detail="Invalid service")
    cursor = mydb.cursor()
    sql = "INSERT INTO Logs (message, service) VALUES (%s, %s)"
    cursor.execute(sql, (entry.message, entry.service))
    mydb.commit()
    return {"message": "Log added successfully"}
  except mysql.connector.Error as err:
    raise HTTPException(status_code=500, detail=f"Database error: {err}")
  except Exception as e:
    raise HTTPException(status_code=500, detail=f"Unexpected error: {e}")