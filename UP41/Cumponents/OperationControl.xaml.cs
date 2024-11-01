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

namespace UP41.Cumponents
{
    /// <summary>
    /// Логика взаимодействия для OperationControl.xaml
    /// </summary>
    public partial class OperationControl : UserControl
    {
        public OperationSpecification operation;

        ProductControl productControl;
        public OperationControl(OperationSpecification operation, ProductControl productControl)
        {
            InitializeComponent();
            this.operation = operation;
            this.productControl = productControl;
            NumberTb.Text = operation.Number.ToString();
            TypeEquipmentCb.ItemsSource = App.db.TypeEquipment.ToList();
            DataContext = operation;
        }

        private void Trash_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот операцию?", "Подтверждение", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    if (operation.IdProduct != 0)
                    {
                        App.db.OperationSpecification.Remove(operation);
                        App.db.SaveChanges();
                    }
                    productControl.operations.Remove(this);
                    productControl.RefreshOperations();
                    MessageBox.Show("Операция успешно удалена!");
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void NameTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            operation.Operation = NameTb.Text;
        }


        private void TimeTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex time = new Regex(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$");
            if (time.IsMatch(TimeTb.Text))
            {
                operation.Duration = new TimeSpan(int.Parse(TimeTb.Text.Split(':')[0]), int.Parse(TimeTb.Text.Split(':')[1]), 0);
                TimeTb.Background = Brushes.LightGreen;
            }
            else
                TimeTb.Background = Brushes.Red;
        }

        private void TypeEquipmentCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            operation.IdTypeEquipment = (TypeEquipmentCb.SelectedItem as TypeEquipment).IdTypeEquipment;
        }
    }
}
