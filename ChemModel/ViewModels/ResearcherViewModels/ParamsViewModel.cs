using ChemModel.Data;
using ChemModel.Data.DbTables;
using ChemModel.Messeges;
using ChemModel.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace ChemModel.ViewModels
{
    public partial class ParamsViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Material> materials;
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SolveCommand))]
        private Material? selectedMaterial;
        [NotifyCanExecuteChangedFor(nameof(SolveCommand))]
        [ObservableProperty]
        private double width = 0.25;
        [NotifyCanExecuteChangedFor(nameof(SolveCommand))]
        [ObservableProperty]
        private double length = 9.5;
        [NotifyCanExecuteChangedFor(nameof(SolveCommand))]
        [ObservableProperty]
        private double height = 0.01;
        [NotifyCanExecuteChangedFor(nameof(SolveCommand))]
        [ObservableProperty]
        private double velocity = 1.5;
        [ObservableProperty]
        private double temperature = 150;
        [NotifyCanExecuteChangedFor(nameof(SolveCommand))]
        [ObservableProperty]
        private double step = 0.1;
        [ObservableProperty]
        private ObservableCollection<MaterialEmpiricBind>? coefs;
        [ObservableProperty]
        private ObservableCollection<MaterialPropertyBind>? properties;
        public ParamsViewModel() 
        {
            using var ctx = new Context();
            Materials = new ObservableCollection<Material>(ctx.Materials.ToList());
            if (Materials.Any())
            {
                SelectedMaterial = Materials[0];
            }
        }
        public void MaterialSelected()
        {
            if (SelectedMaterial is null)
                return;
            using var ctx = new Context();
            Properties = new ObservableCollection<MaterialPropertyBind>(ctx.MaterialPropertyBinds.Where(x => x.MaterialId == SelectedMaterial.Id).Include(x => x.Property).Include(x => x.Property.Units).ToList());
            Coefs = new ObservableCollection<MaterialEmpiricBind>(ctx.MaterialEmpiricBinds.Where(x => x.MaterialId == SelectedMaterial.Id).Include(x => x.Property).Include(x => x.Property.Units).ToList());
        }
        private bool CanSolve() =>
            SelectedMaterial is not null && Width > 0 && Length > 0 && Height > 0 && Velocity > 0 && Step > 0;
        [RelayCommand(CanExecute = nameof(CanSolve))]
        private void Solve(Window window)
        {
            if (!Validator.IsValid(window))
            {
                MessageBox.Show("Устраните все ошибки ввода перед моделированием", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            long mathOperCount = 0;
            double Tg = Properties!.First(x => x.Property.Chars == "T0").Value + 40;
            double C1g = Coefs!.First(x => x.Property.Chars == "Ea").Value;
            double C2g = Coefs!.First(x => x.Property.Chars == "Ea").Value + 40;
            double T0 = Properties!.First(x => x.Property.Chars == "T0").Value;
            double Tr = Coefs!.First(x => x.Property.Chars == "Tr").Value;
            double index = Coefs!.First(x => x.Property.Chars == "n").Value;
            double termCoef = Coefs!.First(x => x.Property.Chars == "αu").Value;
            double mu0 = Coefs!.First(x => x.Property.Chars == "μ0").Value;
            double Ro = Properties!.First(x => x.Property.Chars == "ρ").Value;
            double c = Properties!.First(x => x.Property.Chars == "c").Value;
            double Tch = (T0 + (Tg + 100)) / 2;
            mathOperCount += 3;
            double C2 = C2g + Tr - Tg;
            mathOperCount += 2;
            double C1 = (C1g * C2g) / C2;
            if (double.IsNaN(C1))
            {
                MessageBox.Show("Невозможно константы уравнения ВЛФ при введеых данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            mathOperCount += 2;  
            double geomCoef = 0.125 * Math.Pow(Height / Width, 2) - 0.625 * (Height / Width) + 1;
            if (double.IsNaN(geomCoef))
            {
                MessageBox.Show("Невозможно рассчитать проправочный коэффициент с введенными данными", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            mathOperCount += 7;
            double Qch = ((Height * Width * Velocity) / 2) * geomCoef;
            if (double.IsNaN(Qch))
            {
                MessageBox.Show("Невозможно рассчитать объемный расход потока материала в канале  с введенными данными", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            mathOperCount += 4;
            double gamma = Velocity / Height;
            if (double.IsNaN(gamma))
            {
                MessageBox.Show("Невозможно рассчитать скорость деформации сдвига с введенными данными", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            mathOperCount++;
            double qGamma = Height * Width * mu0 * Math.Pow(gamma, index + 1);
            mathOperCount += 5;
            double b = C1 / (C2 + (Tch - Tr));
            mathOperCount += 3;
            double qAlpha = Width * termCoef * (Math.Pow(b, -1) - Temperature + Tr);
            if (double.IsNaN(qAlpha) || double.IsNaN(qGamma))
            {
                MessageBox.Show("Невозможно рассчитать удельные тепловые потоки  с введенными данными", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            mathOperCount += 5;
            int N = (int)(Length / Step);
            mathOperCount++;
            TableData[] data = new TableData[N + 1];
            for (int i = 0; i <= N; i++)
            {
                double z = Step * i;
                double T = Tr + (1 / b) * Math.Log(((b * qGamma + Width * termCoef) /(b*qAlpha)) * (1 - Math.Exp(-(b * qAlpha)/(Ro * c * Qch)*z)) + Math.Exp(b * (T0 - Tr - (qAlpha/(Ro * c * Qch))*z)));
                double visc = mu0 * Math.Exp(-b * (T - Tr));
                double vaz = visc * Math.Pow(gamma, index - 1);
                data[i] = new TableData() { Coord = Math.Round(z, 5), Temp = T, Vaz = vaz};
                mathOperCount += 36;
            }
            stopwatch.Stop();
            long miliseconds = stopwatch.ElapsedMilliseconds;
            var resultWindow = new ResultsWindow();
            WeakReferenceMessenger.Default.Send(new DataMessage(data));
            WeakReferenceMessenger.Default.Send(new SolveParamsMessage(new SolveParams() { Miliseconds = miliseconds, Operations = mathOperCount }));
            WeakReferenceMessenger.Default.Send(new ResultDataMessage(new ViewModelsData.ResultData() { Temperature = data[data.Length - 1].Temp, Performance = Ro * Qch * 3600, Viscosity = data[data.Length - 1].Vaz }));
            WeakReferenceMessenger.Default.Send(new DataExcelMessage(new ViewModelsData.DataExcel()
            {
                Coefs = Coefs.ToList(),
                Properties = Properties.ToList(),
                Velocity = Velocity,
                Height = Height,
                Length = Length,
                Width = Width,
                Material = SelectedMaterial!, 
                Step = Step,
                TempCr = Temperature,
            }));
            resultWindow.ShowDialog();
        }
    }
}
