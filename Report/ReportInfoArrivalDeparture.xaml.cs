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
        /// <summary>
        /// Конструктор класса ReportInfoArrivalDeparture.
        /// Инициализирует компоненты страницы и заполняет данные о приходе и уходе.
        /// </summary>
        /// <param name="isEmployee">Флаг, указывающий на тип отчета (сотрудник или внешний сотрудник).</param>
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
                    var arrivalTime = item.ArrivalTime.TimeOfDay;
                    var departureTime = item.DepartureTime.TimeOfDay;
                    var totalTime = departureTime - arrivalTime;
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
                    var arrivalTime = item.ArrivalTime.TimeOfDay;
                    var departureTime = item.DepartureTime.TimeOfDay;
                    var totalTime = departureTime - arrivalTime;
                    item.TotalTime = totalTime;
                }

                DtgDataExternalPerson.ItemsSource = data;
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки мыши на гриде.
        /// Позволяет перемещать окно при зажатой левой кнопке мыши.
        /// </summary>
        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Печать".
        /// Открывает диалог печати и печатает содержимое окна.
        /// </summary>
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

        /// <summary>
        /// Обработчик события нажатия кнопки "Назад".
        /// Закрывает окно.
        /// </summary>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Обработчик события загрузки строки в DataGrid.
        /// Устанавливает номер строки в заголовке.
        /// </summary>
        private void DtgData_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

    }
}
