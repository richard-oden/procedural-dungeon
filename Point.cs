using System;
using System.Linq;
using System.Collections.Generic;
using static ProceduralDungeon.ExtensionsAndHelpers;

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

        public Point(Point pointToClone)
        {
            X = pointToClone.X;
            Y = pointToClone.Y;
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

        // Implementation found here: https://stackoverflow.com/questions/563198/how-do-you-detect-where-two-line-segments-intersect
        public static bool DoLineSegmentsIntersect(Point A, Point B, Point C, Point D)
        {
            Point CmP = new Point(C.X - A.X, C.Y - A.Y);
            Point r = new Point(B.X - A.X, B.Y - A.Y);
            Point s = new Point(D.X - C.X, D.Y - C.Y);
    
            float CmPxr = CmP.X * r.Y - CmP.Y * r.X;
            float CmPxs = CmP.X * s.Y - CmP.Y * s.X;
            float rxs = r.X * s.Y - r.Y * s.X;
    
            if (CmPxr == 0f)
            {
                // Lines are collinear, and so intersect if they have any overlap
    
                return ((C.X - A.X < 0f) != (C.X - B.X < 0f)) ||
                    ((C.Y - A.Y < 0f) != (C.Y - B.Y < 0f));
            }
    
            if (rxs == 0f)
                return false; // Lines are parallel.
    
            float rxsr = 1f / rxs;
            float t = CmPxs * rxsr;
            float u = CmPxr * rxsr;
    
            return (t >= 0f) && (t <= 1f) && (u >= 0f) && (u <= 1f);
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
                case ConsoleKey.W:
                    Y -= 1;
                    break;
                case ConsoleKey.X:
                    Y += 1;
                    break;
                case ConsoleKey.D:
                    X += 1;
                    break;
                case ConsoleKey.A:
                    X -= 1;
                    break;
                case ConsoleKey.E:
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
                    WaitForInput();
                    break;
            }
        }
    
        public void Translate(Point point)
        {
            X += point.X;
            Y += point.Y;
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