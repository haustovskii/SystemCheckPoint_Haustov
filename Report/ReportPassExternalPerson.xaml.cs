﻿using MessagingToolkit.QRCode.Codec;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SystemCheckPoint.AppData;

namespace SystemCheckPoint.Report
{
    /// <summary>
    /// Логика взаимодействия для ReportPassExternalPerson.xaml
    /// </summary>
    public partial class ReportPassExternalPerson : Window
    {
        readonly string time;
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
            GenerateQR(IDPass);
            time = AppConnect.modelOdb.Pass.Where(x => x.ID == extPerson.IDPass).Select(x => x.DateOfFormation).FirstOrDefault().ToString();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
        private void GenerateQR(int IDPass)
        {
            QRCodeEncoder encoder = new QRCodeEncoder(); // Создаем объект класса QRCodeEncoder
            Bitmap qrcode = encoder.Encode(IDPass.ToString()); // Кодируем текст, полученный из TextBox'а (qrtext), в переменную qrcode
            ImgQR.Source = BitmapToImageSource(qrcode); // Устанавливаем qrcode как источник изображения для ImgQR
        }

        // Метод для преобразования Bitmap в ImageSource
        public ImageSource BitmapToImageSource(Bitmap bitmap)
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
