package com.exercise01.service2.services

import com.exercise01.service2.models.LogEntry
import com.exercise01.service2.services.contracts.IStorageService
import com.fasterxml.jackson.module.kotlin.jacksonObjectMapper
import okhttp3.MediaType.Companion.toMediaType
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.RequestBody.Companion.toRequestBody
import org.springframework.stereotype.Service

private const val BASE_URL: String = "http://storage:1234/"

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

        client.newCall(request).execute()
    }
}