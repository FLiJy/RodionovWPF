using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PR17.Pages
{
    public partial class AccountPage : Page
    {
        public AccountPage()
        {
            InitializeComponent();
            LoadData();
        }

        void LoadData()
        {
            var user = LoginPage.CurrentUser;

            if (user == null)
            {
                MessageBox.Show("Вы не авторизованы");
                return;
            }

            // Информация о пользователе
            UserInfo.Text =
                $"ФИО: {user.FullName}\n" +
                $"Телефон: {user.Phone}\n" +
                $"Логин: {user.Login}";

            // Записи
            var appointments = Core.Context.Appointments
                .Where(x => x.ClientId == user.Id)
                .ToList();

            AppointmentsList.ItemsSource = appointments
                .Select(x =>
                    $"{x.DateTime} | {x.Status} | {x.Price} ₽");

            // Заказы
            var orders = Core.Context.Orders
                .Where(x => x.UserId == user.Id)
                .ToList();

            OrdersList.ItemsSource = orders
                .Select(x =>
                    $"{x.OrderDate} | {x.Status} | {x.PaymentMethod}");
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.GoBack();
        }
    }
}