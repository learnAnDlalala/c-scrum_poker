using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPoker.Data.Models
{
  /// <summary>
  /// Класс представляющий карты выбранные в раунде.
  /// </summary>
  public class RoundCard
  {
    /// <summary>
    /// ID инстанса.
    /// </summary>
    public int ID { get; set; }
        
    /// <summary>
    /// ID пользователя выбравшего карту.
    /// </summary>
    public int User { get; set; }

    /// <summary>
    /// Выбранная карта.
    /// </summary>
    public Card Card  { get; set; }

    /// <summary>
    /// Раунд в котором была выбранна карта.
    /// </summary>
    virtual public Round Round { get; set; }
  }
}
