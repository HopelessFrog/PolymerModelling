using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemModel.Data.DbTables
{
    public class UserAddMaterial
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required, ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public int MaterialId { get; set; }
        [Required, ForeignKey(nameof(MaterialId))]
        public Material Material { get; set; }
    }
}
