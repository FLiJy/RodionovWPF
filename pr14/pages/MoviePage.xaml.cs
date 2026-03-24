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

namespace PR14.Pages
{
    /// <summary>
    /// Логика взаимодействия для MoviePage.xaml
    /// </summary>
    public partial class MoviePage : Page
    {
        public int _movieId;

        public MoviePage(int movieId)
        {
            InitializeComponent();
            _movieId = movieId;
            LoadMovie();
        }

        private void LoadMovie()
        {
            var movie = Core.Context.Movies
                .FirstOrDefault(m => m.MovieID == _movieId);

            if (movie == null)
                return;

            TitleText.Text = movie.Title;
            RatingText.Text = movie.Rating.ToString();
            AgeText.Text = movie.AgeRating;
            DescriptionText.Text = movie.Description;

            if (!string.IsNullOrWhiteSpace(movie.ImagePath))
            {
                try
                {
                    var uri = new Uri(movie.ImagePath, UriKind.RelativeOrAbsolute);
                    MovieImage.Source = new BitmapImage(uri);
                }
                catch
                {
                    MovieImage.Source = null;
                }
            }

            var genre = Core.Context.Movies.FirstOrDefault(m => m.MovieID == _movieId);

            if (movie != null)
            {
                var genres = movie.Genres.Select(g => g.Name).ToList();
                GenresText.Text = string.Join(", ", genres);
            }

            // Сеансы
            var sessions = Core.Context.Sessions
                .Where(s => s.MovieID == _movieId)
                .OrderBy(s => s.ShowTime)
                .ToList();

            SessionsList.ItemsSource = sessions;
        }

        private void SelectSession_Click(object sender, RoutedEventArgs e)
        {
            int sessionId = (int)((Button)sender).Tag;
            if (Core.CurrentUser == null)
            {
                MessageBox.Show("Войдите в аккаунт или зарегайтесь.");
                NavigationService.Navigate(new LoginPage());

                return;
            }

            else
                NavigationService.Navigate(new SessionPage(sessionId));

        }

        private void Main_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }
    }
}
