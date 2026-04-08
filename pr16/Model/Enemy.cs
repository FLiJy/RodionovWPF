using System;

namespace pr16.Model
{
    public class Enemy
    {
        public int MaxHP;
        public string Name;
        public int HP;
        public int Attack;
        public int Defense;
        public int CritChance;
        public int FreezeChance;
        public bool IgnoreArmor;

        protected static Random rnd = new Random();

        public bool IsAlive => HP > 0;

        public Enemy(string name, int hp, int attack, int defense,
                     int critChance = 0, int freezeChance = 0, bool ignoreArmor = false)
        {
            MaxHP = hp;
            Name = name;
            HP = hp;
            Attack = attack;
            Defense = defense;
            CritChance = critChance;
            FreezeChance = freezeChance;
            IgnoreArmor = ignoreArmor;
        }

        public virtual bool AttackPlayer(Player player, Action<string> log)
        {
            int damage = Attack;
            bool froze = false;

            if (CritChance > 0 && rnd.Next(100) < CritChance)
            {
                damage *= 2;
                log("Критический удар");
            }

            int finalDamage = damage;

            if (player.Defending)
            {
                int dodge = rnd.Next(100);

                if (dodge < 40)
                {
                    log("Вы увернулись");
                    player.Defending = false;
                    return false;
                }

                if (!IgnoreArmor)
                {
                    int blockPercent = rnd.Next(70, 101);
                    finalDamage = finalDamage * (100 - blockPercent) / 100;

                    finalDamage -= player.Armor.DefenseBonus;
                }
                else
                {
                    log("Скелет игнорирует защиту");
                }

                player.Defending = false;
            }
            else
            {
                
                if (!IgnoreArmor)
                    finalDamage -= player.Armor.DefenseBonus;
            }

            if (finalDamage < 0) finalDamage = 0;

            if (FreezeChance > 0 && rnd.Next(100) < FreezeChance)
            {
                froze = true;
                log("Вы заморожены");
            }

            player.TakeDamage(finalDamage);

            return froze;
        }

        public virtual int TakeDamage(int damage)
        {
            damage -= Defense;
            if (damage < 0) damage = 0;

            HP -= damage;
            return damage;
        }
    }

    class Slime : Enemy
    {
        public Slime()
            : base("Слизень", 70, 8, 1)
        {
        }

        public override int TakeDamage(int damage)
        {
            damage -= 2;
            if (damage < 0) damage = 0;

            HP -= damage;
            return damage;
        }
    }
}
