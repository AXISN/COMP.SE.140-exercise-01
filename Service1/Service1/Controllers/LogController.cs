using Microsoft.AspNetCore.Mvc;
using Service1.Services.Contracts;

namespace Service1.Controllers;

public class LogController : ControllerBase
{
    private readonly IStorageService _storageService;
    private readonly ILogger<LogController> _logger;

    public LogController(IStorageService storageService, ILogger<LogController> logger)
    {
        _storageService = storageService;
        _logger = logger;
    }
    
    [HttpGet]
    [Route("log")]
    public async Task<IActionResult> GetLog()
    {
        try
        {
            var response = await _storageService.GetLog();
            
            if (response is null)
                return NotFound("No logs found");
            
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting log");
            return StatusCode(500);
        }
    }
}