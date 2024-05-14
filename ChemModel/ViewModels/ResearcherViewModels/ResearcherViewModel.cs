using ChemModel.Data;
using ChemModel.Data.DbTables;
using ChemModel.Messeges;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ChemModel.ViewModels
{
    public partial class ResearcherViewModel : ObservableObject, IRecipient<SolveParamsMessage>
    {
        [ObservableProperty]
        private string mem = "";
        private DispatcherTimer timer;
        [ObservableProperty]
        private string time = "-";
        [ObservableProperty]
        private long operations = 0;
        public ResearcherViewModel()
        {
           
            WeakReferenceMessenger.Default.Register<SolveParamsMessage>(this);
        }

        public void Receive(SolveParamsMessage message)
        {
            Time = message.Value.Miliseconds.ToString() + " мс";
            Operations = message.Value.Operations;

            using Process proc = Process.GetCurrentProcess();
            Mem = Math.Round((double)(proc.PrivateMemorySize64 / (1024 * 1024)), 2) + " МБ";
            MessageBox.Show($"Время: {Time} \nЧисло операций:{Operations}\nОперативная память:{Mem} ", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
