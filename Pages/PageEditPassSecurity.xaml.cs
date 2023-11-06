using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SystemCheckPoint.AppData;
using SystemCheckPoint.Report;

namespace SystemCheckPoint.Page
{
    /// <summary>
    /// Логика взаимодействия для PageEditPass.xaml
    /// </summary>
    public partial class PageEditPass : System.Windows.Controls.Page
    {
        public PageEditPass()
        {
            InitializeComponent();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.FrameMain.Content = null;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private bool isClicked = false;
        private void ElpPozition_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!isClicked)
            {
                // Первый клик
                ElpPozition.Margin = new Thickness(25, 1, 0, 1);
                StpAccMaterialValue.Visibility = Visibility.Visible;
                StpAddTempPass.Visibility = Visibility.Collapsed;
                BrdTumb.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EAF4E4"));
            }
            else
            {
                // Второй клик
                ElpPozition.Margin = new Thickness(0, 1, 25, 1);
                StpAccMaterialValue.Visibility = Visibility.Collapsed;
                StpAddTempPass.Visibility = Visibility.Visible;
                BrdTumb.Background = Brushes.White;
            }

            // Инвертируем состояние клика
            isClicked = !isClicked;
        }

        private void BtnSelectPerson_Click(object sender, RoutedEventArgs e)
        {
            if (BrdSelectPerson.Visibility == Visibility.Collapsed)
                BrdSelectPerson.Visibility = Visibility.Visible;
            else
                BrdSelectPerson.Visibility = Visibility.Collapsed;
        }
        private void BtnSavePass_Click(object sender, RoutedEventArgs e)
        {
            ReportPassExternalPerson reportPassExternalPerson = new ReportPassExternalPerson(1,1);
            reportPassExternalPerson.Show();
        }
    }
}
