namespace ProceduralDungeon
{
    public class Item : IMappable
    {
        public string Name {get; private set;}
        public int Id {get; private set;}
        public double Weight {get; private set;}
        public int Value {get; private set;}
        public string Description {get; private set;}
        public Point Location {get; set;}
        public virtual char Symbol {get; private set;} = Symbols.Item;

        public Item(string name, int id, double weight, int value, string description)
        {
            Name = name;
            Id = id;
            Weight = weight;
            Value = value;
            Description = description;
        }
        public Item(string name, int id, double weight, int value, string description, Point location) :
            this(name, id, weight, value, description)
        {
            Location = location;
        }

        public Item(Item itemToClone)
        {
            Name = itemToClone.Name;
            Id = itemToClone.Id;
            Weight = itemToClone.Weight;
            Value = itemToClone.Value;
            Description = itemToClone.Description;
            Location = itemToClone.Location;
        }
    }
}