using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using SystemCheckPoint.AppData;

namespace SystemCheckPoint.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageAutoSecurity.xaml
    /// </summary>
    public partial class PageAutoSecurity : System.Windows.Controls.Page
    {
        private int IDAuto = 0;
        private static bool isChecked = false;

        /// <summary>
        /// Конструктор страницы автотранспорта для охранника.
        /// </summary>
        /// <param name="IDUserRole">Идентификатор роли пользователя.</param>
        public PageAutoSecurity(int IDUserRole)
        {
            InitializeComponent();

            if (IDUserRole != 1)
            {
                BtnCheckAutoOrg.Visibility = Visibility.Collapsed;
                BtnSave.Visibility = Visibility.Collapsed;
                TbxIDPassAuto.IsReadOnly = true;
                TbxIDPassAuto.IsReadOnly = true;
                TbxMark.IsReadOnly = true;
                TbxStateNumber.IsReadOnly = true;
                TbxSeriesNumber.IsReadOnly = true;
                TbxColor.IsReadOnly = true;
            }

            LsvAuto.ItemsSource = AppConnect.modelOdb.AutoTransport.ToArray();
        }
        /// <summary>
        /// Обработчик события нажатия кнопки "Выход".
        /// Закрывает текущую страницу и возвращает на главную.
        /// </summary>
        private void BtnExit_Click(object sender, RoutedEventArgs e) => AppFrame.FrameMain.Content = null;
        /// <summary>
        /// Обработчик события нажатия кнопки "Сохранить".
        /// Сохраняет данные об автотранспорте.
        /// </summary>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!AreAutoDataChanged())
                {
                    if (!string.IsNullOrWhiteSpace(TbxMark.Text) &&
                        !string.IsNullOrWhiteSpace(TbxStateNumber.Text) && !string.IsNullOrWhiteSpace(TbxSeriesNumber.Text) &&
                        !string.IsNullOrWhiteSpace(TbxColor.Text))
                    {
                        IDAuto = 0;
                        Pass pass = new Pass
                        {
                            IDTypePass = 4,
                            DateOfFormation = DateTime.Now
                        };

                        AppConnect.modelOdb.Pass.Add(pass);
                        AppConnect.modelOdb.SaveChanges();

                        AutoTransport autoTransport = new AutoTransport
                        {
                            IDPass = AppConnect.modelOdb.Pass.Max(x => x.ID),
                            Mark = TbxMark.Text,
                            StateNumber = TbxStateNumber.Text,
                            SeriesNumber = TbxSeriesNumber.Text,
                            Color = TbxColor.Text
                        };

                        AppConnect.modelOdb.AutoTransport.Add(autoTransport);
                        AppConnect.modelOdb.SaveChanges();

                        MessageBox.Show("Данные успешно сохранены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        LsvAuto.ItemsSource = AppConnect.modelOdb.AutoTransport.ToArray();

                        DispatcherTimer timer = new DispatcherTimer();
                        timer.Tick += (s, _e) =>
                        {
                            TbxIDPassAuto.Text = string.Empty;
                            TbxMark.Text = string.Empty;
                            TbxStateNumber.Text = string.Empty;
                            TbxSeriesNumber.Text = string.Empty;
                            TbxColor.Text = string.Empty;
                            timer.Stop();
                        };

                        timer.Interval = TimeSpan.FromSeconds(3);
                        timer.Start();
                    }
                    else
                    {
                        MessageBox.Show("Данные не были введены", "Ошибка при добавлении данных", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    var auto = AppConnect.modelOdb.AutoTransport.FirstOrDefault(x => x.ID == IDAuto);
                    if (auto != null)
                    {
                        auto.IDPass = int.Parse(TbxIDPassAuto.Text);
                        auto.Mark = TbxMark.Text;
                        auto.StateNumber = TbxStateNumber.Text;
                        auto.SeriesNumber = TbxSeriesNumber.Text;
                        auto.Color = TbxColor.Text;
                    }

                    AppConnect.modelOdb.SaveChanges();
                    LsvAuto.ItemsSource = AppConnect.modelOdb.AutoTransport.ToArray();

                    MessageBox.Show("Данные успешно изменены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла критическая ошибка приложения" + ex.ToString(), "Критическая ошибка приложения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Обработчик изменения текста в поле поиска автотранспорта.
        /// Фильтрует список автотранспорта по введенным данным.
        /// </summary>
        private void TbxFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            LsvAuto.ItemsSource = AppConnect.modelOdb.AutoTransport.Where(x =>
                                  x.ID.ToString().Contains(TbxFind.Text) ||
                                  x.Mark.Contains(TbxFind.Text) ||
                                  x.StateNumber.Contains(TbxFind.Text) ||
                                  x.SeriesNumber.ToString().Contains(TbxFind.Text) ||
                                  x.Color.ToString().Contains(TbxFind.Text) ||
                                  x.IDPass.ToString().Contains(TbxFind.Text)
                                  ).ToList();
        }
        /// <summary>
        /// Обработчик события изменения выбранного элемента в списке автотранспорта.
        /// Отображает данные выбранного автотранспорта в соответствующих полях.
        /// </summary>
        private void LsvAuto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LsvAuto.SelectedItem != null)
            {
                StpPanelData.Visibility = Visibility.Visible;
                AutoTransport auto = (AutoTransport)LsvAuto.SelectedItem;
                IDAuto = auto.ID;
                var selectedAuto = AppConnect.modelOdb.AutoTransport.FirstOrDefault(x => x.ID == IDAuto);

                if (selectedAuto != null)
                {
                    TbxIDPassAuto.Text = selectedAuto.IDPass.ToString();
                    TbxMark.Text = selectedAuto.Mark;
                    TbxStateNumber.Text = selectedAuto.StateNumber;
                    TbxSeriesNumber.Text = selectedAuto.SeriesNumber;
                    TbxColor.Text = selectedAuto.Color;
                }
            }
        }
        /// <summary>
        /// Проверяет, были ли изменены данные об автотранспорте.
        /// </summary>
        /// <returns>True, если данные были изменены; в противном случае - false.</returns>
        private bool AreAutoDataChanged()
        {
            if (LsvAuto.SelectedItem != null && IDAuto != 0)
            {
                AutoTransport auto = (AutoTransport)LsvAuto.SelectedItem;
                var selectedAuto = AppConnect.modelOdb.AutoTransport.FirstOrDefault(x => x.ID == IDAuto);

                if (selectedAuto != null)
                {
                    // Сравнение значений текущих данных с исходными данными
                    return TbxMark.Text != selectedAuto.Mark
                           || TbxStateNumber.Text != selectedAuto.StateNumber
                           || TbxSeriesNumber.Text != selectedAuto.SeriesNumber
                           || TbxColor.Text != selectedAuto.Color;
                }
            }
            return false;
        }
        /// <summary>
        /// Обработчик события ввода текста в поле с фамилией, именем, отчеством.
        /// Запрещает ввод цифр и символов, отличных от букв.
        /// </summary>
        private void Text_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-Яa-zA-Z]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        /// <summary>
        /// Обработчик события ввода текста в поле с государственным номером.
        /// Запрещает ввод символов, отличных от букв и цифр.
        /// </summary>
        private void StateNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-я0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        /// <summary>
        /// Обработчик события ввода текста в поле с числом.
        /// Запрещает ввод символов, отличных от цифр.
        /// </summary>
        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        /// <summary>
        /// Обработчик события ввода текста в поле с серийным номером.
        /// Запрещает ввод символов, отличных от заглавных букв и цифр.
        /// </summary>
        private void TbxSeriesNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-Z0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        /// <summary>
        /// Обработчик события нажатия кнопки "Выбрать организацию".
        /// Переключает режим отображения данных об автотранспорте.
        /// </summary>
        private void BtnCheckAutoOrg_Click(object sender, RoutedEventArgs e)
        {
            isChecked = !isChecked;
            if (isChecked)
            {
                BtnCheckAutoOrg.Content = new Image() { Source = new BitmapImage(new Uri("/Images/check.png", UriKind.Relative)) };
                BrdSelectPerson.Visibility = Visibility.Collapsed;
                StpPanelData.Visibility = Visibility.Visible;
            }
            else
            {
                BtnCheckAutoOrg.Content = null;
                BrdSelectPerson.Visibility = Visibility.Visible;
                StpPanelData.Visibility = Visibility.Collapsed;
            }
            TbxIDPassAuto.Text = string.Empty;
            TbxMark.Text = string.Empty;
            TbxStateNumber.Text = string.Empty;
            TbxSeriesNumber.Text = string.Empty;
            TbxColor.Text = string.Empty;
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Удалить".
        /// Удаляет выбранный автотранспорт из базы данных.
        /// </summary>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (LsvAuto.SelectedItem != null)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Удалить эту строку?", "Удаление данных", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    AutoTransport auto = (AutoTransport)LsvAuto.SelectedItem;
                    var autoValue = AppConnect.modelOdb.AccountingMaterialValue.FirstOrDefault(x => x.IDAutoTransport == auto.ID);
                    if (autoValue != null)
                    {
                        MessageBox.Show("Данные нельзя удалить, при наличии материального пропуска на авто!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    AppConnect.modelOdb.AutoTransport.Remove(AppConnect.modelOdb.AutoTransport.FirstOrDefault(x => x.ID == auto.ID));
                    AppConnect.modelOdb.SaveChanges();
                    LsvAuto.ItemsSource = AppConnect.modelOdb.AutoTransport.ToArray();
                    TbxIDPassAuto.Text = string.Empty;
                    TbxMark.Text = string.Empty;
                    TbxStateNumber.Text = string.Empty;
                    TbxSeriesNumber.Text = string.Empty;
                    TbxColor.Text = string.Empty;
                    MessageBox.Show("Данные успешно удалены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}