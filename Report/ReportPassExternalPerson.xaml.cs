using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SystemCheckPoint.AppData;

namespace SystemCheckPoint.Report
{
    /// <summary>
    /// Логика взаимодействия для ReportPassExternalPerson.xaml
    /// </summary>
    public partial class ReportPassExternalPerson : Window
    {
        readonly string time;

        /// <summary>
        /// Конструктор класса ReportPassExternalPerson.
        /// Инициализирует компоненты страницы и заполняет информацию о внешнем сотруднике.
        /// </summary>
        /// <param name="IDExternalPerson">Идентификатор внешнего сотрудника.</param>
        /// <param name="IDPass">Идентификатор пропуска.</param>
        /// <param name="Time">Время формирования отчета.</param>
        public ReportPassExternalPerson(int IDExternalPerson, int IDPass, string Time)
        {
            AppConnect.modelOdb = new CheckPointDbEntities1();
            InitializeComponent();
            TblTimeReport.Text = Time;

            var extPerson = AppConnect.modelOdb.ExternalPerson.FirstOrDefault(x => x.ID == IDExternalPerson);

            if (extPerson != null)
            {
                TblLastName.Text = extPerson.LastName;
                TblName.Text = extPerson.FirstName;
                TblPatronymic.Text = extPerson.Patronumic;
                TblBirthday.Text = extPerson.Birthday.ToString().Substring(0, 10);
                TblSeries.Text = extPerson.SeriesPassport.ToString();
                TblNumber.Text = extPerson.NumberPassport.ToString();
            }

            ImgQR.Source = CheckPointLibrary.MainClass.GenerateQR(IDPass);
            time = AppConnect.modelOdb.Pass.Where(x => x.ID == extPerson.IDPass).Select(x => x.DateOfFormation).FirstOrDefault().ToString();
        }

        /// <summary>
        /// Обработчик события нажатия кнопки мыши на текстовом блоке.
        /// Позволяет перемещать окно при зажатой левой кнопке мыши.
        /// </summary>
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Назад".
        /// Закрывает окно.
        /// </summary>
        private void BtnBack_Click(object sender, RoutedEventArgs e) => this.Close();

        /// <summary>
        /// Обработчик события нажатия кнопки "Печать".
        /// Скрывает кнопки перед печатью, открывает диалог печати и печатает содержимое окна.
        /// </summary>
        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            StpButton.Visibility = Visibility.Collapsed;
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(GrdMain, time);
            }
            else
                MessageBox.Show("Пользователь прервал печать!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

            StpButton.Visibility = Visibility.Visible;
        }

    }
}
