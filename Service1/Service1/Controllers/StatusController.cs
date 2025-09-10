using Microsoft.AspNetCore.Mvc;
using Service1.Services.Contracts;

namespace Service1.Controllers;

[ApiController]
public class StatusController : ControllerBase
{
    private readonly IStatusService _statusService;
    
    public StatusController(IStatusService statusService)
    {
        _statusService = statusService;
    }

    [HttpGet]
    [Route("status")]
    public IActionResult GetStatus()
    {
        var status = _statusService.GetStatus();
        return Ok(status);
    }
}   