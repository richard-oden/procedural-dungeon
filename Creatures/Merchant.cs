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
            base(name, id, challengeLevel: 0, hp, ac: 12, dr: 2, attackMod: 4, damageDice: new Die[]{Dice.D4}, damageMod: 2,
            attackRange: 1, searchRange: 10, gender, location, maxCarryWeight: 1000, 
            inventory: inventory, gold: gold, memory: memory)
        {
            Team = team;
            if (name == null) Name = NpcNameGenerator.Generate();
            string merchantDescription = $"{Pronouns[0]} appear(s) to be selling an assortment of various wares.";
            _baseDescription += _baseDescription == null ? merchantDescription : " " + merchantDescription;
            _player = player;
        }

        public static Merchant GenerateUsingDifficulty(Difficulty difficulty, int level, Player player)
        {
            
            return new Merchant(id: 001, hp: 100, player, 
            inventory: generateInventoryUsingDifficulty(difficulty, level),
            gold: generateGoldUsingDifficulty(difficulty, level));
        }

        private static int generateGoldUsingDifficulty(Difficulty difficulty, int level)
        {
            var rand = new Random();
            int averageGoldForLevel = difficulty.AverageMerchantGold + (level/3 * 25);
            var goldVariance = (int)Math.Round(averageGoldForLevel * .2);
            return rand.Next(averageGoldForLevel - goldVariance, averageGoldForLevel + goldVariance);
        }

        private static List<Item> generateInventoryUsingDifficulty(Difficulty difficulty, int level)
        {
            var itemValueAverage = difficulty.AverageItemValue*1.5 + (level/3 * 10);
            var totalMerchantInventorySize = difficulty.MerchantInventorySize + (level/3);
            var inventory = new List<Item>();
            for (int i = 0; i < totalMerchantInventorySize; i++)
            {
                Item newItem = null;
                int valueVariance;
                if (i < totalMerchantInventorySize / 6)
                {
                    valueVariance = (int)Math.Round(itemValueAverage*.5);
                    newItem = ItemsRepository.All.Where(i => 
                        ((double)i.Value).IsBetween(itemValueAverage-valueVariance, itemValueAverage+valueVariance)).RandomElement();
                }
                else
                {
                    double currentItemValueSum = inventory.Sum(i => i.Value);
                    valueVariance = (int)Math.Round(itemValueAverage*.25);

                    while (newItem == null)
                    {
                        var potentialItems = ItemsRepository.All.Where(i =>
                            ((currentItemValueSum + i.Value) / (inventory.Count + 1))
                            .IsBetween(itemValueAverage-valueVariance, itemValueAverage+valueVariance));
                        if (potentialItems.Any())
                        {
                            newItem = potentialItems.RandomElement();
                        }
                        else
                        {
                            valueVariance += 5;
                        }
                    }
                }
                inventory.Add(newItem);
            }
            return inventory;
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
            int cursorX = _player.Inventory.Any() ? 0 : 1;
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
                                (this as IContainer).TradeItem(selectedItem, _player, requireGold: true, discount: 1.25);
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

                // Keep cursor X coordinate within menu bounds:
                if (tempCursorX < 0) tempCursorX = 0;
                else if (tempCursorX > 1) tempCursorX = 1;
                // If one inventory is empty, force cursor to other inventory:
                if (tempCursorX == 0 && !_player.Inventory.Any()) tempCursorX = 1;
                else if (tempCursorX == 1 && !Inventory.Any()) tempCursorX = 0;
                cursorX = tempCursorX;

                // Keep cursor Y coordinate within menu bounds:
                int cursorYMax = tempCursorX <= 0 ? _player.Inventory.Count() : Inventory.Count();
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
                if (Inventory.Any() || _player.Inventory.Any())
                {
                    Console.Clear();
                    System.Console.WriteLine("Hello adventurer! May I interest you in a trade?");
                    OpenTrade();
                }
                else
                {
                    System.Console.WriteLine("Neither of you have anything left to trade.");
                }
            }
            else
            {
                System.Console.WriteLine($"{Name} is dead.");
            }
        }
    }
}