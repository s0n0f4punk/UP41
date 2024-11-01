using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UP41.Cumponents;
using Material = UP41.Cumponents.Material;

namespace UP41.Pages
{
    /// <summary>
    /// Логика взаимодействия для MaterialsPage.xaml
    /// </summary>
    public partial class MaterialsPage : Page
    {
        public MaterialsPage()
        {
            InitializeComponent();
            MaterialFilterCb.ItemsSource = App.db.Storage.ToList();
            MaterialFilterCb.DisplayMemberPath = "Name";
            AccessoriesFilterCb.ItemsSource = App.db.Storage.ToList();
            AccessoriesFilterCb.DisplayMemberPath = "Name";
            Refresh();
        }

        public void Refresh()
        {
            IEnumerable<Material> materials = App.db.Material.ToList();
            IEnumerable<Accessories> accessories = App.db.Accessories.ToList();

            if (MaterialFilterCb.SelectedIndex != -1)
            {
                materials = materials.Where(x=>x.IdStorage == (MaterialFilterCb.SelectedItem as Storage).Id).ToList();
            }
            if (AccessoriesFilterCb.SelectedIndex != -1)
            {
                accessories = accessories.Where(x => x.IdStorage == (AccessoriesFilterCb.SelectedItem as Storage).Id).ToList();
            }

            MaterialsList.ItemsSource = materials;
            ComponentsList.ItemsSource = accessories;

            MaterialCountTb.Text = MaterialsList.Items.Count.ToString() + " из " + App.db.Material.Count().ToString();
            decimal MPrice = 0;
            foreach (Material material in MaterialsList.Items)
                MPrice += (material.PriceOneKg == null ? 0 : (decimal)material.PriceOneKg);
            MaterialPriceTb.Text = $"{MPrice}";
            decimal APrice = 0;
            foreach (Accessories accessory in accessories)
                APrice += (accessory.Price == null ? 0 : (decimal)accessory.Price);
            AccessoriesPriceTb.Text = $"{APrice}";
            AccessoriesCountTb.Text = ComponentsList.Items.Count.ToString() + " из " + App.db.Accessories.Count().ToString();
        }

        private void AccessoriesFilterCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void MaterialFilterCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void AddAccessoryButt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddAccessoryPage(new Accessories()));
        }

        private void DeleteAcessoryButt_Click(object sender, RoutedEventArgs e)
        {
            if (ComponentsList.SelectedItem != null)
            {
                Accessories delete = ComponentsList.SelectedItem as Accessories;
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить материал: " + App.db.Accessories.Where(x => x.Article == delete.Article).First().Name + "?", "Подтверждение", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        App.db.Accessories.Remove(ComponentsList.SelectedItem as Accessories);
                        App.db.SaveChanges();
                        Refresh();
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            else MessageBox.Show("Сначала выберите компонент!");
        }

        private void AddMaterialButt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddMaterialPage(new Material()));
        }

        private void DeleteMaterialButt_Click(object sender, RoutedEventArgs e)
        {
            if (MaterialsList.SelectedItem != null)
            {
                Material delete = MaterialsList.SelectedItem as Material;
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить материал: " + App.db.Material.Where(x => x.Article == delete.Article).First().Name + "?", "Подтверждение", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        App.db.Material.Remove(MaterialsList.SelectedItem as Material);
                        App.db.SaveChanges();
                        Refresh();
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            else MessageBox.Show("Сначала выберите материал!");
        }

        private void BackButt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NavigationPage());
        }
    }
}
