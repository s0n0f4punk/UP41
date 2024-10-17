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
    /// Логика взаимодействия для PageSmh.xaml
    /// </summary>
    public partial class PageSmh : Page
    {
        public PageSmh()
        {
            InitializeComponent();
            huy.Text = App.db.User.Where(x => x.Id == App.currentUser).First().Login + " " + App.db.User.Where(x => x.Id == App.currentUser).First().RoleId;
        }

        private void huybutt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthPage());
        }
    }
}
