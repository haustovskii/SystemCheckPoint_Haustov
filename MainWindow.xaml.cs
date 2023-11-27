using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using SystemCheckPoint.AppData;

namespace SystemCheckPoint
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Конструктор окна рабочего пространства.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Обработчик события нажатия кнопки "Закрыть".
        /// Закрывает текущее окно.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ImgClose_MouseDown(object sender, RoutedEventArgs e) => Close();
        /// <summary>
        /// Обработчик события нажатия кнопки "Свернуть окно".
        /// Свертывает текущее окно в панель задач.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ImgPollUp_MouseDown(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        /// <summary>
        /// Обработчик события нажатия кнопки "Развернуть окно" и перетаскивания окна.
        /// Разворачивает/восстанавливает окно при двойном клике на верхней панели, а также позволяет перемещать окно при зажатой левой кнопке мыши на любой части окна.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
        /// <summary>
        /// Обработчик события нажатия кнопки "Вход".
        /// Проверяет введенные учетные данные и, в случае успешного входа, открывает новое окно рабочего пространства.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void BtnEnter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userDb = AppConnect.modelOdb.Employee.FirstOrDefault(x => x.Login == TbxLogin.Text && x.Password == PsbPassword.Password);
                if (userDb != null)
                {
                    MessageBox.Show($"Добро пожаловать {userDb.FirstName} {userDb.Patronumic}", "Успешный вход", MessageBoxButton.OK, MessageBoxImage.Information);
                    WorkWindow workWindow = new WorkWindow(userDb.ID);
                    workWindow.Show();
                    this.Close();
                }
                else
                    MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла критическая ошибка приложения" + ex.ToString(), "Критическая ошибка приложения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Обработчик события изменения размера окна.
        /// При изменении ширины окна подстраивает высоту, и наоборот, чтобы сохранить пропорции.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.WidthChanged)
            {
                Wnd.Height = e.NewSize.Width / (Wnd.MinWidth / Wnd.MinHeight);
            }
            else if (e.HeightChanged)
            {
                Wnd.Width = e.NewSize.Height * (Wnd.MinWidth / Wnd.MinHeight);
            }
        }
        /// <summary>
        /// Обработчик события нажатия кнопки "Развернуть/Свернуть окно".
        /// Переключает состояние окна между развернутым и свернутым.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ImgMaxMinSize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (WindowState != WindowState.Maximized)
            {
                ImgMax.Visibility = Visibility.Collapsed;
                ImgMin.Visibility = Visibility.Visible;
                WindowState = WindowState.Maximized;
            }
            else
            {
                ImgMax.Visibility = Visibility.Visible;
                ImgMin.Visibility = Visibility.Collapsed;
                WindowState = WindowState.Normal;
            }
        }
    }
}