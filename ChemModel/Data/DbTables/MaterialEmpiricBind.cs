using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemModel.Data.DbTables
{
    public class MaterialEmpiricBind
    {
        public int Id { get; set; }
        [Required]
        public int PropertyId { get; set; }
        [Required, ForeignKey(nameof(PropertyId))]
        public EmpiricCoefficient Property { get; set; }
        [Required]
        public int MaterialId { get; set; }
        [Required, ForeignKey(nameof(MaterialId))]
        public Material Material { get; set; }
        [Required]
        public double Value { get; set; }
    }
}
