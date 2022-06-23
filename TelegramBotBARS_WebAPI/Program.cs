using Microsoft.AspNetCore;
using TelegramBotBARS_WebAPI;

public static class Programm
{
    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args)
            //.UseUrls("http://localhost:5002", "https://localhost:5003")
            .Build().Run();
    }
    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>();
}
