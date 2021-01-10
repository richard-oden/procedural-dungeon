using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon
{
    public abstract class Creature : IMappable, INameable, IDescribable, IContainer
    {
        public string Name {get; protected set;}
        public Gender Gender {get; protected set;}
        public string[] Pronouns
        {
            get
            {
                if (Gender == Gender.Male) return new[] {"He", "Him", "His"};
                else if (Gender == Gender.Female) return new[] {"She", "Her", "Her"};
                else if (Gender == Gender.NonBinary) return new[] {"They", "Them", "Their"};
                else if (Gender == Gender.None) return new[] {"It", "It", "Its"};
                else throw new Exception("Invalid gender. Unable to determine pronouns.");
            }
        } 
        public int Id {get; protected set;}
        protected virtual int _maxHp {get; set;}
        protected int _currentHp {get; set;}
        protected virtual int _attackModifier {get; set;} = 0;
        protected virtual int _attackRange {get; set;} = 1;
        protected virtual int _baseArmorClass {get; set;} = 8;
        public int ArmorClass => _baseArmorClass + EquippedArmor.Sum(eA => eA.ArmorClassBonus);
        protected virtual int _baseDamageResistance {get; set;} = 0;
        public virtual int DamageResistance => _baseDamageResistance + EquippedArmor.Sum(eA => eA.DamageResistance);
        protected Die[] _damageDice {get; set;} = new Die[] {Dice.D3};
        protected virtual int _damageModifier {get; set;} = 1;
        public virtual int SearchRange {get; set;}
        public virtual double MaxCarryWeight {get; protected set;}
        public double CurrentCarryWeight => Inventory.Sum(i => i.Weight);
        public List<Item> Inventory {get; protected set;} = new List<Item>();
        public List<IEquippable> EquippedItems {get; protected set;} = new List<IEquippable>();
        public List<Weapon> EquippedWeapons => EquippedItems.Where(i => i is Weapon).Cast<Weapon>().ToList();
        public List<Armor> EquippedArmor => EquippedItems.Where(i => i is Armor).Cast<Armor>().ToList();
        protected List<INameable> _memory {get; set;} = new List<INameable>();
        public int Gold {get; set;}
        public Point Location {get; set;}
        public virtual char Symbol {get; protected set;} = Symbols.Player;
        public int Team {get; protected set;}
        public bool IsDead => _currentHp <= 0;
        protected string _baseDescription {get; set;}
        public CreatureCategory Category {get; protected set;}
        public virtual string Description
        {
            get
            {
                var description = _baseDescription == null ? "" : _baseDescription + " ";
                string hpString;
                if (_currentHp == _maxHp) hpString = "uninjured";
                else if (_currentHp > _maxHp * .75) hpString = "mildly injured";
                else if (_currentHp > _maxHp * .5) hpString = "fairly injured";
                else if (_currentHp > _maxHp * .25) hpString = "greatly injured";
                else if (_currentHp > 0) hpString = "near death";
                else hpString = "dead";

                description += $"{Pronouns[0]} appears {hpString}.";

                if (EquippedWeapons.Any()) 
                {
                    description += $" {Pronouns[0]} is wielding {EquippedWeapons.ListWithIndefiniteArticle()}.";
                }

                if (EquippedArmor.Any())
                {
                    description += $" {Pronouns[0]} is wearing {EquippedArmor.ListWithIndefiniteArticle()}.";
                }
                return description;
            }
        }

        public Creature(string name, int id, int hp, CreatureCategory category, Gender gender = Gender.None, 
            Point location = null, List<Item> inventory = null, int gold = 0, List<INameable> memory = null, 
            string baseDescription = null)
        {
            Name = name;
            Id = id;
            _maxHp = hp;
            _currentHp = hp;
            Gender = gender;
            if (location != null) Location = location;
            if (inventory != null) Inventory = inventory;
            if (memory != null) _memory = memory;
            Gold = gold;
            _baseDescription = baseDescription;
            Category = category;
        }

        public string GetCarryWeightString()
        {
            if (MaxCarryWeight > -1)
            {
                return $"{CurrentCarryWeight}/{MaxCarryWeight} lbs";
            }
            else
            {
                return "";
            }
        }

        public bool AddItemToInventory(Item itemToAdd, bool cloneItem = false)
        {
            var actualItem = cloneItem ? itemToAdd.GetClone() : itemToAdd;
            if (CurrentCarryWeight + actualItem.Weight <= MaxCarryWeight)
            {
                Inventory.Add(actualItem);
                AddToMemory(actualItem);
                itemToAdd.Location = null;
                return true;
            }
            else
            {
                System.Console.WriteLine($"{itemToAdd.Name} is too heavy for {Name} to carry.");
                return false;
            }
        }

        public virtual void PickUpItem(Map map, Item itemToPickUp)
        {     
            if (AddItemToInventory(itemToPickUp))
            {
                map.RemoveAsset(itemToPickUp);
                System.Console.WriteLine($"{Name} picked up the {itemToPickUp.Name}.");
            }
        }

        public bool RemoveItemFromInventory(Item itemToRemove)
        {
            if (Inventory.Contains(itemToRemove))
            {
                if (itemToRemove is IEquippable)
                {
                    if (EquippedItems.Contains(itemToRemove as IEquippable)) UnequipItem(itemToRemove);
                }
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
                    map.AddItem(itemToDrop, validLocations.RandomElement());
                    System.Console.WriteLine($"{Name} dropped the {itemToDrop.Name}.");
                }
                else
                {
                    System.Console.WriteLine($"There's nowhere to drop the {itemToDrop.Name}");
                }
            }
        }
        
        protected bool canEquipItem(Item itemToEquip)
        {
            bool canEquip = false;
            if (Inventory.Contains(itemToEquip))
            {
                if (itemToEquip is IEquippable)
                {
                    var equippable = (IEquippable)itemToEquip;
                    if (!EquippedItems.Contains(equippable))
                    {
                        if (equippable.Slot == EquipmentSlot.OneHanded)
                        {
                            canEquip = EquippedItems.Where(eI => eI.Slot == EquipmentSlot.OneHanded).Count() < 2 &&
                                EquippedItems.All(eI => eI.Slot != EquipmentSlot.TwoHanded);
                        }
                        else if (equippable.Slot == EquipmentSlot.TwoHanded)
                        {
                            canEquip = EquippedItems.All(eI => eI.Slot != EquipmentSlot.TwoHanded && 
                                eI.Slot != EquipmentSlot.OneHanded);
                        }
                        else
                        {
                            canEquip = EquippedItems.All(eI => eI.Slot != (itemToEquip as IEquippable).Slot);
                        }
                        if (!canEquip) Console.WriteLine($"{Name} cannot equip a{equippable.Slot.ToString().FromTitleOrCamelCase()} item.");
                    }
                    else
                    {
                        System.Console.WriteLine($"{Name} already has the {itemToEquip.Name} equipped.");
                    }
                }
                else
                {
                    System.Console.WriteLine($"{itemToEquip.Name} cannot be equipped.");
                }
            }
            else
            {
                System.Console.WriteLine($"{Name} is not holding a {itemToEquip.Name}.");
            }
            return canEquip;
        }
        
        public void EquipItem(Item itemToEquip)
        {
            if (canEquipItem(itemToEquip)) 
            {
                EquippedItems.Add(itemToEquip as IEquippable);
                System.Console.WriteLine($"{Name} equipped the {itemToEquip.Name}.");
            }
        }
        
        protected bool canUnequipItem(Item itemToUnequip)
        {
            if (Inventory.Contains(itemToUnequip))
            {
                if (itemToUnequip is IEquippable)
                {
                    if (EquippedItems.Contains(itemToUnequip as IEquippable))
                    {
                        return true;
                    }
                    else
                    {
                        System.Console.WriteLine($"{itemToUnequip.Name} is not currently equipped");
                    }
                }
                else
                {
                    System.Console.WriteLine($"{itemToUnequip.Name} is not an equippable item.");
                }
            }
            else
            {
                System.Console.WriteLine($"{Name} is not holding a {itemToUnequip.Name}");
            }
            return false;
        }

        public void UnequipItem(Item itemToUnequip)
        {
            if (canUnequipItem(itemToUnequip)) 
            {
                EquippedItems.Remove(itemToUnequip as IEquippable);
                System.Console.WriteLine($"{Name} unequipped the {itemToUnequip.Name}.");
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

        public void RemoveFromMemory(INameable asset)
        {
            if (_memory.Contains(asset)) _memory.Remove(asset);
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
            if (_currentHp == 0) System.Console.WriteLine($"{Name} is dead!");
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
        
        public bool AttackRoll(Creature targetCreature, Weapon equippedWeapon = null)
        {
            int totalAttackMod = _attackModifier;
            if (equippedWeapon != null) totalAttackMod += _attackModifier;
            int attackResult = Dice.D20.Roll(1, totalAttackMod);
            return attackResult >= targetCreature.ArmorClass;
        }

        public int DamageRoll(Weapon equippedWeapon = null)
        {
            int damageSum = 0;
            int totalDamageMod = _damageModifier;
            if (equippedWeapon != null)
            {
                totalDamageMod += equippedWeapon.DamageModifier;
                foreach (var die in equippedWeapon.DamageDice) damageSum += die.Roll(1, totalDamageMod);
            }
            else
            {
                foreach (var die in _damageDice) damageSum += die.Roll(1, totalDamageMod);
            }
            return damageSum;
        }

        public void Attack(Map map, Creature targetCreature)
        {
            if (validateTargetOnMap(map, targetCreature, _attackRange))
            {
                if (!targetCreature.IsDead)
                {
                    if (EquippedWeapons.Any())
                    {
                        var weaponHits = new Dictionary<Weapon, bool>();
                        foreach (var weapon in EquippedWeapons)
                        {
                            System.Console.WriteLine($"{Name} is attacking {targetCreature.Name} with the {weapon.Name}!");
                            targetCreature.AddToMemory(weapon);
                            weaponHits.Add(weapon, AttackRoll(targetCreature, weapon));
                        }
                        foreach (var hit in weaponHits)
                        {
                            if (hit.Value)
                            {
                                int damage = DamageRoll(hit.Key);
                                System.Console.WriteLine($"{Name} dealt {damage} damage to {targetCreature.Name}!");
                                targetCreature.ChangeHp(-damage);
                            }
                            else
                            {
                                System.Console.WriteLine($"{Name} missed the attack!");
                            }
                        }
                    }
                    else 
                    {
                        System.Console.WriteLine($"{Name} is attacking {targetCreature.Name}!");
                        if (AttackRoll(targetCreature))
                        {
                            int damage = DamageRoll();
                            System.Console.WriteLine($"{Name} dealt {damage} damage to {targetCreature.Name}!");
                            targetCreature.ChangeHp(-damage);
                        }
                        else
                        {
                            System.Console.WriteLine($"{Name} missed the attack!");
                        }
                    }
                    targetCreature.AddToMemory(this);
                }
                else
                {
                    System.Console.WriteLine($"{targetCreature.Name} is already dead.");
                }
            }
        }
    }

    public enum Gender
    {
        None,
        Male,
        Female,
        NonBinary
    }

    public enum CreatureCategory
    {
        Humanoid,
        Beast,
        Undead,
        Monstrosity
    }
}