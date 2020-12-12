using System;
using System.Linq;
using static ProceduralDungeon.ExtensionsAndHelpers;

namespace ProceduralDungeon
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var testMap = new Map(60, 40, 20, 80);
            var testPlayer = new Player(name: "Bill", id: 001, hp: 10, speed: 1);
            var testNpcs = new Npc[]
            {
                new Npc(name: "Giant Rat", id: 002, hp: 10, speed: 1),
                new Npc(name: "Giant Rat", id: 003, hp: 10, speed: 1),
                new Npc(name: "Giant Rat", id: 004, hp: 10, speed: 1),
                new Npc(name: "Giant Rat", id: 005, hp: 10, speed: 1),
                new Npc(name: "Giant Rat", id: 006, hp: 10, speed: 1)
            };
            testMap.AddPlayer(testPlayer);
            foreach (var npc in testNpcs) testMap.AddNpc(npc);


            while (true)
            {
                testMap.PrintMap();
                var moveInput = Console.ReadKey();
                testMap.Move(testPlayer, moveInput);
                foreach (var npc in testNpcs) testMap.MoveToward(npc, testPlayer.Location);
                Console.Clear();
            }
        }
    }
}
