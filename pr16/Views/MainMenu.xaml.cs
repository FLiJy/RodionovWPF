using System.Windows;
using System.Windows.Controls;
using pr16.Services;

namespace pr16.Views
{
    public partial class MainMenu : Page
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            var game = new GameManager();
            game.StartGame();

            MainWindow.Instance.Navigate(new GamePage(game));
        }
    }
}