using Service1.Services.Contracts;

namespace Service1.Services;

public class Service2Service : IService2Service
{
    private readonly HttpClient _client;
    private readonly ILogger<Service2Service> _logger;
    
    public Service2Service(ILogger<Service2Service> logger)
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("http://service-2:5000");
        _client.DefaultRequestHeaders.Accept.Clear();
        
        _logger = logger;
    }

    public async Task<string> GetStatus()
    {
        try
        {
            var response = await _client.GetAsync("status");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting status from service 2.");
            return null;
        }
    }
}