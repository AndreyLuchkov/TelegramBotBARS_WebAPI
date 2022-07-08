namespace TelegramBotBARS_WebAPI.Services
{
    public class SmtpOptions
    {
        public string Host { get; init; } = null!;
        public int Port { get; init; }
        public string FromEmail { get; init; } = null!;
        public string Password { get; init; } = null!;
    }
}