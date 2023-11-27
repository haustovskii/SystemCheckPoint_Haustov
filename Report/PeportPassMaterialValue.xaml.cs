using MessagingToolkit.QRCode.Codec;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SystemCheckPoint.AppData;

namespace SystemCheckPoint.Report
{
    /// <summary>
    /// Логика взаимодействия для PeportPassMaterialValue.xaml
    /// </summary>
    public partial class PeportPassMaterialValue : Window
    {
        readonly string time;
        /// <summary>
        /// Конструктор класса PeportPassMaterialValue.
        /// Инициализирует компоненты страницы и заполняет информацию о транспорте и материальной ценности.
        /// </summary>
        /// <param name="IDAuto">Идентификатор транспортного средства.</param>
        /// <param name="IDPass">Идентификатор пропуска.</param>
        /// <param name="Time">Время создания отчета.</param>
        /// <param name="IDMatValue">Идентификатор материальной ценности.</param>
        public PeportPassMaterialValue(int IDAuto, int IDPass, string Time, int IDMatValue)
        {
            InitializeComponent();
            GenerateQR(IDPass);
            time = Time;
            var AutoDb = AppConnect.modelOdb.AutoTransport.FirstOrDefault(x => x.ID == IDPass);
            if (AutoDb != null)
            {
                TblStateNumber.Text = AutoDb.StateNumber;
                TblMark.Text = AutoDb.Mark;
                TblColor.Text = AutoDb.Color;
            }
            var MatValueDb = AppConnect.modelOdb.AccountingMaterialValue.FirstOrDefault(x => x.ID == IDMatValue);
            if (MatValueDb != null)
            {
                TblName.Text = MatValueDb.Name;
                TblCount.Text = MatValueDb.Count.ToString();
                TblWeight.Text = MatValueDb.Weight.ToString();
                TblNumberDoc.Text = MatValueDb.NumberDocument.ToString();
            }
            TblTimeReport.Text = Time;
        }

        /// <summary>
        /// Обработчик события нажатия кнопки мыши на текстовом блоке.
        /// Позволяет перемещать окно при зажатой левой кнопке мыши.
        /// </summary>
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Назад".
        /// Закрывает окно.
        /// </summary>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Генерирует QR-код с использованием идентификатора пропуска.
        /// </summary>
        /// <param name="IDPass">Идентификатор пропуска.</param>
        private void GenerateQR(int IDPass)
        {
            QRCodeEncoder encoder = new QRCodeEncoder();
            Bitmap qrcode = encoder.Encode(IDPass.ToString());
            ImgQR.Source = BitmapToImageSource(qrcode);
        }

        /// <summary>
        /// Преобразует Bitmap в ImageSource.
        /// </summary>
        private ImageSource BitmapToImageSource(Bitmap bitmap)
        {
            var memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Bmp);
            memoryStream.Position = 0;
            var imageSource = new BitmapImage();
            imageSource.BeginInit();
            imageSource.StreamSource = memoryStream;
            imageSource.CacheOption = BitmapCacheOption.OnLoad;
            imageSource.EndInit();
            return imageSource;
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Печать".
        /// Скрывает кнопки перед печатью, отображает диалог печати и печатает содержимое окна.
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
