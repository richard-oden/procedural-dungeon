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
            MaxCarryWeight = 100;
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
            else if (input.Key == J) Interact(map);
            else if (input.Key == F) Attack(map);
            else if (input.Key == R) Recall(map);
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
            var foundAssets = GetVisibleAssets(map).Where(a => a is INameable).ToList();
            if (!foundAssets.Any())
            {
                Console.WriteLine($"{Name} searched but couldn't find anything!");
            }
            else
            {
                var foundAssetsMenu = new Menu($"{Name} searched and found the following. Press Up/Down to highlight object and Enter/Esc to exit.", foundAssets, this, map);
                foundAssetsMenu.Open();
                foreach (var fA in foundAssets.Where(fA => fA is INameable)) AddToMemory(fA as INameable);
            }
        }
    
        public void Recall(Map map)
        {
            System.Console.WriteLine();
            var memoryMenu = new Menu($"{Name}'s memory. Press Up/Down to highlight object and Enter/Esc to exit.", _memory.Cast<IMappable>().ToList(), this, map);
            memoryMenu.Open();
        }
    
        public void Describe()
        {
            var input = PromptLine("Enter the name of the thing to describe:");

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
    
        public void Interact(Map map)
        {
            // Valid objects must be in inventory,
            var validInteractables = _memory.Where(m => (m is Item && Inventory.Contains(m as Item)) || 
                // or located on map,
                (m is IMappable && (m as IMappable).Location != null &&
                // and be adjacent to player:
                (m as IMappable).Location.InRangeOf(Location, 1))).Cast<IMappable>().ToList();
            var interactMenu = new Menu("Select an object to interact with. Press Up/Down to change selection, Enter to interact, and Esc to exit.", validInteractables, this, map);
            var thingToInteractWith = interactMenu.Open();
            if (thingToInteractWith != null)
            {
                (thingToInteractWith as IInteractable).Activate();
            }
            else
            {
                System.Console.WriteLine("Not sure what to interact with.");
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
            var validItemsAsIMappable = map.Items.Where(i => 
                i.Location != null && Location.InRangeOf(i.Location, 1) && _memory.Contains(i))
                .Cast<IMappable>().ToList();

            var pickUpItemMenu = new Menu("Select item to pick up. Press Up/Down to change selection, Enter to pick up, and Esc to exit.", validItemsAsIMappable, this, map);
            var itemToPickUp = pickUpItemMenu.Open();
            if (itemToPickUp != null)
            {
                base.PickUpItem(map, itemToPickUp as Item);
            }
            else
            {
                System.Console.WriteLine("Not sure what to pick up.");
            }
            WaitForInput();
        }
        
        public void DropItem(Map map)
        {
            if (Inventory.Any())
            {
                var dropItemMenu = new Menu("Select item to drop. Press Up/Down to change selection, Enter to drop, and Esc to exit.", Inventory.Cast<IMappable>().ToList(), this);
                var itemToDrop = dropItemMenu.Open();
                if (itemToDrop != null)
                {
                    base.DropItem(map, itemToDrop as Item);
                }
                else
                {
                    System.Console.WriteLine("Not sure what to drop.");
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
             var validItemsAsIMappable = Inventory.Where(i => i is IEquippable && !EquippedItems.Contains(i as IEquippable)).Cast<IMappable>().ToList();
            var equipMenu = new Menu("Select item to equip. Press Up/Down to change selection, Enter to equip, and Esc to exit.", validItemsAsIMappable, this);
            var itemToEquip = equipMenu.Open();
            if (itemToEquip != null)
            {
                base.EquipItem(itemToEquip as Item);
            }
            else
            {
                System.Console.WriteLine("Not sure what to equip.");
            }
            WaitForInput();
        }
        
        public void UnequipItem()
        {
            var unequipMenu = new Menu("Select item to unequip. Press Up/Down to change selection, Enter to unequip, and Esc to exit.", EquippedItems.Cast<IMappable>().ToList(), this);
            var itemToUnequip = unequipMenu.Open();
            if (itemToUnequip != null)
            {
                base.UnequipItem(itemToUnequip as Item);
            }
            else
            {
                System.Console.WriteLine("Not sure what to unequip.");
            }
            WaitForInput();
        }
        
        public void Attack(Map map)
        {
            var targetsAsIMappable = map.Creatures.Where(a => Location.InRangeOf(a.Location, _attackRange) && a != this).Cast<IMappable>().ToList();
            var attackMenu = new Menu("Select creature to attack. Press Up/Down to change selection, Enter to attack, and Esc to exit.", targetsAsIMappable, this, map);
            var target = attackMenu.Open();
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