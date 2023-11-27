using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SystemCheckPoint.AppData;

namespace SystemCheckPoint.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageOtherPass.xaml
    /// </summary>
    public partial class PageOtherPass : System.Windows.Controls.Page
    {
        private bool isClicked = false;
        /// <summary>
        /// Конструктор класса PageOtherPass.
        /// Инициализирует компоненты страницы и загружает данные в DataGrid.
        /// </summary>
        public PageOtherPass()
        {
            InitializeComponent();
            DgrExternal.ItemsSource = AppConnect.modelOdb.ExternalPerson.ToArray();
            DgrMatValue.ItemsSource = AppConnect.modelOdb.AccountingMaterialValue.ToArray();
        }

        /// <summary>
        /// Обработчик события клика на кнопке удаления данных о внешнем сотруднике.
        /// </summary>
        private void BtnDeletePass_Click(object sender, RoutedEventArgs e)
        {
            if (DgrExternal.SelectedItem != null)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Удалить эту строку?", "Удаление данных", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    ExternalPerson external = (ExternalPerson)DgrExternal.SelectedItem;
                    AppConnect.modelOdb.ExternalPerson.Remove(AppConnect.modelOdb.ExternalPerson.FirstOrDefault(x => x.ID == external.ID));
                    AppConnect.modelOdb.SaveChanges();
                    DgrExternal.ItemsSource = AppConnect.modelOdb.ExternalPerson.ToArray();
                    MessageBox.Show("Данные успешно удалены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        /// <summary>
        /// Обработчик события клика на кнопке удаления данных о материальной ценности.
        /// </summary>
        private void BtnDeletePassValue_Click(object sender, RoutedEventArgs e)
        {
            if (DgrMatValue.SelectedItem != null)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Удалить эту строку?", "Удаление данных", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    AccountingMaterialValue materialValue = (AccountingMaterialValue)DgrMatValue.SelectedItem;
                    AppConnect.modelOdb.AccountingMaterialValue.Remove(AppConnect.modelOdb.AccountingMaterialValue.FirstOrDefault(x => x.ID == materialValue.ID));
                    AppConnect.modelOdb.SaveChanges();
                    DgrMatValue.ItemsSource = AppConnect.modelOdb.AccountingMaterialValue.ToArray();
                    MessageBox.Show("Данные успешно удалены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        /// <summary>
        /// Обработчик события клика на кнопке возврата.
        /// Переходит на предыдущую страницу.
        /// </summary>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.FrameMain.GoBack();
        }

        /// <summary>
        /// Обработчик события загрузки строки в DataGrid для данных о внешнем сотруднике.
        /// Устанавливает номер строки в заголовке.
        /// </summary>
        private void DgrExternal_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        /// <summary>
        /// Обработчик события загрузки строки в DataGrid для данных о материальной ценности.
        /// Устанавливает номер строки в заголовке.
        /// </summary>
        private void DgrMatValue_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        /// <summary>
        /// Обработчик события клика по элементу управления Ellipse (круг).
        /// Переключает отображение данных о внешних сотрудниках и материальных ценностях.
        /// </summary>
        private void ElpPozition_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!isClicked)
            {
                ElpPozition.Margin = new Thickness(25, 1, 0, 1);
                StpDataMatValue.Visibility = Visibility.Visible;
                StpDataExternal.Visibility = Visibility.Collapsed;
                BrdTumb.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EAF4E4"));
            }
            else
            {
                ElpPozition.Margin = new Thickness(0, 1, 25, 1);
                StpDataMatValue.Visibility = Visibility.Collapsed;
                StpDataExternal.Visibility = Visibility.Visible;
                BrdTumb.Background = Brushes.White;
            }
            isClicked = !isClicked;
        }
    }
}
