using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using ChemModel.Data;
using System.Diagnostics.Metrics;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using ChemModel.Data.DbTables;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore.Query;
using ChemModel.Windows;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using CommunityToolkit.Mvvm.Messaging;

namespace ChemModel.ViewModels
{
    public partial class AuthViewModel : ObservableObject
    {
        private PasswordBox pwb;
        [ObservableProperty]
        private string error = "";
        [ObservableProperty]
        private string login = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EnterCommand))]
        private int tryEnterCount = 0;

        public AuthViewModel(PasswordBox pwb)
        {
            this.pwb = pwb;
        }
        [RelayCommand(CanExecute = nameof(CanEnter))]
        private void Enter(Window window)
        {
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(pwb.Password))
            {
                Error = "Введите данные";
                return;
            }
            using var ctx = new Context();
            //var adminR = new Role() { Name = "admin" };
            //var userR = new Role() { Name = "user" };
            //ctx.Roles.AddRange([adminR, userR]);
            //var admin = new User() { Role = adminR, Name = "admin", Password = HashStatic.GetHash256("admin") };
            //ctx.Users.Add(admin);
            //ctx.SaveChanges();
            //return;
            var user = ctx.Users.Include(x => x.Role).FirstOrDefault(x => x.Name == Login && x.Password == pwb.Password);
            if (user is null)
            {
                TryEnterCount++;
                Error = "Неверный логин и/или пароль";
                return;
            }
            if (user.Role!.Name == "admin")
            {
                var main = new AdminWindow() { Owner = window };
                WeakReferenceMessenger.Default.Send(new Messeges.UserMessage(user));
                window.Hide();
                main.ShowDialog();
            }
            else if (user.Role.Name == "user")
            {
                var main = new ResearcherWindow() { Owner = window };
                window.Hide();
                main.ShowDialog();
            }
        }
        private bool CanEnter() =>
            TryEnterCount <= 10;
        
    }
}
