using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ScrumPoker.Data;
using ScrumPoker.Data.Models;
using ScrumPoker.SignalR;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace ScrumPoker.Services
{
  /// <summary>
  /// Класс сервиса раундов.
  /// </summary>
  public class RoundService
  {
    /// <summary>
    /// контекст Хаба комнаты.
    /// </summary>
    private IHubContext<RoomsHub> ctx;

    /// <summary>
    /// Конструктор класса.
    /// </summary>
    /// <param name="context">контекст хаба.</param>
    public RoundService (IHubContext<RoomsHub> context)
    {
      this.ctx = context;
    }

    /// <summary>
    /// Старт раунда.
    /// </summary>
    /// <param name="db">контекст бд.</param>
    /// <param name="newRound">инстанс класса раунда.</param>
    /// <returns>ничего не возвращает.</returns>
    public async Task Start(ModelContext db, Round newRound)
    {
      newRound.Start = DateTime.Now;
      db.Rounds.Add(newRound);
      await db.SaveChangesAsync();
      this.CreateTimer(db, newRound);
      this.ctx.Clients.Group($"room={newRound.Room.ID}").SendAsync("StartRound").Wait();
    }

    /// <summary>
    /// Информация о раунде.
    /// </summary>
    /// <param name="db">контекст бд.</param>
    /// <param name="id">id раунда.</param>
    /// <returns>инстант раунда.</returns>
    public async Task<ActionResult<Round>> GetRoundInfo(ModelContext db, int id)
    {
      var info = await db.Rounds.Include(x => x.Cards).FirstOrDefaultAsync(t => t.ID == id);
      return info;
    }

    /// <summary>
    /// Перезапуск раунда.
    /// </summary>
    /// <param name="db">контекст бд.</param>
    /// <param name="round">id раунда.</param>
    /// <returns>ничего не возвращает.</returns>
    public async Task Restart(ModelContext db, Round round)
    {
      var restart = await db.Rounds.FirstOrDefaultAsync(t => t.ID == round.ID);
      restart.Start = DateTime.Now;
      restart.Deck = round.Deck;
      await db.SaveChangesAsync();
      this.ctx.Clients.Group($"room={round.Room.ID}").SendAsync("StartRound").Wait();
    }

    /// <summary>
    /// Окончание раунда.
    /// </summary>
    /// <param name="db">контекст бд.</param>
    /// <param name="round">инстант класса раунда.</param>
    /// <returns>ничего не возвращает.</returns>
    public async Task EndRound(ModelContext db, Round round)
    {
      var currentRound = await db.Rounds.FindAsync(round.ID);
      currentRound.End = DateTime.Now;
      await db.SaveChangesAsync();
      this.ctx.Clients.Group($"room={round.Room.ID}").SendAsync("EndRound").Wait();
    }

    /// <summary>
    /// Создания таймера.
    /// </summary>
    /// <param name="db">контекст бд.</param>
    /// <param name="round">инстанс класса раунда.</param>
    /// <returns>таймер.</returns>
    private Timer CreateTimer(ModelContext db, Round round)
    {
      var timer = new Timer(round.Timer * Math.Pow(10, 4));
      timer.AutoReset = false;
      timer.Elapsed += async (sender, e) => await this.EndRound(db,round);
      timer.Enabled = true;
      timer.Start();
      return timer;
    }
  }
}
