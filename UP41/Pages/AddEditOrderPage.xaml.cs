using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using UP41.Cumponents;
using UP41.Windows;

namespace UP41.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditOrderPage.xaml
    /// </summary>
    public partial class AddEditOrderPage : Page
    {
        Order order;
        bool isNew;
        bool canEdit = true;
        OrdersPage orderPage;
        List<Cumponents.Document> documents = new List<Cumponents.Document>();
        public int RoleId;

        private DispatcherTimer timer = new DispatcherTimer();
        public AddEditOrderPage(Order order, bool isNew, OrdersPage page, string title = "Добавить заказ")
        {
            InitializeComponent();
            RoleId = (int)App.db.User.Where(x=>x.Login == App.currentUser).First().RoleId;
            if (RoleId == 4)
                ClientPanel.Visibility = Visibility.Collapsed;

            timer.Interval = new TimeSpan(0, 0, 0, 0, 600);
            timer.Tick += new EventHandler(Tick);
            TitleTb.Text = title;
            this.order = order;
            this.isNew = isNew;
            orderPage = page;
            DataContext = order;
            ClientCb.ItemsSource = App.db.User.Where(x => x.RoleId == 4).ToList();

            if (!isNew)
            {
                ManagerPanel.Visibility = Visibility.Visible;
                documents = App.db.Document.Where(x => x.OrderNumber == order.OrderNumber).ToList();
                ClientCb.SelectedItem = order.User;
                DateOrderDp.SelectedDate = order.DateOrder;
                EmployeeTb.Text = order.User1.FIO;
                ClientCb.IsEnabled = false;

                if (order.CurrentStatus.IdStatus != 1)
                {
                    MainPanel.IsEnabled = false;
                    SaveBtn.Visibility = Visibility.Collapsed;
                    canEdit = false;
                }
            }
            else
                DateOrderDp.SelectedDate = DateTime.Now.Date;

            if (RoleId == 3 && RoleId == 1)
            {
                MainPanel.IsEnabled = false;
                SaveBtn.Visibility = Visibility.Collapsed;
                canEdit = false;
            }

            if (RoleId == 2)
            {
                MainPanel.IsEnabled = true;
                NamePanel.IsEnabled = false;
                ClientPanel.IsEnabled = false;
                canEdit = false;
                SaveBtn.Visibility = Visibility.Visible;
            }
        }
        private void Tick(object sender, EventArgs e)
        {
            MyPanel.Visibility = Visibility.Collapsed;
            MyRectangle.Visibility = Visibility.Collapsed;
            timer.Stop();
        }

        public void OpenPanel()
        {
            MyRectangle.Visibility = Visibility.Visible;
            MyPanel.Visibility = Visibility.Visible;
            Storyboard fadeOutboard = (Storyboard)this.Resources["FadeIn"];
            fadeOutboard.Begin(MyRectangle);
            Storyboard slideOutboard = (Storyboard)this.Resources["SlideIn"];
            slideOutboard.Begin(MyPanel);
        }
        public void ClosePanel()
        {
            timer.Start();
            Storyboard fadeOutboard = (Storyboard)this.Resources["FadeOut"];
            fadeOutboard.Begin(MyRectangle);
            Storyboard slideOutboard = (Storyboard)this.Resources["SlideOut"];
            slideOutboard.Begin(MyPanel);
        }

        private void SaveBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string mistake = "";

            if (NameTb.Text == "" && mistake == "")
                mistake = "Вы не заполнили наименование!";
            if (ClientCb.SelectedIndex == -1 && mistake == "")
                mistake = "Вы не заполнили заказчика!";
            if (mistake == "" && AmountTb.Text != string.Empty && decimal.TryParse(AmountTb.Text.Replace('.', ','), out decimal result))
                order.Amount = result;
            else if (mistake == "" && AmountTb.Text != string.Empty)
                mistake = "Вы неправильно заполнили стоимость!";

            if (mistake != "")
            {
                MessageBox.Show(mistake);
                return;
            }
            if (isNew && RoleId != 4)
                order.LoginCustomer = (ClientCb.SelectedItem as User).Login;
            else if (isNew && RoleId == 4)
                order.LoginCustomer = App.currentUser;

            if (RoleId != 4)
                order.LoginManager = App.currentUser;

            if (isNew)
                order.OrderNumber = GenerateOrderNumber(ClientCb.SelectedItem as User);
            order.DateOrder = DateTime.Now.Date;

            if (isNew)
            {
                if (App.productControl != null)
                    order.IdProduct = App.productControl.product.Id;
                App.db.Order.Add(order);
                App.productControl = null;
                App.db.StatusOrder.Add(new StatusOrder()
                {
                    IdStatus = RoleId == 4 ? 1 : 3,
                    DateChange = DateTime.Now.Date,
                    TimeChange = DateTime.Now.TimeOfDay,
                    OrderNumber = order.OrderNumber,
                });
            }

            foreach (var doc in documents)
            {
                if (doc.Id == 0)
                {
                    doc.OrderNumber = order.OrderNumber;
                    App.db.Document.Add(doc);
                }
            }

            App.db.SaveChanges();
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
            orderPage.Refresh();
            MessageBox.Show("Изменения успешно сохранены!");
        }
        private string GenerateOrderNumber(User client)
        {
            string number = "ФИГГГГММДД№№";

            number = number.Replace('Ф', client.Surname == null ? '_' : client.Surname.ToUpper()[0]);
            number = number.Replace('И', client.Name == null ? '_' : client.Name.ToUpper()[0]);
            number = number.Replace("ГГГГ", DateTime.Now.Year.ToString());
            number = number.Replace("ММ", DateTime.Now.Month.ToString().Length < 2 ? $"0{DateTime.Now.Month.ToString()}" : DateTime.Now.Month.ToString());
            number = number.Replace("ДД", DateTime.Now.Day.ToString().Length < 2 ? $"0{DateTime.Now.Day.ToString()}" : DateTime.Now.Day.ToString());
            number = number.Replace("№№", $"{client.Order.Count() % 99 + 1}".Length < 2 ? $"0{client.Order.Count() % 99 + 1}" : $"{client.Order.Count() % 99 + 1}");

            return number;
        }

        private void Menu_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OpenPanel();
        }

        private void MyRectangle_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ClosePanel();
        }

        private void DocumentBtn_Click(object sender, RoutedEventArgs e)
        {
            new DocumentWindow(documents, canEdit).ShowDialog();
        }

        private void ProductBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!isNew)
                new ProductWindow(order, isNew, canEdit).ShowDialog();
            else if (isNew && App.productControl != null)
                new ProductWindow(null, isNew, canEdit).ShowDialog();
            else
                new ProductWindow(order, isNew, canEdit).ShowDialog();
        }

        private void BackButt_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                if (isNew && App.productControl != null)
                {
                    App.productControl.DeleteProduct();
                    App.db.SaveChanges();
                }
                App.productControl = null;
                NavigationService.Navigate(new OrdersPage());
            }
        }
    }
}
