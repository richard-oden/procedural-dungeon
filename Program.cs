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
            // var testPlayer = new Player(name: "Bill", id: 001, hp: 10, Gender.Male, gold: 27);

            // foreach (var i in ItemsRepository.Utilities) testPlayer.AddItemToInventory(i, true);

            // var testGame = new Game(Difficulties.Easy, MapSize.Small, testPlayer);
            // testGame.StartAt(0);
            var newGame = NewGameMenu.Open();
            newGame.StartAt(0);
        }
    }
}
