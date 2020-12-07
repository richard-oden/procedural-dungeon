using System;
using System.Collections.Generic;

namespace ProceduralDungeon
{
    public class Die
    {
        public int NumSides {get; private set;}
        private static Random _rand = new System.Random();

        public Die(int numSides)
        {
            NumSides = numSides;
        }

        public int Roll()
        {
            return _rand.Next(1, NumSides+1);
        }

        public int RollBaseZero()
        {
            return _rand.Next(0, NumSides);
        }

        public int Roll(int multiplier)
        {
            int total = 0;
            for (int i = 0; i < multiplier; i++) total += Roll();
            return total;
        }

        public int Roll(int multiplier, bool printOutput)
        {
            string output = $"Rolling {multiplier}d{NumSides}: ";
            int total = 0;
            for (int i = 0; i < multiplier; i++) 
            {   
                int thisRoll = Roll();
                output += thisRoll + ((i == multiplier-1) ? " = " : " + ");
                total += thisRoll;
            }
            if (printOutput)
            {
                output += total;
                Console.WriteLine(output);
            }
            return total;
        }

        public int Roll(int multiplier, int modifier)
        {
            return Roll(multiplier) + modifier;
        }

        public int Roll(int multiplier, int modifier, bool printOutput)
        {
            string output = $"Rolling {multiplier}d{NumSides}+{modifier}: ";
            int total = 0;
            for (int i = 0; i < multiplier; i++) 
            {   
                int thisRoll = Roll();
                output += $"{thisRoll} + ";
                total += thisRoll;
            }
            output += $"{modifier} = ";
            total += modifier;
            if (printOutput)
            {
                output += total;
                Console.WriteLine(output);
            }
            return total;
        }
    
        // Alternative return type that shows natural rolls and modifier:
        public class Result
        {
            public List<int> NaturalResults {get; private set;}
            public int TotalResult {get; private set;}
            public int Modifier {get; private set;}

            public Result(List<int> naturalResults, int totalResult, int modifier)
            {
                NaturalResults = naturalResults;
                TotalResult = totalResult;
                Modifier = modifier;
            }
        }

        // Useful when access to natural roll is needed:
        public Result RollGetResult(int multiplier = 1, int modifier = 0, bool printOutput = false)
        {
            string output = $"Rolling {multiplier}d{NumSides}+{modifier}: ";
            var naturalResults = new List<int>();
            int total = 0;
            for (int i = 0; i < multiplier; i++) 
            {   
                int thisRoll = Roll();
                naturalResults.Add(thisRoll);
                output += $"{thisRoll} + ";
                total += thisRoll;
            }
            if (modifier != 0) output += $"{modifier} = ";
            total += modifier;
            if (printOutput)
            {
                output += total;
                Console.WriteLine(output);
            }
            return new Result(naturalResults, total, modifier);         
        }
    }
}