using System.Windows;
using System.Windows.Controls;

namespace PR17.Pages
{
    public partial class AppointmentDetailsPage : Page
    {
        Appointments _app;

        public AppointmentDetailsPage(Appointments app)
        {
            InitializeComponent();
            _app = app;

            InfoText.Text = $"{app.DateTime} | {app.Price}";
        }

        private void Finish_Click(object sender, RoutedEventArgs e)
        {
            _app.Status = "Завершена";
            Core.Context.SaveChanges();

            MessageBox.Show("Завершено");
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.GoBack();
        }
    }
}