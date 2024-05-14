using ChemModel.Data.DbTables;
using ChemModel.Data;
using ChemModel.Messeges;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using ChemModel.Windows;

namespace ChemModel.ViewModels
{
    public partial class UsersTabViewModel : ObservableObject, IRecipient<NewUserMessage>
    {
        private readonly Dictionary<string, string> roles = new Dictionary<string, string>()
        {
            { "Исследователь", "user"},
            { "Администратор", "admin"},
        };
        [ObservableProperty]
        private ObservableCollection<User> researchers;
        [ObservableProperty]
        private ObservableCollection<User> admins;
        

        public UsersTabViewModel()
        {
            using Context ctx = new Context();
            Researchers = new ObservableCollection<User>(ctx.Users.Include(x => x.Role).Where(x => x.Role!.Name == "user").ToList());
            Admins = new ObservableCollection<User>(ctx.Users.Include(x => x.Role).Where(x => x.Role!.Name == "admin").ToList());
           
            WeakReferenceMessenger.Default.Register<NewUserMessage>(this);
        }
        [RelayCommand]
        private void AddUser()
        {
            AddUser add = new AddUser();
            add.ShowDialog();
        }
        [RelayCommand]
        private void DeleteUser(User user)
        {
            if (user.Role.Name == "admin")
            {
                var res = MessageBox.Show("Вы уверены, что хотите удалить данного админестратора", "Подтверждение", MessageBoxButton.YesNo);
                if (res != MessageBoxResult.Yes)
                    return;
            }
            else
            {
                var res = MessageBox.Show("Вы уверены, что хотите удалить данного исследователя", "Подтверждение", MessageBoxButton.YesNo);
                if (res != MessageBoxResult.Yes)
                    return;
            }
         
          
            using Context ctx = new Context();
            if (user is null)
            {
                MessageBox.Show("Внутернняя ошибка, повтроите попытку позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ctx.Users.Remove(user);
            ctx.SaveChanges();
            if (user.Role.Name == "admin")
            {
                admins.Remove(user);
            }
            else
            {
                Researchers.Remove(user);
            }
           
            
        }
      
        

        public void Receive(NewUserMessage message)
        {
            NewUser user = message.Value;
            string role = user.Role;
            string? dbRole = roles.GetValueOrDefault(role);
            if (dbRole is null)
            {
                MessageBox.Show("Внутернняя ошибка, повтроите попытку позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            using Context ctx = new Context();
            Role? getRole = ctx.Roles.First(x => x.Name == dbRole);
            if (getRole is null)
            {
                MessageBox.Show("Внутернняя ошибка, повтроите попытку позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            User addUser = new User() { Name = user.Name, Password = user.Password, Role = getRole };
            ctx.Users.Add(addUser);
            ctx.SaveChanges();
            if (dbRole == "admin")
            {
                Admins.Add(new User() { Id = addUser.Id, Name = addUser.Name, Password = addUser.Password });
            }
            if (dbRole == "user")
            {
                Researchers.Add(new User() { Id = addUser.Id, Name = addUser.Name, Password = addUser.Password });
            }
        }

      
    }
}
