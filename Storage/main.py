from pydantic import BaseModel
from fastapi import FastAPI, HTTPException
import mysql.connector

class LogEntry(BaseModel):
  message: str
  service: str

app = FastAPI()

mydb = mysql.connector.connect(
  host="storage-db",
  user="StorageAPI",
  password="SuperSecurePassw0rd",
  database="StorageDB",
  consume_results=True
)

@app.get("/logs")
def get_logs():
  try:
    cursor = mydb.cursor()
    cursor.execute("SELECT message FROM Logs")
    logs = cursor.fetchall()
    print(mydb.is_connected)
    return {"logs": logs}
  except mysql.connector.Error as err:
    raise HTTPException(status_code=500, detail=f"Database error: {err}")
  except Exception as e:
    raise HTTPException(status_code=500, detail=f"Unexpected error: {e}")

@app.post("/logs", status_code=201)
def append_log(entry: LogEntry):
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