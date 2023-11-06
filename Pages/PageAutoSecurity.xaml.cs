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
        public PageAutoSecurity()
        {
            InitializeComponent();
            DgrDataAuto.ItemsSource = AppConnect.modelOdb.AutoTransport.ToArray();
        }
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.FrameMain.Content = null;
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

        }
        private void DgrDataAuto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //учесть пустой выбор таблицы
            AutoTransport product = (AutoTransport)DgrDataAuto.SelectedItem;
            int IDAuto = product.ID;
            int IDPassAuto;
            var PassAuto = AppConnect.modelOdb.Pass.FirstOrDefault(x => x.IDTypePass == 4);
            var selectedAuto = AppConnect.modelOdb.AutoTransport.FirstOrDefault(x => x.ID == IDAuto);
            if (selectedAuto != null && PassAuto != null)
            {
                IDPassAuto = PassAuto.ID;
                TbxIDPassAuto.Text = IDPassAuto.ToString();
                TbxMark.Text = selectedAuto.Mark;
                TbxStateNumber.Text = selectedAuto.StateNumber;
                TbxSeriesNumber.Text = selectedAuto.SeriesNumber;
                TbxColor.Text = selectedAuto.Color;
            }
        }
        private void TbxFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            DgrDataAuto.ItemsSource = AppConnect.modelOdb.AutoTransport.Where(x =>
                                  x.ID.ToString().Contains(TbxIDPassAuto.Text) ||
                                  x.Mark.Contains(TbxFind.Text) ||
                                  x.StateNumber.Contains(TbxFind.Text) ||
                                  x.SeriesNumber.ToString().Contains(TbxFind.Text) ||
                                  x.Color.ToString().Contains(TbxFind.Text) ||
                                  x.IDPass.ToString().Contains(TbxFind.Text)
                              ).ToArray();
        }

        private void TbxMark_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}