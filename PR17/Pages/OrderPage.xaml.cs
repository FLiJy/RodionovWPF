using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PR17.Pages
{
    public partial class OrderPage : Page
    {
        public OrderPage()
        {
            InitializeComponent();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (DatePicker.SelectedDate == null || PaymentBox.SelectedItem == null)
                return;

            var order = new Orders
            {
                UserId = LoginPage.CurrentUser.Id,
                OrderDate = DateTime.Now,
                DeliveryDate = DatePicker.SelectedDate.Value,
                PaymentMethod = (PaymentBox.SelectedItem as ComboBoxItem).Content.ToString(),
                Status = "Создан"
            };

            Core.Context.Orders.Add(order);
            Core.Context.SaveChanges();

            foreach (var item in Cart.Items)
            {
                Core.Context.OrderItems.Add(new OrderItems
                {
                    OrderId = order.Id,
                    ProductId = item.Product.Id,
                    Quantity = item.Count
                });
            }

            Core.Context.SaveChanges();
            Cart.Items.Clear();

            MessageBox.Show("Заказ оформлен!");
            MainWindow.Instance.MainFrame.Navigate(new StartPage());
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.GoBack();
        }
    }
}