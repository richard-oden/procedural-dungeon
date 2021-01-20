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

        public bool InRangeOf(Point originPoint, int range)
        {
            // Allows points that are diagonally adjacent to be considered within range of 1:
            if (range == 1 && GetAdjacentPoints().Any(p =>
                p.X == originPoint.X && p.Y == originPoint.Y)) return true;
            return DistanceTo(originPoint) <= range;
        }

        public static bool IsPointOnLineSegment(Point start, Point end, Point middle)
        {
            return start.DistanceTo(middle) + middle.DistanceTo(end) == start.DistanceTo(end);
        }

        // Implementation found here: https://stackoverflow.com/questions/563198/how-do-you-detect-where-two-line-segments-intersect
        public static bool DoLineSegmentsIntersect(Point A, Point B, Point C, Point D)
        {
            int direction(Point a, Point b, Point c) {
                int val = (b.Y-a.Y)*(c.X-b.X)-(b.X-a.X)*(c.Y-b.Y);
                if (val == 0)
                    return 0;     //colinear
                else if(val < 0)
                    return 2;    //anti-clockwise direction
                    return 1;    //clockwise direction
            }

            int dir1 = direction(A, B, C);
            int dir2 = direction(A, B, D);
            int dir3 = direction(C, D, A);
            int dir4 = direction(C, D, B);
            
            if(dir1 != dir2 && dir3 != dir4)
                return true; //theY are intersecting

            if(dir1==0 && IsPointOnLineSegment(A, B, C)) //when p2 of line2 are on the line1
                return true;

            if(dir2==0 && IsPointOnLineSegment(A, B, D)) //when p1 of line2 are on the line1
                return true;

            if(dir3==0 && IsPointOnLineSegment(C, D, A)) //when p2 of line1 are on the line2
                return true;

            if(dir4==0 && IsPointOnLineSegment(C, D, B)) //when p1 of line1 are on the line2
                return true;
                    
            return false;
        }

        public List<Point> GetAdjacentPoints(bool includeOrigin = false)
        {
            var output = new List<Point>();
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    if ((x == 0 && y == 0 && includeOrigin) || (x != 0 || y != 0)) 
                    {
                        output.Add(new Point(X+x, Y+y));
                    }
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