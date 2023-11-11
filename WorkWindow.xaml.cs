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
            }
            else
            {
                //Меню автотранспорта для охранника
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
                this.DragMove();
        }
        private void BtnPassMenu_Click(object sender, RoutedEventArgs e)
        {
            if (AppFrame.FrameMain.Content == null)
            {
                BrdPassMenu.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
                BrdPassMenu.BorderThickness = new Thickness(1, 1, 1, 0);

                if (userDb == 1)
                    FrameMain.Navigate(new PageMenuPassAdmin());
                else
                    FrameMain.Navigate(new PageEditPass());
            }
            else
                ContentFrame();
        }
        private void BtnPassAuto_Click(object sender, RoutedEventArgs e)
        {
            if (AppFrame.FrameMain.Content == null)
            {
                BrdPassAuto.BorderThickness = new Thickness(1, 1, 1, 0);
                BrdPassAuto.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));

                FrameMain.Navigate(new PageAutoSecurity(userDb));
            }
            else
                ContentFrame();
        }
        private void BtnReportMenu_Click(object sender, RoutedEventArgs e)
        {
            if (AppFrame.FrameMain.Content == null)
            {
                BrdReportMenu.BorderThickness = new Thickness(1, 1, 1, 0);
                BrdReportMenu.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
                FrameMain.Navigate(new PageReportAdmin());
            }
            else
                ContentFrame();
        }
        private void ContentFrame()
        {
            AppFrame.FrameMain.Content = null;
            BrdPassMenu.BorderThickness = new Thickness(1);
            BrdPassMenu.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EAF4E4"));

            BrdPassAuto.BorderThickness = new Thickness(1);
            BrdPassAuto.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EAF4E4"));

            BrdReportMenu.BorderThickness = new Thickness(1);
            BrdReportMenu.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EAF4E4"));
        }
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

    }
}
