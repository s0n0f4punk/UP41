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
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
            App.currentUser = 0;
            App.Current.Properties[0] = 0;
        }

        private void EnterButt_Click(object sender, RoutedEventArgs e)
        {
            if (App.db.User.Any(x => x.Login == LoginTbx.Text && x.Password == PassTbx.Password))
            {
                App.currentUser = App.db.User.Where(x => x.Login == LoginTbx.Text && x.Password == PassTbx.Password).First().Id;
                if ((bool)RemberCheck.IsChecked) App.Current.Properties[0] = App.currentUser;
                NavigationService.Navigate(new PageSmh());
            }
            else MessageBox.Show("Неверный логин или пароль");
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegPage());
        }
    }
}
