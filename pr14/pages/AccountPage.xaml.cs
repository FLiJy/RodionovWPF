using pr14;
using pr14.Pages;
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

namespace pr14.Pages
{
    /// <summary>
    /// Логика взаимодействия для AccountPage.xaml
    /// </summary>
    public partial class AccountPage : Page
    {
        public AccountPage()
        {
            InitializeComponent();
            LoadUserData();
        }

        private void LoadUserData()
        {
            if (Core.CurrentUser != null)
            {
                FullNameText.Text = Core.CurrentUser.FullName;
                LoginText.Text = Core.CurrentUser.Login;
            }
        }

        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            if (Core.CurrentUser == null)
            {
                MessageBox.Show("Вы не авторизованы.");
                ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new LoginPage());
                return;
            }

            var tickets = Core.Context.Tickets
                .Where(t => t.UserID == Core.CurrentUser.UserID)
                .ToList();

            if (tickets.Count == 0)
            {
                MessageBox.Show("У вас пока нет билетов.");
                return;
            }

            string message = "Ваши билеты:\n\n";

            foreach (var t in tickets)
            {
                var session = Core.Context.Sessions.FirstOrDefault(s => s.SessionID == t.SessionID);
                var movie = Core.Context.Movies.FirstOrDefault(m => m.MovieID == session.MovieID);
                var seat = Core.Context.Seats.FirstOrDefault(s => s.SeatID == t.SeatID);

                message += $"{movie.Title} | Зал: {session.HallID} | {session.ShowTime} | Место: Ряд {seat.RowNumber}, Место {seat.SeatNumber}\n";
            }

            MessageBox.Show(message);
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Core.CurrentUser = null;
            NavigationService.Navigate(new LoginPage());
        }

        private void Main_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }
    }
}
