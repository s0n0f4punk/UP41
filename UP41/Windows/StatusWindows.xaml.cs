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
using UP41.Pages;

namespace UP41.Windows
{
    /// <summary>
    /// Логика взаимодействия для StatusWindows.xaml
    /// </summary>
    public partial class StatusWindows : Window
    {
        Order order;
        StatusOrder statusOrder;
        public int RoleId;
        public StatusWindows(Order order)
        {
            InitializeComponent();
            this.order = order;
            statusOrder = new StatusOrder()
            {
                IdOldStatus = order.CurrentStatus.IdStatus,
                OrderNumber = order.OrderNumber,
            };

            OldStatusCb.ItemsSource = App.db.OrderStatus.ToList();


            RoleId = (int)App.db.User.Where(x=>x.Login == App.currentUser).First().RoleId;
            if (RoleId != 4 && RoleId != 2 && RoleId != 1)
                NewStatusCb.ItemsSource = App.db.OrderStatus.Where(x => x.Id == 2 || (order.CurrentStatus.IdStatus != 1 && x.Id == order.CurrentStatus.IdStatus + 1)
                || (order.CurrentStatus.IdStatus == 1 && x.Id == 3)).ToList();
            else if (RoleId == 2)
                NewStatusCb.ItemsSource = App.db.OrderStatus.Where(x => x.Id == 4).ToList();
            else if (RoleId == 1)
                NewStatusCb.ItemsSource = App.db.OrderStatus.Where(x => x.Id == order.CurrentStatus.IdStatus + 1).ToList();
            else
                NewStatusCb.ItemsSource = App.db.OrderStatus.Where(x => x.Id == 2).ToList();


            DataContext = statusOrder;
            OldStatusCb.SelectedItem = order.CurrentStatus.OrderStatus;
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (NewStatusCb.SelectedIndex == -1)
                {
                    MessageBox.Show("Вы не выбрали новый статус!");
                    return;
                }
                if (DescriptionPanel.Visibility == Visibility.Visible && DescriptionTb.Text == "")
                {
                    MessageBox.Show("Вы не указали причину отмены заказа!");
                    return;
                }
                
                if (statusOrder.IdOldStatus == 7 && statusOrder.IdStatus == 8 && order.Product != null)
                {
                    if(order.Product.Test.Any(x => x.isPassed == false))
                    {
                        MessageBox.Show("Не все тесты пройдены!");
                        return;
                    }
                }

                if (statusOrder.IdOldStatus == 5 && statusOrder.IdStatus == 6)
                {
                    var materials = order.GetMaterials();
                    foreach (var mat in materials)
                    {
                        Material material = App.db.Material.FirstOrDefault(x => x.Article == mat.Key.Article && x.Count >= mat.Value);
                        if (material != null)

                            material.Count -= mat.Value;
                        else
                        {
                            MessageBox.Show("На складе не хватает количества материала, закупитесь!");
                            return;
                        }
                    }
                    foreach (var mat in order.GetAccessories())
                    {
                        Accessories accessories = App.db.Accessories.FirstOrDefault(x => x.Article == mat.Key.Article && x.Count >= mat.Value);
                        if (accessories != null)
                            accessories.Count -= mat.Value;
                        else
                        {
                            MessageBox.Show("На складе не хватает количества комплектующих, закупитесь!");
                            return;
                        }
                    }
                    App.db.SaveChanges();
                }

                statusOrder.DateChange = DateTime.Now.Date;
                statusOrder.TimeChange = DateTime.Now.TimeOfDay;

                if (order.LoginManager == null)
                    order.LoginManager = App.currentUser;

                App.db.StatusOrder.Add(statusOrder);
                App.db.SaveChanges();
                App.mainWindow.MainFrame.Navigate(new OrdersPage());
                this.Close();
                MessageBox.Show("Статус успешно изменен!");
            }
            catch (Exception ex) { MessageBox.Show($"Не удалось сохранить новый статус!\n{ex.Message}"); }
        }

        private void NewStatusCb_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            OrderStatus status = NewStatusCb.SelectedItem as OrderStatus;
            if (status.Id == 2)
                DescriptionPanel.Visibility = Visibility.Visible;
            else
                DescriptionPanel.Visibility = Visibility.Collapsed;
            statusOrder.IdStatus = status.Id;
        }
    }
}
