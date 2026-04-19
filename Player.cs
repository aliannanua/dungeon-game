namespace Dungeon
{
    class Player : Character
    {
        public int PotionsCount { get; private set; }
        public Weapon Weapon { get; private set; }

        public Player(int startX, int startY)
        {
            Name = "Player";
            Health = 50;
            Damage = 10;

            X = startX;
            Y = startY;

            Weapon = new Weapon("Sword", 5);
            PotionsCount = 2;
        }

        public int TotalDamage => Damage + Weapon.BonusDamage;

        public void UsePotion(Potion potion)
        {
            if (PotionsCount > 0)
            {
                Health += potion.HealAmount;

                if (Health > 50)
                    Health = 50;

                PotionsCount--;
            }
        }
    }
}