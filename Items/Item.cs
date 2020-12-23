namespace ProceduralDungeon
{
    public class Item : IMappable, INameable, IDescribable
    {
        public string Name {get; protected set;}
        public double Weight {get; protected set;}
        public int Value {get; protected set;}
        public string Description {get; protected set;}
        public virtual Point Location {get; set;}
        public ItemRarity Rarity {get; protected set;}
        public virtual char Symbol {get; protected set;} = Symbols.Item;

        public Item(){}

        public Item(string name, double weight, int value, ItemRarity rarity, string description)
        {
            Name = name;
            Weight = weight;
            Value = value;
            Description = description;
            Rarity = rarity;
        }
        public Item(string name, double weight, int value, ItemRarity rarity, string description, Point location) :
            this(name, weight, value, rarity, description)
        {
            Location = location;
            Rarity = rarity;
        }

        public Item(Item itemToClone)
        {
            Name = itemToClone.Name;
            Weight = itemToClone.Weight;
            Value = itemToClone.Value;
            Description = itemToClone.Description;
            Rarity = itemToClone.Rarity;
            Location = itemToClone.Location;
        }

        public virtual string GetDetails()
        {
            return $"{Name} - {Weight}lbs - {Value} gold\n{Description}";
        }
    }

    public enum ItemRarity
    {
        Common,
        Uncommon,
        Rare,
        VeryRare
    }
}