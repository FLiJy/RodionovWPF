using System;

namespace pr16.Model
{
     public class Weapon
    {
        public string Name;
        public int AttackBonus;

        static string[] names =
        {
            "Меч",
            "Моргенштерн",
            "Алебарда",
            "Копьё крестьянина"
        };

        public Weapon(string name, int attack)
        {
            Name = name;
            AttackBonus = attack;
        }

        public static Weapon GenerateRandomWeapon()
        {
            Random rnd = new Random();
            string name = names[rnd.Next(names.Length)];
            int bonus = rnd.Next(5, 20);

            return new Weapon(name, bonus);
        }
    }
}
