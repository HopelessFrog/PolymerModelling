using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using ChemModel.Messeges;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Primitives;

namespace ChemModel.ViewModels
{
    public partial class AddMaterialViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        private string name = "";
        private bool CanOk() => !string.IsNullOrEmpty(Name);
        [RelayCommand(CanExecute = nameof(CanOk))]
        private void Ok(Window window)
        {
            WeakReferenceMessenger.Default.Send(new MaterialMessage(new NewMat() { Name = Name}));
            window.Close();
        }
    }
}
