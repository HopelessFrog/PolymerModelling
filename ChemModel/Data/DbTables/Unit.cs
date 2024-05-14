using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemModel.Data.DbTables
{
    public class Unit
    {
        public int Id { get; set; }
        [Required, Display(Name = "Единица измерения"), ColumnName("Название"), Column(TypeName = "varchar(20)")]
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }

    }
}
