using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace UP41.Cumponents
{
    /// <summary>
    /// Логика взаимодействия для SizeContorl.xaml
    /// </summary>
    public partial class SizeControl : UserControl
    {
        public Size size;

        ProductControl productControl;
        public SizeControl(Size size, ProductControl productControl)
        {
            InitializeComponent();
            this.size = size;
            this.productControl = productControl;
            UnitCb.ItemsSource = App.db.Unit.ToList();
            DataContext = size;
        }
        private void CountTb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Trash_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (size.IdProduct != 0)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот размер?", "Подтверждение", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        if (size.Id != 0)
                        {
                            App.db.Size.Remove(size);
                            App.db.SaveChanges();
                        }
                        productControl.sizes.Remove(this);
                        productControl.RefreshSizes();
                        MessageBox.Show("Размер успешно удален!");
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
        }

        private void NameTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            size.Name = NameTb.Text;
        }

        private void CountTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CountTb.Text != "")
                size.SizeValue = Convert.ToInt32(CountTb.Text);
        }

        private void UnitCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            size.IdUnit = (UnitCb.SelectedItem as Unit).Id;
        }
    }
}
