using System.Windows;
using SystemCheckPoint.AppData;
using SystemCheckPoint.Report;

namespace SystemCheckPoint.Page
{
    /// <summary>
    /// Логика взаимодействия для PageReportAdmin.xaml
    /// </summary>
    public partial class PageReportAdmin : System.Windows.Controls.Page
    {
        /// <summary>
        /// Конструктор класса PageReportAdmin.
        /// Инициализирует компоненты страницы.
        /// </summary>
        public PageReportAdmin()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик события клика на кнопке возврата.
        /// Очищает содержимое главного фрейма.
        /// </summary>
        private void BtnBack_Click(object sender, RoutedEventArgs e) => AppFrame.FrameMain.Content = null;

        private bool SelectEmployee = false;
        private bool SelectPerson = false;
        private bool isEmployee = false;

        /// <summary>
        /// Обработчик события клика по элементу управления ElpPozition1 (круг 1).
        /// Переключает состояние выбора сотрудника и обновляет видимость.
        /// </summary>
        private void ElpPozition1_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SelectEmployee = !SelectEmployee;
            SelectPerson = false;
            isEmployee = SelectEmployee;
            UpdateVisibility();
        }

        /// <summary>
        /// Обработчик события клика по элементу управления ElpPozition2 (круг 2).
        /// Переключает состояние выбора внешнего сотрудника и обновляет видимость.
        /// </summary>
        private void ElpPozition2_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SelectPerson = !SelectPerson;
            SelectEmployee = false;
            isEmployee = SelectEmployee;
            UpdateVisibility();
        }

        /// <summary>
        /// Обновляет видимость элементов управления в зависимости от выбранного типа отчета.
        /// </summary>
        private void UpdateVisibility()
        {
            ElpPozition1.Visibility = SelectEmployee ? Visibility.Visible : Visibility.Collapsed;
            ElpPozition2.Visibility = SelectPerson ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Обработчик события клика на кнопке создания отчета.
        /// Проверяет выбранный тип отчета и открывает соответствующий отчет.
        /// </summary>
        private void BtnCreateReport_Click(object sender, RoutedEventArgs e)
        {
            if (SelectEmployee == SelectPerson)
            {
                MessageBox.Show("Выберите тип отчета!");
                return;
            }
            if (isEmployee)
            {
                ReportInfoArrivalDeparture report = new ReportInfoArrivalDeparture(true);
                report.Show();
            }
            else
            {
                ReportInfoArrivalDeparture report = new ReportInfoArrivalDeparture(false);
                report.Show();
            }
        }

    }
}
