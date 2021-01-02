﻿using System;
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
            var testPlayer = new Player(name: "Bill", id: 001, hp: 10, Gender.Male, gold: 27);
            var testGame = new Game(Difficulties.Medium, MapSize.Medium, testPlayer);
            testGame.StartAt(0);
        }
    }
}
