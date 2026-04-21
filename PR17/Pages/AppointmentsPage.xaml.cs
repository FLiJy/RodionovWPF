using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PR17.Pages
{
    public partial class AppointmentsPage : Page
    {
        Services _service;

        public AppointmentsPage(Services service)
        {
            InitializeComponent();
            _service = service;

            LoadTime();
        }

        void LoadTime()
        {
            TimeList.Items.Clear();

            for (int i = 9; i < 18; i++)
            {
                TimeList.Items.Add($"{i}:00");
            }
        }

        private void Book_Click(object sender, RoutedEventArgs e)
        {
            if (LoginPage.CurrentUser == null)
            {
                MessageBox.Show("Войдите!");
                return;
            }

            if (TimeList.SelectedItem == null || DatePicker.SelectedDate == null)
                return;

            DateTime dt = DatePicker.SelectedDate.Value
                .AddHours(int.Parse(TimeList.SelectedItem.ToString().Split(':')[0]));

            var result = MessageBox.Show("Подтвердить запись?", "", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes) return;

            var app = new Appointments
            {
                ClientId = LoginPage.CurrentUser.Id,
                DateTime = dt,
                ServiceId = _service.Id,
                Price = _service.Price,
                Status = "Активна"
            };

            Core.Context.Appointments.Add(app);
            Core.Context.SaveChanges();

            MessageBox.Show("Запись создана!");
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.GoBack();
        }
    }
}