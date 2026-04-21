using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PR17.Pages
{
    public partial class ProductsPage : Page
    {
        public ProductsPage()
        {
            InitializeComponent();
            LoadProducts();
        }

        void LoadProducts()
        {
            var list = Core.Context.Products.ToList();

            if (!string.IsNullOrEmpty(SearchBox.Text))
                list = list.Where(x => x.Name.Contains(SearchBox.Text)).ToList();

            ProductsPanel.Children.Clear();

            foreach (var item in list)
            {
                var btn = new Button
                {
                    Content = $"{item.Name}\n{item.Price}",
                    Width = 150,
                    Height = 100,
                    Margin = new Thickness(5)
                };

                btn.Click += (s, e) =>
                {
                    MainWindow.Instance.MainFrame.Navigate(new ProductDetailsPage(item));
                };

                ProductsPanel.Children.Add(btn);
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadProducts();
        }

        private void Cart_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.Navigate(new CartPage());
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.GoBack();
        }

    }
}