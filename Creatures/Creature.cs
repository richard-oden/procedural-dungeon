using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon
{
    public abstract class Creature : IMappable, INameable
    {
        public string Name {get; protected set;}
        public int Id {get; protected set;}
        // public Gender Gender {get; protected set;}
        // public AbilityScores AbilityScores {get; protected set;}
        protected int _hp {get; set;}
        protected int _currentHp {get; set;}
        protected int _attackModifier {get; set;}
        protected Die[] _damageDice {get; set;}
        protected int _speed {get; set;}
        protected double _maxCarryWeight {get; set;}
        protected double _currentCarryWeight => Inventory.Sum(i => i.Weight);
        public List<Item> Inventory {get; protected set;} = new List<Item>();
        protected List<IMappable> _memory {get; set;} = new List<IMappable>();
        public Point Location {get; set;}
        public virtual char Symbol {get; protected set;} = Symbols.Player;

        public Creature(string name, int id, int hp, int speed, Point location = null,
            List<Item> inventory = null, List<IMappable> memory = null)
        {
            Name = name;
            Id = id;
            _hp = hp;
            _speed = speed;
            if (location != null) Location = location;
            if (inventory != null) Inventory = inventory;
            if (memory != null) _memory = memory;
        }

        public void AddItemToInventory(Item itemToAdd)
        {
            if (_currentCarryWeight + itemToAdd.Weight <= _maxCarryWeight)
            {
                Inventory.Add(itemToAdd);
            }
            else
            {
                System.Console.WriteLine($"{itemToAdd.Name} is too heavy for {Name} to carry.");
            }
        }

        public void PickUpItem(Item itemToPickUp)
        {
            if (Location.InRangeOf(itemToPickUp.Location, 1))
            {
                AddItemToInventory(itemToPickUp);
            }
            else
            {
                System.Console.WriteLine($"{itemToPickUp.Name} is too far away.");
            }
        }

        public bool RemoveItemFromInventory(Item itemToRemove)
        {
            if (Inventory.Contains(itemToRemove))
            {
                Inventory.Remove(itemToRemove);
                return true;
            }
            else
            {
                System.Console.WriteLine($"{itemToRemove.Name} could not be found in {Name}'s inventory.");
                return false;
            }
        }
    }
}