namespace TelegramBotBARS_WebAPI.Options
{
    public class DbCheckOptions
    {
        public int CheckInterval { get; init; }
        public string ToEmail { get; init; } = null!;
    }
}