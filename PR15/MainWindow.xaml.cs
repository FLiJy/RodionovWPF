using System.Windows;
using System.Windows.Controls;
using PR15.Pages;

namespace PR15
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Загружаем первую страницу
            MainFrame.Navigate(new BuilderPage());
        }

        private void BtnBuilder_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new BuilderPage());
            StatusText.Text = "Режим конфигуратора";
        }

        private void BtnAssemblies_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AssembliesPage());
            StatusText.Text = "Просмотр сохранённых сборок";
        }
    }
}