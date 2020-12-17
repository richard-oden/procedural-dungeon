namespace ProceduralDungeon
{
    public class Item : IMappable, INameable, IDescribable
    {
        public string Name {get; protected set;}
        public double Weight {get; protected set;}
        public int Value {get; protected set;}
        public string Description {get; protected set;}
        public Point Location {get; set;}
        public virtual char Symbol {get; protected set;} = Symbols.Item;

        public Item(){}

        public Item(string name, double weight, int value, string description)
        {
            Name = name;
            Weight = weight;
            Value = value;
            Description = description;
        }
        public Item(string name, double weight, int value, string description, Point location) :
            this(name, weight, value, description)
        {
            Location = location;
        }

        public Item(Item itemToClone)
        {
            Name = itemToClone.Name;
            Weight = itemToClone.Weight;
            Value = itemToClone.Value;
            Description = itemToClone.Description;
            Location = itemToClone.Location;
        }

        public string GetDetails()
        {
            return $"{Name} - {Weight}lbs - {Value} gold\n{Description}";
        }
    }
}