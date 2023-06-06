using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using SystemCheckPoint.AppData;
using SystemCheckPoint.Page;

namespace SystemCheckPoint
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AppFrame.FrameMain = FrameMain;
            FrameMain.Navigate(new PageLogin());
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e) => Close();
        private void BtnPollUp_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void BtnEnter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userDb = AppConnect.modelOdb.Employee.FirstOrDefault(x => x.Login == TbxLogin.Text && x.Password == PsbPassword.Password);
                if (userDb != null)
                {
                    MessageBox.Show($"Добро пожаловать {userDb.FirstName} {userDb.Patronumic}", "Успешный вход", MessageBoxButton.OK, MessageBoxImage.Information);
                    FrameMain.Visibility = Visibility.Visible;
                    //AppFrame.FrameMain.Navigate(new PageMenuUser(userDb.IDPost));
                }
                else
                    MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла критическая ошибка приложения" + ex.ToString(), "Критическая ошибка приложения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}