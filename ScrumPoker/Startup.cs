using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScrumPoker.Data;
using ScrumPoker.Services;
using ScrumPoker.SignalR;

namespace ScrumPoker
{
  /// <summary>
  /// Конфигурация приложения.
  /// </summary>
  public class Startup
  {
    /// <summary>
    /// Конструктор класса Startup.
    /// </summary>
    /// <param name="configuration">Инстанс интерфейса конфигурация.</param>
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    /// <summary>
    /// Инстанс интерфейса конфигурация.
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// Конфигурация сервисов.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    public void ConfigureServices(IServiceCollection services)
    {
      string connection = Configuration.GetConnectionString("DefaultConnection");
      services.AddDbContext<ModelContext>(options => options.UseSqlServer(connection));
      services.AddSingleton<UserService>();
      services.AddSingleton<RoomService>();
      services.AddSingleton<DeckService>();
      services.AddSingleton<RoundService>();
      services.AddSignalR();
      services.AddControllers();
    }
    /// <summary>
    /// Конфигурация HTTP.
    /// </summary>
    /// <param name="app">инстанс интерфейса билдера.</param>
    /// <param name="env">инстантс интерфейса окружения.</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      app.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(origin => true)
        .AllowCredentials());
      app.UseRouting();
      app.UseAuthorization();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapHub<RoomsHub>("/roomsHub");
      });
    }
  }
}
