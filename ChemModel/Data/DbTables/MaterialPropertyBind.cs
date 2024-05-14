using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemModel.Data.DbTables
{
    public class MaterialPropertyBind
    {
        public int Id { get; set; }
        [Required]
        public double Value { get; set; }
        public int MaterialId { get; set; }
        [Required, ForeignKey("MaterialId")]
        public Material Material { get; set; }
        public int PropertyId { get; set; }
        [Required, ForeignKey("PropertyId")]
        public Property Property { get; set; }
    }
}
