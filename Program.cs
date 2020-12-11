using System;
using static ProceduralDungeon.ExtensionsAndHelpers;

namespace ProceduralDungeon
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var testMap = new Map(60, 40, 20, 80);
            var testPlayer = new Player(name: "Bill", id: 001, hp: 10, speed: 1,
                location: testMap._emptyPoints.RandomElement());
            testMap.AddAsset(testPlayer);

            while (true)
            {
                testMap.PrintMap();
                var moveInput = Console.ReadKey();
                testMap.Move(testPlayer, moveInput);
                // testPlayer.Location.Translate(moveInput);
                Console.Clear();
            }
        }
    }
}
