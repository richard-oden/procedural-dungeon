using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProceduralDungeon
{
    public static class NewGameMenu
    {
        private static int _highlightedRow = 0;
        private static MapSize[] _mapSizes = new[] {MapSize.XSmall, MapSize.Small, MapSize.Medium, MapSize.Large, MapSize.XLarge};
        private static int _highlightedMapSize = 2;
        private static Difficulty[] _difficulties = new[] {Difficulties.VeryEasy, Difficulties.Easy, Difficulties.Medium, Difficulties.Hard, Difficulties.VeryHard};
        private static int _highlightedDifficulty = 2;
        private static string _playerName = "";
        
        public static void Open()
        {
            while (true)
            {
                printRows();
                handleInput(Console.ReadKey(true));
                Console.Clear();
            }
        }

        private static void printRows()
        {
            if (_highlightedRow == 0) Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write("Select a map size: ");
            printMultipleChoices(_mapSizes.Select(mS => Enum.GetName(typeof(MapSize), mS)).ToArray(), _highlightedMapSize);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine('\n');

            if (_highlightedRow == 1) Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write("Select a difficulty: ");
            printMultipleChoices(_difficulties.Select(d => d.Name).ToArray(), _highlightedDifficulty);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine('\n');

            if (_highlightedRow == 2) Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write("Enter a player name: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(_playerName);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        private static void printMultipleChoices(string[] choices, int highlightedChoice)
        {
            for(int i = 0; i < choices.Length; i++)
            {
                if (i == highlightedChoice) Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write(choices[i]);
                Console.ForegroundColor = ConsoleColor.White;
                if (i < choices.Length - 1) Console.Write(" - ");
            }
        }

        private static int handleMultipleChoiceInput(ConsoleKeyInfo input, int numChoices, int highlightedChoice)
        {
            int tempChoice = highlightedChoice;

            if (input.Key == ConsoleKey.LeftArrow) tempChoice--;
            else if (input.Key == ConsoleKey.RightArrow) tempChoice++;

            if (tempChoice > numChoices-1) tempChoice = numChoices-1;
            else if (tempChoice < 0) tempChoice = 0;
            
            return tempChoice;
        }

        private static string handleTextInput(ConsoleKeyInfo input, string textToChange)
        {
            var inputChar = input.KeyChar;
            if (char.IsLetterOrDigit(inputChar) && textToChange.Length < 10)
            {
                return textToChange + inputChar;
            }
            else if (input.Key == ConsoleKey.Backspace && textToChange.Length > 0) 
            {
                return textToChange.Remove(textToChange.Length-1, 1);
            }
            return textToChange;
        }

        private static void handleInput(ConsoleKeyInfo input)
        {
            // Handle highlighted row:
            int tempRow = _highlightedRow;

            if (input.Key == ConsoleKey.UpArrow) tempRow--;
            else if (input.Key == ConsoleKey.DownArrow) tempRow++;
            else if (_highlightedRow == 0) _highlightedMapSize = handleMultipleChoiceInput(input, _mapSizes.Length, _highlightedMapSize);
            else if (_highlightedRow == 1) _highlightedDifficulty = handleMultipleChoiceInput(input, _difficulties.Length, _highlightedDifficulty);
            else if (_highlightedRow == 2) _playerName = handleTextInput(input, _playerName);

            if (tempRow > 2) tempRow = 2;
            else if (tempRow < 0) tempRow = 0;
            else _highlightedRow = tempRow;
        }
    }
}