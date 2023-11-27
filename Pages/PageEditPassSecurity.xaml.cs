using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using SystemCheckPoint.AppData;
using SystemCheckPoint.Report;

namespace SystemCheckPoint.Page
{
    /// <summary>
    /// Логика взаимодействия для PageEditPass.xaml
    /// </summary>
    public partial class PageEditPass : System.Windows.Controls.Page
    {
        /// <summary>
        /// Идентификатор стороннего лица.
        /// </summary>
        private int IDExteral;

        /// <summary>
        /// Флаг, указывающий, был ли выполнен клик.
        /// </summary>
        private bool isClicked = false;

        /// <summary>
        /// Конструктор страницы редактирования пропуска.
        /// </summary>
        public PageEditPass()
        {
            AppConnect.modelOdb = new CheckPointDbEntities1();
            InitializeComponent();
            LsvExternalPerson.ItemsSource = AppConnect.modelOdb.ExternalPerson.ToArray();
        }

        /// <summary>
        /// Обработчик события предварительного ввода текста в поле с фамилией, именем, отчеством.
        /// Запрещает ввод цифр и символов, отличных от букв.
        /// </summary>
        private void Text_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-Яa-zA-Z]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Обработчик события предварительного ввода текста в поле с числом.
        /// Запрещает ввод символов, отличных от цифр.
        /// </summary>
        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Выход".
        /// Закрывает текущую страницу.
        /// </summary>
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.FrameMain.Content = null;
        }

        /// <summary>
        /// Обработчик события нажатия на круглый элемент, изменяющий позицию.
        /// Переключает видимость элементов на странице в зависимости от флага isClicked.
        /// </summary>
        private void ElpPozition_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!isClicked)
            {
                ElpPozition.Margin = new Thickness(25, 1, 0, 1);
                StpAccMaterialValue.Visibility = Visibility.Visible;
                StpAddTempPass.Visibility = Visibility.Collapsed;
                BrdTumb.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EAF4E4"));
            }
            else
            {
                ElpPozition.Margin = new Thickness(0, 1, 25, 1);
                StpAccMaterialValue.Visibility = Visibility.Collapsed;
                StpAddTempPass.Visibility = Visibility.Visible;
                BrdTumb.Background = Brushes.White;
            }
            isClicked = !isClicked;
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Выбрать персону".
        /// Переключает видимость блока выбора персоны.
        /// </summary>
        private void BtnSelectPerson_Click(object sender, RoutedEventArgs e)
        {
            if (BrdSelectPerson.Visibility == Visibility.Collapsed)
                BrdSelectPerson.Visibility = Visibility.Visible;
            else
                BrdSelectPerson.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Проверяет, были ли изменены поля стороннего лица с указанным идентификатором.
        /// </summary>
        /// <param name="id">Идентификатор стороннего лица.</param>
        /// <returns>True, если поля были изменены; в противном случае - false.</returns>
        private bool AreFieldsChanged(int id)
        {
            var externalDb = AppConnect.modelOdb.ExternalPerson.FirstOrDefault(x => x.ID == id);
            if (externalDb != null)
            {
                return externalDb.LastName != TbxLastName.Text ||
                       externalDb.FirstName != TbxName.Text ||
                       externalDb.Patronumic != TbxPatronymic.Text ||
                       externalDb.NumberPassport != int.Parse(TbxNumberPass.Text) ||
                       externalDb.SeriesPassport != int.Parse(TbxSerriesPass.Text) ||
                       externalDb.Birthday != DtpBirthday.SelectedDate;
            }
            return false;
        }

        /// <summary>
        /// Обработчик события изменения выбранной персоны в списке сторонних лиц.
        /// Заполняет соответствующие поля данными о выбранной персоне.
        /// </summary>
        private void LsvExternalPerson_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LsvExternalPerson.SelectedItem != null)
            {
                ExternalPerson external = (ExternalPerson)LsvExternalPerson.SelectedItem;
                IDExteral = external.ID;
                var ExternalDb = AppConnect.modelOdb.ExternalPerson.FirstOrDefault(x => x.ID == IDExteral);

                if (ExternalDb != null)
                {
                    TbxIDPass.Text = ExternalDb.IDPass.ToString();
                    TbxLastName.Text = ExternalDb.LastName;
                    TbxName.Text = ExternalDb.FirstName;
                    TbxPatronymic.Text = ExternalDb.Patronumic;
                    TbxNumberPass.Text = ExternalDb.NumberPassport.ToString();
                    TbxSerriesPass.Text = ExternalDb.SeriesPassport.ToString();
                    DtpBirthday.SelectedDate = ExternalDb.Birthday;
                }
            }
            BrdSelectPerson.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Обработчик события изменения текста в поле выбора персоны.
        /// Обновляет список сторонних лиц в соответствии с введенным текстом.
        /// </summary>
        private void TbxSelectPerson_TextChanged(object sender, TextChangedEventArgs e)
        {
            LsvExternalPerson.ItemsSource = AppConnect.modelOdb.ExternalPerson.Where(x =>
                x.ID.ToString().Contains(TbxSelectPerson.Text) ||
                x.LastName.Contains(TbxSelectPerson.Text) ||
                x.FirstName.Contains(TbxSelectPerson.Text) ||
                x.Patronumic.Contains(TbxSelectPerson.Text)).ToArray();
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Сохранить пропуск".
        /// Сохраняет данные о стороннем лице и формирует пропуск.
        /// </summary>
        private void BtnSavePass_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int IDPass = 0;
                int IDExtPerson = 0;

                // Проверяем, были ли изменены поля стороннего лица
                if (AreFieldsChanged(IDExteral) == false)
                    goto linkPrint;

                // Проверяем, все ли обязательные поля заполнены
                if (!string.IsNullOrWhiteSpace(TbxLastName.Text) && !string.IsNullOrWhiteSpace(TbxName.Text) &&
                    !string.IsNullOrWhiteSpace(TbxPatronymic.Text) && !string.IsNullOrWhiteSpace(TbxNumberPass.Text) &&
                    !string.IsNullOrWhiteSpace(TbxSerriesPass.Text) && DtpBirthday.SelectedDate != null)
                {
                    // Проверяем валидность введенных ФИО
                    if (!CheckPointLibrary.MainClass.ValidateFIO(TbxLastName.Text, TbxName.Text, TbxPatronymic.Text))
                    {
                        MessageBox.Show("Введите корректные данные для ФИО", "Ошибка при добавлении данных", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Создаем пропуск
                    Pass pass = new Pass
                    {
                        IDTypePass = 1,
                        DateOfFormation = DateTime.Now
                    };
                    AppConnect.modelOdb.Pass.Add(pass);
                    AppConnect.modelOdb.SaveChanges();
                    IDPass = AppConnect.modelOdb.Pass.Max(x => x.ID);

                    // Создаем стороннее лицо
                    IDExtPerson = AppConnect.modelOdb.ExternalPerson.Max(x => x.ID) + 1;
                    ExternalPerson externalPerson = new ExternalPerson
                    {
                        ID = IDExtPerson,
                        IDPass = IDPass,
                        LastName = TbxLastName.Text,
                        FirstName = TbxName.Text,
                        Patronumic = TbxPatronymic.Text,
                        Birthday = DtpBirthday.SelectedDate,
                        SeriesPassport = int.Parse(TbxSerriesPass.Text),
                        NumberPassport = int.Parse(TbxNumberPass.Text)
                    };
                    AppConnect.modelOdb.ExternalPerson.Add(externalPerson);
                    AppConnect.modelOdb.SaveChanges();

                    MessageBox.Show("Данные успешно сохранены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Данные не были введены", "Ошибка при добавлении данных", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            linkPrint:
                // Формируем и отображаем отчет
                ReportPassExternalPerson reportPassExternalPerson = new ReportPassExternalPerson(IDExtPerson, IDPass, DateTime.Now.ToString());
                reportPassExternalPerson.Show();

                // Очищаем поля и устанавливаем таймер для очистки через 3 секунды
                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += (s, _e) =>
                {
                    TbxIDPass.Text = string.Empty;
                    TbxLastName.Text = string.Empty;
                    TbxName.Text = string.Empty;
                    TbxPatronymic.Text = string.Empty;
                    TbxNumberPass.Text = string.Empty;
                    TbxSerriesPass.Text = string.Empty;
                    DtpBirthday.SelectedDate = null;
                    timer.Stop();
                };
                timer.Interval = TimeSpan.FromSeconds(3);
                timer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла критическая ошибка приложения" + ex.ToString(), "Критическая ошибка приложения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Сохранить материальный пропуск".
        /// Сохраняет данные о материальной ценности и формирует пропуск на материальную ценность.
        /// </summary>
        private void BtnSaveMaterial_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            if (!string.IsNullOrWhiteSpace(TbxStateNumber.Text) &&
                !string.IsNullOrWhiteSpace(TbxNameMaterial.Text) &&
                !string.IsNullOrWhiteSpace(TbxCount.Text) &&
                !string.IsNullOrWhiteSpace(TbxWeight.Text) &&
                !string.IsNullOrWhiteSpace(TbxNumberDoc.Text))
            {
                AddAuto(TbxStateNumber.Text, TbxNameMaterial.Text, TbxCount.Text, TbxWeight.Text, TbxNumberDoc.Text);
                int IDAuto = int.Parse(AppConnect.modelOdb.AutoTransport.Where(x => x.StateNumber == TbxStateNumber.Text).Select(x => x.ID).ToString());
                int IDMatValue = AppConnect.modelOdb.AccountingMaterialValue.Max(x => x.ID);
                int IDPass = AppConnect.modelOdb.Pass.Max(x => x.ID);
                MessageBox.Show("Данные успешно сохранены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                PeportPassMaterialValue reportPass = new PeportPassMaterialValue(IDAuto, IDPass, DateTime.Now.ToString(), IDMatValue);
                reportPass.Show();
                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += (s, _e) =>
                {
                    TbxStateNumber.Text = string.Empty;
                    TbxNameMaterial.Text = string.Empty;
                    TbxCount.Text = string.Empty;
                    TbxWeight.Text = string.Empty;
                    TbxNumberDoc.Text = string.Empty;
                    timer.Stop();
                };
                timer.Interval = TimeSpan.FromSeconds(3);
                timer.Start();
            }
            else
                MessageBox.Show("Данные не были введены", "Ошибка при добавлении данных", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Произошла критическая ошибка приложения" + ex.ToString(), "Критическая ошибка приложения", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }
        /// <summary>
        /// Добавляет информацию об автотранспорте в базу данных, если он отсутствует.
        /// Затем создает материальный пропуск и сохраняет его в базе данных.
        /// </summary>
        /// <param name="StateNumber">Государственный номер автотранспорта.</param>
        /// <param name="NameMaterial">Наименование материальной ценности.</param>
        /// <param name="Count">Количество материальной ценности.</param>
        /// <param name="Weight">Вес материальной ценности.</param>
        /// <param name="NumberDoc">Номер документа.</param>
        public void AddAuto(string StateNumber, string NameMaterial, string Count, string Weight, string NumberDoc)
        {
            var AutoDb = AppConnect.modelOdb.AutoTransport.FirstOrDefault(x => x.StateNumber == StateNumber);
            if (AutoDb == null)
            {
                MessageBox.Show("Автотранспорт с таким гос. номером не найден");
                return;
            }
            AccountingMaterialValue materialValue = new AccountingMaterialValue
            {
                Name = NameMaterial,
                Count = int.Parse(Count),
                Weight = int.Parse(Weight),
                IDAutoTransport = AutoDb.ID,
                NumberDocument = NumberDoc
            };
            AppConnect.modelOdb.AccountingMaterialValue.Add(materialValue);
            Pass pass = new Pass
            {
                IDTypePass = 3,
                DateOfFormation = DateTime.Now
            };
            AppConnect.modelOdb.Pass.Add(pass);
            AppConnect.modelOdb.SaveChanges();
        }
        /// <summary>
        /// Обработчик события ввода текста в поле гос. номера автотранспорта.
        /// Проверяет ввод на допустимые символы.
        /// </summary>
        private void TbxStateNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^А-Я0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
