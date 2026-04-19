namespace Dungeon
{
    abstract class Enemy : Character
    {
        public Enemy(string name, int health, int damage, int x, int y)
        {
            Name = name;
            Health = health;
            Damage = damage;

            X = x;
            Y = y;
        }
    }
}