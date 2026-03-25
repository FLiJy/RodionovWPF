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
    /// Логика взаимодействия для CheckoutPage.xaml
    /// </summary>
    public partial class CheckoutPage : Page
    {
        private int _sessionId;
        private int _seatId;

        public CheckoutPage(int sessionId, int seatId)
        {
            InitializeComponent();
            _sessionId = sessionId;
            _seatId = seatId;

            LoadInfo();
        }

        private void LoadInfo()
        {
            var session = Core.Context.Sessions
                .FirstOrDefault(s => s.SessionID == _sessionId);

            var seat = Core.Context.Seats
                .FirstOrDefault(s => s.SeatID == _seatId);

            if (session == null || seat == null)
                return;

            var movie = Core.Context.Movies
                .FirstOrDefault(m => m.MovieID == session.MovieID);

            MovieText.Text = $"Фильм: {movie.Title}";
            HallText.Text = $"Зал: {session.Halls.Name}";
            DateText.Text = $"Дата и время: {session.ShowTime}";
            SeatText.Text = $"Место: Ряд {seat.RowNumber}, Место {seat.SeatNumber}";
            PriceText.Text = $"Цена: {session.Price} ₽";
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            bool alreadyTaken = Core.Context.Tickets
                .Any(t => t.SessionID == _sessionId && t.SeatID == _seatId);

            if (alreadyTaken)
            {
                MessageBox.Show("Это место уже занято.");
                return;
            }

            var ticket = new Tickets
            {
                SessionID = _sessionId,
                SeatID = _seatId,
                UserID = Core.CurrentUser.UserID
            };

            Core.Context.Tickets.Add(ticket);
            Core.Context.SaveChanges();

            MessageBox.Show("Билет успешно оформлен!");

            NavigationService.Navigate(new MainPage());
        }
    }
}
