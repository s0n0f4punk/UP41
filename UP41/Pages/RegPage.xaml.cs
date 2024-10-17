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
using UP41.Pages;

namespace UP41.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
        }

        private void BackButt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthPage());
        }

        private void RegButt_Click(object sender, RoutedEventArgs e)
        {
            char[] forbiddenSymbols = "*&{}|+".ToCharArray();
            char[] BigLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ".ToCharArray();
            char[] Numbers = "1234567890".ToCharArray();
            char[] mass = PassTbx.Password.ToCharArray();
            bool symbolsCheck = false;
            bool lettersCheck = false;
            bool numbersCheck = false;
            for (int i = 0; i < mass.Length; i++)
            {
                for (int j = 0; j < forbiddenSymbols.Length; j++)
                {
                    if (mass[i] == forbiddenSymbols[j]) symbolsCheck = true;
                }

                for (int j = 0; j < BigLetters.Length; j++)
                {
                    if (mass[i] == BigLetters[j]) lettersCheck = true;
                }

                for (int j = 0; j < Numbers.Length; j++)
                {
                    if (mass[i] == Numbers[j]) numbersCheck = true;
                }
            }
            if (LoginTbx.Text == "" || PassTbx.Password == "") MessageBox.Show("Все поля должны быть заполнены.");
            else if (App.db.User.Any(x => x.Login == LoginTbx.Text)) MessageBox.Show("Этот логин занят.");
            else if (PassTbx.Password != PassRepTbx.Password) MessageBox.Show("Пароли не совпадают.");
            else if (PassTbx.Password.Length < 4 || PassTbx.Password.Length > 16) MessageBox.Show("Пароль должен содержать от 4 до 16 символов.");
            else if (symbolsCheck) MessageBox.Show("Пароль не должен содержать следующие символы: * & { } | +");
            else if (!lettersCheck) MessageBox.Show("Пароль должен содержать хотя бы одну заглавную букву");
            else if (!numbersCheck) MessageBox.Show("Пароль должен содержать хотя бы одну цифру");
            else
            {
                try
                {
                    User user = new User();
                    user.Login = LoginTbx.Text;
                    user.Password = PassTbx.Password;
                    user.RoleId = 5;
                    App.db.User.Add(user); 
                    App.db.SaveChanges();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Произошла ошибка. Код ошибки: " + ex.Message);
                }
                finally
                {
                    MessageBox.Show("Вы успешно зарегестировались!");
                    NavigationService.Navigate(new AuthPage());
                }
            }
        }
    }
}
