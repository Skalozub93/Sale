using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Entities
{
    internal class TodaySales
    {
        public Guid Id { get; set; }
        public String Name { get; set; } = null!;
        public int Count { get; set; }
        public double Sum { get; set; }

        public String ToShortString()
        {
            return $"{Id.ToString()[..4]} ... {Name} ({Count} шт.) ({Sum} грн)";
            //return $"{Id.ToString()[..4]} ... {Name} ({Count} шт.)";
        }
    }
}
