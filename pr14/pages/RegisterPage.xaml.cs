using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PR14.Pages
{
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        // 🔹 Метод для тестов
        public bool Register(string fullName, string login, string password)
        {
            if (string.IsNullOrEmpty(fullName) ||
                string.IsNullOrEmpty(login) ||
                string.IsNullOrEmpty(password))
                return false;

            bool loginExists = Core.Context.Users
                .Any(u => u.Login == login);

            if (loginExists)
                return false;

            var newUser = new Users
            {
                FullName = fullName,
                Login = login,
                Password = password
            };

            Core.Context.Users.Add(newUser);
            Core.Context.SaveChanges();

            return true;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string fullName = FullNameBox.Text.Trim();
            string login = LoginBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (!Register(fullName, login, password))
            {
                MessageBox.Show("Ошибка регистрации.");
                return;
            }

            MessageBox.Show("Регистрация успешна.");
            NavigationService.Navigate(new LoginPage());
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }
    }
}
