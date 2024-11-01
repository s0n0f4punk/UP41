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
using UP41.Cumponents;

namespace UP41.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditFailurePage.xaml
    /// </summary>
    public partial class AddEditFailurePage : Page
    {
        FailurePage page;
        HardwareFailure hardware;
        TimeSpan startTime;
        TimeSpan endTime;
        public AddEditFailurePage(HardwareFailure hardware, FailurePage page)
        {
            InitializeComponent();
            if (hardware.Id != 0)
            {
                TitleTb.Text = "Редактировать поломку";
                StartDate.SelectedDate = hardware.FailureStart.Value.Date;
                StartTimeTb.Text = hardware.FailureStart.Value.ToString("HH\\:mm");
                EndDate.SelectedDate = hardware.FailureEnd.Value.Date;
                EndTimeTb.Text = hardware.FailureEnd.Value.ToString("HH\\:mm");
            }
            this.hardware = hardware;
            this.page = page;
            EquipmentCb.ItemsSource = App.db.Equipment.ToList();
            TypeEquipmentCb.ItemsSource = App.db.TypeEquipment.ToList();
            DataContext = hardware;
        }

        private void SaveBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string mistake = "";

            if (EquipmentCb.SelectedIndex == -1 && mistake == "")
                mistake = "Вы не выбрали оборудование!";
            if (ReasonTb.Text == string.Empty && mistake == "")
                mistake = "Вы не написали причину поломки!";
            if (StartDate.SelectedDate == null && mistake == "")
                mistake = "Вы не выбрали дату начала поломки!";
            if (EndDate.SelectedDate == null && mistake == "")
                mistake = "Вы не выбрали дату конца поломки!";
            if (StartTimeTb.Background != Brushes.LightGreen && mistake == "")
                mistake = "Вы не выбрали время начала поломки!";
            if (EndTimeTb.Background != Brushes.LightGreen && mistake == "")
                mistake = "Вы не выбрали время конца поломки!";
            if (StartDate.SelectedDate.Value.Add(startTime) > EndDate.SelectedDate.Value.Add(endTime) && mistake == "")
                mistake = "Время начала поломки не может быть позже конца поломки!";

            if (mistake != "")
            {
                MessageBox.Show(mistake);
                return;
            }

            hardware.FailureStart = StartDate.SelectedDate.Value.Add(startTime);
            hardware.FailureEnd = EndDate.SelectedDate.Value.Add(endTime);

            if (hardware.Id == 0)
                App.db.HardwareFailure.Add(hardware);

            App.db.SaveChanges();
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
                page.Refresh();
            }
            MessageBox.Show("Изменения успешно сохранены!");
        }

        private void StartTimeTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateTime(StartTimeTb, out startTime);
        }
        private void EndTimeTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateTime(EndTimeTb, out endTime);
        }
        private void ValidateTime(TextBox timeText, out TimeSpan timeSpan)
        {
            timeSpan = TimeSpan.Zero;
            Regex time = new Regex(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$");
            if (time.IsMatch(timeText.Text))
            {
                timeSpan = new TimeSpan(int.Parse(timeText.Text.Split(':')[0]), int.Parse(timeText.Text.Split(':')[1]), 0);
                timeText.Background = Brushes.LightGreen;
            }
            else
                timeText.Background = Brushes.Red;
        }

        private void EquipmentCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TypeEquipmentCb.SelectedItem = (EquipmentCb.SelectedItem as Equipment).TypeEquipment;
        }

        private void BackButt_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.Navigate(new FailurePage());
        }

        private void BackButt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FailurePage());
        }
    }
}
