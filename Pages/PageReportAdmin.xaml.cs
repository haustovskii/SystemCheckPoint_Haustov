using System;
using System.Windows;
using SystemCheckPoint.AppData;
using System.Linq;
using System.Data.Entity;
using OfficeOpenXml; // Импорт пространства имен из библиотеки EPPlus
using OfficeOpenXml.Style;

namespace SystemCheckPoint.Page
{
    /// <summary>
    /// Логика взаимодействия для PageReportAdmin.xaml
    /// </summary>
    public partial class PageReportAdmin : System.Windows.Controls.Page
    {
        private static bool SelectEmployee = false;
        private static bool SelectPerson = false;
        public PageReportAdmin()
        {
            InitializeComponent();
        }        
        private void BtnBack_Click(object sender, RoutedEventArgs e)=> AppFrame.FrameMain.Content = null;
        private void ElpPozition1_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {            
            SelectEmployee = !SelectEmployee;
            SelectPerson = false;            
            UpdateVisibility();
        }
        private void ElpPozition2_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SelectPerson = !SelectPerson;
            SelectEmployee = false;
            UpdateVisibility();
        }
        private void UpdateVisibility()
        {
            ElpPozition1.Visibility = SelectEmployee ? Visibility.Visible : Visibility.Collapsed;
            ElpPozition2.Visibility = SelectPerson ? Visibility.Visible : Visibility.Collapsed;
        }
        private void BtnCreateReport_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show((CldDataReport.SelectedDate ?? DateTime.Now.Date).ToString());
            //получить дату из CldDataReport, сделать запрос из БД, создать файл excel
            //if (SelectEmployee)
            //{
            //    using (var context = new CheckPointDbEntities())
            //    {
            //        var query = from em in context.Employee
            //                    join i in context.InfoArrivalDeparture on em.ID equals i.IDEmployee
            //                    where DbFunctions.TruncateTime(i.ArrivalTime) == (DateTime)CldDataReport.SelectedDate
            //                    select new
            //                    {
            //                        EmployeeFullName = em.LastName + " " + em.FirstName.Substring(0, 1) + "." + em.Patronumic.Substring(0, 1) + ".",
            //                        i.ArrivalTime,
            //                        i.DepartureTime
            //                    };
            //        int rowCount = query.Count();
            //        var result = query.ToList();
            //        using (var package = new ExcelPackage())
            //        {
            //            //создаем рабочий лист
            //            var worksheet = package.Workbook.Worksheets.Add("EmployeeData");
            //            //задаем размеры колонкам
            //            worksheet.Column(1).Width = 30;
            //            worksheet.Column(235).Width = 30;
            //            worksheet.Column(70).Width = 30;
            //            worksheet.Column(70).Width = 30;
            //            worksheet.Column(70).Width = 30;
            //            //форматируем
            //            string rangeAddress = "A1:E" + rowCount;
            //            var range = worksheet.Cells[rangeAddress];
            //            range.Style.Border.BorderAround(ExcelBorderStyle.Thick);
            //            worksheet.Cells["A1"].Value = "Учет";
            //            worksheet.Cells["A1"].Style.Font.Size = 20;
            //            worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //            worksheet.Cells["A1:B1"].Merge = true;
            //            worksheet.Cells["A2"].Value = "прихода, ухода, время присутствия";
            //            worksheet.Cells["A2"].Style.Font.Size = 18;
            //            worksheet.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //            worksheet.Cells["A2:B2"].Merge = true;
            //            worksheet.Cells["A3"].Value = "сотрудников за "+ CldDataReport.SelectedDate ?? DateTime.Now.ToString();
            //            worksheet.Cells["A3"].Style.Font.Size = 20;
            //            worksheet.Cells["A3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //            worksheet.Cells["A3:B3"].Merge = true;                      







            //            worksheet.Cells["A1"].Style.Font.Size = 12; //размер шрифта
            //            worksheet.Cells["A1"].Value = "Значение в ячейке A1"; //значение в ячейках
            //            worksheet.Cells["A1:B1"].Merge = true; //объеденение
            //            var rang1e = worksheet.Cells["A1:B3"];
            //            range.Style.Border.BorderAround(ExcelBorderStyle.Thick); //толстые границы

            //        }
            //    }

            //}
            //else
            //{

            //}
        }


    }
}
