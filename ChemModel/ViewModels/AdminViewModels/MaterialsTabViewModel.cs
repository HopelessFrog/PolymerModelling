using ChemModel.Data;
using ChemModel.Data.DbTables;
using ChemModel.Messeges;
using ChemModel.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using OpenTK.Compute.OpenCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;

namespace ChemModel.ViewModels
{

    public partial class MaterialsTabViewModel : ObservableObject, IRecipient<MaterialMessage>, IRecipient<UserMessage>
    {
        private User? user;
        [ObservableProperty]
        private ObservableCollection<MatGrid> mats;
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteMatCommand))]
        private MatGrid? selectedMat;
        [ObservableProperty]
        private ObservableCollection<MaterialPropertyBind>? properties;
        [ObservableProperty]
        private ObservableCollection<MaterialEmpiricBind>? mathProps;
        public MaterialsTabViewModel()
        {
            using Context ctx = new Context();
            Mats = new ObservableCollection<MatGrid>(ctx.Materials.Select(x => new MatGrid() { Id = x.Id, Name = x.Name }).ToList());
            if (Mats.Any())
            {
                SelectedMat = Mats[0];
                Properties = new ObservableCollection<MaterialPropertyBind>(ctx.MaterialPropertyBinds.Where(x => x.MaterialId == SelectedMat.Id).Include(x => x.Property).Include(x => x.Property.Units).ToList());
                MathProps = new ObservableCollection<MaterialEmpiricBind>(ctx.MaterialEmpiricBinds.Where(x => x.MaterialId == SelectedMat.Id).Include(x => x.Property).Include(x => x.Property.Units).ToList());
            }
            WeakReferenceMessenger.Default.Register<MaterialMessage>(this);
            WeakReferenceMessenger.Default.Register<UserMessage>(this);
        }

        private bool CanDeleteMat()
        {
            return SelectedMat is not null;
        }
        [RelayCommand(CanExecute = nameof(CanDeleteMat))]
        private void DeleteMat()
        {
            if (!Mats.Any())
                return;
            using Context ctx = new Context();
            ctx.Materials.Remove(ctx.Materials.Find(SelectedMat!.Id)!);
            Mats.Remove(SelectedMat!);
            ctx.SaveChanges();
            SelectedMat = null;
            Properties = null;
            MathProps = null;
        }
        [RelayCommand]
        private void AddMat()
        {
            new AddMaterialWindow().ShowDialog();
        }
        public void PropChange()
        {
            if (SelectedMat is null)
                return;
            using Context ctx = new Context();
            Properties = new ObservableCollection<MaterialPropertyBind>(ctx.MaterialPropertyBinds.Where(x => x.MaterialId == SelectedMat.Id).Include(x => x.Property).Include(x => x.Property.Units).ToList());
            MathProps = new ObservableCollection<MaterialEmpiricBind>(ctx.MaterialEmpiricBinds.Where(x => x.MaterialId == SelectedMat.Id).Include(x => x.Property).Include(x => x.Property.Units).ToList());
        }
        [RelayCommand]
        private void SaveChanges()
        {
            if (Properties is null || !Properties.Any() || MathProps is null || !MathProps.Any())
            {
                return;
            }
            using Context ctx = new Context();
            ctx.MaterialPropertyBinds.UpdateRange(Properties);
            ctx.MaterialEmpiricBinds.UpdateRange(MathProps);
            ctx.SaveChanges();
            MessageBox.Show("Сохранение прошло успешно", "Сохранение успешно", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public void Receive(MaterialMessage message)
        {
            if (message.Value is null)
                return;
            Material material = new Material()
            {
                Name = message.Value.Name,
            };
            using Context ctx = new Context();
            ctx.Materials.Add(material);
            ctx.SaveChanges();
            var props = ctx.Properties.ToList();
            foreach (var prop in props)
            {
                ctx.MaterialPropertyBinds.Add(new MaterialPropertyBind()
                {
                    Material = material,
                    MaterialId = material.Id,
                    Property = prop,
                    PropertyId = prop.Id,
                    Value = 0,
                });
            }
            var mathModProps = ctx.EmpiricCoefficients.ToList();
            foreach (var prop in mathModProps)
            {
                ctx.MaterialEmpiricBinds.Add(new MaterialEmpiricBind()
                {
                    Material = material,
                    MaterialId = material.Id,
                    Property = prop,
                    PropertyId = prop.Id,
                    Value = 0,
                });
            }
            ctx.SaveChanges();
            if (user is not null)
            {
                user = ctx.Users.Find(user.Id);
                ctx.UserAddMaterials.Add(new UserAddMaterial()
                {
                    Material = material,
                    MaterialId = material.Id,
                    User = user,
                    UserId = user.Id,
                });
                ctx.SaveChanges();
            }
            Mats.Add(new MatGrid() { Id = material.Id, Name = material.Name });
        }

        public void Receive(UserMessage message)
        {
            user = message.Value;
        }
    }
}
