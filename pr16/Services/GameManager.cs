using pr16;
using pr16.Model;
using pr16.Model;
using pr16.Views;
using System.Collections.ObjectModel;

namespace pr16.Services
{
    public class GameManager
    {
        public Player Player;
        public Enemy CurrentEnemy;
        public int Turn;
        public bool PlayerFrozen = false;
        public bool IsChoosingItem = false;
        public Weapon PendingWeapon;
        public Armor PendingArmor;

        public ObservableCollection<string> Logs = new ObservableCollection<string>();

        public void StartGame()
        {
            Player = new Player("Игрок");
            Turn = 1;
            Logs.Clear();
            Logs.Add("Игра началась");

            NextTurn(); 
        }

        public void NextTurn()
        {
            Logs.Add($"--- Ход {Turn} ---");

            if (Turn % 10 == 0)
            {
                CurrentEnemy = EnemyFactory.CreateBoss();
                Logs.Add($"Босс: {CurrentEnemy.Name}");
                return;
            }

            if (GameRandom.NextBool())
            {
                OpenChest();
            }
            else
            {
                CurrentEnemy = EnemyFactory.CreateEnemy();
                Logs.Add($"Враг: {CurrentEnemy.Name}");
            }
        }

        public void Attack()
        {
            if (IsChoosingItem) return;

            if (PlayerFrozen)
            {
                Logs.Add("Вы заморожены и пропускаете ход");
                PlayerFrozen = false;
                EnemyTurn();
                return;
            }

            int dmg = Player.AttackEnemy(CurrentEnemy);
            Logs.Add($"Вы нанесли {dmg} урона");

            EnemyTurn();
        }

        public void Defend()
        {
            if (IsChoosingItem) return;

            if (PlayerFrozen)
            {
                Logs.Add("Вы заморожены и пропускаете ход");
                PlayerFrozen = false;
                EnemyTurn();
                return;
            }

            Player.Defend();
            Logs.Add("Вы в защите");

            EnemyTurn();
        }

        private void EnemyTurn()
        {
            if (!CurrentEnemy.IsAlive)
            {
                Logs.Add($"Враг {CurrentEnemy.Name} убит");
                Turn++;
                NextTurn(); 
                return;
            }

            int beforeHP = Player.HP;

            bool froze = CurrentEnemy.AttackPlayer(Player, msg => Logs.Add(msg));

            int damage = beforeHP - Player.HP;

            if (damage > 0)
                Logs.Add($"Враг нанес {damage} урона");
            else
                Logs.Add("Вы избежали урон");

            if (froze)
            {
                Logs.Add("Вы заморожены");
                PlayerFrozen = true;
            }

            if (!Player.IsAlive)
            {
                Logs.Add("Вы умерли :(");
                return;
            }
        }

        private void OpenChest()
        {
            CurrentEnemy = null; 

            Logs.Add("Вы нашли сундук");

            int drop = GameRandom.rnd.Next(3);

            if (drop == 0)
            {
                Player.HP = Player.MaxHP;
                Logs.Add("Вы полностью восстановили здоровье");
                Turn++;
                NextTurn();
            }
            else if (drop == 1)
            {
                PendingWeapon = Weapon.GenerateRandomWeapon();
                IsChoosingItem = true;

                Logs.Add($"Новое оружие: {PendingWeapon.Name} (+{PendingWeapon.AttackBonus})");
                Logs.Add($"Текущее: {Player.Weapon.Name} (+{Player.Weapon.AttackBonus})");
                Logs.Add("Взять предмет?");
            }
            else
            {
                PendingArmor = Armor.GenerateRandomArmor();
                IsChoosingItem = true;

                Logs.Add($"Новая броня: {PendingArmor.Name} (+{PendingArmor.DefenseBonus})");
                Logs.Add($"Текущая: {Player.Armor.Name} (+{Player.Armor.DefenseBonus})");
                Logs.Add("Взять предмет?");
            }
        }

        public void TakeItem()
        {
            if (PendingWeapon != null)
            {
                Player.Weapon = PendingWeapon;
                Logs.Add("Вы взяли оружие");
            }

            if (PendingArmor != null)
            {
                Player.Armor = PendingArmor;
                Logs.Add("Вы надели броню");
            }

            PendingWeapon = null;
            PendingArmor = null;
            IsChoosingItem = false;

            Turn++;
            NextTurn(); 
        }

        public void SkipItem()
        {
            Logs.Add("Вы отказались от предмета");

            PendingWeapon = null;
            PendingArmor = null;
            IsChoosingItem = false;

            Turn++;
            NextTurn(); 
        }
    }
}