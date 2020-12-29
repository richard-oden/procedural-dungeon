using System;
using System.Collections.Generic;
using System.Linq;

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

        public Item(ItemPart[] itemTemplate, ItemRarity itemRarity, Craftsmanship craftsmanship)
        {
            foreach (var itemPart in itemTemplate) itemPart.Generate(materialRarity: itemRarity);
            var primaryMaterialString = itemTemplate.Single(p => p.TotalWeight == itemTemplate.Max(p => p.TotalWeight)).Material.Name;
            
            Name = $"{craftsmanship.ToString()} {primaryMaterialString} Dagger";
            Weight = itemTemplate.Sum(iP => iP.TotalWeight);
            Value = (int)Math.Round(itemTemplate.Sum(iP => iP.TotalValue) * ((double)craftsmanship/(double)100));
            
            var description = "";
            foreach (var itemPart in itemTemplate)
            {
                description += $"The {itemPart.Name.ToLower()} is made of {itemPart.Material.Name.ToLower()}. It's worth about {Math.Round(itemPart.TotalValue)} gold and weighs {Math.Round(itemPart.TotalWeight, 1)}lbs.\n";
            }
            var moreOrLess = (int)craftsmanship > 100 ? "more" : "less";
            var craftsmanshipString = (int)craftsmanship != 100 ? $" Due to its craftsmanship, it's worth {moreOrLess} than normal." : "";
            description += $"The dagger is worth about {Value} gold and weighs {Math.Round(Weight, 1)}lbs in total.{craftsmanshipString}";
            
            Description = description;
        }

        public virtual Item GetClone()
        {
            return new Item(this);
        }

        public virtual string GetDetails()
        {
            return $"{GetBasicDetails()}\n{GetSecondaryDetails()}";
        }

        public virtual string GetBasicDetails()
        {
            return $"{Name} - {Math.Round(Weight, 1)}lbs - {Value} gold";
        }

        public virtual string GetSecondaryDetails()
        {
            return Description;
        }
    }

    public enum ItemRarity
    {
        Common,
        Uncommon,
        Rare,
        VeryRare
    }

    public enum Craftsmanship
    {
        // Represents percentage of full value
        Ruined = 25,
        Shoddy = 50,
        Poor = 75,
        Common = 100,
        WellMade = 125,
        Fine = 150,
        Masterwork = 175
    }
}