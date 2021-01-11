using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon
{
    public static class NewGameMenu
    {
        private static int _highlightedRow = 0;
        private static MapSize[] _mapSizes = new[] {MapSize.XSmall, MapSize.Small, MapSize.Medium, MapSize.Large, MapSize.XLarge};
        private static int _highlightedMapSize = 2;
        private static Difficulty[] _difficulties = new[] {Difficulties.VeryEasy, Difficulties.Easy, Difficulties.Medium, Difficulties.Hard, Difficulties.VeryHard};
        private static int _highlightedDifficulty = 2;
        private static string _playerName = "Bill";
        private static Gender[] _genders = new[] {Gender.Male, Gender.Female, Gender.NonBinary};
        private static int _highlightedGender = 2;
        private static PlayerBackground[] _playerBackgrounds = new[] {PlayerBackgrounds.Miner, PlayerBackgrounds.Farmer, PlayerBackgrounds.Hunter, PlayerBackgrounds.Priest, PlayerBackgrounds.Noble};
        private static int _highlightedBackground = 2;
        private static bool _startGame = false;
        
        public static Game Open()
        {
            while (!_startGame)
            {
                printRows();
                handleInput(Console.ReadKey(true));
                Console.Clear();
            }
            var player = new Player(_playerName, id: 0, _playerBackgrounds[_highlightedBackground], level: 1, _genders[_highlightedGender]);
            return new Game(_difficulties[_highlightedDifficulty], _mapSizes[_highlightedMapSize], player);
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
            Console.Write(_playerName);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine('\n');

            if (_highlightedRow == 3) Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write("Select your gender: ");
            printMultipleChoices(_genders.Select(g => Enum.GetName(typeof(Gender), g)).ToArray(), _highlightedGender);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine('\n');
            
            if (_highlightedRow == 4) Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write("Select your background: ");
            printMultipleChoices(_playerBackgrounds.Select(b => b.Name).ToArray(), _highlightedBackground);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine('\n');

            if (_highlightedRow == 5) Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write("START NEW GAME! (Press Enter)");
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
            else if (_highlightedRow == 3) _highlightedGender = handleMultipleChoiceInput(input, _genders.Length, _highlightedGender);
            else if (_highlightedRow == 4) _highlightedBackground = handleMultipleChoiceInput(input, _playerBackgrounds.Length, _highlightedBackground);
            else if (_highlightedRow == 5 && input.Key == ConsoleKey.Enter) _startGame = true; 

            if (tempRow > 5) tempRow = 5;
            else if (tempRow < 0) tempRow = 0;
            else _highlightedRow = tempRow;
        }
    }
}