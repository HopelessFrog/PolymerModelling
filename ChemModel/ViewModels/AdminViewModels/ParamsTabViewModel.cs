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
    public partial class ParamsTabViewModel : ObservableObject
    {
        Context ctx = new Context();
        [ObservableProperty]
        private ObservableCollection<EmpiricCoefficient> parameters;
        [NotifyCanExecuteChangedFor(nameof(DeleteParamCommand))]
        [ObservableProperty]
        private EmpiricCoefficient? selectedParam;
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddParamCommand))]
        private Unit? newParamUnit;
        [NotifyCanExecuteChangedFor(nameof(AddParamCommand))]
        [ObservableProperty]
        private string newParamName = "";
        [NotifyCanExecuteChangedFor(nameof(AddParamCommand))]
        [ObservableProperty]
        private string newParamChars = "";
        [ObservableProperty]
        private ObservableCollection<Unit>? units;
        public ParamsTabViewModel()
        {
            Units = new ObservableCollection<Unit>(ctx.Units.ToList());
            Parameters = new ObservableCollection<EmpiricCoefficient>(ctx.EmpiricCoefficients.ToList());
            if (Parameters.Any())
            {
                SelectedParam = Parameters[0];
            }
        }

        private bool CanAddParam() =>
            !string.IsNullOrEmpty(NewParamName) && !string.IsNullOrEmpty(NewParamChars) && NewParamUnit is not null;
        [RelayCommand(CanExecute = nameof(CanAddParam))]
        private void AddParam()
        {
            var allProps = ctx.Properties.ToList();
            var allParams = ctx.Properties.ToList();
            if (allProps.FirstOrDefault(x => x.Name == NewParamName || x.Chars == NewParamChars) is not null || allParams.FirstOrDefault(x => x.Name == NewParamName || x.Chars == NewParamChars) is not null)
            {
                MessageBox.Show("Переменная, участвующая в уравнениях, с таким именем или обозначением уже существует", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            EmpiricCoefficient prop = new EmpiricCoefficient()
            {
                Name = NewParamName,
                Chars = NewParamChars,
                Units = NewParamUnit!,
            };
            ctx.EmpiricCoefficients.Add(prop);
            ctx.SaveChanges();
            var mats = ctx.Materials.ToList();
            foreach (var mat in mats)
            {
                ctx.MaterialEmpiricBinds.Add(new MaterialEmpiricBind()
                {
                    Material = mat,
                    MaterialId = mat.Id,
                    Property = prop,
                    PropertyId = prop.Id,
                    Value = 0
                });
            }
            ctx.SaveChanges();
            Parameters.Add(prop);
            NewParamChars = "";
            NewParamName = "";
            NewParamUnit = null;
        }
        
        [RelayCommand]
        private void DeleteParam(EmpiricCoefficient param)
        {
            if (Parameters is null || !Parameters.Any())
            {
                return;
            }
            ctx.EmpiricCoefficients.Remove(param!);
            ctx.SaveChanges();
            Parameters.Remove(param!);
            SelectedParam = null;
        }
        public void GetLatestUnits()
        {
            Units = new ObservableCollection<Unit>(ctx.Units.ToList());
        }
    }
}
