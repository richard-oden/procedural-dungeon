using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon
{
    public class Game
    {
        public Difficulty Difficulty {get; protected set;}
        public MapSize Size {get; protected set;}
        public Player Player {get; protected set;}
        private bool _gameRunning {get; set;}

        public Game(Difficulty difficulty, MapSize size, Player player)
        {
            Difficulty = difficulty;
            Size = size;
            Player = player;
        }

        public void StartAt(int level)
        {
            _gameRunning = true;
            while (_gameRunning)
            {
                // Every 3 levels create a merchant area:
                var thisMap = level % 3 == 0 ? Map.CreateMerchantMap(Player, Difficulty, level) 
                    : new Map(Size, Player, Difficulty, level);

                var potions = ItemsRepository.GetMapDependantItems(thisMap).Where(i => i is Potion);
                foreach (var p in potions) Player.AddItemToInventory(p);

                while (!Player.IsDead && !thisMap.HasPlayerExited)
                {
                    thisMap.PrintMapFromViewport(Player);
                    Console.WriteLine(Player.GetDetails());
                    var input = Console.ReadKey();
                    System.Console.WriteLine();
                    Player.ParseInput(thisMap, input);
                    foreach (var npc in thisMap.Npcs) npc.Act(thisMap);
                    Console.Clear();
                }
                if (Player.IsDead) _gameRunning = false;
                level++;
            }
        }
    }
}