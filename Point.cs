using System;
using System.Linq;
using System.Collections.Generic;

namespace ProceduralDungeon
{
    public class Point
    {
        public int X {get; set;}
        public int Y {get; set;}
        
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point(Orientation borderOrientation, int i)
        {
            if (borderOrientation == Orientation.Horizontal)
            {
                X = i;
                Y = 0;
            }
            else if (borderOrientation == Orientation.Vertical)
            {
                X = 0;
                Y = i;
            }
            else
            {
                throw new Exception("Unable to create point due to invalid orientation.");
            }
        }
        
        public decimal DistanceTo(int x, int y)
        {
            return (decimal)Math.Sqrt(Math.Pow(X-x, 2) + Math.Pow(Y-y, 2));
        }
        
        public decimal DistanceTo(Point point)
        {
            return DistanceTo(point.X, point.Y);
        }

        public bool InRangeOf(Point point, int range)
        {
            // Allows points that are diagonally adjacent to be considered within range of 1:
            if (range == 1 && GetAdjacentCoordinates().Any(c =>
                c[0] == point.X && c[1] == point.Y)) return true;
            return DistanceTo(point) <= range;
        }

        public static bool IsPointOnLineSegment(Point start, Point end, Point middle)
        {
            return start.DistanceTo(middle) + middle.DistanceTo(end) == start.DistanceTo(end);
        }

        // Based on an algorithm in Andre LeMothe's "Tricks of the Windows Game Programming Gurus"
        // Found here: https://stackoverflow.com/questions/563198/how-do-you-detect-where-two-line-segments-intersect
        public static bool DoLinesIntersect(Point line1Start, Point line1End, 
            Point line2Start, Point line2End)
        {
            int line1Width, line1Height, line2Width, line2Height;
            line1Width = line1End.X - line1Start.X;     line1Height = line1End.Y - line1Start.Y;
            line2Width = line2End.X - line2Start.X;     line2Height = line2End.Y - line2Start.Y;

            decimal s, t;
            s = (-line1Height * (line1Start.X - line2Start.X) + line1Width * (line1Start.Y - line2Start.Y)) / 
                (-line2Width * line1Height + line1Width * line2Height);
            t = ( line2Width * (line1Start.Y - line2Start.Y) - line2Height * (line1Start.X - line2Start.X)) / 
                (-line2Width * line1Height + line1Width * line2Height);

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            {
                // Collision detected
                // if (i_x != NULL)
                //     *i_x = line1Start.X + (t * line1Width);
                // if (i_y != NULL)
                //     *i_y = line1Start.Y + (t * line1Height);
                return true;
            }
            else
            {
                return false; // No collision
            }
        }

        public List<int[]> GetAdjacentCoordinates()
        {
            var output = new List<int[]>();
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    if (x != 0 || y != 0) output.Add(new int[]{X+x, Y+y});
                }
            }
            return output;
        }

        public void Translate(string direction, int distance)
        {
            switch (direction.ToLower())
            {
                case "n":
                    Y -= distance;
                    break;
                case "s":
                    Y += distance;
                    break;
                case "e":
                    X += distance;
                    break;
                case "w":
                    X -= distance;
                    break;
                case "ne":
                    Y -= distance;
                    X += distance;
                    break;
                case "nw":
                    Y -= distance;
                    X -= distance;
                    break;
                case "se":
                    Y += distance;
                    X += distance;
                    break;
                case "sw":
                    Y += distance;
                    X -= distance;
                    break;
                default:
                    Console.WriteLine($"'{direction}' is not a valid direction. Should be abbreviated as follows: 'north' = 'n', 'southeast' = 'se', etc.");
                    break;
            }
        }

        public void Translate(ConsoleKeyInfo input)
        {
            switch (input.Key)
            {
                case ConsoleKey.E:
                    Y -= 1;
                    break;
                case ConsoleKey.X:
                    Y += 1;
                    break;
                case ConsoleKey.F:
                    X += 1;
                    break;
                case ConsoleKey.A:
                    X -= 1;
                    break;
                case ConsoleKey.R:
                    Y -= 1;
                    X += 1;
                    break;
                case ConsoleKey.Q:
                    Y -= 1;
                    X -= 1;
                    break;
                case ConsoleKey.C:
                    Y += 1;
                    X += 1;
                    break;
                case ConsoleKey.Z:
                    Y += 1;
                    X -= 1;
                    break;
                default:
                    Console.WriteLine("Invalid key.");
                    break;
            }
        }
    
        public string DirectionTo(Point that)
        {
            string output = "";
            if (that.Y < this.Y) output += "N";
            else if (that.Y > this.Y) output += "S";
            if (that.X > this.X) output += "E";
            else if (that.X < this.X) output += "W";
            return output;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}