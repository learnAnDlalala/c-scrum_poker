using Microsoft.AspNetCore.Mvc;
using ScrumPoker.Data;
using ScrumPoker.Data.Models;
using ScrumPoker.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScrumPoker.Controllers
{
  /// <summary>
  /// Контроллер пользователей.
  /// </summary>
  [ApiController]
  [Route("api/[controller]")]
   public class UserController : Controller
  {
    /// <summary>
    /// Контекст бд.
    /// </summary>
    public readonly ModelContext db;
    /// <summary>
    /// Сервис пользователей.
    /// </summary>
    public readonly UserService userService;

    /// <summary>
    /// Конструктор пользователей.
    /// </summary>
    /// <param name="db">контекст бд.</param>
    /// <param name="userService">сервис пользователей.</param>
    public UserController(ModelContext db, UserService userService)
    {
      this.db = db;
      this.userService = userService;
    }

    /// <summary>
    /// Запрос на создания нового пользователя.
    /// </summary>
    /// <param name="newUser">инстанс класса пользователь.</param>
    /// <returns>id пользователя.</returns>
    [HttpPost]
    public async Task<IActionResult> Post(User newUser)
    {
      var id = await this.userService.Create(this.db, newUser);
      return new OkObjectResult(id);
    }

    /// <summary>
    /// Запрос на полученя списка пользователей.
    /// </summary>
    /// <returns>Список пользователей.</returns>
    [HttpGet]
    public async Task<ActionResult<List<User>>> Get()
    {
      return await this.userService.ShowAll(this.db);
    }

    /// <summary>
    /// Запрос на вход в учетную запись.
    /// </summary>
    /// <param name="user">инстанс класса пользователь.</param>
    /// <returns>true / false.</returns>
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(User user)
    {
      return await this.userService.CheckRegistration(this.db, user);
    }
  }
}
