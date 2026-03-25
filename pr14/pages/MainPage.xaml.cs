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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            LoadMovies();
        }

        private void LoadMovies()
        {
            var movies = Core.Context.Movies.ToList();

            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                string search = SearchBox.Text.ToLower();
                movies = movies
                    .Where(m => m.Title.ToLower().Contains(search))
                    .ToList();
            }

            if (SortBox.SelectedIndex == 1)
                movies = movies.OrderBy(m => m.Title).ToList();
            else if (SortBox.SelectedIndex == 2)
                movies = movies.OrderByDescending(m => m.Rating).ToList();

            MoviesList.ItemsSource = movies;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadMovies();
        }

        private void SortBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadMovies();
        }

        private void Details_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).Tag;

            NavigationService.Navigate(new MoviePage(id));
        }

        private void Account_Click(object sender, RoutedEventArgs e)
        {
            if (Core.CurrentUser == null)
            {
                NavigationService.Navigate(new LoginPage());
            }
            else
            {
                NavigationService.Navigate(new AccountPage());
            }

        }
    }
}
