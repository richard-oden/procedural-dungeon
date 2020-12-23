namespace ProceduralDungeon
{
    public class Key : Item
    {
        public override char Symbol {get; protected set;} = Symbols.Key;
        public Key(Point location)
        {
            Name = "Floor key";
            Weight = 0.1;
            Value = 100;
            Rarity = ItemRarity.Rare;
            Description = "This key opens the door to the next floor.";
            Location = location;
        }
    }
}