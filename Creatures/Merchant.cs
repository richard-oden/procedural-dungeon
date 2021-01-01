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
            base(name, id, difficulty: Difficulty.Medium, hp, ac: 12, dr: 2, attackMod: 4, damageDice: new Die[]{Dice.D4}, damageMod: 2,
            attackRange: 1, searchRange: 10, gender, location, maxCarryWeight: 300, 
            inventory: inventory, gold: gold, memory: memory)
        {
            Team = team;
            if (name == null) Name = NpcNameGenerator.Generate();
            string merchantDescription = $"{Pronouns[0]} appear(s) to be selling an assortment of various wares.";
            _baseDescription += _baseDescription == null ? merchantDescription : " " + merchantDescription;
            _player = player;
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
        public void OpenTrade()
        {
            int cursorX = 0;
            int cursorY = 0;
            bool stillTrading = true;

            while (stillTrading)
            {
                Item selectedItem = (this as IContainer).ListTwoInventoriesAndSelect(_player, cursorX, cursorY);
                System.Console.WriteLine("\nUse the arrow keys to navigate, Enter to buy/sell an item, or Esc to stop trading.");
                var input = Console.ReadKey();

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
                    case ConsoleKey.Enter:
                        if (_player.Inventory.Contains(selectedItem))
                        {
                            var buyInput = PromptKey($"\nI'll purchase the {selectedItem.Name} for {(int)Math.Round(selectedItem.Value * 0.75)} gold. Deal? (Y/N)");
                            if (buyInput == ConsoleKey.Y)
                            {
                                (_player as IContainer).TradeItem(selectedItem, this, requireGold: true, discount: 0.75);
                                Console.WriteLine("Thank you good sir/ma'am!");
                            }
                            else if (buyInput == ConsoleKey.N)
                            {
                                Console.WriteLine("All right, but you won't find a better price elsewhere!");
                            }
                            else
                            {
                                Console.WriteLine("Sorry, I didn't catch that.");
                            }
                        }
                        else if (Inventory.Contains(selectedItem))
                        {
                            var sellInput = PromptKey($"\nI'll sell you the {selectedItem.Name} for {(int)Math.Round(selectedItem.Value * 1.25)} gold. Deal? (Y/N)");
                            if (sellInput == ConsoleKey.Y)
                            {
                                (this as IContainer).TradeItem(selectedItem, _player, requireGold: true, discount: 0.75);
                                Console.WriteLine("Thank you good sir/ma'am!");
                            }
                            else if (sellInput == ConsoleKey.N)
                            {
                                Console.WriteLine("All right, but you won't find a better price elsewhere!");
                            }
                            else
                            {
                                Console.WriteLine("Sorry, I didn't catch that.");
                            }
                        }
                        WaitForInput();
                        break;
                    case ConsoleKey.Escape:
                        Console.WriteLine("Pleasure doing business with you.");
                        WaitForInput();
                        stillTrading = false;
                        break;
                }
                int cursorYMax = tempCursorX <= 0 ? _player.Inventory.Count() : Inventory.Count();

                if (tempCursorX < 0) cursorX = 0;
                else if (tempCursorX > 1) cursorX = 1;
                else cursorX = tempCursorX;

                if (tempCursorY < 0) cursorY = 0;
                else if (tempCursorY >= cursorYMax) cursorY = cursorYMax - 1;
                else cursorY = tempCursorY;
                Console.Clear();
            }
        }  
        public void Activate()
        {
            if (!IsDead)
            {
                Console.Clear();
                System.Console.WriteLine("Hello adventurer! May I interest you in a trade?");
                OpenTrade();
            }
            else
            {
                System.Console.WriteLine($"{Name} is dead.");
            }
        }
    }
}