using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Linq.Expressions;
using SystemCheckPoint.AppData;
using SystemCheckPoint.Pages;

namespace SystemCheckPoint.Page
{
    /// <summary>
    /// Логика взаимодействия для PageMenuPassAdmin.xaml
    /// </summary>
    public partial class PageMenuPassAdmin : System.Windows.Controls.Page
    {
        /// <summary>
        /// Идентификатор текущего сотрудника/стороннего лица.
        /// </summary>
        public int IDEmployee = 0;

        /// <summary>
        /// Конструктор страницы меню для администратора пропускной системы.
        /// Инициализирует компоненты и устанавливает источники данных для элементов управления.
        /// </summary>
        public PageMenuPassAdmin()
        {
            InitializeComponent();

            // Установка источника данных для таблицы и списка сотрудников/сторонних лиц
            DgrData.ItemsSource = AppConnect.modelOdb.Employee.ToArray();
            LsvEmployee.ItemsSource = AppConnect.modelOdb.Employee.ToArray();

            // Установка источника данных для выпадающего списка должностей
            CmbPost.ItemsSource = AppConnect.modelOdb.Post.Select(x => x.Name).ToArray();
        }

        /// <summary>
        /// Переключение на режим добавления/редактирования данных о сотруднике/стороннем лице.
        /// </summary>
        private void BtnAddPass_Click(object sender, RoutedEventArgs e)
        {
            GrdSort.Visibility = Visibility.Collapsed;
            StpDataGrid.Visibility = Visibility.Collapsed;
            StpAddEditPass.Visibility = Visibility.Visible;
            BtnBack.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Сохранение и редактирование данных о сотруднике/стороннем лице.
        /// </summary>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TbxLastName.Text) && !string.IsNullOrEmpty(TbxName.Text) &&
                !string.IsNullOrEmpty(TbxPatronymic.Text) && DtpDate.SelectedDate != null &&
                !string.IsNullOrEmpty(TbxSerriesPass.Text) && !string.IsNullOrEmpty(TbxNumberPass.Text) &&
                CmbPost.SelectedIndex > -1)
            {
                if (!CheckPointLibrary.MainClass.ValidateFIO(TbxLastName.Text, TbxName.Text, TbxPatronymic.Text))
                {
                    MessageBox.Show("Введите корректные данные для ФИО", "Ошибка при добавлении данных", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                //Добавление данных
                if (TbxIDPass == null)
                {
                    Pass pass = new Pass
                    {
                        IDTypePass = 1,
                        DateOfFormation = DateTime.Now
                    };
                    AppConnect.modelOdb.Pass.Add(pass);
                    AppConnect.modelOdb.SaveChanges();
                    int IDPass = AppConnect.modelOdb.Pass.Max(x => x.ID);

                    Employee employee = new Employee
                    {
                        LastName = TbxLastName.Text,
                        FirstName = TbxName.Text,
                        Patronumic = TbxPatronymic.Text,
                        //Birthday = DtpDate.SelectedDate,
                        //Login = TbxLogin.Text,
                        //Password = TbxPaswword.Text,
                        SeriesPassport = int.Parse(TbxSerriesPass.Text),
                        NumberPassport = int.Parse(TbxNumberPass.Text),
                        IDPost = CmbPost.SelectedIndex + 1,
                        IDPass = IDPass
                    };
                    AppConnect.modelOdb.Employee.Add(employee);
                    AppConnect.modelOdb.SaveChanges();
                    DgrData.ItemsSource = AppConnect.modelOdb.Employee.ToArray();
                    MessageBox.Show("Данные успешно сохранены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                //Редактирование данных
                else
                {
                    byte[] imageData = GetImageDataFromImageControl(ImgEmpl);
                    Employee employee = AppConnect.modelOdb.Employee.FirstOrDefault(x => x.ID == IDEmployee);
                    if (employee != null)
                    {
                        employee.LastName = TbxLastName.Text;
                        employee.FirstName = TbxName.Text;
                        employee.Patronumic = TbxPatronymic.Text;
                        employee.IDPost = CmbPost.SelectedIndex + 1;
                        employee.NumberPassport = int.Parse(TbxNumberPass.Text);
                        employee.SeriesPassport = int.Parse(TbxSerriesPass.Text);
                        if (imageData != null)
                            employee.ImagePath = imageData;
                        AppConnect.modelOdb.SaveChanges();
                        DgrData.ItemsSource = AppConnect.modelOdb.Employee.ToArray();
                        MessageBox.Show("Данные успешно изменены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                GrdSort.Visibility = Visibility.Visible;
                StpDataGrid.Visibility = Visibility.Visible;
                StpAddEditPass.Visibility = Visibility.Collapsed;
            }
            else
                MessageBox.Show("Данные не были введены", "Ошибка при добавлении данных", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        /// <summary>
        /// Получает массив байтов изображения из элемента управления Image.
        /// </summary>
        /// <param name="imageControl">Элемент управления Image.</param>
        /// <returns>Массив байтов изображения или null, если изображение отсутствует.</returns>
        private byte[] GetImageDataFromImageControl(Image imageControl)
        {
            if (imageControl.Source is BitmapSource bitmapSource)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    BitmapEncoder encoder = new PngBitmapEncoder(); // Используйте соответствующий энкодер
                    encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                    encoder.Save(memoryStream);
                    return memoryStream.ToArray();
                }
            }

            return null;
        }
        /// <summary>
        /// Обработчик события клика на кнопке выбора сотрудника.
        /// </summary>
        private void BtnSelectEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (BrdSelectEmployee.Visibility == Visibility.Collapsed)
                BrdSelectEmployee.Visibility = Visibility.Visible;
            else
                BrdSelectEmployee.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Обработчик события изменения текста в поле автоматического заполнения сотрудника.
        /// </summary>
        private void TbxSelectEmployee_TextChanged(object sender, TextChangedEventArgs e)
        {
            LsvEmployee.ItemsSource = AppConnect.modelOdb.Employee.Where(x =>
                x.LastName.Contains(TbxSelectEmployee.Text) ||
                x.FirstName.Contains(TbxSelectEmployee.Text) ||
                x.Patronumic.Contains(TbxSelectEmployee.Text)
            ).ToArray();
        }

        /// <summary>
        /// Обработчик события изменения выбора в списке сотрудников.
        /// Заполняет поля данными выбранного сотрудника для редактирования.
        /// </summary>
        private void LsvEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LsvEmployee.SelectedItem != null)
            {
                Employee employee = (Employee)LsvEmployee.SelectedItem;
                IDEmployee = employee.ID;
                var EmployeeDb = AppConnect.modelOdb.Employee.FirstOrDefault(x => x.ID == IDEmployee);
                if (EmployeeDb != null)
                {
                    TbxIDPass.Text = EmployeeDb.ID.ToString();
                    TbxLastName.Text = EmployeeDb.LastName;
                    TbxName.Text = EmployeeDb.FirstName;
                    TbxPatronymic.Text = EmployeeDb.Patronumic;
                    DtpDate.SelectedDate = EmployeeDb.Birthday;
                    CmbPost.SelectedIndex = EmployeeDb.IDPost - 1;
                    TbxSerriesPass.Text = EmployeeDb.SeriesPassport.ToString();
                    TbxNumberPass.Text = EmployeeDb.NumberPassport.ToString();
                    if (EmployeeDb.ImagePath != null)
                        ImgEmpl.Source = LoadImage(EmployeeDb.ImagePath);
                }
                BrdSelectEmployee.Visibility = Visibility.Collapsed;
            }
        }
        /// <summary>
        /// Обработчик события клика на кнопке возврата к таблице.
        /// </summary>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (StpAddEditPass.Visibility == Visibility.Visible)
            {
                GrdSort.Visibility = Visibility.Visible;
                StpDataGrid.Visibility = Visibility.Visible;
                StpAddEditPass.Visibility = Visibility.Collapsed;
                BtnBack.Visibility = Visibility.Collapsed;
            }
        }
        /// <summary>
        /// Обработчик события клика на кнопке удаления пропуска.
        /// </summary>
        private void BtnDeletePass_Click(object sender, RoutedEventArgs e)
        {
            if (DgrData.SelectedItem != null)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Удалить эту строку?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    Employee employee = (Employee)DgrData.SelectedItem;
                    AppConnect.modelOdb.Employee.Remove(AppConnect.modelOdb.Employee.FirstOrDefault(x => x.ID == employee.ID));
                    AppConnect.modelOdb.SaveChanges();
                    DgrData.ItemsSource = AppConnect.modelOdb.Employee.ToArray();
                    MessageBox.Show("Данные успешно удалены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        /// <summary>
        /// Обновляет отображаемые данные с учетом фильтрации, сортировки и поиска.
        /// </summary>
        private void UpdateData()
        {
            var query = AppConnect.modelOdb.Employee.AsQueryable();

            // Применяем фильтрацию
            string searchText = TbxFind.Text.ToLower();
            query = query.Where(x =>
                x.LastName.ToLower().Contains(searchText) ||
                x.FirstName.ToLower().Contains(searchText) ||
                x.Patronumic.ToLower().Contains(searchText) ||
                x.Birthday.ToString().Contains(searchText) ||
                x.Pass.ID.ToString().Contains(searchText) ||
                x.Pass.DateOfFormation.ToString().Contains(searchText));

            // Применяем сортировку
            switch (CmbSort.SelectedIndex)
            {
                case 0:
                    query = query.OrderBy(x => x.LastName);
                    break;
                case 1:
                    query = query.OrderByDescending(x => x.LastName);
                    break;
                default:
                    break;
            }

            // Применяем фильтр по выбранному полю
            if (CmbFilter.SelectedIndex >= 0)
            {
                switch (CmbFilter.SelectedIndex)
                {
                    case 0:
                        query = query.OrderBy(x => x.LastName);
                        break;
                    case 1:
                        query = query.OrderBy(x => x.Birthday);
                        break;
                    case 2:
                        query = query.OrderBy(x => x.Pass.ID);
                        break;
                    case 3:
                        query = query.OrderBy(x => x.Pass.DateOfFormation);
                        break;
                    default:
                        break;
                }
            }

            DgrData.ItemsSource = query.ToList();
        }
        /// <summary>
        /// Обработчик изменения выбранного значения в выпадающем списке фильтра.
        /// Вызывает метод UpdateData для обновления отображаемых данных.
        /// </summary>
        private void CmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }
        /// <summary>
        /// Обработчик изменения текста в поле поиска.
        /// Вызывает метод UpdateData для обновления отображаемых данных.
        /// </summary>
        private void TbxFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateData();
        }
        /// <summary>
        /// Обработчик изменения выбранного значения в выпадающем списке сортировки.
        /// Вызывает метод UpdateData для обновления отображаемых данных.
        /// </summary>
        private void CmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }
        /// <summary>
        /// Обработчик события загрузки строки в DataGrid.
        /// Устанавливает номер строки в заголовке.
        /// </summary>
        private void DgrData_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        /// <summary>
        /// Обработчик события клика на кнопке добавления изображения.
        /// Позволяет выбрать изображение и отображает его в элементе управления Image.
        /// </summary>
        private void BtnAddImage_Click(object sender, RoutedEventArgs e)
        {
            byte[] selectedImageData;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;

                // Преобразование изображения в массив байтов
                using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        selectedImageData = br.ReadBytes((int)fs.Length);
                    }
                }
                // Отображение изображения в ImgEmpl
                ImgEmpl.Source = LoadImage(selectedImageData);
            }
        }
        /// <summary>
        /// Загружает изображение из массива байтов и возвращает объект BitmapImage.
        /// </summary>
        /// <param name="imageData">Массив байтов изображения.</param>
        /// <returns>Объект BitmapImage с загруженным изображением.</returns>
        private BitmapImage LoadImage(byte[] imageData)
        {
            BitmapImage image = new BitmapImage();
            using (MemoryStream memoryStream = new MemoryStream(imageData))
            {
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = memoryStream;
                image.EndInit();
            }
            return image;
        }
        /// <summary>
        /// Обработчик события клика на кнопке перехода к другим пропускам.
        /// </summary>
        private void BtnOtherPass_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.FrameMain.Navigate(new PageOtherPass());
        }
    }
}

