namespace Service1.Services.Contracts;

public interface IStorageService
{
    public Task<string?> GetLog();
    
    public Task AddLog(string message);
}