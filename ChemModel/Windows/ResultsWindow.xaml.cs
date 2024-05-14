using ChemModel.ViewModels;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChemModel.Windows
{
    /// <summary>
    /// Interaction logic for ResultsWindow.xaml
    /// </summary>
    public partial class ResultsWindow : Window
    {
        public ResultsWindow()
        {
            InitializeComponent();
            DataContext = new ResearcherViewModel();
            var graph = new GraphicsViewModel(temp, vaz);
            graphics.DataContext = graph;
            results.DataContext = new ResultsViewModel();
            ScottPlot.Plottables.Crosshair? CrosshairTemp = temp.Plot.Add.Crosshair(0, 0);
            ScottPlot.Plottables.Crosshair? CrooshairVaz = vaz.Plot.Add.Crosshair(0, 0);
            CrosshairTemp.IsVisible = false;
            CrooshairVaz.IsVisible = false;
            temp.MouseMove += (s, e) =>
            {
                if (graph.ScatterTemp is null)
                {
                    return;
                }
                var pos = this.PointToScreen(Mouse.GetPosition(temp));
                pos.Y -= 25;
                // determine where the mouse is and get the nearest point
                Pixel mousePixel = new(pos.X, pos.Y);
                Coordinates mouseLocation = temp.Plot.GetCoordinates(mousePixel);
                DataPoint nearest = graph.ScatterTemp.Data.GetNearest(mouseLocation, temp.Plot.LastRender);

                // place the crosshair over the highlighted point
                if (nearest.IsReal)
                {
                    CrosshairTemp.IsVisible = true;
                    CrosshairTemp.Position = nearest.Coordinates;
                    temp.Refresh();
                    graph.NearXTemp = $"{nearest.X:0.##}";
                    graph.NearYTemp = $"{nearest.Y:0.##}";
                }

                // hide the crosshair when no point is selected
                if (!nearest.IsReal && CrosshairTemp.IsVisible)
                {
                    CrosshairTemp.IsVisible = false;
                    temp.Refresh();
                    graph.NearXTemp = "--";
                    graph.NearYTemp = "--";
                }
            };
            vaz.MouseMove += (s, e) =>
            {
                if (graph.ScatterVaz is null)
                {
                    return;
                }
                var pos = this.PointToScreen(Mouse.GetPosition(vaz));
                pos.Y -= 25;

                // determine where the mouse is and get the nearest point
                Pixel mousePixel = new(pos.X, pos.Y);
                Coordinates mouseLocation = vaz.Plot.GetCoordinates(mousePixel);
                DataPoint nearest = graph.ScatterVaz.Data.GetNearest(mouseLocation, vaz.Plot.LastRender);

                // place the crosshair over the highlighted point
                if (nearest.IsReal)
                {
                    CrooshairVaz.IsVisible = true;
                    CrooshairVaz.Position = nearest.Coordinates;
                    vaz.Refresh();
                    graph.NearXVaz = $"{nearest.X:0.##}";
                    graph.NearYVaz = $"{nearest.Y:0.##}";
                }

                // hide the crosshair when no point is selected
                if (!nearest.IsReal && CrooshairVaz.IsVisible)
                {
                    CrooshairVaz.IsVisible = false;
                    vaz.Refresh();
                    graph.NearXVaz = "--";
                    graph.NearYVaz = "--";
                }
            };
        }
        private static readonly Regex _posReg = new Regex("[^0-9,]+");
        private static readonly Regex _reg = new Regex("[^0-9,-]+");
        private static bool IsTextAllowedPos(string text)
        {
            return !_posReg.IsMatch(text);
        }
        private static bool IsTextAllowed(string text)
        {
            return !_reg.IsMatch(text);
        }

        private void TextBox_PreviewPositive(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowedPos(e.Text);
        }
        private void TextBox_Preview(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
    }
}
