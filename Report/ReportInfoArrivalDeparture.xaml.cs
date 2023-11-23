using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
                DtgData.ItemsSource = AppConnect.modelOdb.InfoArrivalDeparture.Where(x => x.IDEmployee != null).ToList();

            }
            else
            {
                DtgData.ItemsSource = AppConnect.modelOdb.InfoArrivalDeparture.Where(x => x.IDExternalPerson != null).ToList();
            }
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
