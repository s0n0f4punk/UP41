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
    /// Логика взаимодействия для TestPage.xaml
    /// </summary>
    public partial class TestPage : Page
    {
        public TestPage()
        {
            InitializeComponent();
            SortCb.SelectedIndex = 0;
        }

        public void Refresh()
        {
            IEnumerable<Product> products = App.db.Product.Where(x => x.Test.Count() > 0 && x.Order.Count() > 0);

            if (SearchTb.Text != "")
                products = products.Where(x => x.Name.Contains(SearchTb.Text)
                || x.Order.First().OrderNumber.Contains(SearchTb.Text));

            if (SortCb.SelectedIndex == 1)
                products = products.OrderBy(x => x.Name);
            else if (SortCb.SelectedIndex == 2)
                products = products.OrderByDescending(x => x.Name);
            else if (SortCb.SelectedIndex == 3)
                products = products.OrderBy(x => x.Passed.Text);

            MyList.ItemsSource = products.ToList();
        }

        private void Delete_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот тест продукта?", "Подтверждение", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        List<Test> tests = ((sender as Image).DataContext as Product).Test.ToList();
                        foreach (var test in tests)
                            App.db.Test.Remove(test);
                        App.db.SaveChanges();
                        Refresh();
                        MessageBox.Show("Тест успешн удален!");
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            catch (Exception ex) { MessageBox.Show("Невозможео удалить тесты!\n" + ex.Message); }

        }

        private void Edit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new AddEditTestPage((sender as Image).DataContext as Product));
        }

        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            Refresh();
        }

        private void SortCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void AddOrderBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditTestPage(new Product()));
        }

        private void BackButt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NavigationPage());
        }
    }
}
