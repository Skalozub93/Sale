using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public String Name { get; set; } = null!;
        public double Price { get; set; } // !! тип FLOAT в БД соотвествует типу double в с#

        public String ToShortString()
        {
            return $"{Id.ToString()[..4]} ... {Name} ({Price} грн)";
        }
    }
}
