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
        public List<IMappable> Assets {get; protected set;} = new List<IMappable>();
        private List<Tile> _tiles {get; set;} = new List<Tile>();
        private List<int[]> _bloodSplatterCoordinates = new List<int[]>();
        
        public Map()
        {
        }

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
        }
        public Map(int width, int height, int numTiles, int numAttempts)
        {
            Width = width;
            Height = height;
            generateTiles(numTiles, numAttempts);
            fillSpaceBetweenTiles();
            validateAssets(Assets);
        }
        private bool canAddTile(Tile tileToAdd)
        {
            if (tileToAdd.OpeningPoints.Any())
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        tileToAdd.TranslateAssets(new Point(x, y));
                        if (WithinBorder(tileToAdd, 3) &&
                            _tiles.Any(t => t.DoOpeningPointsMatch(tileToAdd)) &&
                            _tiles.All(t => !t.DoesIntersectTile(tileToAdd)))
                        {
                            return true;
                        }
                        else
                        {
                            tileToAdd.TranslateAssets(new Point(-x, -y));
                        }
                    }
                }
            }
            return false;
        }
        
        private void generateTiles(int numTiles, int numAttempts)
        {
            var centralTile = new Tile(TileSize.Large, TileSize.Large, 2, 2, 2, 2); 
            var centralTileStart = new Point(Width/2 - (int)TileSize.Large/2, Height/2 - (int)TileSize.Large/2);
            centralTile.TranslateAssets(centralTileStart);
            AddTile(centralTile);

            int attempts = 0;
            while (attempts < numAttempts && _tiles.Count < numTiles)
            {
                var tempTile = Tile.CreateRandom();
                if (canAddTile(tempTile)) 
                {
                    AddTile(tempTile);
                }
                attempts++;
            }
        }
        
        private void fillSpaceBetweenTiles()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var tempPoint = new Point(x, y);
                    if (_tiles.All(t => !t.OnMap(tempPoint)))
                    {
                        AddAsset(new Wall(tempPoint));
                    }
                }
            }
        }
        
        public virtual bool OnMap(Point point)
        {
            return point.X >= 0 && point.X < Width && 
                   point.Y >= 0 && point.Y < Height;
        }

        public bool OnMap(Rectangle rect)
        {
            for (int y = rect.YMin; y <= rect.YMax; y++)
            {
                for (int x = rect.XMin; x <= rect.XMax; x++)
                {
                    if (!OnMap(new Point(x, y))) return false;
                }
            }
            return true;
        }

        public bool OnMap(Tile tile)
        {
            return tile.Assets.All(a => OnMap(a.Location)) &&
                tile.Assets.All(a => a is IRectangular &&
                OnMap((a as IRectangular).Rect));
        }

        public bool WithinBorder(Tile tile, int borderWidth)
        {
            return tile.startLocation.X >= borderWidth &&
                tile.startLocation.Y >= borderWidth &&
                new Map(Width-borderWidth, Height-(borderWidth-1)).OnMap(tile);
        }

        private void validateAssets(List<IMappable> assets)
        {
            // All points and rectangles in map assets:
            var points = assets.Where(a => !(a is IRectangular)).Select(a => a.Location);
            var rects = assets.Where(a => a is IRectangular).Select(a => (a as IRectangular).Rect);

            // Invalid points:
            var pointsOutOfBounds = points.Where(p => !OnMap(p));
            var pointDuplicates = points.Where(p1 => points.Any(p2 => p1 != p2 && p1.X == p2.X && p1.Y == p2.Y));
            var pointsWithinRects = points.Where(p => rects.Any(r => Rectangle.DoesRectContainPoint(p, r)));

            // Invalid rectangles:
            var rectsOutOfBounds = rects.Where(r => !OnMap(r));
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
                throw new IntersectingRectanglesException($"Rectangles at {rectsIntersectingStartPoints} are intersecting.");
            }
        }

        public void AddAsset(IMappable asset)
        {
            Assets.Add(asset);
        }

        public void AddAssets(List<IMappable> assets)
        {
            Assets.AddRange(assets);
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

        public void AddTile(Tile tileToAdd)
        {
            AddAssets(tileToAdd.Assets);
            _tiles.Add(tileToAdd);
        }
        
        public void AddTile(Tile tileToAdd, Point start)
        {
            AddAssets(tileToAdd.Assets, start);
            _tiles.Add(tileToAdd);
        }

        public void RemoveAsset(IMappable asset)
        {
            if (Assets.Contains(asset))
            {
                Assets.Remove(asset);
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