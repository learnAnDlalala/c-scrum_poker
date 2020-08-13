using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScrumPoker.Data.Models
{
  /// <summary>
  /// Класс представляющий раунды.
  /// </summary>
  public class Round
  {
    /// <summary>
    /// Конструткор класса.
    /// </summary>
    public Round()
    {
      this.Cards = new List<RoundCard>();
    }

    /// <summary>
    /// ID раунда.
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// Предмет голосования.
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Таймер голосования.
    /// </summary>
    public int Timer { get; set; }

    /// <summary>
    /// Среднее значение.
    /// </summary>
    /// <returns></returns>
    public int Average()
    {
      int sum = 0;
      foreach (var card in this.Cards)
      {
        sum += card.Card.Value;
      }
      return sum / this.Cards.Count;
    }

    /// <summary>
    /// Дата начала.
    /// </summary>
    public DateTime Start { get; set; }

    /// <summary>
    /// Дата конца.
    /// </summary>
    public DateTime End { get; set; }

    /// <summary>
    /// Продолжительность голосования.
    /// </summary>
    [NotMapped]
    public TimeSpan Duration => this.End.Subtract(this.Start);

    /// <summary>
    /// Использумая в голосовании колода.
    /// </summary>
    public virtual Deck Deck { get; set; }

    /// <summary>
    /// Список карт выбранных в течении голосования.
    /// </summary>
    public virtual ICollection<RoundCard> Cards { get; set; }

    /// <summary>
    /// Комната в которой проходит голосование.
    /// </summary>
    public virtual Room Room { get; set; }
  }
}
