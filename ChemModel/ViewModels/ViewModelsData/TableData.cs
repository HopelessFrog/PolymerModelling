namespace ChemModel.ViewModels
{
    public class TableData
    {
        [ColumnName("Координата по длине канала, м")]
        public double Coord { get; set; }
        [ColumnName("Температура, °С")]
        public double Temp { get; set; }
        [ColumnName("Вязкость материала, Па*с")]
        public double Vaz { get; set; }
    }
}
