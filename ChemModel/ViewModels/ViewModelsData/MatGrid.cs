namespace ChemModel.ViewModels
{
    public class MatGrid
    {
        public int Id { get; set; }
        [ColumnName("Название материала")]
        public string Name { get; set; }
    }
}
