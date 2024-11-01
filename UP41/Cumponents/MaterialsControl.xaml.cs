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
    /// Логика взаимодействия для MaterialsControl.xaml
    /// </summary>
    public partial class MaterialsControl : UserControl
    {
        public ProductMaterial material;

        ProductControl productControl;
        public MaterialsControl(ProductMaterial material, ProductControl productControl)
        {
            InitializeComponent();
            this.material = material;
            this.productControl = productControl;
            MaterialCb.ItemsSource = App.db.Material.ToList();
            DataContext = material;
        }


        private void CountTb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void MaterialCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            material.MaterialArticle = (MaterialCb.SelectedItem as Material).Article;
        }

        private void Trash_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот материал?", "Подтверждение", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    if(material.Id != 0)
                    {
                        App.db.ProductMaterial.Remove(material);
                        App.db.SaveChanges();
                    }
                        productControl.materials.Remove(this);
                        productControl.RefreshMaterals();
                    MessageBox.Show("Материал успешно удален!");
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void CountTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CountTb.Text != "")
                material.Count = Convert.ToInt32(CountTb.Text);
        }
    }
}
