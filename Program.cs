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
            var testMap = new Map(60, 40, 20, 80);
            var testPlayer = new Player(name: "Bill", id: 001, hp: 10, Gender.Male);
            testPlayer.AddItemToInventory(ItemsRepository.CommonMisc.RandomElement());
            testPlayer.AddItemToInventory(ItemsRepository.CommonWeapons[0]);
            testPlayer.AddItemToInventory(ItemsRepository.VeryRareWeapons[0]);
            testPlayer.AddItemToInventory(ItemsRepository.VeryRareArmor[0]);
            testPlayer.AddItemToInventory(ItemsRepository.CommonArmor[1]);
            testPlayer.AddItemToInventory(ItemsRepository.CommonWeapons[2]);
            testPlayer.AddItemToInventory(new Compass((Door)testMap.Assets.Single(a => a is Door), testPlayer));
            var testNpcs = new Npc[]
            {
                new Npc(name: "Giant Rat", id: 002, hp: 10),
                new Npc(name: "Giant Rat", id: 003, hp: 10),
                new Npc(name: "Giant Rat", id: 004, hp: 10),
                new Npc(name: "Giant Rat", id: 005, hp: 10),
                new Npc(name: "Giant Rat", id: 006, hp: 10)
                
            };
            testMap.AddPlayer(testPlayer);
            foreach (var npc in testNpcs) testMap.AddNpc(npc);


            while (true)
            {
                testMap.PrintMap(testPlayer);
                Console.WriteLine(testPlayer.GetDetails());
                var input = Console.ReadKey();
                System.Console.WriteLine();
                testPlayer.ParseInput(testMap, input);
                foreach (var npc in testNpcs) npc.Act(testMap);
                Console.Clear();
            }
        }
    }
}
