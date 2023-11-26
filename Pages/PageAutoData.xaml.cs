using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using SystemCheckPoint.AppData;

namespace SystemCheckPoint.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageAutoSecurity.xaml
    /// </summary>
    public partial class PageAutoSecurity : System.Windows.Controls.Page
    {
        int IDAuto;
        public PageAutoSecurity(int IDUserRole)
        {
            InitializeComponent();
            if (IDUserRole != 1)
            {
                BtnCheckAutoOrg.Visibility = Visibility.Collapsed;
                BtnSave.Visibility = Visibility.Collapsed;
                TbxIDPassAuto.IsReadOnly = true;
                TbxIDPassAuto.IsReadOnly = true;
                TbxMark.IsReadOnly = true;
                TbxStateNumber.IsReadOnly = true;
                TbxSeriesNumber.IsReadOnly = true;
                TbxColor.IsReadOnly = true;
            }
            LsvAuto.ItemsSource = AppConnect.modelOdb.AutoTransport.ToArray();
        }
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.FrameMain.Content = null;
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TbxIDPassAuto.Text) && !string.IsNullOrWhiteSpace(TbxMark.Text) &&
                !string.IsNullOrWhiteSpace(TbxStateNumber.Text) && !string.IsNullOrWhiteSpace(TbxSeriesNumber.Text) &&
                !string.IsNullOrWhiteSpace(TbxColor.Text))
            {                
                AutoTransport autoTransport = new AutoTransport
                {
                    IDPass = int.Parse(TbxIDPassAuto.Text),
                    Mark = TbxMark.Text,
                    StateNumber = TbxStateNumber.Text,
                    SeriesNumber = TbxSeriesNumber.Text,
                    Color = TbxColor.Text
                };
                AppConnect.modelOdb.AutoTransport.Add(autoTransport);
                AppConnect.modelOdb.SaveChanges();
                MessageBox.Show("Данные успешно сохранены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += (s, _e) =>
                {
                    TbxIDPassAuto.Text = string.Empty;
                    TbxMark.Text = string.Empty;
                    TbxStateNumber.Text = string.Empty;
                    TbxSeriesNumber.Text = string.Empty;
                    TbxColor.Text = string.Empty;
                    timer.Stop();
                };
                timer.Interval = TimeSpan.FromSeconds(3);
                timer.Start();
            }
            else
                MessageBox.Show("Данные не были введены", "Ошибка при добавлении данных", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Произошла критическая ошибка приложения" + ex.ToString(), "Критическая ошибка приложения", MessageBoxButton.OK, MessageBoxImage.Error);
            //}

        }
        private void TbxFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            LsvAuto.ItemsSource = AppConnect.modelOdb.AutoTransport.Where(x =>
                                  x.ID.ToString().Contains(TbxFind.Text) ||
                                  x.Mark.Contains(TbxFind.Text) ||
                                  x.StateNumber.Contains(TbxFind.Text) ||
                                  x.SeriesNumber.ToString().Contains(TbxFind.Text) ||
                                  x.Color.ToString().Contains(TbxFind.Text) ||
                                  x.IDPass.ToString().Contains(TbxFind.Text)
                                  ).ToList();
        }
        private void LsvAuto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LsvAuto.SelectedItem != null)
            {
                StpPanelData.Visibility = Visibility.Visible;
                AutoTransport auto = (AutoTransport)LsvAuto.SelectedItem;
                IDAuto = auto.ID;
                var selectedAuto = AppConnect.modelOdb.AutoTransport.FirstOrDefault(x => x.ID == IDAuto);
                //int IDPassAuto = int.Parse(selectedAuto.IDPass.ToString());
                if (selectedAuto != null)
                {
                    TbxIDPassAuto.Text = selectedAuto.IDPass.ToString();
                    TbxMark.Text = selectedAuto.Mark;
                    TbxStateNumber.Text = selectedAuto.StateNumber;
                    TbxSeriesNumber.Text = selectedAuto.SeriesNumber;
                    TbxColor.Text = selectedAuto.Color;
                }
            }
        }
        private void Text_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-Яa-zA-Z]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void TbxStateNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^А-Я0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void TbxSeriesNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-Z0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private bool AreFieldsChanged(int id)
        {
            //var autoDb = AppConnect.modelOdb.AutoTransport.FirstOrDefault(x => x.ID == id);
            //if (autoDb != null)
            //{
            //    return autoDb.IDPass != int.Parse(TbxIDPassAuto.Text)
            //        || autoDb.Mark != TbxMark.Text
            //        || autoDb.StateNumber != TbxStateNumber.Text
            //        || autoDb.SeriesNumber != TbxSeriesNumber.Text
            //        || autoDb.Color != TbxColor.Text;
            //}
            return false;
        }

        private void AreFieldsChanged_TextChanged(object sender, TextChangedEventArgs e)
        {
            BtnSave.IsEnabled = AreFieldsChanged(IDAuto);
        }
        static bool isChecked = false;
        private void BtnCheckAutoOrg_Click(object sender, RoutedEventArgs e)
        {
            isChecked = !isChecked;
            if (isChecked)
            {
                BtnCheckAutoOrg.Content = new Image() { Source = new BitmapImage(new Uri("/Images/check.png", UriKind.Relative)) };
                BrdSelectPerson.Visibility = Visibility.Collapsed;
                StpPanelData.Visibility = Visibility.Visible;
                TbxIDPassAuto.Text = string.Empty;
                TbxMark.Text = string.Empty;
                TbxStateNumber.Text = string.Empty;
                TbxSeriesNumber.Text = string.Empty;
                TbxColor.Text = string.Empty;
            }
            else
            {
                BtnCheckAutoOrg.Content = null;
                BrdSelectPerson.Visibility = Visibility.Visible;
                StpPanelData.Visibility = Visibility.Collapsed;
                TbxIDPassAuto.Text = string.Empty;
                TbxMark.Text = string.Empty;
                TbxStateNumber.Text = string.Empty;
                TbxSeriesNumber.Text = string.Empty;
                TbxColor.Text = string.Empty;
            }
        }
    }
}