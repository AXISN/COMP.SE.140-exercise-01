namespace Service1.Services.Contracts;

public interface IStatusService
{
    Task<string?> GetStatus();
}