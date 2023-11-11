using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SystemCheckPoint.AppData;

namespace SystemCheckPoint.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageAutoSecurity.xaml
    /// </summary>
    public partial class PageAutoSecurity : System.Windows.Controls.Page
    {
        public PageAutoSecurity(int IDUserRole)
        {            
            InitializeComponent();
            if (IDUserRole != 1)
            {
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
                int IDAuto = auto.ID;
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
    }
}