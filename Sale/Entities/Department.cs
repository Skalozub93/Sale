using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Entities
{
    // Класс-сущность, отображающая таблицу Departments
    public class Department
    {
        // класс-сущность, отображающую таблицу Departments
        public Guid Id { get; set; }     //набор сущностей посторяет структуру таблицы
        public String Name { get; set; } = null!;

        public String ToShortString()
        {
            return Id.ToString()[..4] + "... " + Name;
        }
    }

}
