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
                Player.AddItemToInventory(new FloorMap(thisMap));
                // Destroy key from previous floor
                foreach (var i in Player.Inventory) if (i is Key) i.IsDestroyed = true;

                while (!Player.IsDead && !thisMap.HasPlayerExited)
                {
                    Console.Clear();
                    thisMap.ManageIDegradables();
                    thisMap.PurgeDestroyedAssets();
                    Player.RemoveDestroyedAssetsFromMemory();
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

        public static void ShowTitleScreen()
        {
            Console.WriteLine(
@"=====================================================================
______ ______  _____  _____  _____ ______  _   _ ______   ___   _      
| ___ \| ___ \|  _  |/  __ \|  ___||  _  \| | | || ___ \ / _ \ | |     
| |_/ /| |_/ /| | | || /  \/| |__  | | | || | | || |_/ // /_\ \| |     
|  __/ |    / | | | || |    |  __| | | | || | | ||    / |  _  || |     
| |    | |\ \ \ \_/ /| \__/\| |___ | |/ / | |_| || |\ \ | | | || |____ 
\_|    \_| \_| \___/  \____/\____/ |___/   \___/ \_| \_|\_| |_/\_____/                                                            
           ______  _   _  _   _  _____  _____  _____  _   _              
           |  _  \| | | || \ | ||  __ \|  ___||  _  || \ | |             
           | | | || | | ||  \| || |  \/| |__  | | | ||  \| |             
           | | | || | | || . ` || | __ |  __| | | | || . ` |             
           | |/ / | |_| || |\  || |_\ \| |___ \ \_/ /| |\  |             
           |___/   \___/ \_| \_/ \____/\____/  \___/ \_| \_/         

=====================================================================

                     Press any key to start...");
            Console.ReadKey(true);
            Console.Clear();
        }

        public static void ShowDeathScreen()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(
@"▓██   ██▓ ▒█████   █    ██    ▓█████▄  ██▓▓█████ ▓█████▄                
 ▒██  ██▒▒██▒  ██▒ ██  ▓██▒   ▒██▀ ██▌▓██▒▓█   ▀ ▒██▀ ██▌               
  ▒██ ██░▒██░  ██▒▓██  ▒██░   ░██   █▌▒██▒▒███   ░██   █▌               
  ░ ▐██▓░▒██   ██░▓▓█  ░██░   ░▓█▄   ▌░██░▒▓█  ▄ ░▓█▄   ▌               
  ░ ██▒▓░░ ████▓▒░▒▒█████▓    ░▒████▓ ░██░░▒████▒░▒████▓  ██▓  ██▓  ██▓ 
   ██▒▒▒ ░ ▒░▒░▒░ ░▒▓▒ ▒ ▒     ▒▒▓  ▒ ░▓  ░░ ▒░ ░ ▒▒▓  ▒  ▒▓▒  ▒▓▒  ▒▓▒ 
 ▓██ ░▒░   ░ ▒ ▒░ ░░▒░ ░ ░     ░ ▒  ▒  ▒ ░ ░ ░  ░ ░ ▒  ▒  ░▒   ░▒   ░▒  
 ▒ ▒ ░░  ░ ░ ░ ▒   ░░░ ░ ░     ░ ░  ░  ▒ ░   ░    ░ ░  ░  ░    ░    ░   
 ░ ░         ░ ░     ░           ░     ░     ░  ░   ░      ░    ░    ░  
 ░ ░                           ░                  ░        ░    ░    ░  ");
            Console.ResetColor();
            ExtensionsAndHelpers.WaitForInput();
        }
    }
}