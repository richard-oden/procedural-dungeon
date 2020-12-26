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
            // Console.OutputEncoding = System.Text.Encoding.UTF8;
            var testPlayer = new Player(name: "Bill", id: 001, hp: 10, Gender.Male);
            var testWeapons = ItemsRepository.Weapons.RandomSample(3);
            var testArmor = ItemsRepository.Armor.RandomSample(3);
            // var testMap = new Map(60, 40, 20, 80, testPlayer);
            for (int i = 0; i < 3; i++)
            {
                testPlayer.AddItemToInventory(testWeapons[i]);
                testPlayer.AddItemToInventory(testArmor[i]);
            }
            // testPlayer.AddItemToInventory(new Compass((Door)testMap.Assets.Single(a => a is Door), testPlayer));
            // testPlayer.AddItemToInventory(new FloorMap(testMap, testPlayer));
            // var testNpcs = new Npc[]
            // {
            //     new Npc(name: "Giant Rat", id: 002, hp: 10),
            //     new Npc(name: "Giant Rat", id: 003, hp: 10),
            //     new Npc(name: "Giant Rat", id: 004, hp: 10),
            //     new Npc(name: "Giant Rat", id: 005, hp: 10),
            //     new Npc(name: "Giant Rat", id: 006, hp: 10)
                
            // };
            // foreach (var npc in testNpcs) testMap.AddNpc(npc);


            // while (true)
            // {
            //     testMap.PrintMapFromViewport(testPlayer);
            //     Console.WriteLine(testPlayer.GetDetails());
            //     var input = Console.ReadKey();
            //     System.Console.WriteLine();
            //     testPlayer.ParseInput(testMap, input);
            //     foreach (var npc in testNpcs) npc.Act(testMap);
            //     Console.Clear();
            // }

            // var m = new Map(30, 30);
            // var r1 = new Rectangle(new Point(5, 5), 0, 10);
            // var p1 = new Point(14, 6);
            // var p2 = new Point(24, 14);

            // var npc = new Npc("Test", 0, 10, location: p1);
            // m.AddAssets(new List<IMappable>(){npc, new Wall(p2), new Barrier(r1)});
            // m.PrintMap();
            // // System.Console.WriteLine(m.GetPathObstructions(p1, p2).Select(a => a.Location).ToString("and"));
            // System.Console.WriteLine(Rectangle.DoesLineIntersectRect(p1, p2, r1));


            var testMerchant = new Merchant(id: 0, hp: 10, testPlayer);
            testMerchant.AddItemToInventory(ItemsRepository.All.RandomElement());
            testMerchant.AddItemToInventory(ItemsRepository.All.RandomElement());
            testMerchant.AddItemToInventory(ItemsRepository.All.RandomElement());
            testMerchant.AddItemToInventory(ItemsRepository.All.RandomElement());
            testMerchant.AddItemToInventory(ItemsRepository.All.RandomElement());
            testMerchant.AddItemToInventory(ItemsRepository.All.RandomElement());
            testMerchant.AddItemToInventory(ItemsRepository.All.RandomElement());
            testMerchant.AddItemToInventory(ItemsRepository.All.RandomElement());

            testMerchant.Trade();
        }
    }
}
