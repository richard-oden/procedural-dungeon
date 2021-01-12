using System;
using System.Collections.Generic;
using System.Linq;
using static System.ConsoleKey;
using static ProceduralDungeon.ExtensionsAndHelpers;

namespace ProceduralDungeon
{
    public class Player : Creature
    {
        public int Strength {get; protected set;}
        public int Endurance {get; protected set;}
        public int Perception {get; protected set;}
        public int Charisma {get; protected set;}
        public int Level {get; protected set;}
        private int _exp;
        private int _expCeiling => (int)Math.Round((4 * Math.Pow(Level, 3)) / 5.0); //exp curve taken from pokemon gen 1 :)
        public override double MaxCarryWeight => Strength * 10;
        public override int SearchRange => Perception;
        protected override int _maxHp => 2 + (int)Math.Round(Endurance * (Level * 1.05));
        protected override int _damageModifier => Strength - 5;
        protected override int _attackModifier => Perception - 5;
        protected override int _baseDamageResistance => (Endurance - 5) / 2;
        // Represents the markdown on sold items and markup on purchased items:
        // e.g., with a value of .2, an item worth 10 gold is solid for 8 and purchased for 12.
        public double TradeMarkup => .2 - (Perception - 5) * .05;
        public PlayerBackground Background {get; protected set;}
        public Player(string name, int id, PlayerBackground background, int level = 1, Gender gender = Gender.NonBinary, 
            Point location = null, List<INameable> memory = null) :
            base(name, id, 6, CreatureCategory.Humanoid, gender, location, background.Inventory, background.StartingGold, memory)
        {
            Team = 0;
            
            Strength = 5 + background.StrengthMod;
            Endurance = 5 + background.EnduranceMod;
            Perception = 5 + background.PerceptionMod;
            Charisma = 5 + background.CharismaMod;
            Background = background;
            Level = level;

            _currentHp = _maxHp;
        }

        public string GetDetails()
        {
            return $@"{Name}, the {Background.Name} - Lvl {Level} - Exp: {_exp}/{_expCeiling}
HP: {_currentHp}/{_maxHp} - AC: {ArmorClass} - DR: {DamageResistance} - Weight carried: {GetCarryWeightString()}";
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
            else if (input.Key == Escape) {}// Quit menu
            else 
            {
                System.Console.WriteLine("Hotkey not recognized. Press 'H' for a full list of hotkeys.");
                WaitForInput();
            }
            return true;
        }

        public void ShowHotkeys()
        {
            Console.WriteLine("All hotkeys:");
            Console.WriteLine("- Movement keys: Q, W, E, A, D, Z, X, C");
            Console.WriteLine("- Map legend: L");
            Console.WriteLine("- Search: S");
            Console.WriteLine("- Show memory: R");
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
            var foundAssets = getVisibleAssets(map).Where(a => a is INameable).ToList();
            if (!foundAssets.Any())
            {
                Console.WriteLine($"{Name} searched but couldn't find anything!");
                WaitForInput();
            }
            else
            {
                var foundAssetsMenu = new IMappableMenu($"{Name} searched and found the following. Press Up/Down to highlight object and Enter/Esc to exit.", foundAssets, this, map);
                foundAssetsMenu.Open();
                foreach (var fA in foundAssets.Where(fA => fA is INameable)) AddToMemory(fA as INameable);
            }
        }
    
        public void Recall(Map map)
        {
            System.Console.WriteLine();
            var memoryMenu = new IMappableMenu($"{Name}'s memory. Press Up/Down to highlight object and Enter/Esc to exit.", _memory.Cast<IMappable>().ToList(), this, map);
            memoryMenu.Open();
        }
    
