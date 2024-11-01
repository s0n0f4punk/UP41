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

namespace UP41.Pages
{
    /// <summary>
    /// Логика взаимодействия для NavigationPage.xaml
    /// </summary>
    public partial class NavigationPage : Page
    {
        public NavigationPage()
        {
            InitializeComponent();
            int RoleId = (int)App.db.User.Where(x => x.Login == App.currentUser).FirstOrDefault().RoleId;
            OrdersButt.Visibility = Visibility.Collapsed;
            if (RoleId != 3) WorkersButt.Visibility = Visibility.Collapsed;
            if (RoleId != 3) PlanButt.Visibility = Visibility.Collapsed;
            if (RoleId == 4) MaterialsButt.Visibility = Visibility.Collapsed;
            if (RoleId == 3 || RoleId == 4 || RoleId == 5) OrdersButt.Visibility = Visibility.Visible;
            if (RoleId != 1) FailureButt.Visibility = Visibility.Collapsed;
            if (RoleId != 1) TestButt.Visibility = Visibility.Collapsed;
        }

        private void ExitButt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthPage());
        }

        private void WorkersButt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new WorkersListPage());
        }

        private void MaterialsButt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MaterialsPage());
        }

        private void PlanButt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PlanPage());
        }

        private void OrdersButt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrdersPage());
        }

        private void FailrueButt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FailurePage());
        }

        private void TestButt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TestPage());
        }
    }
}
