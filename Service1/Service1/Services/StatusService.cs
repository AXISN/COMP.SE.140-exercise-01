using System.Text;
using Service1.Services.Contracts;

namespace Service1.Services;

public class StatusService : IStatusService
{
    private readonly DateTime _startTime;
    
    private readonly IService2Service _service2Service;
    private readonly ILogger<StatusService> _logger;
    
    public StatusService(IService2Service service2Service,  ILogger<StatusService> logger)
    {
        _startTime = DateTime.Now;
        _service2Service = service2Service;
        _logger = logger;
    }
    
    public async Task<string> GetStatus()
    {
        try
        {
            var builder = new StringBuilder();
            builder.AppendLine(GetInternalStatus());
            var service2Status = await _service2Service.GetStatus();
            builder.Append(service2Status);

            return builder.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting statuses");
            return null;
        }
    }

    private string GetInternalStatus()
    {
        var now = DateTime.Now.ToUniversalTime();
        var hourUptime = (now - _startTime).Hours;
        var freeSpace = DriveInfo.GetDrives().FirstOrDefault()?.AvailableFreeSpace / 1024 / 1024;
        
        return $"{now:O}: uptime {hourUptime} hours, free disk in root: {freeSpace:N0} MBytes";
    }
}