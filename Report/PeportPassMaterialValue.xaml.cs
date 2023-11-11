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
        public PeportPassMaterialValue(int IDAuto, int IDPass, string Time, int IDMatValue)
        {
            InitializeComponent();
            GenerateQR(IDPass);
            time = Time;
            var AutoDb = AppConnect.modelOdb.AutoTransport.FirstOrDefault(x=> x.ID == IDPass);
            if (AutoDb != null)
            {
                TblStateNumber.Text = AutoDb.StateNumber;
                TblMark.Text = AutoDb.Mark;
                TblColor.Text = AutoDb.Color;
            }
            var MatValueDb = AppConnect.modelOdb.AccountingMaterialValue.FirstOrDefault(x => x.ID == IDMatValue);
            if(MatValueDb != null)
            {
                TblName.Text = MatValueDb.Name;
                TblCount.Text = MatValueDb.Count.ToString();
                TblWeight.Text = MatValueDb.Weight.ToString();
                TblNumberDoc.Text = MatValueDb.NumberDocument.ToString();
            }
            TblTimeReport.Text = Time;
        }
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void GenerateQR(int IDPass)
        {
            QRCodeEncoder encoder = new QRCodeEncoder(); // Создаем объект класса QRCodeEncoder
            Bitmap qrcode = encoder.Encode(IDPass.ToString()); // Кодируем текст, полученный из TextBox'а (qrtext), в переменную qrcode
            ImgQR.Source = BitmapToImageSource(qrcode); // Устанавливаем qrcode как источник изображения для ImgQR
        }
        // Метод для преобразования Bitmap в ImageSource
        private ImageSource BitmapToImageSource(Bitmap bitmap)
        {
            var memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Bmp); // Можно выбрать другой формат, если нужно
            memoryStream.Position = 0;
            var imageSource = new BitmapImage();
            imageSource.BeginInit();
            imageSource.StreamSource = memoryStream;
            imageSource.CacheOption = BitmapCacheOption.OnLoad;
            imageSource.EndInit();
            return imageSource;
        }

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
