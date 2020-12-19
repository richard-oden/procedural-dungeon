using System;
using System.Collections.Generic;
using System.Linq;
using static System.ConsoleKey;
using static ProceduralDungeon.ExtensionsAndHelpers;

namespace ProceduralDungeon
{
    public class Player : Creature
    {
        public Player(string name, int id, int hp, int speed, Point location = null,
            List<Item> inventory = null, List<INameable> memory = null) :
            base (name, id, hp, speed, location, inventory, memory)
        {
            SearchRange = 8;
            _maxCarryWeight = 100;
        }

        public bool ParseInput(Map map, ConsoleKeyInfo input)
        {
            if (new[]{Q, W, E, A, D, Z, X, C}.Contains(input.Key)) map.Move(this, input);
            else if (input.Key == S) Search(map);
            else if (input.Key == L) Map.ShowLegend();
            else if (input.Key == M) {}// Open menu
            else if (input.Key == I) ListInventory();
            else if (input.Key == P) PickUpItem(map);
            else if (input.Key == O) DropItem(map);
            else if (input.Key == U) EquipItem();
            else if (input.Key == Y) UnequipItem();
            else if (input.Key == J) {}// Interact
            else if (input.Key == F) Attack(map);// Attack
            else if (input.Key == R) Recall();
            else if (input.Key == T) Describe();
            else if (input.Key == Escape) {}// Quit menu
            else System.Console.WriteLine("Command not recognized. Press 'M' to open the menu for a full list of commands.");
            return true;
        }

        public override void Search(Map map)
        {
            var foundAssets = GetVisibleAssets(map).Where(a => a is INameable);
            if (!foundAssets.Any())
            {
                Console.WriteLine($"{Name} searched but couldn't find anything!");
            }
            else
            {
                System.Console.WriteLine($"{Name} searched and found:");
                foundAssets.ListDistanceAndDirectionFrom(Location);
                foreach (var fA in foundAssets.Where(fA => fA is INameable)) AddToMemory(fA as INameable);
            }
            PressAnyKeyToContinue();
        }
    
        public void Recall()
        {
            System.Console.WriteLine($"{Name}'s memory:");
            var convertedMemory = from a in _memory where a is IMappable select (a as IMappable);
            convertedMemory.ListDistanceAndDirectionFrom(Location);
            PressAnyKeyToContinue();
        }
    
        public void Describe()
        {
            Console.WriteLine("Enter the name of the thing to describe:");
            var thingToDescribe = _memory.GetByName(Console.ReadLine());
            if (thingToDescribe != null && thingToDescribe is IDescribable)
            {
                System.Console.WriteLine((thingToDescribe as IDescribable).Description);
            }
            else
            {
                System.Console.WriteLine("Hmmm. That doesn't ring any bells.");
            }
            PressAnyKeyToContinue();
        }
    
        public void ListInventory()
        {
            System.Console.WriteLine($"{Name}'s inventory:");
            System.Console.WriteLine();
            foreach(var i in Inventory)
            {
                Console.WriteLine(i.GetDetails());
                if (i is IEquippable && EquippedItems.Contains(i as IEquippable))
                {
                    Console.WriteLine($"Currently equipped:{(i as IEquippable).Slot.ToString().FromTitleOrCamelCase()} slot");
                }
                if (Inventory.IndexOf(i) != Inventory.Count -1) Console.WriteLine("---");
            }
            Console.WriteLine();
            PressAnyKeyToContinue();
        }

        public void PickUpItem(Map map)
        {
            Console.WriteLine("Enter name of item to pick up:");
            var itemToPickUp = map.Items.GetByName(Console.ReadLine());
            if (itemToPickUp != null)
            {
                base.PickUpItem(map, itemToPickUp as Item);
            }
            else
            {
                System.Console.WriteLine($"{Name} could not find the item!");
            }
            PressAnyKeyToContinue();
        }
        
        public void DropItem(Map map)
        {
            if (Inventory.Any())
            {
                System.Console.WriteLine("Enter name of item to drop:");
                var itemToDrop = Inventory.GetByName(Console.ReadLine());
                if (itemToDrop != null)
                {
                    base.DropItem(map, itemToDrop as Item);
                }
                else
                {
                    System.Console.WriteLine($"{Name} could not find the item!");
                }
            }
            else
            {
                System.Console.WriteLine($"{Name} has nothing to drop!");
            }
            PressAnyKeyToContinue();
        }
    
        public void EquipItem()
        {
            if (Inventory.Any())
            {
                System.Console.WriteLine("Enter name of item to equip:");
                var itemToEquip = _memory.Where(a => 
                    a is IEquippable && !EquippedItems.Contains(a as IEquippable))
                    .GetByName(Console.ReadLine());
                if (itemToEquip != null)
                {
                    base.EquipItem(itemToEquip as Item);
                }
                else
                {
                    System.Console.WriteLine($"Invalid item! It might not exist, might be unequippable, might not be in memory, or might already be equipped.");
                }
            }
            else
            {
                System.Console.WriteLine($"{Name} has nothing to equip!");
            }
            PressAnyKeyToContinue();
        }
        
        public void UnequipItem()
        {
            if (EquippedItems.Any())
            {
                System.Console.WriteLine("Enter name of item to unequip:");
                var itemToEquip = _memory.GetByName(Console.ReadLine());
                if (itemToEquip != null)
                {
                    if (itemToEquip is Item)
                    {
                        base.UnequipItem(itemToEquip as Item);
                    }
                    else
                    {
                        System.Console.WriteLine($"{itemToEquip.Name} is not an item!");
                    }
                }
                else
                {
                    System.Console.WriteLine($"{Name} could not find the item!");
                }
            }
            else
            {
                System.Console.WriteLine($"{Name} has nothing equipped!");
            }
            PressAnyKeyToContinue();
        }
        
        public void Attack(Map map)
        {
            Console.WriteLine("Enter the name of the creature to attack:");
            var targets = map.Creatures.Where(a => Location.InRangeOf(a.Location, _attackRange));
            var target = targets.GetByName(Console.ReadLine());
            if (target != null)
            {
                base.Attack(map, target as Creature);
            }
            else
            {
                System.Console.WriteLine($"{Name} does not know of the creature or it is out of range!");
            }
            PressAnyKeyToContinue();
        }
    }
}