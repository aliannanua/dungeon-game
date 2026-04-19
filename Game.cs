using System;
using System.Collections.Generic;

namespace Dungeon
{
    class Game
    {
        private char[,] map;
        private Player player;
        private List<Enemy> enemies;
        private Potion potion;

        private bool isRunning = true;

        public Game()
        {
            map = new char[,]
            {
                { '#','#','#','#','#','#','#','#','#','#' },
                { '#','.','.','.','.','.','.','.','.','#' },
                { '#','.','.','.','.','.','.','.','.','#' },
                { '#','.','.','.','.','.','.','.','.','#' },
                { '#','#','#','#','#','#','#','#','#','#' }
            };

            player = new Player(1, 1);

            enemies = new List<Enemy>()
            {
                new Goblin(4, 1),
                new Skeleton(3, 3),
                new Orc(6, 3)
            };

            potion = new Potion(30);
        }

        public void Run()
        {
            while (isRunning)
            {
                Draw();
                HandleInput();
                CheckGameOver();
            }
        }

        private void Draw()
        {
            Console.Clear();

            char[,] tempMap = (char[,])map.Clone();

            foreach (var enemy in enemies)
            {
                if (enemy.IsAlive)
                {
                    char symbol = enemy.Name[0];
                    tempMap[enemy.Y, enemy.X] = symbol;
                }
            }

            tempMap[player.Y, player.X] = '@';

            for (int y = 0; y < tempMap.GetLength(0); y++)
            {
                for (int x = 0; x < tempMap.GetLength(1); x++)
                {
                    Console.Write(tempMap[y, x]);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine($"HP: {player.Health}");
            Console.WriteLine($"Damage: {player.TotalDamage}");
            Console.WriteLine($"Potions: {player.PotionsCount}");
            Console.WriteLine();
            Console.WriteLine("WASD - move | SPACE - attack | E - potion | ESC - exit");
        }

        private void HandleInput()
        {
            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.W:
                    MovePlayer(0, -1);
                    break;

                case ConsoleKey.S:
                    MovePlayer(0, 1);
                    break;

                case ConsoleKey.A:
                    MovePlayer(-1, 0);
                    break;

                case ConsoleKey.D:
                    MovePlayer(1, 0);
                    break;

                case ConsoleKey.Spacebar:
                    AttackEnemy();
                    break;

                case ConsoleKey.E:
                    UsePotion();
                    break;

                case ConsoleKey.Escape:
                    isRunning = false;
                    break;
            }
        }

        private void MovePlayer(int dx, int dy)
        {
            int newX = player.X + dx;
            int newY = player.Y + dy;

            if (map[newY, newX] == '#')
                return;

            foreach (var enemy in enemies)
            {
                if (enemy.IsAlive && enemy.X == newX && enemy.Y == newY)
                    return;
            }

            player.X = newX;
            player.Y = newY;
        }

        private void AttackEnemy()
        {
            Enemy enemy = GetAdjacentEnemy();

            if (enemy == null)
                return;

            enemy.TakeDamage(player.TotalDamage);

            Console.WriteLine();
            Console.WriteLine($"{enemy.Name} получил {player.TotalDamage} урона");

            if (enemy.IsAlive)
            {
                player.TakeDamage(enemy.Damage);

                Console.WriteLine($"{enemy.Name} атаковал и нанёс {enemy.Damage} урона");
            }
            else
            {
                Console.WriteLine($"{enemy.Name} побеждён!");
            }

            Console.ReadKey();
        }

        private Enemy GetAdjacentEnemy()
        {
            foreach (var enemy in enemies)
            {
                if (!enemy.IsAlive)
                    continue;

                int dx = Math.Abs(enemy.X - player.X);
                int dy = Math.Abs(enemy.Y - player.Y);

                if (dx + dy == 1)
                    return enemy;
            }

            return null;
        }

        private void UsePotion()
        {
            if (player.PotionsCount <= 0)
            {
                Console.WriteLine();
                Console.WriteLine("Зелий нет!");
                Console.ReadKey();
                return;
            }

            player.UsePotion(potion);

            Console.WriteLine();
            Console.WriteLine("Вы выпили зелье (+30 HP)");
            Console.ReadKey();
        }

        private void CheckGameOver()
        {
            if (!player.IsAlive)
            {
                Console.Clear();
                Console.WriteLine("Вы погибли...");
                isRunning = false;
            }

            bool allEnemiesDead = true;

            foreach (var enemy in enemies)
            {
                if (enemy.IsAlive)
                    allEnemiesDead = false;
            }

            if (allEnemiesDead)
            {
                Console.Clear();
                Console.WriteLine("Вы победили всех врагов!");
                isRunning = false;
            }
        }
    }
}