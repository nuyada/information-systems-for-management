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
           .WriteTo.File("logs/app-log.txt", rollingInterval: RollingInterval.Day) // ����������� � ����
           .WriteTo.Console() // ����������� � �������
           .CreateLogger();

            try
            {
                Log.Information("������ ���-����������...");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "����������� ������ ��� ������� ����������!");
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
