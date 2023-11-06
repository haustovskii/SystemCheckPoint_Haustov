using MessagingToolkit.QRCode.Codec;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace SystemCheckPoint.Report
{
    /// <summary>
    /// Логика взаимодействия для ReportPassExternalPerson.xaml
    /// </summary>
    public partial class ReportPassExternalPerson : Window
    {
        public ReportPassExternalPerson(int IDExternalPerson,int IDPass)
        {
            InitializeComponent();
            GenerateQR();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        private void GenerateQR()
        {
            string qrtext = "0"; // Считываем текст из TextBox'а
            QRCodeEncoder encoder = new QRCodeEncoder(); // Создаем объект класса QRCodeEncoder
            Bitmap qrcode = encoder.Encode(qrtext); // Кодируем текст, полученный из TextBox'а (qrtext), в переменную qrcode
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
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
