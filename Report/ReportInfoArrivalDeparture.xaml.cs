using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SystemCheckPoint.AppData;

namespace SystemCheckPoint.Report
{
    /// <summary>
    /// Логика взаимодействия для ReportInfoArrivalDeparture.xaml
    /// </summary>
    public partial class ReportInfoArrivalDeparture : Window
    {
        public ReportInfoArrivalDeparture(bool isEmployee)
        {
            InitializeComponent();
            TblTimeReport.Text = DateTime.Now.ToString();
            if (isEmployee)
            {
                DtgDataExternalPerson.Visibility = Visibility.Collapsed;

                var data = AppConnect.modelOdb.InfoArrivalDeparture.Where(x => x.IDEmployee != null).ToList();
                foreach (var item in data)
                {
                    // Предполагаем, что ArrivalTime и DepartureTime имеют тип данных DateTime
                    var arrivalTime = item.ArrivalTime.TimeOfDay;
                    var departureTime = item.DepartureTime.TimeOfDay;

                    // Вычисляем разницу времени
                    var totalTime = departureTime - arrivalTime;

                    // Присваиваем результат обратно свойству TotalTime
                    item.TotalTime = totalTime;
                }
                DtgDataEmployee.ItemsSource = data;

            }
            else
            {
                DtgDataEmployee.Visibility = Visibility.Collapsed;
                var data = AppConnect.modelOdb.InfoArrivalDeparture.Where(x => x.IDExternalPerson != null).ToList();
                foreach (var item in data)
                {
                    // Предполагаем, что ArrivalTime и DepartureTime имеют тип данных DateTime
                    var arrivalTime = item.ArrivalTime.TimeOfDay;
                    var departureTime = item.DepartureTime.TimeOfDay;

                    // Вычисляем разницу времени
                    var totalTime = departureTime - arrivalTime;

                    // Присваиваем результат обратно свойству TotalTime
                    item.TotalTime = totalTime;
                }
                DtgDataExternalPerson.ItemsSource = data;
            }
        }
        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                StpButton.Visibility = Visibility.Collapsed;
                printDialog.PrintVisual(GrdMain, "");
            }
            else
                MessageBox.Show("Печать прервана");
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DtgData_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}
