namespace WebApplication1.RateLimit;

public sealed class RateLimitOptions
{
  public const string RateLimit = "RateLimit";
  public int Window { get; set; } = 10;
  public int PermitLimit { get; set; } = 5;
  public int ReplenishmentPeriod { get; set; } = 2;
  public int QueueLimit { get; set; } = 2;
  public int SegmentsPerPeriod { get; set; } = 8;
  public int TokenLimit { get; set; } = 10;
  public int TokenLimitTwo { get; set; } = 20;
  public int TokensPerPeriod { get; set; } = 4;
  public bool AutoReplenishment { get; set; } = false;
}