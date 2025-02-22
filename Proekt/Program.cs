using Proekt.Entites;
using Proekt.Repository;
using Proekt.Service;
using Proekt.SQL_DB;
using Serilog;
using System;
using System.Linq;
namespace Proekt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
           .WriteTo.File("logs/app-log.txt", rollingInterval: RollingInterval.Day) // Логирование в файл
           .WriteTo.Console() // Логирование в консоль
           .CreateLogger();

            try
            {
                Log.Information("Запуск веб-приложения...");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Критическая ошибка при запуске приложения!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
