using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon
{
    public abstract class Creature : IMappable, INameable, IDescribable
    {
        public string Name {get; protected set;}
        public int Id {get; protected set;}
        // public Gender Gender {get; protected set;}
        // public AbilityScores AbilityScores {get; protected set;}
        protected int _maxHp {get; set;}
        protected int _currentHp {get; set;}
        protected int _attackModifier {get; set;} = 0;
        protected int _attackRange {get; set;} = 1;
        public int ArmorClass {get; protected set;} = 10;
        public int DamageResistance {get; protected set;} = 0;
        protected Die[] _damageDice {get; set;} = new[]{Dice.D6};
        protected int _damageModifier {get; set;} = 0;
        protected int _speed {get; set;}
        public int SearchRange {get; set;}
        protected double _maxCarryWeight {get; set;}
        protected double _currentCarryWeight => Inventory.Sum(i => i.Weight);
        public List<Item> Inventory {get; protected set;} = new List<Item>();
        protected List<INameable> _memory {get; set;} = new List<INameable>();
        public Point Location {get; set;}
        public virtual char Symbol {get; protected set;} = Symbols.Player;
        public virtual string Description {get; protected set;}

        public Creature(string name, int id, int hp, int speed, Point location = null,
            List<Item> inventory = null, List<INameable> memory = null)
        {
            Name = name;
            Id = id;
            _maxHp = hp;
            _speed = speed;
            if (location != null) Location = location;
            if (inventory != null) Inventory = inventory;
            if (memory != null) _memory = memory;

            // Placeholders:
            _attackModifier = 0;
            _attackRange = 1;
            ArmorClass = 10;
            DamageResistance = 0;
            _damageDice = new[]{Dice.D8};
            _damageModifier = 2;
        }

        public void AddItemToInventory(Item itemToAdd)
        {
            if (_currentCarryWeight + itemToAdd.Weight <= _maxCarryWeight)
            {
                Inventory.Add(itemToAdd);
                AddToMemory(itemToAdd);
                itemToAdd.Location = null;
            }
            else
            {
                System.Console.WriteLine($"{itemToAdd.Name} is too heavy for {Name} to carry.");
            }
        }

        public virtual void PickUpItem(Map map, Item itemToPickUp)
        {
            if (validateTargetOnMap(map, itemToPickUp, 1))
            {
                map.RemoveAsset(itemToPickUp);
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
    
        public virtual void DropItem(Map map, Item itemToDrop)
        {
            var validLocations = map.EmptyPoints.Where(eP => Location.InRangeOf(eP, 1));
            if (validLocations.Any())
            {
                if (RemoveItemFromInventory(itemToDrop))
                {
                    map.AddAsset(itemToDrop, validLocations.RandomElement());
                }
                else
                {
                    System.Console.WriteLine($"There's nowhere to drop the {itemToDrop.Name}");
                }
            }
        }
        
        public bool HasLineOfSightTo(Map map, Point target)
        {
            return !map.GetPathObstructions(Location, target).Any();
        }
        
        public IMappable[] GetVisibleAssets(Map map)
        {
            return map.GetAssetsInRangeOf(Location, SearchRange)
                .Where(a => a != this)
                .Where(a => HasLineOfSightTo(map, a.Location)).ToArray();
        }

        public void AddToMemory(INameable asset)
        {
            if (!_memory.Contains(asset)) _memory.Add(asset);
        }
    
        public virtual void Search(Map map)
        {
            var foundAssets = GetVisibleAssets(map).Where(a => a is INameable);
            foreach (var fA in foundAssets) AddToMemory(fA as INameable);
        }
        
        public void ChangeHp(int amount)
        {
            if (amount < 0) amount += DamageResistance;
            if (_currentHp + amount > _maxHp)
            {
                _currentHp = _maxHp;
            }
            else if (_currentHp + amount < 0)
            {
                _currentHp = 0;
            }
            else
            {
                _currentHp += amount;
            }
        }
        
        protected bool validateTargetOnMap(Map map, IMappable target, int range)
        {
            string targetName = target is INameable ? (target as INameable).Name : target.GetType().Name;
            // If target is INameable, it must be within memory:
            if ((target is INameable && _memory.Contains(target as INameable)) ||
                !(target is INameable))
            {
                if (GetVisibleAssets(map).Contains(target))
                {
                    if (Location.InRangeOf(target.Location, _attackRange))
                    {
                        return true;
                    }
                    else
                    {
                        System.Console.WriteLine($"{targetName} is out of {Name}'s reach!");
                    }
                }
                else
                {
                    System.Console.WriteLine($"{Name} cannot see {targetName}!");
                }
            }
            else
            {
                System.Console.WriteLine($"{Name} does not know know about the {targetName}!");
            }
            return false;
        }
        
        public bool AttackRoll(Creature targetCreature)
        {
            int attackResult = Dice.D20.Roll(1, _attackModifier, true);
            return attackResult >= targetCreature.ArmorClass;
        }

        public int DamageRoll()
        {
            int damageSum = 0;
            foreach (var die in _damageDice) damageSum += die.Roll(1, _damageModifier, true);
            return damageSum;
        }

        public void Attack(Map map, Creature targetCreature)
        {
            if (validateTargetOnMap(map, targetCreature, _attackRange))
            {
                System.Console.WriteLine($"{Name} is attacking {targetCreature.Name}!");
                if (AttackRoll(targetCreature))
                {
                    int damage = DamageRoll();
                    System.Console.WriteLine($"{Name} dealt {damage} damage to {targetCreature.Name}!");
                    targetCreature.ChangeHp(-damage);
                    targetCreature.AddToMemory(this);
                }
                else
                {
                    System.Console.WriteLine($"{Name} missed the attack!");
                }
            }
        }
    }
}