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
        public int IDEmployee = 0;
        public PageMenuPassAdmin()
        {
            InitializeComponent();
            DgrData.ItemsSource = AppConnect.modelOdb.Employee.ToArray();
            LsvEmployee.ItemsSource = AppConnect.modelOdb.Employee.ToArray();
            CmbPost.ItemsSource = AppConnect.modelOdb.Post.Select(x => x.Name).ToArray();
        }
        //Переключение на режим добавления/редактирования
        private void BtnAddPass_Click(object sender, RoutedEventArgs e)
        {
            GrdSort.Visibility = Visibility.Collapsed;
            StpDataGrid.Visibility = Visibility.Collapsed;
            StpAddEditPass.Visibility = Visibility.Visible;
            BtnBack.Visibility = Visibility.Visible;
        }
        //Сохранение и редактирование данных о сотруднике/стороннем лице
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
        //Открытие автоматической проверки
        private void BtnSelectEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (BrdSelectEmployee.Visibility == Visibility.Collapsed)
                //добавить проверку на клик вне области
                BrdSelectEmployee.Visibility = Visibility.Visible;
            else
                BrdSelectEmployee.Visibility = Visibility.Collapsed;
        }
        //Поиск в автоматическом заполнении
        private void TbxSelectEmployee_TextChanged(object sender, TextChangedEventArgs e)
        {
            LsvEmployee.ItemsSource = AppConnect.modelOdb.Employee.Where(x =>
            x.LastName.Contains(TbxSelectEmployee.Text) ||
            x.FirstName.Contains(TbxSelectEmployee.Text) ||
            x.Patronumic.Contains(TbxSelectEmployee.Text)
                ).ToArray();
        }
        //Автоматическое заполнение данных для редактирования
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
        //Возврат к таблице
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (StpAddEditPass.Visibility == Visibility)
            {
                GrdSort.Visibility = Visibility.Visible;
                StpDataGrid.Visibility = Visibility.Visible;
                StpAddEditPass.Visibility = Visibility.Collapsed;
                BtnBack.Visibility = Visibility.Collapsed;
            }
        }
        //Удаление пропуска
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

        private void CmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void TbxFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateData();
        }

        private void CmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        } 
        private void DgrData_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
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

        private void BtnOtherPass_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.FrameMain.Navigate(new PageOtherPass());
        }
    }
}

