using System;

namespace pr16.Model
{
    public class EnemyFactory
    {
        private static Random rnd = new Random();

        public static Enemy CreateEnemy()
        {
            int type = rnd.Next(3);

            switch (type)
            {
                case 0: // Гоблин
                    return new Enemy(
                        "Гоблин",
                        50,
                        10,
                        3,
                        critChance: 20
                    );

                case 1: // Скелет
                    return new Enemy(
                        "Скелет",
                        60,
                        12,
                        4,
                        ignoreArmor: true
                    );

                case 2: // Маг
                    return new Enemy(
                        "Маг",
                        40,
                        9,
                        2,
                        freezeChance: 20
                    );

                default:
                    return new Enemy("Слизень", 30, 8, 1);
            }
        }

        public static Enemy CreateBoss()
        {
            int type = rnd.Next(3);

            switch (type)
            {
                case 0: // Босс-гоблин
                    return new Enemy(
                        "ВВГ (босс-гоблин)",
                        (int)(50 * 2.0),
                        (int)(10 * 1.5),
                        (int)(3 * 1.2),
                        critChance: 30 
                    );

                case 1: // Босс-скелет
                    return new Enemy(
                        "Ковальский (босс-скелет)",
                        (int)(60 * 2.5),
                        (int)(12 * 1.3),
                        (int)(4 * 1.4),
                        ignoreArmor: true
                    );

                case 2: // Босс-маг
                    return new Enemy(
                        "Архимаг C++",
                        (int)(40 * 1.8),
                        (int)(9 * 1.6),
                        (int)(2 * 1.1),
                        freezeChance: 30 
                    );

                default:
                    return new Enemy("Босс-слизень", 100, 10, 5);
            }
        }
    }
}