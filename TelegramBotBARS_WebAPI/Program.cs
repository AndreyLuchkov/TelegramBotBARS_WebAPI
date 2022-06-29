using Microsoft.AspNetCore;
using TelegramBotBARS_WebAPI;
using TelegramBotBARS_WebAPI.DapperTypeHandlers;

public static class Programm
{
    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args).Build().Run();
    }
    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>();
}
