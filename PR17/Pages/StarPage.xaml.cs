using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PR17.Pages
{
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();

            ServicesList.ItemsSource = Core.Context.Services.ToList();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.Navigate(new LoginPage());
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.Navigate(new RegisterPage());
        }

        private void Products_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.Navigate(new ProductsPage());
        }

        private void GoAppointment_Click(object sender, RoutedEventArgs e)
        {
            var service = ServicesList.SelectedItem as Services;

            if (service == null)
            {
                MessageBox.Show("Выберите услугу");
                return;
            }

            MainWindow.Instance.MainFrame.Navigate(new AppointmentsPage(service));
        }
    }
}