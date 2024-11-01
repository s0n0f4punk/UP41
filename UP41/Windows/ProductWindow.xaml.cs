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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using UP41.Cumponents;

namespace UP41.Windows
{
    /// <summary>
    /// Логика взаимодействия для ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        Order order;
        ProductControl productControl;

        public ProductWindow(Order order, bool isNew, bool canEdit)
        {
            InitializeComponent();
            if (order != null)
            {
                this.order = order;
                if (isNew)
                    productControl = new ProductControl(true, new Product(), null, null);
                else if (order.Product != null)
                    productControl = new ProductControl(true, order.Product, null, null);
            }
            else { productControl = App.productControl; }

            MyPanel.IsEnabled = canEdit;

            if(App.productControl != null)
                MyPanel.Children.Add(productControl);
            else
            {
                productControl = new ProductControl(true, new Product(), null, null);
                MyPanel.Children.Add(productControl);
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
            {
                MessageBox.Show("Вы не заполнили все нужные поля!");
                return;
            }

            if (Save())
            {

                this.Close();
                MessageBox.Show("Успешно сохранено!");
            }
        }

        private bool Validate()
        {
            return productControl.CheckData();
        }

        private bool Save()
        {
            try
            {
                //Save products (каскадный метод)
                productControl.SaveProduct();

                //Save hierarchy (product detail) (каскадный метод)
                productControl.SaveHierarchy();

                App.db.SaveChanges();
                App.productControl = productControl;

                return true;
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка при сохранении!\n{ex.Message}"); return false; }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите закрыть окно изделий?", "Подтверждение", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    MyPanel.Children.Clear();
                    break;
                case MessageBoxResult.No:
                    e.Cancel = true;
                    return;
            }
        }
    }
}
