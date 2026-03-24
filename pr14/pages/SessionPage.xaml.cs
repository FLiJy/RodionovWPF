using pr14;
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

namespace PR14.Pages
{
    /// <summary>
    /// Логика взаимодействия для SessionPage.xaml
    /// </summary>
    public partial class SessionPage : Page
    {
        private int _sessionId;
        private int _selectedSeatId = -1;

        public SessionPage(int sessionId)
        {
            InitializeComponent();
            _sessionId = sessionId;

            if (Core.CurrentUser == null)
            {
                MessageBox.Show("Для покупки билета необходимо войти в аккаунт.");
                ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new LoginPage());
                return;

            }

            LoadSession();
        }

        private void LoadSession()
        {
            var session = Core.Context.Sessions
                .FirstOrDefault(s => s.SessionID == _sessionId);

            if (session == null)
            {
                MessageBox.Show("Сеанс не найден.");
                NavigationService.GoBack();
                return;
            }

            var hall = Core.Context.Halls.FirstOrDefault(h => h.HallID == session.HallID);
            if (hall == null)
            {
                MessageBox.Show("Зал не найден.");
                NavigationService.GoBack();
                return;
            }

            SessionInfoText.Text =
                $"{session.ShowTime} | Зал: {hall.Name} | Цена: {session.Price}";

            var seats = Core.Context.Seats
                .Where(s => s.HallID == hall.HallID)
                .OrderBy(s => s.RowNumber)
                .ThenBy(s => s.SeatNumber)
                .ToList();

            var occupiedSeatIds = Core.Context.Tickets
                .Where(t => t.SessionID == _sessionId)
                .Select(t => t.SeatID)
                .ToList();

            SeatsPanel.Children.Clear();

            foreach (var seat in seats)
            {
                var btn = new Button
                {
                    Content = $"{seat.RowNumber}-{seat.SeatNumber}",
                    Width = 60,
                    Height = 40,
                    Margin = new Thickness(5),
                    Tag = seat.SeatID
                };

                if (occupiedSeatIds.Contains(seat.SeatID))
                {
                    btn.Background = Brushes.Red;
                    btn.IsEnabled = false;
                }
                else
                {
                    btn.Background = Brushes.LightGreen;
                    btn.Click += Seat_Click;
                }

                SeatsPanel.Children.Add(btn);
            }
        }

        private void Seat_Click(object sender, RoutedEventArgs e)
        {
            foreach (Button btn in SeatsPanel.Children)
                btn.BorderThickness = new Thickness(1);

            Button clicked = sender as Button;
            clicked.BorderThickness = new Thickness(3);
            clicked.BorderBrush = Brushes.Blue;

            _selectedSeatId = (int)clicked.Tag;
        }

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedSeatId == -1)
            {
                MessageBox.Show("Выберите место.");
                return;
            }

            NavigationService.Navigate(
                new CheckoutPage(_sessionId, _selectedSeatId));
        }

        private void Main_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }
    }
}
