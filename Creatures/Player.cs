using System;
using System.Collections.Generic;
using System.Linq;
using static System.ConsoleKey;
using static ProceduralDungeon.ExtensionsAndHelpers;

namespace ProceduralDungeon
{
    public class Player : Creature
    {
        public Player(string name, int id, int hp, Gender gender = Gender.NonBinary, Point location = null,
            List<Item> inventory = null, int gold = 0, List<INameable> memory = null) :
            base (name, id, hp, gender, location, inventory, gold, memory)
        {
            SearchRange = 8;
            _maxCarryWeight = 100;
            Team = 0;
        }

        public bool ParseInput(Map map, ConsoleKeyInfo input)
        {
            if (new[]{Q, W, E, A, D, Z, X, C}.Contains(input.Key)) map.Move(this, input);
            else if (input.Key == S) Search(map);
            else if (input.Key == L) Map.ShowLegend();
            else if (input.Key == H) ShowHotkeys();
            else if (input.Key == Tab) {}// Switch to command input
            else if (input.Key == I) ListInventory();
            else if (input.Key == P) PickUpItem(map);
            else if (input.Key == O) DropItem(map);
            else if (input.Key == U) EquipItem();
            else if (input.Key == Y) UnequipItem();
            else if (input.Key == J) Interact();
            else if (input.Key == F) Attack(map);
            else if (input.Key == R) Recall();
            else if (input.Key == T) Describe();
            else if (input.Key == Escape) {}// Quit menu
            else System.Console.WriteLine("Hotkey not recognized. Press 'H' for a full list of hotkeys.");
            return true;
        }

        public void ShowHotkeys()
        {
            Console.WriteLine("All hotkeys:");
            Console.WriteLine("- Movement keys: Q, W, E, A, D, Z, X, C");
            Console.WriteLine("- Map legend: L");
            Console.WriteLine("- Search: S");
            Console.WriteLine("- Show memory: R");
            Console.WriteLine("- Describe: T");
            Console.WriteLine("- Inventory: I");
            Console.WriteLine("- Pick up item: P");
            Console.WriteLine("- Drop item: O");
            Console.WriteLine("- Equip item: U");
            Console.WriteLine("- Unequip item: Y");
            Console.WriteLine("- Interact: J");
            Console.WriteLine("- Attack: F");
            Console.WriteLine("- Show all hotkeys: H");
            Console.WriteLine("- Toggle between hotkey and command input: Tab");
            Console.WriteLine("- Quit: Escape");
            WaitForInput();
        }
        
        public void ShowCommands()
        {
            Console.WriteLine("- Movement: move [direction] (e.g., n, s, se, nw, etc");
            Console.WriteLine("- Map legend: legend");
            Console.WriteLine("- Search: search");
            Console.WriteLine("- Show memory: recall");
            Console.WriteLine("- Describe: describe [target]");
            Console.WriteLine("- Inventory: inventory");
            Console.WriteLine("- Pick up item: pick up [item]");
            Console.WriteLine("- Drop item: drop [item]");
            Console.WriteLine("- Equip item: equip [item]");
            Console.WriteLine("- Unequip item: unequip [item]");
            Console.WriteLine("- Interact: interact [target]");
            Console.WriteLine("- Attack: attack [creature]");
            Console.WriteLine("- Show all commands: help");
            Console.WriteLine("- Toggle between hotkey and command input: switch");
            Console.WriteLine("- Quit: quit");
            WaitForInput();
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
            WaitForInput();
        }
    
        public void Recall()
        {
            System.Console.WriteLine($"{Name}'s memory:");
            var convertedMemory = from a in _memory where a is IMappable select (a as IMappable);
            convertedMemory.ListDistanceAndDirectionFrom(Location);
            WaitForInput();
        }
    
        public void Describe()
        {
            var input = Prompt("Enter the name of the thing to describe:");

            INameable thingToDescribe = input.ToLower() == Name.ToLower() ? this : _memory.GetByName(input);

            if (thingToDescribe != null && thingToDescribe is IDescribable)
            {
                System.Console.WriteLine((thingToDescribe as IDescribable).Description);
            }
            else
            {
                System.Console.WriteLine($"Hmmm. {input} doesn't ring any bells.");
            }
            WaitForInput();
        }
    
        public void Interact()
        {
            var input = Prompt("Enter the name of the thing to interact with:");
            var thingToInteractWith = _memory.GetByName(input);
            if (thingToInteractWith != null && thingToInteractWith is IInteractable)
            {
                base.Interact(thingToInteractWith as IInteractable);
            }
            else
            {
                System.Console.WriteLine($"Hmmm. {input} doesn't ring any bells.");
            }
            WaitForInput();
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
                    Console.WriteLine("(Currently Equipped)");
                }
                if (Inventory.IndexOf(i) != Inventory.Count -1) Console.WriteLine("---");
            }
            Console.WriteLine();
            WaitForInput();
        }

        public void PickUpItem(Map map)
        {
            var input = Prompt("Enter name of item to pick up:");
            var itemToPickUp = map.Items.GetByName(input);
            if (itemToPickUp != null)
            {
                base.PickUpItem(map, itemToPickUp as Item);
            }
            else
            {
                System.Console.WriteLine($"{Name} could not find the {input}!");
            }
            WaitForInput();
        }
        
        public void DropItem(Map map)
        {
            if (Inventory.Any())
            {
                var input = Prompt("Enter name of item to drop:");
                var itemToDrop = Inventory.GetByName(input);
                if (itemToDrop != null)
                {
                    base.DropItem(map, itemToDrop as Item);
                }
                else
                {
                    System.Console.WriteLine($"{Name} could not find the {input}!");
                }
            }
            else
            {
                System.Console.WriteLine($"{Name} has nothing to drop!");
            }
            WaitForInput();
        }
    
        public void EquipItem()
        {
            if (Inventory.Any())
            {
                var input = Prompt("Enter name of item to equip:");
                var itemToEquip = _memory.Where(a => 
                    a is IEquippable && !EquippedItems.Contains(a as IEquippable))
                    .GetByName(input);
                if (itemToEquip != null)
                {
                    base.EquipItem(itemToEquip as Item);
                }
                else
                {
                    System.Console.WriteLine($"{Name} could not equip the {input}.");
                }
            }
            else
            {
                System.Console.WriteLine($"{Name} has nothing to equip!");
            }
            WaitForInput();
        }
        
        public void UnequipItem()
        {
            if (EquippedItems.Any())
            {
                var input = Prompt("Enter name of item to unequip:");
                var itemToEquip = _memory.GetByName(input);
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
                    System.Console.WriteLine($"{Name} could not find the {input}!");
                }
            }
            else
            {
                System.Console.WriteLine($"{Name} has nothing equipped!");
            }
            WaitForInput();
        }
        
        public void Attack(Map map)
        {
            var input = Prompt("Enter the name of the creature to attack:");
            var targets = map.Creatures.Where(a => Location.InRangeOf(a.Location, _attackRange));
            var target = targets.GetByName(input);
            if (target != null)
            {
                base.Attack(map, target as Creature);
            }
            else
            {
                System.Console.WriteLine($"{Name} does not know of the creature or it is out of range!");
            }
            WaitForInput();
        }
    }
}