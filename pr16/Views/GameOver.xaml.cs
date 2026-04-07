using System.Windows;
using System.Windows.Controls;

namespace pr16.Views
{
    public partial class GameOver : Page
    {
        public GameOver()
        {
            InitializeComponent();
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Navigate(new MainMenu());
        }
    }
}