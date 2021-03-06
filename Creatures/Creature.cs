using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon
{
    public abstract class Creature : IMappable, INameable, IDescribable, IContainer, IDestroyable
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
        public virtual ConsoleColor Color {get; protected set;} = ConsoleColor.Black;
        public int Team {get; protected set;}
        public bool IsDead => _currentHp <= 0;
        public bool IsDestroyed {get; set;} = false;
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

                description += $"{Pronouns[0]} appear(s) {hpString}.";

                if (EquippedWeapons.Any()) 
                {
                    description += $" {Pronouns[0]} is/are wielding {EquippedWeapons.ListWithIndefiniteArticle()}.";
                }

                if (EquippedArmor.Any())
                {
                    description += $" {Pronouns[0]} is/are wearing {EquippedArmor.ListWithIndefiniteArticle()}.";
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
            if (inventory != null) Inventory = inventory.GetClones();
            if (memory != null) _memory = memory;
            foreach (var i in Inventory) AddToMemory(i);
            Gold = gold;
            _baseDescription = baseDescription;
            Category = category;
        }

        public string GetCarryWeightString()
        {
                return $"{Math.Round(CurrentCarryWeight, 1)}/{MaxCarryWeight} lbs";
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
                Console.WriteLine($"{itemToAdd.Name} is too heavy for {Name} to carry.");
                return false;
            }
        }

        public virtual void PickUpItem(Map map, Item itemToPickUp)
        {     
            if (AddItemToInventory(itemToPickUp))
            {
                map.RemoveAsset(itemToPickUp);
                Console.WriteLine($"{Name} picked up the {itemToPickUp.Name}.");
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
                Console.WriteLine($"{itemToRemove.Name} could not be found in {Name}'s inventory.");
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
                    Console.WriteLine($"{Name} dropped the {itemToDrop.Name}.");
                }
                else
                {
                    Console.WriteLine($"There's nowhere to drop the {itemToDrop.Name}");
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
                        Console.WriteLine($"{Name} already has the {itemToEquip.Name} equipped.");
                    }
                }
                else
                {
                    Console.WriteLine($"{itemToEquip.Name} cannot be equipped.");
                }
            }
            else
            {
                Console.WriteLine($"{Name} is not holding a {itemToEquip.Name}.");
            }
            return canEquip;
        }
        
        public void EquipItem(Item itemToEquip)
        {
            if (canEquipItem(itemToEquip)) 
            {
                EquippedItems.Add(itemToEquip as IEquippable);
                Console.WriteLine($"{Name} equipped the {itemToEquip.Name}.");
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
                        Console.WriteLine($"{itemToUnequip.Name} is not currently equipped");
                    }
                }
                else
                {
                    Console.WriteLine($"{itemToUnequip.Name} is not an equippable item.");
                }
            }
            else
            {
                Console.WriteLine($"{Name} is not holding a {itemToUnequip.Name}");
            }
            return false;
        }

        public void UnequipItem(Item itemToUnequip)
        {
            if (canUnequipItem(itemToUnequip)) 
            {
                EquippedItems.Remove(itemToUnequip as IEquippable);
                Console.WriteLine($"{Name} unequipped the {itemToUnequip.Name}.");
            }
        }
        
        protected bool hasLineOfSightTo(Map map, Point target)
        {
            return !map.GetPathObstructions(Location, target).Any();
        }
        
        protected IMappable[] getVisibleAssets(Map map)
        {
            return map.GetAssetsInRangeOf(Location, SearchRange)
                .Where(a => a != this)
                .Where(a => this.hasLineOfSightTo(map, a.Location)).ToArray();
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
            var foundAssets = getVisibleAssets(map).Where(a => a is INameable);
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
            if (_currentHp == 0) Console.WriteLine($"{Name} is dead!");
        }
        
        protected bool validateAttackTarget(Map map, Creature targetCreature, int range)
        {
            if (_memory.Contains(targetCreature))
            {
                if (getVisibleAssets(map).Contains(targetCreature))
                {
                    if (Location.InRangeOf(targetCreature.Location, range))
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"{targetCreature.Name} is out of {Name}'s reach!");
                    }
                }
                else
                {
                    Console.WriteLine($"{Name} cannot see {targetCreature.Name}!");
                }
            }
            else
            {
                Console.WriteLine($"{Name} does not know know about the {targetCreature.Name}!");
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
                foreach (var die in equippedWeapon.DamageDice) damageSum += die.Roll();
            }
            else
            {
                foreach (var die in _damageDice) damageSum += die.Roll();
            }
            return damageSum + totalDamageMod;
        }

        //TODO: Dry this code up:
        public void Attack(Map map, Creature targetCreature)
        {
            if (EquippedWeapons.Any())
            {
                var weaponHits = new Dictionary<Weapon, bool>();
                foreach (var weapon in EquippedWeapons)
                {
                    Console.WriteLine($"{Name} is attacking {targetCreature.Name} with the {weapon.Name}!");
                    if (validateAttackTarget(map, targetCreature, weapon.Range))
                    {
                        // Keep tally of which weapons hit:
                        weaponHits.Add(weapon, AttackRoll(targetCreature, weapon));
                        // Thrown weapons are destroyed after use:
                        if (weapon.IsThrown) weapon.IsDestroyed = true;
                        // Target creature now knows of this weapon:
                        targetCreature.AddToMemory(weapon);
                    }
                }
                foreach (var hit in weaponHits)
                {
                    if (hit.Value)
                    {
                        int damage = DamageRoll(hit.Key);
                        Console.WriteLine($"{Name} dealt {damage} damage to {targetCreature.Name} with the {hit.Key.Name}!");
                        targetCreature.ChangeHp(-damage);
                        map.AddBloodSplatter(targetCreature.Location);
                    }
                    else
                    {
                        Console.WriteLine($"{Name} missed the attack with the {hit.Key.Name}!");
                    }
                }
            }
            else 
            {
                Console.WriteLine($"{Name} is attacking {targetCreature.Name}!");
                if (validateAttackTarget(map, targetCreature, _attackRange))
                {
                    if (AttackRoll(targetCreature))
                    {
                        int damage = DamageRoll();
                        Console.WriteLine($"{Name} dealt {damage} damage to {targetCreature.Name}!");
                        targetCreature.ChangeHp(-damage);
                        map.AddBloodSplatter(targetCreature.Location);
                    }
                    else
                    {
                        Console.WriteLine($"{Name} missed the attack!");
                    }
                }
            }
            // Target creature now knows of attacker
            targetCreature.AddToMemory(this);
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