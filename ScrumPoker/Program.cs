using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ScrumPoker.Data;

namespace ScrumPoker
{
  /// <summary>
  /// main �����.
  /// </summary>
  public class Program
  {
    /// <summary>
    /// Entry point.
    /// </summary>
    /// <param name="args">������ ����������.</param>
    public static void Main(string[] args)
    {
      var host = CreateHostBuilder(args).Build();
      using (var scope = host.Services.CreateScope())
      {
        var services = scope.ServiceProvider;
        try
        {
          var context = services.GetRequiredService<ModelContext>();
          DbInitializer.Initialize(context);
        }
        catch (Exception ex)
        {
          var logger = services.GetRequiredService<ILogger<Program>>();
          logger.LogError(ex, "An error occurred while seeding the database.");
        }

        host.Run();
      }
    }

    /// <summary>
    /// ������������ �����. 
    /// </summary>
    /// <param name="args">������ ����������.</param>
    /// <returns>��������� �����.</returns>
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            });
  }
}
