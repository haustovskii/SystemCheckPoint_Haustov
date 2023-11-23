using System;
using System.Windows;
using SystemCheckPoint.AppData;
using System.Linq;
using System.Data.Entity;
using OfficeOpenXml; // Импорт пространства имен из библиотеки EPPlus
using OfficeOpenXml.Style;
using SystemCheckPoint.Report;

namespace SystemCheckPoint.Page
{
    /// <summary>
    /// Логика взаимодействия для PageReportAdmin.xaml
    /// </summary>
    public partial class PageReportAdmin : System.Windows.Controls.Page
    {
        public PageReportAdmin()
        {
            InitializeComponent();
        }        
        private void BtnBack_Click(object sender, RoutedEventArgs e)=> AppFrame.FrameMain.Content = null;
        private bool SelectEmployee = false;
        private bool SelectPerson = false;
        private bool isEmployee = false;
        private void ElpPozition1_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SelectEmployee = !SelectEmployee;
            SelectPerson = false;
            isEmployee = SelectEmployee;
            UpdateVisibility();
        }

        private void ElpPozition2_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SelectPerson = !SelectPerson;
            SelectEmployee = false;
            isEmployee = SelectEmployee;
            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            ElpPozition1.Visibility = SelectEmployee ? Visibility.Visible : Visibility.Collapsed;
            ElpPozition2.Visibility = SelectPerson ? Visibility.Visible : Visibility.Collapsed;
        }

        // Теперь переменная isEmployee будет содержать состояние выбора ElpPozition1

        private void BtnCreateReport_Click(object sender, RoutedEventArgs e)
        {
            if(isEmployee)
            {
                ReportInfoArrivalDeparture report = new ReportInfoArrivalDeparture(true);
                report.Show();
            }
            else
            {
                ReportInfoArrivalDeparture report = new ReportInfoArrivalDeparture(false);
                report.Show();
            }
        }


    }
}
