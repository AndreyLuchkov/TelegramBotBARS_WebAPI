using Microsoft.Extensions.Options;
using Npgsql;
using System.Net;
using System.Net.Mail;
using TelegramBotBARS_WebAPI.Options;

namespace TelegramBotBARS_WebAPI.Services
{
    public class DbConnectionCheckService : BackgroundService
    {
        private readonly DbCheckOptions _options;
        private readonly SmtpOptions _smtpOptions;
        private readonly string _connectionString;

        public DbConnectionCheckService(
            IOptions<DbCheckOptions> options, 
            IOptions<SmtpOptions> smtpOptions,
            IConfiguration configuration)
        {
            _options = options.Value;
            _smtpOptions = smtpOptions.Value;
            _connectionString = configuration.GetConnectionString("PostgreSQL_Check");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
                try
                {
                    conn.Open();
                }
                catch
                {
                    SmtpClient smtp = new();
                    MailMessage message = new(
                        from: _smtpOptions.FromEmail,
                        to: _options.ToEmail,
                        subject: "Admin",
                        body: "PostgreSQL сервер не отвечает.")
                    {
                        BodyEncoding = System.Text.Encoding.UTF8
                    };

                    smtp.Port = _smtpOptions.Port;
                    smtp.Host = _smtpOptions.Host;
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(_smtpOptions.FromEmail, _smtpOptions.Password);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                    smtp.Send(message);
                }
             
                conn.Close();

                await Task.Delay(_options.CheckInterval, stoppingToken);
            }
        }
    }
}
