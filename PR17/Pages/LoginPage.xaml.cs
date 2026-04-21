using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PR17.Pages
{
    public partial class LoginPage : Page
    {
        public static Users CurrentUser;

        public LoginPage()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var user = Core.Context.Users.FirstOrDefault(x =>
                x.Login == LoginBox.Text &&
                x.Password == PasswordBox.Password);

            if (user == null)
            {
                MessageBox.Show("Ошибка входа");
                return;
            }

            CurrentUser = user;

            switch (user.RoleId)
            {
                case 1:
                    MainWindow.Instance.MainFrame.Navigate(new StartPage());
                    break;
                case 2:
                    MainWindow.Instance.MainFrame.Navigate(new MasterPage());
                    break;
                case 3:
                    MainWindow.Instance.MainFrame.Navigate(new ManagerPage());
                    break;
                case 4:
                    MainWindow.Instance.MainFrame.Navigate(new AdminPage());
                    break;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.GoBack();
        }
    }
}