using System.Windows;
using System.Windows.Controls;

namespace PR17.Pages
{
    public partial class ProductDetailsPage : Page
    {
        Products product;

        public ProductDetailsPage(Products p)
        {
            InitializeComponent();
            product = p;

            Load();
        }

        void Load()
        {
            NameText.Text = product.Name;
            PriceText.Text = $"Цена: {product.Price} ₽";

            DiscountText.Text = $"Скидка: {product.Discount}%";
            RatingText.Text = $"Оценка: {product.Rating}/5";

            DescText.Text = product.Description;
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (LoginPage.CurrentUser == null)
            {
                MessageBox.Show("Нужно войти в аккаунт");
                return;
            }

            Cart.Items.Add(new CartItem
            {
                Product = product,
                Count = 1
            });

            MessageBox.Show("Добавлено в корзину");
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.GoBack();
        }
    }
}