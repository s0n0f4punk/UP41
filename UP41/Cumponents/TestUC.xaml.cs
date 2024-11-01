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
using System.Xml.Linq;
using UP41.Pages;

namespace UP41.Cumponents
{
    /// <summary>
    /// Логика взаимодействия для TestUC.xaml
    /// </summary>
    public partial class TestUC : UserControl
    {
        public Test test;
        AddEditTestPage page;
        public TestUC(Test test, AddEditTestPage page)
        {
            InitializeComponent();
            this.page = page;
            this.test = test;

            if (test.Id == 0)
                PassedCb.IsChecked = true;
            if (test.isPassed != null && test.isPassed == false)
            {
                PassedCb.IsChecked = false;
                ReasonTb.Visibility = System.Windows.Visibility.Visible;
            }
            else if (test.isPassed != null && test.isPassed == true)
            {
                PassedCb.IsChecked = true;
                ReasonTb.Visibility = System.Windows.Visibility.Collapsed;
            }

            DataContext = test;
        }

        private void Trash_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот критерий", "Подтверждение", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    page.tests.Remove(this);
                    if (test.Id != 0)
                        App.db.Test.Remove(test);
                    page.Refresh();
                    MessageBox.Show("Критерий успешно удален!");
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void PassedCb_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            ReasonTb.Visibility = System.Windows.Visibility.Collapsed;
            test.isPassed = PassedCb.IsChecked;
        }

        private void PassedCb_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            ReasonTb.Visibility = System.Windows.Visibility.Visible;
            test.isPassed = PassedCb.IsChecked;
        }

        private void ReasonTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            test.Description = ReasonTb.Text;
        }
    }
}
