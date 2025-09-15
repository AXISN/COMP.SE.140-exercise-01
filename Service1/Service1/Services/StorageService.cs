using Service1.Models;
using Service1.Responses;
using Service1.Services.Contracts;

namespace Service1.Services;

public class StorageService : IStorageService
{
    private const string VStorageLogPath = "/app/data/log.txt";
    
    private readonly HttpClient _client;
    private readonly ILogger<StorageService> _logger;
    
    public StorageService(ILogger<StorageService> logger)
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("http://storage:1234/");
        _client.DefaultRequestHeaders.Accept.Clear();

        _logger = logger;
    }
    
    public async Task<string?> GetLog()
    {
        try
        {
            var response = await _client.GetAsync("logs");
            response.EnsureSuccessStatusCode();
            
            var logEntries = await response.Content.ReadFromJsonAsync<GetLogsResponse>();

            return logEntries is null ? null : string.Join(Environment.NewLine, logEntries.Logs.Select(l => l.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting log");
            return null;
        }
    }

    public async Task AddLog(string message)
    {
        try
        {
            var logEntry = new LogEntry
            {
                Message = message,
                Service = "Service1"
            };

            var response = await _client.PostAsJsonAsync("logs", logEntry);
            response.EnsureSuccessStatusCode();
            
            AppendLogToVStorage(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding log");
        }
    }

    private void AppendLogToVStorage(string message)
    {
        try
        {
            var directory = Path.GetDirectoryName(VStorageLogPath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory!);
            
            if (!File.Exists(VStorageLogPath))
                File.Create(VStorageLogPath).Close();
            
            File.AppendAllText(VStorageLogPath, message + Environment.NewLine);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to write log to volume storage");
        }
    }
}