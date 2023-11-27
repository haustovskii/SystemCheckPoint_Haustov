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
        /// <summary>
        /// Ролевой идентификатор пользователя, хранящийся в поле только для чтения.
        /// </summary>
        private readonly int userDb;
        /// <summary>
        /// Конструктор окна рабочего пространства.
        /// Инициализирует компоненты окна и устанавливает видимость меню в зависимости от роли пользователя.
        /// </summary>
        /// <param name="IDUser">Идентификатор пользователя.</param>
        public WorkWindow(int IDUser)
        {
            InitializeComponent();
            AppFrame.FrameMain = FrameMain;

            userDb = AppConnect.modelOdb.Employee.Where(x => x.ID == IDUser).Select(x => x.IDPost).FirstOrDefault();

            if (userDb == 1)
            {
                // Меню отчетов для администратора
                BrdReportMenu.Visibility = Visibility.Visible;
            }
            else
            {
                // Меню автотранспорта для охранника
                BrdReportMenu.Visibility = Visibility.Collapsed;
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
        /// <summary>
        /// Обработчик события загрузки окна.
        /// Устанавливает текст наименования текущей страницы в зависимости от роли пользователя.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (userDb == 1)
                TbxNamePage.Text = "Главное меню администратора";
            else
                TbxNamePage.Text = "Главное меню охранника";
        }
        /// <summary>
        /// Обработчик события нажатия кнопки "Закрыть окно".
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
        /// Обработчик события нажатия на область окна.
        /// Позволяет перемещать окно при зажатой левой кнопке мыши на любой части окна.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
        /// <summary>
        /// Обработчик события нажатия кнопки "Меню пропусков".
        /// Открывает соответствующую страницу в главном фрейме.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
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
        /// <summary>
        /// Обработчик события нажатия кнопки "Меню транспорта".
        /// Открывает соответствующую страницу в главном фрейме.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
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
        /// <summary>
        /// Обработчик события нажатия кнопки "Меню отчетов".
        /// Открывает соответствующую страницу в главном фрейме.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
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
        /// <summary>
        /// Метод сброса содержимого главного фрейма.
        /// Очищает содержимое и сбрасывает стили кнопок меню.
        /// </summary>
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
        /// <summary>
        /// Обработчик события нажатия кнопки "Выход".
        /// Возвращает к окну авторизации.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

    }
}
