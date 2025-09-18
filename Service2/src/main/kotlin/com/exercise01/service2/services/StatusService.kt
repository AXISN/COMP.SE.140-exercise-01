package com.exercise01.service2.services

import com.exercise01.service2.services.contracts.IStatusService
import org.springframework.stereotype.Service
import java.time.Duration
import java.time.Instant
import java.time.LocalDateTime
import java.time.ZoneOffset
import java.time.format.DateTimeFormatter

@Service
class StatusService : IStatusService {
    private val _startTime: Instant = LocalDateTime.now().toInstant(ZoneOffset.UTC)

    override fun getStatus(): String {
        val now = LocalDateTime.now().toInstant(ZoneOffset.UTC)
        val uptimeHour = Duration.between(_startTime, now).toMillis() / 1000.0 / 3600.0
        val freeSpace = java.io.File("/").usableSpace / 1024 / 1024

        return String.format(
            "%s: uptime %.2f hours, free disk in root: %,d MBytes",
            DateTimeFormatter.ISO_INSTANT.format(now).replace(Regex("\\.\\d+Z$"), "Z"),
            uptimeHour,
            freeSpace
        )
    }
}