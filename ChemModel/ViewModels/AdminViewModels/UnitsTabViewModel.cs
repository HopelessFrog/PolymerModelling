using ChemModel.Data;
using ChemModel.Data.DbTables;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemModel.ViewModels
{
    public partial class UnitsTabViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Unit> units;
      
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddUnitCommand))]
        private string newUnit = "";
        public UnitsTabViewModel() 
        {
            using Context ctx = new Context();
            Units = new ObservableCollection<Unit>(ctx.Units.ToList());
           
        }
        public bool CanAdd() => !string.IsNullOrEmpty(NewUnit);
        [RelayCommand(CanExecute = nameof(CanAdd))]
        public void AddUnit()
        {
            using Context ctx = new Context();
            var unit = new Unit { Name = NewUnit };
            Units.Add(unit);
            ctx.Units.Add(unit);
            ctx.SaveChanges();
            NewUnit = "";
        }

        [RelayCommand]

        public void DeleteUnit(Unit unit)
        {
          
            using Context ctx = new Context();
            ctx.Units.Remove(unit!);
            ctx.SaveChanges();
            Units.Remove(unit!);
          
        }
    }
}
