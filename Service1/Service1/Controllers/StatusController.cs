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
    public async Task<IActionResult> GetStatus()
    {
        var status = await _statusService.GetStatus();
        return Ok(status);
    }
}   