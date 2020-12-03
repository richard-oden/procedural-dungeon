using System;
using System.Collections.Generic;
using System.Linq;
using static System.ConsoleColor;

namespace ProceduralDungeon
{
    public class Map
    {
        public int Width {get; protected set;}
        public int Height {get; protected set;}
        public List<IMappable> Assets {get; protected set;}
        private List<int[]> _bloodSplatterCoordinates = new List<int[]>();
        
        public Map()
        {
            Assets = new List<IMappable>();
        }
        
        public bool OnMap(Point point)
        {
            return point.X >= 0 && point.X < Width && 
                   point.Y >= 0 && point.Y < Height;
        }

        private void validateAssets(List<IMappable> assets)
        {
            // All points and rectangles in map assets:
            var points = assets.Where(a => !(a is IRectangular)).Select(a => (a as IMappable).Location);
            var rects = assets.Where(a => a is IRectangular).Select(a => (a as IRectangular).Rect);

            // Invalid points:
            var pointsOutOfBounds = points.Where(p => !OnMap(p));
            var pointDuplicates = points.Where(p1 => points.Any(p2 => p1.X == p2.X && p1.Y == p2.Y));
            var pointsWithinRects = points.Where(p => rects.Any(r => Rectangle.DoesRectContainPoint(p, r)));

            // Invalid rectangles:
            var rectsOutOfBounds = rects.Where(r => !OnMap(r.NeCorner) || !OnMap(r.NwCorner) ||
                !OnMap(r.SwCorner) || !OnMap(r.SeCorner));
            var rectsIntersecting = rects.Where(r1 => rects.Any(r2 => r1 != r2 && Rectangle.DoRectsIntersect(r1, r2)));

            if (pointsOutOfBounds.Any())
            {
                throw new OutOfMapBoundsException($"Point(s) at {pointsOutOfBounds.ToString("and")} are outside the boundaries of the map.");
            }
            else if (pointDuplicates.Any())
            {
                throw new DuplicateLocationException($"Points at {pointDuplicates.ToString("and")} are duplicates.");
            }
            else if (pointsWithinRects.Any())
            {
                throw new PointWithinRectangleException($"Point(s) within rectangle(s) found at {pointsWithinRects.ToString("and")}.");
            }
            else if (rectsOutOfBounds.Any())
            {
                var rectsOobStartPoints = rectsOutOfBounds.Select(r => r.StartLocation).ToString("and");
                throw new OutOfMapBoundsException($"Rectangle(s) at {rectsOobStartPoints} are outside the boundaries of the map.");
            }
            else if (rectsIntersecting.Any())
            {
                var rectsIntersectingStartPoints = rectsIntersecting.Select(r => r.StartLocation).ToString("and");
                throw new OutOfMapBoundsException($"Rectangles at {rectsIntersectingStartPoints} are intersecting.");
            }
        }

        public void AddAsset(IMappable obj)
        {
            Assets.Add(obj);
            validateAssets(Assets);
        }

        public void AddAssets(List<IMappable> assets)
        {
            Assets.AddRange(assets);
            validateAssets(Assets);
        }

        public void AddAssets(List<IMappable> assets, Point start)
        {
            foreach (var asset in assets)
            {
                asset.Location.X += start.X;
                asset.Location.Y += start.Y;
            }
            AddAssets(assets);
        }

        public void RemoveAsset(IMappable obj)
        {
            if (Assets.Contains(obj))
            {
                Assets.Remove(obj);
            }
            else
            {
                Console.WriteLine("Map does not contain asset.");
            }
        }

        public int[][] GetOpenSpaces(IEnumerable<int[]> coordinates)
        {
           return coordinates.Where(c => !Assets.Any(o => o.Location.X == c[0] && o.Location.Y == c[1])).ToArray();
        }
        public int[][] GetCoordinatesWithin(int xStart, int xEnd, int yStart, int yEnd)
        {
            var coords = new List<int[]>();
            for (int x = xStart; x <= xEnd; x++)
            {
                for (int y = yStart; y <= yEnd; y++)
                {
                    coords.Add(new[] {x, y});
                }
            }
            return coords.ToArray();
        }

         public List<IMappable> GetAssetsInRangeOf(Point origin, int range)
        {
            // Allows points that are diagonally adjacent to be considered within range:
            if (range == 1)
            {
                var adjacentAssets = new List<IMappable>();
                foreach (var coord in origin.GetAdjacentCoordinates())
                {
                    var adjacentAsset = Assets.SingleOrDefault(o => 
                        o.Location.X == coord[0] && o.Location.Y == coord[1] &&
                        o.Location != origin);
                    if (adjacentAsset != null) adjacentAssets.Add(adjacentAsset);
                }
                return adjacentAssets;
            }
            return Assets.Where(o => origin.InRangeOf(o.Location, range)).ToList();
        }
        public void PrintMap()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    ConsoleColor fgColor = DarkGray;
                    if (_bloodSplatterCoordinates.Any(c => c[0] == x && c[1] == y))
                    {
                        Console.BackgroundColor = DarkRed;
                    }
                    var thisAsset = Assets.FirstOrDefault(a => 
                        a.Location.X == x && a.Location.Y == y || a is IRectangular && 
                        Rectangle.DoesRectContainPoint(new Point(x, y), (a as IRectangular).Rect));
                    if (thisAsset != null)
                    {
                        // if (thisAsset is Entity || thisAsset is Item)
                        // {
                        //     fgColor = White;
                        //     if ((thisAsset is Entity) && (thisAsset as Entity).TakingTurn)
                        //     {
                        //         fgColor = DarkBlue;
                        //     }
                        // }
                        Console.ForegroundColor = fgColor;
                        Console.Write(thisAsset.Symbol + " ");
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }
    }
}