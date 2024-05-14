using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemModel.Data.DbTables
{
    public class Material
    {
        public int Id { get; set; }
        [Required, Display(Name = "Навзвание"), Column(TypeName = "varchar(100)")]
        public string Name { get; set; }
        public List<MaterialPropertyBind> Properties { get; set; } = new();
        public List<MaterialEmpiricBind> MathModelProperties { get; set; } = new();
    }
}
