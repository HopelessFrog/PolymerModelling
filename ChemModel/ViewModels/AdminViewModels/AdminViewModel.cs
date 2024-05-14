using ChemModel.Data;
using ChemModel.Data.DbTables;
using ChemModel.Messeges;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ChemModel.ViewModels
{
    public partial class AdminViewModel : ObservableObject
    {
        [ObservableProperty]
        private string mem = "";

        private DispatcherTimer timer;
        public AdminViewModel()
        {
            timer = new();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (e, args) =>
            {
                using Process proc = Process.GetCurrentProcess();
                Mem = Math.Round((double)(proc.PrivateMemorySize64 / (1024 * 1024)), 2) + " МБ";
            };
            timer.Start();
        }
        [RelayCommand]
        private void Copy()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "База данных";
            dlg.DefaultExt = ".db";
            dlg.Filter = "База данных (.db)|*.db";
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                File.Copy(DBConfig.Destination, dlg.FileName);
                MessageBox.Show("Сохранение прошло успешно", "Сохранение завершено", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
