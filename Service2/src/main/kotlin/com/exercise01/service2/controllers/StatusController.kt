package com.exercise01.service2.controllers

import com.exercise01.service2.services.contracts.IStatusService
import org.springframework.http.MediaType
import org.springframework.web.bind.annotation.GetMapping
import org.springframework.web.bind.annotation.RestController

@RestController
class StatusController(
    val statusService: IStatusService
) {
    @GetMapping(path = ["/status"], produces = [MediaType.TEXT_PLAIN_VALUE])
    fun getStatus(): String {
        return statusService.getStatus()
    }
}