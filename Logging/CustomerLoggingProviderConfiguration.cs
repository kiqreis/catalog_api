namespace WebApplication1.Logging;

public class CustomerLoggingProviderConfiguration
{
    public LogLevel LogLevel { get; set; } = LogLevel.Warning;
    public int EventId { get; set; } = 0;
}