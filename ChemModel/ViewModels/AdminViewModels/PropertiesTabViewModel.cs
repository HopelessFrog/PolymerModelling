using ChemModel.Data;
using ChemModel.Data.DbTables;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace ChemModel.ViewModels
{
    public partial class PropertiesTabViewModel : ObservableObject
    {
        Context ctx = new Context();
        [ObservableProperty]
        private ObservableCollection<Property> props;
        [NotifyCanExecuteChangedFor(nameof(DeletePropCommand))]
        [ObservableProperty]
        private Property? selectedProp;
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddPropCommand))]
        private Unit? newPropUnit;
        [NotifyCanExecuteChangedFor(nameof(AddPropCommand))]
        [ObservableProperty]
        private string newPropName = "";
        [NotifyCanExecuteChangedFor(nameof(AddPropCommand))]
        [ObservableProperty]
        private string newPropChars = "";
        [ObservableProperty]
        private ObservableCollection<Unit>? units;
        public PropertiesTabViewModel()
        {
            Units = new ObservableCollection<Unit>(ctx.Units.ToList());
            Props = new ObservableCollection<Property>(ctx.Properties.ToList());
            if (Props.Any())
            {
                SelectedProp = Props[0];
            }
        }

        private bool CanAddProp() =>
            !string.IsNullOrEmpty(NewPropName) && !string.IsNullOrEmpty(NewPropChars) && NewPropUnit is not null;
        [RelayCommand(CanExecute = nameof(CanAddProp))]
        private void AddProp()
        {
            var allProps = ctx.Properties.ToList();
            var allParams = ctx.EmpiricCoefficients.ToList();
            if (allProps.FirstOrDefault(x => x.Name == NewPropName || x.Chars == NewPropChars) is not null || allParams.FirstOrDefault(x => x.Name == NewPropName || x.Chars == NewPropChars) is not null)
            {
                MessageBox.Show("Переменная, участвующая в уравнениях, с таким именем или обозначением уже существует","Предупреждение",MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Property prop = new Property()
            {
                Name = NewPropName,
                Chars = NewPropChars,
                Units = NewPropUnit!,
            };
            ctx.Properties.Add(prop);
            ctx.SaveChanges();
            var mats = ctx.Materials.ToList();
            foreach (var mat in mats)
            {
                ctx.MaterialPropertyBinds.Add(new MaterialPropertyBind()
                {
                    Material = mat,
                    MaterialId = mat.Id,
                    Property = prop,
                    PropertyId = prop.Id,
                    Value = 0
                });
            }
            ctx.SaveChanges();
            Props.Add(prop);
            NewPropName = "";
            NewPropChars = "";
            NewPropUnit = null;
        }
        private bool CanDelete() =>
            SelectedProp is not null;
        [RelayCommand]
        private void DeleteProp(Property  prop)
        {
            if (Props is null || !Props.Any())
            {
                return;
            }
            ctx.Properties.Remove(prop!);
            ctx.SaveChanges();
            Props.Remove(prop!);
           
        }
        public void GetLatestUnits()
        {
            Units = new ObservableCollection<Unit>(ctx.Units.ToList());
        }
    }
}
