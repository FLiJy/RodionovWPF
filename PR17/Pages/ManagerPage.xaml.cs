using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PR17.Pages
{
    public partial class ManagerPage : Page
    {
        string mode = "appointments";

        public ManagerPage()
        {
            InitializeComponent();
            Load();
        }

        void Load()
        {
            switch (mode)
            {
                case "appointments":
                    MainList.ItemsSource = Core.Context.Appointments
                        .ToList()
                        .Select(x => $"{x.Id} | {x.DateTime} | {x.Status}");
                    break;

                case "orders":
                    MainList.ItemsSource = Core.Context.Orders
                        .ToList()
                        .Select(x => $"{x.Id} | {x.OrderDate} | {x.Status}");
                    break;

                case "products":
                    MainList.ItemsSource = Core.Context.Products
                        .ToList()
                        .Select(x => $"{x.Id} | {x.Name} | {x.Price}");
                    break;
            }
        }

        private void ShowAppointments(object sender, RoutedEventArgs e)
        {
            mode = "appointments";
            Load();
        }

        private void ShowOrders(object sender, RoutedEventArgs e)
        {
            mode = "orders";
            Load();
        }

        private void ShowProducts(object sender, RoutedEventArgs e)
        {
            mode = "products";
            Load();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MainList.SelectedIndex == -1) return;

            int id = int.Parse(MainList.SelectedItem.ToString().Split('|')[0]);

            if (mode == "products")
            {
                var item = Core.Context.Products.First(x => x.Id == id);
                Core.Context.Products.Remove(item);
            }

            if (mode == "appointments")
            {
                var item = Core.Context.Appointments.First(x => x.Id == id);
                item.Status = "Отменена";
            }

            if (mode == "orders")
            {
                var item = Core.Context.Orders.First(x => x.Id == id);
                item.Status = "Выдан";
            }

            Core.Context.SaveChanges();
            Load();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (mode == "products")
            {
                var product = new Products
                {
                    Name = "Новый товар",
                    Price = 1000,
                    Discount = 0
                };

                Core.Context.Products.Add(product);
                Core.Context.SaveChanges();
            }

            Load();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.GoBack();
        }
    }
}