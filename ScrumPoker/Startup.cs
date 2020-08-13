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
  /// ������������ ����������.
  /// </summary>
  public class Startup
  {
    /// <summary>
    /// ����������� ������ Startup.
    /// </summary>
    /// <param name="configuration">������� ���������� ������������.</param>
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    /// <summary>
    /// ������� ���������� ������������.
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// ������������ ��������.
    /// </summary>
    /// <param name="services">��������� ��������.</param>
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
    /// ������������ HTTP.
    /// </summary>
    /// <param name="app">������� ���������� �������.</param>
    /// <param name="env">�������� ���������� ���������.</param>
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
