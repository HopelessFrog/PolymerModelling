using ChemModel.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
         
            matGrid.AutoGeneratingColumn += AutoGeneratingColumn;
          
            
            DataContext = new ViewModels.AdminViewModel();
            this.Closed += (sender, e) => Owner.Close();
           // users.DataContext = new ViewModels.UsersTabViewModel();
            var matDC = new ViewModels.MaterialsTabViewModel();
            materials.DataContext = matDC;
            matGrid.SelectionChanged += (sender, e) => matDC.PropChange();
        }
        void AutoGeneratingColumn(object? sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
            }
        }

        private void dataTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                if (propertiesTab.IsSelected)
                {
                    ((PropertiesTabViewModel)propertiesTab.DataContext).GetLatestUnits();
                }
                if (paramsTab.IsSelected)
                {
                    ((ParamsTabViewModel)paramsTab.DataContext).GetLatestUnits();
                }
            }
        }
        private static readonly Regex _reg = new Regex("[^0-9,-]+");
        private static bool IsTextAllowed(string text)
        {
            return !_reg.IsMatch(text);
        }
        private void TextBox_Preview(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
    }
}
