using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SystemCheckPoint.AppData;
namespace SystemCheckPoint.Page
{
    /// <summary>
    /// Логика взаимодействия для PageMenuPassAdmin.xaml
    /// </summary>
    public partial class PageMenuPassAdmin : System.Windows.Controls.Page
    {
        public int IDEmployee = 0;
        List<EmployeeAndPerson> list = new List<EmployeeAndPerson>();
        public PageMenuPassAdmin()
        {
            InitializeComponent();
            LoadData();
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
            //НЕ РАБОТАЕТ, НАДО ВСЕ ЧЕКАЙ(((((
            if (!string.IsNullOrEmpty(TbxLastName.Text) && !string.IsNullOrEmpty(TbxName.Text) &&
                !string.IsNullOrEmpty(TbxPatronymic.Text) && DtpDate.SelectedDate != null &&
                !string.IsNullOrEmpty(TbxSerriesPass.Text) && !string.IsNullOrEmpty(TbxNumberPass.Text) &&
                CmbPost.SelectedIndex > -1)
            {
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
                    LoadData();
                    MessageBox.Show("Данные успешно сохранены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                //Редактирование данных
                else
                {
                    Employee employee = AppConnect.modelOdb.Employee.FirstOrDefault(x => x.ID == IDEmployee);
                    if (employee != null)
                    {
                        employee.LastName = TbxLastName.Text;
                        employee.FirstName = TbxName.Text;
                        employee.Patronumic = TbxPatronymic.Text;
                        employee.IDPost = CmbPost.SelectedIndex;
                        employee.NumberPassport = int.Parse(TbxNumberPass.Text);
                        employee.SeriesPassport = int.Parse(TbxSerriesPass.Text);
                        AppConnect.modelOdb.SaveChanges();
                        LoadData();
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
        //Поиск на главном экране
        private void TbxFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            SortFiler();
        }
        private void CmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortFiler();
        }

        private void SortFiler()
        {
            var filteredList = list.ToList(); // Копируем исходный список для фильтрации

            // Фильтрация на основе введенного текста
            filteredList = filteredList.Where(x =>
                x.LastName.Contains(TbxFind.Text) ||
                x.FirstName.Contains(TbxFind.Text) ||
                x.Patronumic.Contains(TbxFind.Text) ||
                x.Bithday.ToString().Contains(TbxFind.Text) ||
                x.NumberPass.ToString().Contains(TbxFind.Text) ||
                x.DateOfFormation.ToString().Contains(TbxFind.Text) ||
                x.Type.Contains(TbxFind.Text)
            ).ToList();
            if (CmbViewType.SelectedIndex == 0)
            {
                filteredList = filteredList.ToList();
            }
            if (CmbViewType.SelectedIndex == 1) // Только сотрудники
            {
                filteredList = filteredList.Where(x => x.Type == "СТ").ToList();
            }
            else if (CmbViewType.SelectedIndex == 2) // Только сторонние лица
            {
                filteredList = filteredList.Where(x => x.Type == "СТЛ").ToList();
            }
            if (CmbSort.SelectedIndex != -1)
            {
                // Применение выбранных полей для сортировки
                if (CmbFilter.SelectedIndex == 0) // По фамилии
                    if (CmbSort.SelectedIndex == 1)
                        filteredList = filteredList.OrderByDescending(x => x.LastName).ToList();
                    else
                        filteredList = filteredList.OrderBy(x => x.LastName).ToList();
                else if (CmbFilter.SelectedIndex == 1) // По дате рождения
                    if (CmbSort.SelectedIndex == 1)
                        filteredList = filteredList.OrderByDescending(x => x.Bithday).ToList();
                    else
                        filteredList = filteredList.OrderBy(x => x.Bithday).ToList();

                else if (CmbFilter.SelectedIndex == 2) // По № пропуска

                    if (CmbSort.SelectedIndex == 1)
                        filteredList = filteredList.OrderByDescending(x => x.NumberPass).ToList();
                    else
                        filteredList = filteredList.OrderBy(x => x.NumberPass).ToList();

                else if (CmbFilter.SelectedIndex == 3) // По дате добавления

                    if (CmbSort.SelectedIndex == 1)
                        filteredList = filteredList.OrderByDescending(x => x.DateOfFormation).ToList();
                    else
                        filteredList = filteredList.OrderBy(x => x.DateOfFormation).ToList();
            }
            if (DgrData.ItemsSource != null)
                DgrData.ItemsSource = filteredList;

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
        private void LoadData()
        {
            using (var context = new CheckPointDbEntities()) // Создайте экземпляр вашего контекста данных
            {
                var queryEmployee = from p in context.Pass
                                    join e in context.Employee on p.ID equals e.IDPass
                                    join t in context.TypePass on p.IDTypePass equals t.ID
                                    select new EmployeeAndPerson
                                    {
                                        ID = e.ID,
                                        LastName = e.LastName,
                                        FirstName = e.FirstName,
                                        Patronumic = e.Patronumic,
                                        Bithday = e.Birthday.ToString(),
                                        NumberPass = p.ID,
                                        Type = t.Name,
                                        DateOfFormation = (DateTime)p.DateOfFormation
                                    };
                var queryExternalPerson = from p in context.Pass
                                          join e in context.ExternalPerson on p.ID equals e.IDPass
                                          join t in context.TypePass on p.IDTypePass equals t.ID
                                          select new EmployeeAndPerson
                                          {
                                              ID = e.ID,
                                              LastName = e.LastName,
                                              FirstName = e.FirstName,
                                              Patronumic = e.Patronumic,
                                              Bithday = e.Birthday.ToString(),
                                              NumberPass = p.ID,
                                              Type = t.Name,
                                              DateOfFormation = (DateTime)p.DateOfFormation
                                          };
                var combinedQuery = queryEmployee.Union(queryExternalPerson);
                list = combinedQuery.ToList();
            }
            DgrData.ItemsSource = list.ToArray();
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
                }
                BrdSelectEmployee.Visibility = Visibility.Collapsed;

            }
        }

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

        private void DgrData_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;

            // Убедимся, что это не заголовок строки (заголовок тоже является строкой)
            if (e.Row != null && !e.Row.IsNewItem)
            {
                e.Row.Header = e.Row.GetIndex() + 1;
            }
        }

        private void BtnDeletePass_Click(object sender, RoutedEventArgs e)
        {
            if (DgrData.SelectedItem != null)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Удалить эту строку?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    EmployeeAndPerson selectedPerson = (EmployeeAndPerson)DgrData.SelectedItem;
                    if (selectedPerson.Type == "Сотрудник")
                        AppConnect.modelOdb.Employee.Remove(AppConnect.modelOdb.Employee.FirstOrDefault(x => x.ID == selectedPerson.ID));
                    else
                        AppConnect.modelOdb.Employee.Remove(AppConnect.modelOdb.Employee.FirstOrDefault(x => x.ID == selectedPerson.ID));
                    AppConnect.modelOdb.SaveChanges();
                    LoadData();
                    MessageBox.Show("Данные успешно удалены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
