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
    /// Логика взаимодействия для AddWorkerPage.xaml
    /// </summary>
    public partial class AddWorkerPage : Page
    {
        public AddWorkerPage()
        {
            InitializeComponent();
            CityCbx.ItemsSource = App.db.City.ToList();
            CityCbx.DisplayMemberPath = "Title";
            TaskCbx.ItemsSource = App.db.PerformTasks.ToList();
            TaskCbx.DisplayMemberPath = "Name";
        }

        private void BackButt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new WorkersListPage());
        }

        private void SaveButt_Click(object sender, RoutedEventArgs e)
        {
            if (LoginTbx.Text == "" || PassTbx.Text == "" || SurnameTbx.Text == "" || NameTbx.Text == "" || CityCbx.SelectedIndex == -1 ||
                StreetCbx.SelectedIndex == -1 || HomeTbx.Text == "" || QualCbx.SelectedIndex == -1 || EduCbx.SelectedIndex == -1 || BirthDP.Text == "")
                MessageBox.Show("Пожалуйста, заполните все необходимые данные. ");
            else if (App.db.User.Any(x => x.Login == LoginTbx.Text)) MessageBox.Show("Данный логин уже испольуется.");
            else
            {
                try
                {
                    User user = new User();
                    user.Login = LoginTbx.Text;
                    user.Password = PassTbx.Text;
                    user.Surname = SurnameTbx.Text;
                    user.Name = NameTbx.Text;
                    user.Patronymic = PatronymicTbx.Text;
                    user.BirthDate = BirthDP.DisplayDate;
                    user.Education = EduCbx.Text;
                    user.Qualification = QualCbx.Text;
                    user.RoleId = 6;
                    user.Id_Street = ((Street)StreetCbx.SelectedItem).Id;
                    user.House = HomeTbx.Text;
                    user.Flat = FlatTbx.Text;
                    App.db.User.Add(user);
                    App.db.SaveChanges();
                    foreach (var item in TaskList.Items)
                    {
                        UserTasks ut = new UserTasks();
                        ut.Login = user.Login;
                        ut.IdTask = ((PerformTasks)item).Id;
                        App.db.UserTasks.Add(ut);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка: " + ex.Message);
                }
                finally
                {
                    App.db.SaveChanges();
                    MessageBox.Show("Работник успешно добавлен.");
                    NavigationService.Navigate(new WorkersListPage());
                }
            }
        }

        private void CityCbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StreetSP.Visibility = Visibility.Visible;
            int city = ((City)CityCbx.SelectedItem).Id;
            StreetCbx.ItemsSource = App.db.Street.Where(x=>x.Id_City==city).ToList();
            StreetCbx.DisplayMemberPath = "Title";
        }

        private void AddBut_Click(object sender, RoutedEventArgs e)
        {
            bool isDuplicate = false;
            if (TaskCbx.SelectedIndex == -1) MessageBox.Show("Сначала выберите задачу.");
            else
            {
                foreach (var item in TaskList.Items)
                {
                    if (((PerformTasks)item).Name == ((PerformTasks)TaskCbx.SelectedItem).Name) isDuplicate = true;
                }
                if (isDuplicate) MessageBox.Show("Эта задача уже выбрана.");
                else TaskList.Items.Add((PerformTasks)(TaskCbx.SelectedItem));
            }
        }
    }
}
