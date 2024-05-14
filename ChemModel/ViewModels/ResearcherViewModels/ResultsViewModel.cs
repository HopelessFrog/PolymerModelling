using ChemModel.Messeges;
using ChemModel.ViewModels.ViewModelsData;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Data;
using ChemModel.Data.DbTables;
using System.Reflection;
using System.IO;
using System.Data.OleDb;
using Microsoft.Win32;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Office.Interop.Excel;

namespace ChemModel.ViewModels
{
    public partial class ResultsViewModel : ObservableObject, IRecipient<DataMessage>, IRecipient<ResultDataMessage>, IRecipient<DataExcelMessage>
    {
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        [ObservableProperty]
        private DataExcel? excelData;
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private ObservableCollection<TableData>? data;
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        [ObservableProperty]
        private ResultData result = new ResultData();
        public ResultsViewModel() 
        {
            WeakReferenceMessenger.Default.Register<DataMessage>(this);
            WeakReferenceMessenger.Default.Register<ResultDataMessage>(this);
            WeakReferenceMessenger.Default.Register<DataExcelMessage>(this);
        }
        private bool CanSave() => Data is not null && Result is not null && ExcelData is not null;
        [RelayCommand(CanExecute = nameof(CanSave))]
        private void Save()
        {
            var data = ToDataTable(Data.ToList());
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "Файл";
            dlg.DefaultExt = ".xlsx";
            dlg.Filter = "Элктронная таблица (.xlsx)|*.xlsx";
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                try
                {
                    ToExcelFile(data, dlg.FileName);
                }
                catch
                {
                    MessageBox.Show("Невозможно сохранить этот файл, сначала закройте его", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                ExcelInfo(dlg.FileName);
                MessageBox.Show("Сохранение прошло успешно", "Сохранение завершено", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public static System.Data.DataTable ToDataTable<T>(List<T> items)
        {
            var dataTable = new System.Data.DataTable(typeof(T).Name);

            //Get all the properties
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in properties)
            {
                var att = prop.GetCustomAttribute(typeof(ColumnNameAttribute)) as ColumnNameAttribute;
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(att.Name, type);
            }
            foreach (var item in items)
            {
                var values = new object[properties.Length];
                for (var i = 0; i < properties.Length; i++)
                {
                    //inserting property values to data table rows
                    values[i] = properties[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check data table
            return dataTable;
        }

        public static void ToExcelFile(System.Data.DataTable dataTable, string filePath, bool overwriteFile = true)
        {
            try
            {
                if (File.Exists(filePath) && overwriteFile)
                    File.Delete(filePath);
            }
            catch
            {
                throw new Exception();
            }

            using (var connection = new OleDbConnection())
            {
                connection.ConnectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};" +
                                              "Extended Properties='Excel 12.0 Xml;HDR=YES;'";
                connection.Open();
                using (var command = new OleDbCommand())
                {
                    command.Connection = connection;
                    var columnNames = (from DataColumn dataColumn in dataTable.Columns select dataColumn.ColumnName).ToList();
                    var tableName = !string.IsNullOrWhiteSpace(dataTable.TableName) ? dataTable.TableName : Guid.NewGuid().ToString();
                    command.CommandText = $"CREATE TABLE [{tableName}] ({string.Join(",", columnNames.Select(c => $"[{c}] VARCHAR").ToArray())});";
                    command.ExecuteNonQuery();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        var rowValues = (from DataColumn column in dataTable.Columns select (row[column] != null && row[column] != DBNull.Value) ? row[column].ToString() : string.Empty).ToList();
                        command.CommandText = $"INSERT INTO [{tableName}]({string.Join(",", columnNames.Select(c => $"[{c}]"))}) VALUES ({string.Join(",", rowValues.Select(r => $"'{r}'").ToArray())});";
                        command.ExecuteNonQuery();
                    }
                }
                connection.Close();
            }
            
        }
        public void ExcelInfo(string fileName)
        {
            int row = 1;
            Excel.Application excelAppObj = new Excel.Application();
            excelAppObj.DisplayAlerts = false;
            Excel.Workbook workBook = excelAppObj.Workbooks.Open(fileName, 0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, false, false);
            Excel.Worksheet worksheet = (Excel.Worksheet)workBook.Worksheets.get_Item(1);
            Excel.Range rng = worksheet.Cells[row, 4] as Excel.Range;
            rng.Font.Bold = true;
            rng = worksheet.Cells[row, 1] as Excel.Range;
            rng.Font.Bold = true;
            rng = worksheet.Cells[row, 2] as Excel.Range;
            rng.Font.Bold = true;
            worksheet.Cells[row, 4] = "Исходные данные";
            rng = worksheet.Cells[row, 3] as Excel.Range;
            rng.Font.Bold = true;
            row++;
            worksheet.Cells[row, 4] = "Тип материала";
            worksheet.Cells[row, 5] = ExcelData!.Material.Name;
            row++;
            worksheet.Cells[row, 4] = "Геометрические параметры канала:";
            rng = worksheet.Cells[row, 4] as Excel.Range;
            rng.Font.Bold = true;
            row++;
            worksheet.Cells[row, 4] = "Ширина, м";
            worksheet.Cells[row, 5] = ExcelData!.Width.ToString(System.Globalization.CultureInfo.InvariantCulture);
            row++;
            worksheet.Cells[row, 4] = "Глубина, м";
            worksheet.Cells[row, 5] = ExcelData!.Height.ToString(System.Globalization.CultureInfo.InvariantCulture);
            row++;
            worksheet.Cells[row, 4] = "Длина, м";
            worksheet.Cells[row, 5] = ExcelData!.Length.ToString(System.Globalization.CultureInfo.InvariantCulture);
            row++;
            worksheet.Cells[row, 4] = "Режимные параметры процесса:";
            rng = worksheet.Cells[row, 4] as Excel.Range;
            rng.Font.Bold = true;
            row++;
            worksheet.Cells[row, 4] = "Скорость крышки, м/с";
            worksheet.Cells[row, 5] = ExcelData!.Velocity.ToString(System.Globalization.CultureInfo.InvariantCulture);
            row++;
            worksheet.Cells[row, 4] = "Температура крышки, °С";
            worksheet.Cells[row, 5] = ExcelData!.TempCr.ToString(System.Globalization.CultureInfo.InvariantCulture);
            row++;
            worksheet.Cells[row, 4] = "Параметры метода решения уравнений модели:";
            rng = worksheet.Cells[row, 4] as Excel.Range;
            rng.Font.Bold = true;
            row++;
            worksheet.Cells[row, 4] = "Шаг расчета по длине канала, м";
            worksheet.Cells[row, 5] = ExcelData!.Step.ToString(System.Globalization.CultureInfo.InvariantCulture);
            row++;
            worksheet.Cells[row, 4] = "Параметры свойств материала:";
            rng = worksheet.Cells[row, 4] as Excel.Range;
            rng.Font.Bold = true;
            row++;
            foreach (var prop in ExcelData.Properties)
            {
                worksheet.Cells[row, 4] = prop.Property.Name + ", " + prop.Property.Units.Name;
                worksheet.Cells[row, 5] = prop.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
                row++;
            }
            worksheet.Cells[row, 4] = "Эмпирические коэффициента математической модели:";
            rng = worksheet.Cells[row, 4] as Excel.Range;
            rng.Font.Bold = true;
            row++;
            foreach (var prop in ExcelData.Coefs)
            {
                worksheet.Cells[row, 4] = prop.Property.Name + ", " + prop.Property.Units.Name;
                worksheet.Cells[row, 5] = prop.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
                row++;
            }
            worksheet.Cells[row, 4] = "Критериальные показатели процесса:";
            rng = worksheet.Cells[row, 4] as Excel.Range;
            rng.Font.Bold = true;
            row++;
            worksheet.Cells[row, 4] = "Производительность, кг/ч";
            worksheet.Cells[row, 5] = Result.Performance.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
            row++;
            worksheet.Cells[row, 4] = "Температура продукта, °С";
            worksheet.Cells[row, 5] = Result.Temperature.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
            row++;
            worksheet.Cells[row, 4] = "Вязкость продукта, Па*с";
            worksheet.Cells[row, 5] = Result.Viscosity.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);

            workBook.SaveAs(fileName, Excel.XlFileFormat.xlOpenXMLWorkbook, null, null, false,
            false, Excel.XlSaveAsAccessMode.xlShared, false, false, null, null, null);
            workBook.Close();
        }
        public void Receive(DataMessage message)
        {
            Data = new ObservableCollection<TableData>(message.Value.Select(x => new TableData() { 
                Temp = Math.Round(x.Temp, 2),
                Vaz = Math.Round(x.Vaz, 2),
                Coord = x.Coord,
            }));
        }

        public void Receive(ResultDataMessage message)
        {
            Result = message.Value;
        }

        public void Receive(DataExcelMessage message)
        {
            ExcelData = message.Value;
        }
    }
}
