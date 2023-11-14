using System.Windows;
using System.Windows.Controls;

namespace SystemCheckPoint.Report
{
    /// <summary>
    /// Логика взаимодействия для ReportInfoArrivalDeparture.xaml
    /// </summary>
    public partial class ReportInfoArrivalDeparture : Window
    {
        public ReportInfoArrivalDeparture()
        {
            InitializeComponent();
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
    }
}
