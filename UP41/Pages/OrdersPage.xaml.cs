﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UP41.Cumponents;
using UP41.Windows;

namespace UP41.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        public int RoleId;
        public OrdersPage()
        {
            InitializeComponent();
            SortCb.SelectedIndex = 0;
            FilterCb.SelectedIndex = 0;
            if (App.productControl != null)
            {
                App.productControl.DeleteProduct();
                App.db.SaveChanges();
                App.productControl = null;
            }

            RoleId = (int)App.db.User.Where(x => x.Login == App.currentUser).FirstOrDefault().RoleId;
            if (RoleId == 3)
            {
                StatusBtn.Visibility = Visibility.Collapsed;
                AddOrderBtn.Visibility = Visibility.Collapsed;
            }

            if (RoleId == 2 || RoleId == 1)
                AddOrderBtn.Visibility = Visibility.Collapsed;

            if (RoleId != 3 && RoleId != 5)
                HistoryBtn.Visibility = Visibility.Collapsed;
        }

        public void Refresh()
        {
            IEnumerable<Order> orders = App.db.Order;
            if (RoleId == 4)
                orders = orders.Where(x => x.LoginCustomer == App.currentUser);
            if (RoleId == 5)
                orders = orders.Where(x => x.LoginManager == null || x.LoginManager == App.currentUser);
            if (RoleId == 2)
                orders = orders.Where(x => x.CurrentStatus.IdStatus == 3);
            if (RoleId == 1)
                orders = orders.Where(x => x.CurrentStatus.IdStatus == 6 || x.CurrentStatus.IdStatus == 7);



            if (SearchTb.Text != "")
                orders = orders.Where(x => x.User1.FIO.Contains(SearchTb.Text) || x.User.FIO.Contains(SearchTb.Text)
                || x.Name.Contains(SearchTb.Text));

            if (FilterCb.SelectedIndex == 1)
                orders = orders.Where(x => x.CurrentStatus.IdStatus == 1 || x.CurrentStatus.IdStatus == 3 || x.CurrentStatus.IdStatus == 4);
            else if (FilterCb.SelectedIndex == 2)
                orders = orders.Where(x => x.CurrentStatus.IdStatus == 8 || x.CurrentStatus.IdStatus == 9);
            else if (FilterCb.SelectedIndex == 3)
                orders = orders.Where(x => x.CurrentStatus.IdStatus == 5 || x.CurrentStatus.IdStatus == 6 || x.CurrentStatus.IdStatus == 7);
            else if (FilterCb.SelectedIndex == 4)
                orders = orders.Where(x => x.CurrentStatus.IdStatus == 2);

            if (SortCb.SelectedIndex == 1)
                orders = orders.OrderBy(x => x.Name);
            else if (SortCb.SelectedIndex == 2)
                orders = orders.OrderByDescending(x => x.Name);
            else if (SortCb.SelectedIndex == 3)
                orders = orders.OrderBy(x => x.Amount);
            else if (SortCb.SelectedIndex == 4)
                orders = orders.OrderByDescending(x => x.Amount);
            else if (SortCb.SelectedIndex == 5)
                orders = orders.OrderBy(x => x.LoginCustomer);

            MyList.ItemsSource = orders.ToList();
        }

        private void Delete_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот заказ?", "Подтверждение", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    App.db.Order.Remove((sender as Image).DataContext as Order);
                    App.db.SaveChanges();
                    MessageBox.Show("Заказ успешно удален!");
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void Edit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new AddEditOrderPage((sender as Image).DataContext as Order, false, this, "Редактировать заказ"));
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
            NavigationService.Navigate(new AddEditOrderPage(new Order(), true, this));
        }

        private void FilterCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void StatusBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Order order = MyList.SelectedItem as Order;
            if (order != null)
            {
                new StatusWindows(order).ShowDialog();
            }
            else MessageBox.Show("Вы не выбрали заказ!");
        }

        private void MyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Order order = MyList.SelectedItem as Order;
            if (order != null && order.CurrentStatus.IdStatus != 2 && order.CurrentStatus.IdStatus != 9
                && RoleId != 3)
            {
                if (RoleId == 4 && order.CurrentStatus.IdStatus < 5 && order.CurrentStatus.IdStatus != 2)
                    StatusBtn.Visibility = Visibility.Visible;
                else if (RoleId == 5 || RoleId == 2)
                    StatusBtn.Visibility = Visibility.Visible;
                else if (RoleId == 1 && (order.CurrentStatus.IdStatus == 6 || order.CurrentStatus.IdStatus == 7))
                    StatusBtn.Visibility = Visibility.Visible;
                else
                    StatusBtn.Visibility = Visibility.Collapsed;
            }
            else
                StatusBtn.Visibility = Visibility.Collapsed;
        }

        private void HistoryBtn_Click(object sender, RoutedEventArgs e)
        {
            new HistoryWindow().ShowDialog();
        }

        private void BackButt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NavigationPage());
        }
    }
}
