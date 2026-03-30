using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PR14.Pages
{
    public partial class LoginPage : Page
    {
     
        private int failedAttempts = 0;

        public LoginPage()
        {
            InitializeComponent();
        }

        public bool Auth(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return false;

            var user = Core.Context.Users
                .FirstOrDefault(u => u.Login == login && u.Password == password);

            if (user == null)
                return false;

            Core.CurrentUser = user;
            return true;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (!Auth(login, password))
            {
                failedAttempts++;

                MessageBox.Show("Неверный логин или пароль.");

                // ===== КАПЧА (ЗАКОММЕНТИРОВАНО) =====
                /*
                if (failedAttempts >= 3)
                {
                    string captcha = GenerateCaptcha();
                    MessageBox.Show($"Введите капчу: {captcha}");

                    // тут должен быть ввод капчи пользователем (например через TextBox)
                    string userInput = ""; // сюда подставить ввод

                    if (userInput != captcha)
                    {
                        MessageBox.Show("Неверная капча.");
                        return;
                    }
                }
                */
                // ===================================

                return;
            }

            failedAttempts = 0;

            MessageBox.Show("Вы успешно вошли.");
            NavigationService.Navigate(new MainPage());
        }

 

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegisterPage());
        }

        private void Main_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }
    }
}
