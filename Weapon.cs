namespace Dungeon
{
    class Weapon
    {
        public string Name { get; }
        public int BonusDamage { get; }

        public Weapon(string name, int bonusDamage)
        {
            Name = name;
            BonusDamage = bonusDamage;
        }
    }
}