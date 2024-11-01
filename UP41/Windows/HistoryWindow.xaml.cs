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
using System.Windows.Shapes;
using UP41.Cumponents;

namespace UP41.Windows
{
    /// <summary>
    /// Логика взаимодействия для HistoryWindow.xaml
    /// </summary>
    public partial class HistoryWindow : Window
    {
        public HistoryWindow()
        {
            InitializeComponent();
            Refresh();
        }

        private void Refresh()
        {
            IEnumerable<StatusOrder> statuses = App.db.StatusOrder;
            int RoleId = (int)App.db.User.Where(x=>x.Login == App.currentUser).First().RoleId;
            if (RoleId == 5)
                statuses = statuses.Where(x => x.Order.LoginManager == null || x.Order.LoginManager == App.currentUser);

            MyList.ItemsSource = statuses.ToList();
        }
    }
}
