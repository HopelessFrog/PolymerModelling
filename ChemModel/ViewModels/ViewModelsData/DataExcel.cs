using ChemModel.Data.DbTables;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemModel.ViewModels.ViewModelsData
{
    public class DataExcel
    {
        public double Length { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double Velocity { get; set; }
        public double TempCr {  get; set; }
        public double Step {  get; set; }
        public Material Material { get; set; }
        public List<MaterialEmpiricBind> Coefs { get; set; }
        public List<MaterialPropertyBind> Properties { get; set; }
    }
}
