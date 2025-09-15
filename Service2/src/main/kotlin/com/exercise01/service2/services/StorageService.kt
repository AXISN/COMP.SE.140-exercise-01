package com.exercise01.service2.services

import com.exercise01.service2.models.LogEntry
import com.exercise01.service2.services.contracts.IStorageService
import com.fasterxml.jackson.module.kotlin.jacksonObjectMapper
import okhttp3.MediaType.Companion.toMediaType
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.RequestBody.Companion.toRequestBody
import org.springframework.stereotype.Service
import java.io.File

private const val BASE_URL: String = "http://storage:1234/"
private const val V_STORAGE_LOG_PATH = "/app/data/log.txt"

@Service
class StorageService : IStorageService {
    private val client: OkHttpClient = OkHttpClient()
    private val objectMapper = jacksonObjectMapper()

    override fun addLog(message: String) {
        val logEntry = LogEntry(
            message = message,
            service = "Service2"
        )

        val jsonBody = objectMapper.writeValueAsString(logEntry)

        val request: Request = Request.Builder()
            .url(BASE_URL + "logs")
            .post(jsonBody.toRequestBody("application/json".toMediaType()))
            .build()

        try {
            client.newCall(request).execute()
            appendLogToVStorage(message)
        } catch (e: Exception) {
            println("Error adding log: ${e.message}")
        }
    }

    private fun appendLogToVStorage(message: String) {
        try {
            val logFile = File(V_STORAGE_LOG_PATH)

            logFile.parentFile?.mkdirs()

            if (!logFile.exists()) {
                logFile.createNewFile()
            }

            logFile.appendText(message + System.lineSeparator())
        } catch (e: Exception) {
            println("Failed to write log to volume storage: ${e.message}")
        }
    }
}