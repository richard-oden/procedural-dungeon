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
            var newGame = NewGameMenu.Open();
            newGame.StartAt(0);
            ExtensionsAndHelpers.WaitForInput();
        }
    }
}
