using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PR17.Pages
{
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            Load();
        }

        void Load()
        {
            UsersList.ItemsSource = Core.Context.Users
                .ToList()
                .Select(x => $"{x.Id} | {x.FullName} | Роль:{x.RoleId} | Активен:{x.IsActive}");
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var user = new Users
            {
                FullName = "Новый пользователь",
                Login = "login",
                Password = "1234",
                RoleId = 1,
                IsActive = true
            };

            Core.Context.Users.Add(user);
            Core.Context.SaveChanges();

            Load();
        }

        private void Role_Click(object sender, RoutedEventArgs e)
        {
            if (UsersList.SelectedIndex == -1) return;

            int id = int.Parse(UsersList.SelectedItem.ToString().Split('|')[0]);

            var user = Core.Context.Users.First(x => x.Id == id);

            user.RoleId++;

            if (user.RoleId > 4)
                user.RoleId = 1;

            Core.Context.SaveChanges();
            Load();
        }

        private void Block_Click(object sender, RoutedEventArgs e)
        {
            if (UsersList.SelectedIndex == -1) return;

            int id = int.Parse(UsersList.SelectedItem.ToString().Split('|')[0]);

            var user = Core.Context.Users.First(x => x.Id == id);

            user.IsActive = !user.IsActive;

            Core.Context.SaveChanges();
            Load();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.GoBack();
        }
    }
}