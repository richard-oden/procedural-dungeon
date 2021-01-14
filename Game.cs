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

        public void StartAt(int floor)
        {
            _gameRunning = true;
            while (_gameRunning)
            {
                Console.Clear();
                Console.WriteLine("Creating next floor...");
                // Every 3 levels create a merchant area:
                var thisMap = floor % 3 == 0 ? Map.CreateMerchantMap(Player, Difficulty, floor) 
                    : new Map(Size, Player, Difficulty, floor);
                Player.RemoveAllFromMemoryIfNotOnMap(thisMap);
                foreach (var i in Player.Inventory) if (i is Key) i.IsDestroyed = true;

                while (!Player.IsDead && !thisMap.HasPlayerExited)
                {
                    Console.Clear();
                    thisMap.ManageIDegradables();
                    thisMap.PurgeDestroyedItems();
                    Console.WriteLine($"Floor: {floor}");
                    thisMap.PrintMapFromViewport(Player);
                    Console.WriteLine(Player.GetDetails());
                    Console.WriteLine();
                    Player.ParseInput(thisMap, Console.ReadKey(true));
                    foreach (var npc in thisMap.Npcs) npc.Act(thisMap);
                }
                if (Player.IsDead) _gameRunning = false;
                // Gain exp equal to 2*floor if this floor is not a merchant map:
                else if (floor % 3 != 0) Player.GainExp(floor * 2);
                floor++;
            }
        }
    }
}