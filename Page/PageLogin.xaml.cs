using System;
using System.Linq;
using System.Windows;
using SystemCheckPoint.AppData;

namespace SystemCheckPoint.Page
{
    /// <summary>
    /// Логика взаимодействия для PageLogin.xaml
    /// </summary>
    public partial class PageLogin : System.Windows.Controls.Page
    {
        public PageLogin()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Метод для авторизации пользователя в систему
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEnter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userDb = AppConnect.modelOdb.Employee.FirstOrDefault(x => x.Login == TbxLogin.Text && x.Password == PsbPassword.Password);
                if (userDb != null)
                {
                    switch (userDb.IDPost)
                    {
                        case 1:
                            MessageBox.Show($"Добро пожаловать {userDb.FirstName} {userDb.Patronumic}", "Успешный вход", MessageBoxButton.OK, MessageBoxImage.Information);
                            AppFrame.FrameMain.Navigate(new PageMenuSecurity()); break; //Администратор
                        case 2:
                            MessageBox.Show($"Добро пожаловать {userDb.FirstName} {userDb.Patronumic}", "Успешный вход", MessageBoxButton.OK, MessageBoxImage.Information);
                            AppFrame.FrameMain.Navigate(new PageMenuAdmin()); break; //Охранник
                        default: MessageBox.Show("Авторизация возможна только для сотрудников КПП и Администрации"); break;
                    }
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
