using ChemModel.Messeges;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using ScottPlot;
using ScottPlot.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;

namespace ChemModel.ViewModels
{
    public partial class GraphicsViewModel : ObservableObject, IRecipient<DataMessage>
    {
        [ObservableProperty]
        private TableData[]? data;
        private WpfPlot tempPlot;
        private WpfPlot vazPlot;
        [ObservableProperty]
        private string nearXTemp = "--";
        [ObservableProperty]
        private string nearYTemp = "--";
        [ObservableProperty]
        private string nearXVaz = "--";
        [ObservableProperty]
        private string nearYVaz = "--";
        public ScottPlot.Plottables.Scatter? ScatterTemp { get; set; }
        public ScottPlot.Plottables.Scatter? ScatterVaz { get; set; }
        public GraphicsViewModel(WpfPlot tempPlot, WpfPlot vazPlot)
        {
            this.tempPlot = tempPlot;
            this.vazPlot = vazPlot;
            WeakReferenceMessenger.Default.Register<DataMessage>(this);
            
        }

        public void Receive(DataMessage message)
        {
            tempPlot.Plot.Remove(ScatterTemp!);
            vazPlot.Plot.Remove(ScatterVaz!);
            Data = message.Value;
            double[] xs = Data.Select(x => x.Coord).ToArray();
            double[] temp = Data.Select(x => x.Temp).ToArray();
            double[] vaz = Data.Select(x => x.Vaz).ToArray();
            ScatterTemp = tempPlot.Plot.Add.Scatter(xs, temp);
            tempPlot.Plot.Axes.Bottom.Label.Text = "Координата по длине канала z, м";
            tempPlot.Plot.Axes.Left.Label.Text = "Температура T, °С";
            tempPlot.Plot.Axes.Title.Label.Text = "График распределения температуры по длине канала";
            ScatterVaz = vazPlot.Plot.Add.Scatter(xs, vaz);
            vazPlot.Plot.Axes.Bottom.Label.Text = "Координата по длине канала z, м";
            vazPlot.Plot.Axes.Left.Label.Text = "Вязкость материала Eta, Па*с";
            vazPlot.Plot.Axes.Title.Label.Text = "График распределения вязкости по длине канала";
            tempPlot.Plot.Axes.SetLimits(xs[0], xs[xs.Length - 1], temp.Min(), temp.Max());
            vazPlot.Plot.Axes.SetLimits(xs[0], xs[xs.Length - 1], vaz.Min(), vaz.Max());
            tempPlot.Refresh();
            vazPlot.Refresh();
        }
    }
}
