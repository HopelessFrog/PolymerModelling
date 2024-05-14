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
    /// Interaction logic for ResearcherWindow.xaml
    /// </summary>
    public partial class ResearcherWindow : Window
    {
        
        public ResearcherWindow()
        {
            InitializeComponent();
            this.Closed += (sender, e) => Owner.Close();
            var paramsModel = new ParamsViewModel();
            DataContext = paramsModel;
            matCombo.SelectionChanged += (sender, e) => paramsModel.MaterialSelected();
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
