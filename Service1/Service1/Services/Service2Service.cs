using Service1.Services.Contracts;

namespace Service1.Services;

public class Service2Service : IService2Service
{
    private readonly HttpClient _client;
    private readonly ILogger<Service2Service> _logger;
    
    public Service2Service(HttpClient client, ILogger<Service2Service> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<string> GetStatus()
    {
        try
        {
            _client.BaseAddress = new Uri("http://localhost:5000");
            _client.DefaultRequestHeaders.Accept.Clear();

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