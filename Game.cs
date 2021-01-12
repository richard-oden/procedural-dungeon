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
                // Every 3 levels create a merchant area:
                var thisMap = floor % 3 == 0 ? Map.CreateMerchantMap(Player, Difficulty, floor) 
                    : new Map(Size, Player, Difficulty, floor);
                Player.RemoveAllFromMemoryIfNotOnMap(thisMap);
                foreach (var i in Player.Inventory) if (i is Key) i.IsDestroyed = true;

                while (!Player.IsDead && !thisMap.HasPlayerExited)
                {
                    thisMap.PurgeDestroyedItems();
                    Console.WriteLine($"Floor: {floor}");
                    thisMap.PrintMapFromViewport(Player);
                    Console.WriteLine(Player.GetDetails());
                    var input = Console.ReadKey();
                    System.Console.WriteLine();
                    Player.ParseInput(thisMap, input);
                    thisMap.ManageIDegradables();
                    foreach (var npc in thisMap.Npcs) npc.Act(thisMap);
                    Console.Clear();
                }
                if (Player.IsDead) _gameRunning = false;
                // Gain exp equal to 2*floor if this floor is not a merchant map:
                else if (floor % 3 != 0) Player.GainExp(floor * 2);
                floor++;
            }
        }
    }
}