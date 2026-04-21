using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PR17.Pages
{
    public partial class CartPage : Page
    {
        public CartPage()
        {
            InitializeComponent();
            Load();
        }

        void Load()
        {
            CartList.ItemsSource = null;
            CartList.ItemsSource = Cart.Items.Select(x => $"{x.Product.Name} x{x.Count}");
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            if (LoginPage.CurrentUser == null)
            {
                MessageBox.Show("Войдите!");
                return;
            }

            MainWindow.Instance.MainFrame.Navigate(new OrderPage());
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.GoBack();
        }
    }
}