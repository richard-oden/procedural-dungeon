using System;
using System.Collections.Generic;
using System.Linq;
using static ProceduralDungeon.ExtensionsAndHelpers;
using ProceduralDungeon.TextGeneration;

namespace ProceduralDungeon
{
    public class Merchant : Npc, IInteractable
    {
        private Player _player {get; set;}
        public Merchant(int id, int hp, Player player, string name = null, 
            Gender gender = Gender.NonBinary, Point location = null,
            List<Item> inventory = null, int gold = 0, List<INameable> memory = null, int team = 0) :
            base (name, id, hp, gender, location, inventory, gold, memory)
        {
            Team = team;
            SearchRange = 5;
            if (name == null) Name = NpcNameGenerator.Generate();
            string merchantDescription = $"{Pronouns[0]} appear(s) to be selling an assortment of various wares.";
            _baseDescription += _baseDescription == null ? merchantDescription : " " + merchantDescription;
            _player = player;
            _maxCarryWeight = 300;
        }

        public override void Act(Map map)
        {
            if (!IsDead)
            {
                var visibleEnemies = GetVisibleAssets(map).Where(a => a is Creature)
                    .Cast<Creature>().Where(c => c.Team != this.Team);
                if (visibleEnemies.Any())
                {
                    var knownVisibleEnemies = visibleEnemies.Where(vE => _memory.Contains(vE));
                    if (knownVisibleEnemies.Any())
                    {
                        var fleeTarget = knownVisibleEnemies.RandomElement();
                        System.Console.WriteLine($"{Name} is fleeing from the {fleeTarget.Name}!");
                        MoveAwayFrom(map, fleeTarget.Location);
                    }
                    else
                    {
                        System.Console.WriteLine($"{Name} has spotted something. It's searching...");
                        WaitForInput();
                        Search(map);
                    }
                }
                else
                {
                    Wander(map);
                }
            }
        }

        public void ListBothInventories(int cursorX, int cursorY)
        {
            Item highlightedItem = cursorX == 0 ? _player.Inventory[cursorY] : Inventory[cursorY];
            int listLength = _player.Inventory.Count() > Inventory.Count() ? _player.Inventory.Count() : Inventory.Count();
            int padSize = _player.Inventory.Select(i => i.GetBasicDetails().Length).Max() + 5;
            Console.WriteLine($"{_player.Name} ({_player.Gold} gold):".PadRight(padSize, ' ') + $"{Name} ({Gold} gold):\n\n");
            for (int y = 0; y < listLength; y++)
            {
                for (int x = 0; x < 2; x++)
                {
                    if (cursorX == x && cursorY == y) Console.ForegroundColor = ConsoleColor.DarkYellow;
                    if (x == 0)
                    {
                        if (_player.Inventory.Count() > y) Console.Write(_player.Inventory[y].GetBasicDetails().PadRight(padSize, ' '));
                        Console.ResetColor();
                    }
                    else if (x == 1)
                    {
                        if (Inventory.Count > y) Console.WriteLine(Inventory[y].GetBasicDetails());
                        else
                        {
                            Console.WriteLine();
                        }
                    }
                    Console.ResetColor();
                }
                System.Console.WriteLine("---".PadRight(padSize, ' ') + "---");
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine('\n' + highlightedItem.GetSecondaryDetails());
            Console.ResetColor();
        }
        public void Trade()
        {
            int cursorX = 0;
            int cursorY = 0;
            
            while (true)
            {
                ListBothInventories(cursorX, cursorY);
                var input = Console.ReadKey();
                Console.Clear();

                int tempCursorX = cursorX;
                int tempCursorY = cursorY;
                switch (input.Key)
                {
                    case ConsoleKey.UpArrow:
                        tempCursorY--;
                        break;
                    case ConsoleKey.DownArrow:
                        tempCursorY++;
                        break;
                    case ConsoleKey.LeftArrow:
                        tempCursorX--;
                        break;
                    case ConsoleKey.RightArrow:
                        tempCursorX++;
                        break;
                }
                int cursorYMax = tempCursorX <= 0 ? _player.Inventory.Count() : Inventory.Count();

                if (tempCursorX < 0) cursorX = 0;
                else if (tempCursorX > 1) cursorX = 1;
                else cursorX = tempCursorX;

                if (tempCursorY < 0) cursorY = 0;
                else if (tempCursorY >= cursorYMax) cursorY = cursorYMax - 1;
                else cursorY = tempCursorY;
            }
        }
        
        public void Activate()
        {
            if (!IsDead)
            {
                System.Console.WriteLine("Hello adventurer! May I interest you in a trade?");
            }
            else
            {
                System.Console.WriteLine($"{Name} is dead.");
            }
        }
    }
}