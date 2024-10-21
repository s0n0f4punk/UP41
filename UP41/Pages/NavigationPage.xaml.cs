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
            if (RoleId != 3) WorkersButt.Visibility = Visibility.Collapsed;
            if (RoleId == 4) MaterialsButt.Visibility = Visibility.Collapsed;
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
    }
}
