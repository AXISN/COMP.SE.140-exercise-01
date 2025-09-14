package com.exercise01.service2.controllers

import com.exercise01.service2.services.contracts.IStatusService
import com.exercise01.service2.services.contracts.IStorageService
import org.springframework.http.MediaType
import org.springframework.web.bind.annotation.GetMapping
import org.springframework.web.bind.annotation.RestController

@RestController
class StatusController(
    val statusService: IStatusService,
    val storageService: IStorageService
) {
    @GetMapping(path = ["/status"], produces = [MediaType.TEXT_PLAIN_VALUE])
    fun getStatus(): String {
        val status = statusService.getStatus()
        storageService.addLog(status)
        return status
    }
}