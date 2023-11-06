using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using SystemCheckPoint.AppData;
using SystemCheckPoint.Page;
using SystemCheckPoint.Pages;

namespace SystemCheckPoint
{
    /// <summary>
    /// Логика взаимодействия для WorkWindow.xaml
    /// </summary>
    public partial class WorkWindow : Window
    {
        readonly int userDb;
        public WorkWindow(int IDUser)
        {
            InitializeComponent();
            AppFrame.FrameMain = FrameMain;
            //BtnReportMenu
            userDb = AppConnect.modelOdb.Employee.Where(x => x.ID == IDUser).Select(x => x.IDPost).FirstOrDefault();
            if (userDb == 1)
            {
                //Меню отчетов для администратора
                BrdReportMenu.Visibility = Visibility.Visible;
                BrdPassAuto.Visibility = Visibility.Collapsed;
            }
            else
            {
                //Меню автотранспорта для охранника
                BrdPassAuto.Visibility = Visibility.Visible;
                BrdReportMenu.Visibility = Visibility.Collapsed;
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (userDb == 1)
                TbxNamePage.Text = "Главное меню администратора";

            else
                TbxNamePage.Text = "Главное меню охранника";
        }
        private void ImgClose_MouseDown(object sender, RoutedEventArgs e) => Close();
        private void ImgPollUp_MouseDown(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        private void BtnPassMenu_Click(object sender, RoutedEventArgs e)
        {
            if (AppFrame.FrameMain.Content == null)
            {
                BrdPassMenuLine.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
                BrdPassMenu.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
                if (userDb == 1)
                    FrameMain.Navigate(new PageMenuPassAdmin());
                else
                    FrameMain.Navigate(new PageEditPass());
            }
            else if (BrdPassAuto.ForceCursor == true || BrdReportMenu.ForceCursor == true || AppFrame.FrameMain.Content != null)
            {
                AppFrame.FrameMain.Content = null;
                BrdPassMenuLine.Visibility = Visibility.Visible;
                BrdPassMenuLine.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A9D18E"));
                BrdPassMenu.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EAF4E4"));
            }
        }
        private void BtnPassAuto_Click(object sender, RoutedEventArgs e)
        {
            if (AppFrame.FrameMain.Content == null)
            {
                BrdPassAutoLine.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
                BrdPassAuto.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
                FrameMain.Navigate(new PageAutoSecurity());
            }
            else if (BrdPassMenu.ForceCursor == true || BrdReportMenu.ForceCursor == true || AppFrame.FrameMain.Content != null)
            {
                AppFrame.FrameMain.Content = null;
                BrdPassAutoLine.Visibility = Visibility.Visible;
                BrdPassAutoLine.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A9D18E"));
                BrdPassAuto.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EAF4E4"));
            }
        }
        private void BtnReportMenu_Click(object sender, RoutedEventArgs e)
        {
            if (AppFrame.FrameMain.Content == null)
            {
                BrdReportMenuLine.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
                BrdReportMenu.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
                FrameMain.Navigate(new PageReportAdmin());
            }
            else if (BrdPassMenu.ForceCursor == true || BrdPassAuto.ForceCursor == true || AppFrame.FrameMain.Content != null)
            {
                AppFrame.FrameMain.Content = null;
                BrdReportMenuLine.Visibility = Visibility.Visible;
                BrdReportMenuLine.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A9D18E"));
                BrdReportMenu.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EAF4E4"));
            }
        }
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void StyleFrame()
        {
            if (AppFrame.FrameMain.Content != null)
            {
                FrameMain.BorderBrush = Brushes.Transparent;
                FrameMain.BorderThickness = new Thickness(0);
            }
            else if (AppFrame.FrameMain.Content == null)
            {
                FrameMain.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A9D18E"));
                FrameMain.BorderThickness = new Thickness(1);
            }
        }
    }
}
