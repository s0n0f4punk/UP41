using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UP41.Cumponents;

namespace UP41.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditTestPage.xaml
    /// </summary>
    public partial class AddEditTestPage : Page
    {
        public Product product;
        public List<TestUC> tests = new List<TestUC>();
        bool isNew = true;
        public AddEditTestPage(Product product)
        {
            InitializeComponent();
            this.product = product;

            Order order = product.Order.FirstOrDefault();
            List<Product> products = new List<Product>();
            if (order != null && order.CurrentStatus.IdStatus >= 8)
                products = App.db.Product.ToList();
            else
            {
                foreach (var pro in App.db.Product)
                {
                    Order order1 = pro.Order.FirstOrDefault();
                    if (order1 != null && order1.CurrentStatus.IdStatus != 2 && order1.CurrentStatus.IdStatus < 8)
                        products.Add(pro);
                }
            }
            ProductCb.ItemsSource = products;

            if(product.Test.Count() != 0)
            {
                TitleTb.Text = "Редактировать тест продукта";
                isNew = true;
                ProductCb.IsEnabled = false;
            }
            foreach (var test in product.Test)
                tests.Add(new TestUC(test, this));

            DataContext = product;
            Refresh();

            ProductCb.SelectedItem = product;
        }

        public void Refresh()
        {
            MyPanel.Children.Clear();
            foreach (var test in tests)
                MyPanel.Children.Add(test);
        }

        private void Back_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void SaveBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string mistake = "";

            if (ProductCb.SelectedIndex == -1 && mistake == "")
                mistake = "Вы не выбрали изделие для теста!";
            if (tests.Count() == 0 && mistake == "")
                mistake = "У теста нет критериев оценки!";

            if (mistake != "")
            {
                MessageBox.Show(mistake);
                return;
            }
            
            foreach(var test in tests)
            {
                test.test.IdProduct = product.Id;
                if (test.test.Id == 0)
                    App.db.Test.Add(test.test);
            }

            App.db.SaveChanges();
            NavigationService.Navigate(new TestPage());
            MessageBox.Show("Изменения успешно сохранены!");
        }

        private void AddBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tests.Add(new TestUC(new Test(), this));
            Refresh();
        }

        private void ProductCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            product = ProductCb.SelectedItem as Product;
            foreach (var test in tests)
                test.test.IdProduct = product.Id;
        }

        private void BackButt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TestPage());
        }
    }
}