using System.Windows;
using System.Windows.Controls;
using pr16.Services;

namespace pr16.Views
{
    public partial class GamePage : Page
    {
        private GameManager game;

        public GamePage(GameManager gm)
        {
            InitializeComponent();
            game = gm;

            LogBox.ItemsSource = game.Logs;

            game.Logs.CollectionChanged += (s, e) =>
            {
                if (LogBox.Items.Count > 0)
                    LogBox.ScrollIntoView(LogBox.Items[LogBox.Items.Count - 1]);
            };

            UpdateUI();
        }

        void UpdateUI()
        {
            TurnText.Text = game.Turn.ToString();

            PlayerHP.Maximum = game.Player.MaxHP;
            PlayerHP.Value = game.Player.HP;

            PlayerHPText.Text = $"{game.Player.HP} / {game.Player.MaxHP}";

            WeaponText.Text = "Оружие: " + game.Player.Weapon.Name;
            ArmorText.Text = "Броня: " + game.Player.Armor.Name;

            if (game.IsChoosingItem)
            {
                BattlePanel.Visibility = Visibility.Collapsed;
                ItemPanel.Visibility = Visibility.Visible;

                if (game.PendingWeapon != null)
                {
                    ItemName.Text = game.PendingWeapon.Name;
                    ItemStats.Text = $"+{game.PendingWeapon.AttackBonus} атаки";
                }
                else
                {
                    ItemName.Text = game.PendingArmor.Name;
                    ItemStats.Text = $"+{game.PendingArmor.DefenseBonus} защиты";
                }
            }
            else
            {
                BattlePanel.Visibility = Visibility.Visible;
                ItemPanel.Visibility = Visibility.Collapsed;

                if (game.CurrentEnemy != null)
                {
                    EnemyText.Text = game.CurrentEnemy.Name;

                    EnemyHP.Maximum = game.CurrentEnemy.MaxHP;
                    EnemyHP.Value = game.CurrentEnemy.HP;

                    EnemyHPText.Text = $"{game.CurrentEnemy.HP} / {game.CurrentEnemy.MaxHP}";
                }
                else
                {
                    EnemyText.Text = "Сундук";
                    EnemyHP.Value = 0;
                    EnemyHPText.Text = "";
                }
            }

            if (!game.Player.IsAlive)
                MainWindow.Instance.Navigate(new GameOver());
        }

        private void Attack_Click(object sender, RoutedEventArgs e)
        {
            game.Attack();
            UpdateUI();
        }

        private void Defend_Click(object sender, RoutedEventArgs e)
        {
            game.Defend();
            UpdateUI();
        }

        private void Take_Click(object sender, RoutedEventArgs e)
        {
            game.TakeItem();
            UpdateUI();
        }

        private void Skip_Click(object sender, RoutedEventArgs e)
        {
            game.SkipItem();
            UpdateUI();
        }
    }
}