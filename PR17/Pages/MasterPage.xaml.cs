using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PR17.Pages
{
    public partial class MasterPage : Page
    {
        public MasterPage()
        {
            InitializeComponent();
            Load();
        }

        void Load()
        {
            var list = Core.Context.Appointments
                .Where(x => x.MasterId == LoginPage.CurrentUser.Id)
                .ToList();

            AppointmentsList.ItemsSource = list.Select(x =>
                $"{x.DateTime} | {x.Services.Name} | {x.Status}");
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = AppointmentsList.SelectedIndex;
            if (selectedIndex == -1) return;

            var app = Core.Context.Appointments
                .Where(x => x.MasterId == LoginPage.CurrentUser.Id)
                .ToList()[selectedIndex];

            MainWindow.Instance.MainFrame.Navigate(new AppointmentDetailsPage(app));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.GoBack();
        }
    }
}