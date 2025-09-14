namespace Service1.Models;

public class LogEntry
{
    public int Id { get; set; }
    
    public string Message { get; set; }
    
    public string Service { get; set; }
    
    public DateTime CreatedAt { get; set; }
}