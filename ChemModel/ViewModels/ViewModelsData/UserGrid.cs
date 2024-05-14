using System.ComponentModel.DataAnnotations;

namespace ChemModel.ViewModels
{
    public class UserGrid
    {
        public int Id { get; set; }
        [Required, Display(Name = "Имя"), ColumnName("Логин")]
        public string? Name { get; set; }
        [Required, Display(Name = "Пароль"), ColumnName("Пароль")]
        public string? Password { get; set; }
    }
}
