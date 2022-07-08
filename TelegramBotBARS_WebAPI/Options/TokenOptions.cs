namespace TelegramBotBARS_WebAPI.Options
{
    public class TokenOptions
    {
        public string Issuer { get; init; } = null!;
        public string Audience { get; init; } = null!;
        public string SigningKey { get; init; } = null!;
    }
}
