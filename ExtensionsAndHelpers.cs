using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProceduralDungeon
{
    static class ExtensionsAndHelpers
    {
        public static T RandomElement<T>(this IEnumerable<T> enumerable)
        {
            var rand = new Random();
            int index = rand.Next(0, enumerable.Count());
            return enumerable.ElementAt(index);
        }
        
        public static List<T> RandomSample<T>(this IEnumerable<T> enumerable, int sampleSize)
        {
            if (sampleSize < enumerable.Count())
            {
                var sample = new List<T>();
                int i = 0;
                while (i < enumerable.Count())
                {
                    var potentialRandomElement = enumerable.RandomElement();
                    if (!sample.Contains(potentialRandomElement))
                    {
                        sample.Add(potentialRandomElement);
                        i++;
                    }
                }
                return sample;
            }
            return enumerable.ToList();
        }
        
        public static void MoveElement<T>(this List<T> list, T TElement, int distance)
        {
            if (list.Contains(TElement))
            {
                int oldIndex = list.IndexOf(TElement);
                int newIndex = oldIndex + distance;
                if (newIndex < 0) 
                {
                    newIndex = 0;
                }
                else if (newIndex > list.Count - 1) 
                {
                    newIndex = list.Count -1;
                }
                list.RemoveAt(oldIndex);
                list.Insert(newIndex, TElement);
            }
            else
            {
                throw new Exception("List does not contain element.");
            }
        }

        public static void ListDistanceAndDirectionFrom(this IEnumerable<IMappable> assets, Point origin)
        {
            foreach (var a in assets)
            {
                Console.Write("- ");
                if (a is INameable) 
                {
                    Console.Write((a as INameable).Name);
                    if (a is Creature && (a as Creature).IsDead) Console.Write(" (Dead)");
                }
                else
                {
                    Console.Write(a.GetType().Name);
                }

                if (a.Location != null)
                {
                    Console.Write($" located {Math.Round(origin.DistanceTo(a.Location)*5)} feet {origin.DirectionTo(a.Location)}");
                }
                System.Console.WriteLine();
            }
        }
        
        public static INameable GetByName(this IEnumerable<INameable> assets, string name)
        {
            return assets.FirstOrDefault(a => a.Name.ToLower() == name.ToLower());
        }

        public static IList<T> Clone<T>(this IList<T> listToClone) where T: ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        
        public static string ToString(this IEnumerable<string> source, string conjunction)
        {
            if (source == null || !source.Any()) return null;
            var sourceArr = source.ToArray();
            string output = "";
            for (int i = 0; i < sourceArr.Length; i++) 
            {
                output += sourceArr[i];
                if (i != sourceArr.Length-1) output += (sourceArr.Length == 2 ? " " : ", ");
                if (i == sourceArr.Length-2) output += $"{conjunction} ";
            }
            return output;
        }

        
        public static string ToString(this IEnumerable<Point> points, string conjunction)
        {
            return points.Select(p => p.ToString()).ToString("and");
        }
   
        public static string ListWithIndefiniteArticle(this IEnumerable<INameable> source)
        {
            return source.Select(s => $"{s.Name.IndefiniteArticle()} {s.Name}").ToString("and");
        }

        public static string DiceToString(this IEnumerable<Die> dice)
        {
            var groupedDice = dice.GroupBy(d => d.NumSides).ToList();
            var output = "";
            foreach (var group in groupedDice)
            {
                if (group.Key > 1) output += group.Count();
                output += group.First().ToString();
                if (groupedDice.IndexOf(group) != groupedDice.Count - 1) output += " + ";
            }
            return output;
        }
        
        public static string FromTitleOrCamelCase(this string source)
        {
            string output = Regex.Replace(source, @"([A-Z])", " " + "$1").ToLower();
            output = Regex.Replace(output, @"_", "");
            if (output[0] == ' ') output.ToCharArray().ToList().RemoveAt(0);
            return output;
        }

        public static string IndefiniteArticle(this string noun)
        {
            return "AEIOUaeiou".IndexOf(noun[0])
            >= 0 ? "an" : "a";
        }
    
        public static void WaitForInput()
        {
            Console.Write("Press any key to continue... ");
            Console.ReadKey();
            Console.WriteLine();
        }
    
        public static string Prompt(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine();
        }
    }
}