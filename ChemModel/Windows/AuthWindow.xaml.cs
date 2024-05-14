using System.Windows;

namespace ChemModel
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.AuthViewModel(Pwb);
        }
    }
}
