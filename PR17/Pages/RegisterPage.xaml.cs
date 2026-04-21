using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PR17.Pages
{
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                MessageBox.Show("Заполните обязательные поля");
                return;
            }

            var exists = Core.Context.Users
                .Any(x => x.Login == LoginBox.Text);

            if (exists)
            {
                MessageBox.Show("Логин уже занят");
                return;
            }

            var user = new Users
            {
                FullName = FullNameBox.Text,
                Phone = PhoneBox.Text,
                Login = LoginBox.Text,
                Password = PasswordBox.Password,
                RoleId = 1,
                IsActive = true
            };

            Core.Context.Users.Add(user);
            Core.Context.SaveChanges();

            MessageBox.Show("Аккаунт создан");

            MainWindow.Instance.MainFrame.Navigate(new LoginPage());
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.GoBack();
        }
    }
}