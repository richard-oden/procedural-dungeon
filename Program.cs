using System;
using System.Collections.Generic;
using System.Linq;
using static ProceduralDungeon.ExtensionsAndHelpers;

namespace ProceduralDungeon
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Game.ShowTitleScreen();
            var newGame = NewGameMenu.Open();
            newGame.StartAt(0);
            Game.ShowDeathScreen();

            // foreach (var chest in ChestsRepository.All)
            // {
            //     Console.WriteLine($"{chest.Name} ({chest.Gold + chest.Inventory.Sum(i => i.Value)} total value): {chest.Inventory.Select(i => i.Name).ToString("and")}");
            // }
        }
    }
}