        public void Interact(Map map)
        {
            // Valid objects must be in inventory,
            var validInteractables = _memory.Where(m => m is IInteractable)
                .Where(m => (m is Item && Inventory.Contains(m as Item)) || 
                // or located on map,
                (m is IMappable && (m as IMappable).Location != null &&
                // and be adjacent to player:
                (m as IMappable).Location.InRangeOf(Location, 1))).Cast<IMappable>().ToList();
            var interactMenu = new IMappableMenu("Select an object to interact with. Press Up/Down to change selection, Enter to interact, and Esc to exit.", validInteractables, this, map);
            var thingToInteractWith = interactMenu.Open();
            if (thingToInteractWith != null)
            {
                (thingToInteractWith as IInteractable).Activate(this);
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

            var pickUpItemMenu = new IMappableMenu("Select item to pick up. Press Up/Down to change selection, Enter to pick up, and Esc to exit.", validItemsAsIMappable, this, map);
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
                var dropItemMenu = new IMappableMenu("Select item to drop. Press Up/Down to change selection, Enter to drop, and Esc to exit.", Inventory.Cast<IMappable>().ToList(), this);
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
            var equipMenu = new IMappableMenu("Select item to equip. Press Up/Down to change selection, Enter to equip, and Esc to exit.", validItemsAsIMappable, this);
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
            var unequipMenu = new IMappableMenu("Select item to unequip. Press Up/Down to change selection, Enter to unequip, and Esc to exit.", EquippedItems.Cast<IMappable>().ToList(), this);
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
            var attackMenu = new IMappableMenu("Select creature to attack. Press Up/Down to change selection, Enter to attack, and Esc to exit.", targetsAsIMappable, this, map);
            var target = (Creature)attackMenu.Open();
            if (target != null)
            {
                if (!target.IsDead)
                {
                    base.Attack(map, target);
                    if (target is Npc && target.IsDead) GainExp((target as Npc).ChallengeLevel);
                }
                else
                {
                    System.Console.WriteLine($"{target.Name} is already dead.");
                }
            }
            WaitForInput();
        }

        public void GainExp(int exp)
        {
            _exp += exp;
            System.Console.WriteLine($"{Name} gained {exp} exp.");
            while (_exp >= _expCeiling)
            {
                Level++;
                Console.WriteLine($"{Name} leveled up! {Pronouns[0]} is/are now level {Level}.");
                _currentHp = _maxHp;
                if (Level % 2 == 0) attributePointBuy(1);
            }
        }

        private void attributePointBuy(int points)
        {
            int cursor = 0;
            
            // TODO: Make this dry:
            void transferPoints(int amount)
            {
                if (cursor == 0)
                {
                    Strength += amount;
                }
                else if (cursor == 1)
                {
                    Endurance += amount;
                }
                else if (cursor == 2)
                {
                    Perception += amount;
                }
                else if (cursor == 3)
                {
                    Charisma += amount;
                }
                points -= amount;
            }
            
            void handleInput(ConsoleKeyInfo input)
            {
                int tempCursor = cursor;

                if (input.Key == ConsoleKey.UpArrow) tempCursor--;
                else if (input.Key == ConsoleKey.DownArrow) tempCursor++;
                else if (input.Key == ConsoleKey.Enter) transferPoints(1);

                int cursorMax = 3;
                if (tempCursor > cursorMax) tempCursor = cursorMax;
                else if (tempCursor < 0) tempCursor = 0;
                else cursor = tempCursor;
            }

            while (points > 0)
            {
                string[] attributeStrings = new[]
                {
                    $"Strength: {Strength}",
                    $"Endurance: {Endurance}",
                    $"Perception: {Perception}",
                    $"Charisma: {Charisma}"
                };
                Console.WriteLine($"Choose which skills to increase. You have {points} point(s) remaining.");
                for (int i = 0; i < attributeStrings.Length; i++)
                {
                    if (i == cursor) Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(attributeStrings[i]);
                    Console.ResetColor();
                }
                Console.WriteLine("Use Up/Down arrow keys to select an attribute and Enter to increase it.");
                handleInput(Console.ReadKey());
                Console.Clear();
            }
        }
    
        public void RemoveAllFromMemoryIfNotOnMap(Map map)
        {
            _memory.RemoveAll(m => !map.AllAssets.Contains(m as IMappable));
        }
    }
}