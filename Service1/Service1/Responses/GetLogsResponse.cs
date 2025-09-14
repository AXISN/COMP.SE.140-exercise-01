using Service1.Models;

namespace Service1.Responses;

public class GetLogsResponse
{
    public IEnumerable<LogEntry> Logs { get; set; }
}