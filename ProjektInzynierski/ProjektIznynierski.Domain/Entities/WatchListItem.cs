using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektIznynierski.Domain.Entities
{
    public class WatchListItem : BaseEntity
    {
        public int WatchListId { get; set; }
        public WatchList WatchList { get; set; }

        public int InvestInstrumentId { get; set; }
        public InvestInstrument InvestInstrument { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
