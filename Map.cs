using System;
using System.Collections.Generic;
using System.Linq;
using static System.ConsoleColor;
using static ProceduralDungeon.ExtensionsAndHelpers;

namespace ProceduralDungeon
{
    public class Map
    {
        public int Width {get; protected set;}
        public int Height {get; protected set;}
        public List<IMappable> Assets {get; protected set;} = new List<IMappable>();
        public List<Item> Items => (from a in Assets where a is Item select (Item)a).ToList();
        public List<Creature> Creatures => (from a in Assets where a is Creature select (Creature)a).ToList();
        private List<Tile> _tiles {get; set;} = new List<Tile>();
        private Tile _centralTile {get; set;}
        private Point[] _assetPointLocations => Assets.Where(a => !(a is IRectangular)).Select(a => a.Location).ToArray();
        private Rectangle[] _assetRectLocations => Assets.Where(a => a is IRectangular).Select(a => (a as IRectangular).Rect).ToArray();
        public List<Point> EmptyPoints
        {
            get
            {
                var emptyPoints = new List<Point>();
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {   
                        var tempPoint = new Point(x, y);
                        if (_assetPointLocations.All(p => p.X != tempPoint.X || p.Y != tempPoint.Y) &&
                            _assetRectLocations.All(r => !Rectangle.DoesRectContainPoint(tempPoint, r)))
                        {
                            emptyPoints.Add(tempPoint);
                        }
                    }
                }
                return emptyPoints;
            }
        }
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
            generateDoor();
            generateKey();
            generateItems(ItemsRepository.CommonMisc, numTiles/2);
            generateItems(ItemsRepository.UncommonMisc, numTiles/4);
            generateItems(ItemsRepository.RareMisc, 2);
            generateItems(ItemsRepository.VeryRareMisc, 1);
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
            var centralTile = new Tile(TileSize.Large, TileSize.Large, 1, 1, 1, 1, new TileInterior(TileSize.Large, TileSize.Large, InteriorPreset.IndentedCorners)); 
            var centralTileStart = new Point(Width/2 - (int)TileSize.Large/2, Height/2 - (int)TileSize.Large/2);
            centralTile.TranslateAssets(centralTileStart);
            _centralTile = centralTile;
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
        
        private void generateDoor()
        {
            var validPoints = EmptyPoints.Where(p =>
                // Central tile does not contain point:
                !_centralTile.OnMap(p) && 
                // Tiles connected to central tile do not contain point:
                _tiles.Where(t => t.DoOpeningPointsMatch(_centralTile)).All(tA => 
                    !tA.OnMap(p)));
            var spawnPoint = validPoints.RandomElement();
            AddAsset(new Door(spawnPoint));
        }

        private void generateKey()
        {
            var doorTile = _tiles.Single(t => t.OnMap(Assets.Single(a => a is Door).Location));
            var validPoints = EmptyPoints.Where(p =>
                // Central tile does not contain point:
                !_centralTile.OnMap(p) && 
                // Tiles connected to central tile do not contain point:
                _tiles.Where(t => t.DoOpeningPointsMatch(_centralTile)).All(tA => 
                    !tA.OnMap(p)) &&
                // Door tile does not contain point:
                !doorTile.OnMap(p) &&
                // Tiles connected to door tile do not contain point:
                _tiles.Where(t => t.DoOpeningPointsMatch(doorTile)).All(tA => 
                    !tA.OnMap(p)));
            var spawnPoint = validPoints.RandomElement();
            AddAsset(new Key(spawnPoint));
        }

        private void generateItems(Item[] repository, int numItems)
        {
            for (int i = 0; i < numItems; i++)
            {
                var itemToAdd = new Item(repository.RandomElement());
                var validPoints = EmptyPoints.Where(p =>
                    // Central tile does not contain items:
                    !_centralTile.OnMap(p) && 
                    // Tiles can only have up to 3 items:
                    _tiles.Where(t => t.Assets.Where(a => a is Item).Count() > 3).All(tA =>
                        !tA.OnMap(p)));
                itemToAdd.Location = validPoints.RandomElement();
                AddAsset(itemToAdd);
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
            // Invalid points:
            var pointsOutOfBounds = _assetPointLocations.Where(p => !OnMap(p));
            var pointDuplicates = _assetPointLocations.Where(p1 => _assetPointLocations.Any(p2 => p1 != p2 && p1.X == p2.X && p1.Y == p2.Y));
            var pointsWithinRects = _assetPointLocations.Where(p => _assetRectLocations.Any(r => Rectangle.DoesRectContainPoint(p, r)));

            // Invalid rectangles:
            var rectsOutOfBounds = _assetRectLocations.Where(r => !OnMap(r));
            var rectsIntersecting = _assetRectLocations.Where(r1 => _assetRectLocations.Any(r2 => r1 != r2 && Rectangle.DoRectsIntersect(r1, r2)));

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

        public void AddAsset(IMappable asset, Point start)
        {
            if (!(asset is Barrier))
            {
                asset.Location.X = start.X;
                asset.Location.Y = start.Y;
                Assets.Add(asset);
            }
            else
            {
                throw new Exception("ProceduralDungeon.Map.AddAsset(IMappable asset, Point start) cannot be used with barriers.");
            }
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

        public void AddItem(Item itemToAdd, Point start)
        {
            itemToAdd.Location = new Point(start);
            Assets.Add(itemToAdd);
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

        public void AddPlayer(Player player)
        {
            var validSpawns = EmptyPoints.Where(eP => _centralTile.OnMap(eP));
            player.Location = validSpawns.RandomElement();
            AddAsset(player);
        }
        
        public void AddNpc(Npc npc)
        {
            var validSpawns = EmptyPoints.Where(eP => !_centralTile.OnMap(eP));
            npc.Location = validSpawns.RandomElement();
            AddAsset(npc);
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

        public IMappable[] GetAssetsByName(string name)
        {
            return Assets.Where(a => a is INameable && (a as INameable).Name == name).ToArray();
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

        public IMappable[] GetAssetsInRangeOf(Point origin, int range)
        {
            return Assets.Where(o => origin.InRangeOf(o.Location, range)).ToArray();
        }
        
        public void PrintMap()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Console.ForegroundColor = DarkGray;
                    
                    // if (_bloodSplatterCoordinates.Any(c => c[0] == x && c[1] == y))
                    // {
                    //     Console.BackgroundColor = DarkRed;
                    // }
                    var thisAsset = Assets.FirstOrDefault(a => 
                        a.Location.X == x && a.Location.Y == y || a is IRectangular && 
                        Rectangle.DoesRectContainPoint(new Point(x, y), (a as IRectangular).Rect));
                    if (thisAsset != null)
                    {
                        if (thisAsset is Door || thisAsset is Npc) Console.ForegroundColor = White;
                        if (thisAsset is Item) Console.ForegroundColor = DarkYellow;
                        if (thisAsset is Player) Console.ForegroundColor = Blue;
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
    
        // Prints map from perspective of a creature:
        public void PrintMap(Creature creature)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var thisPoint = new Point(x, y);
                    var thisAsset = Assets.FirstOrDefault(a => 
                        a.Location.X == x && a.Location.Y == y || a is IRectangular && 
                        Rectangle.DoesRectContainPoint(new Point(x, y), (a as IRectangular).Rect));

                    Console.ForegroundColor = DarkGray;
                    bool inSearchRange = creature.Location.InRangeOf(thisPoint, creature.SearchRange);
                    if (inSearchRange) 
                    {
                        Console.ForegroundColor = Gray;
                        Console.BackgroundColor = DarkGray;
                    }

                    if (thisAsset != null &&
                        !(thisAsset is INameable && !inSearchRange))
                    {
                        if (thisAsset is Door || thisAsset is Npc) Console.ForegroundColor = White;
                        if (thisAsset is Item) Console.ForegroundColor = DarkYellow;
                        if (thisAsset is Player) Console.ForegroundColor = DarkBlue;
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
        public bool Move(IMappable assetToMove, ConsoleKeyInfo input)
        {
            var tempLocation = new Point(assetToMove.Location);
            tempLocation.Translate(input);
            bool isDestinationValid = OnMap(tempLocation) &&
                _assetPointLocations.All(p => p.X != tempLocation.X || p.Y != tempLocation.Y) &&
                _assetRectLocations.All(r => !Rectangle.DoesRectContainPoint(tempLocation, r));
            if (isDestinationValid) assetToMove.Location.Translate(input);
            return isDestinationValid;
        }

        public List<IMappable> GetPathObstructions(Point pathStart, Point pathEnd)
        {
            var otherAssets = Assets.Where(a => a.Location.X != pathStart.X && a.Location.Y != pathStart.Y &&
                a.Location.X != pathEnd.X && a.Location.Y != pathEnd.Y);
            return otherAssets.Where(a =>
                a is IRectangular ? Rectangle.DoesLineIntersectRect(pathStart, pathEnd, (a as IRectangular).Rect) :
                Point.IsPointOnLineSegment(pathStart, pathEnd, a.Location)).ToList();
        }

        public static void ShowLegend()
        {
            Console.WriteLine("Map Legend:");

            Console.ForegroundColor = DarkBlue;
            Console.Write(Symbols.Player);
            Console.ForegroundColor = White;
            Console.WriteLine("  - player character");

            Console.WriteLine($"{Symbols.Npc}  - enemy");
            Console.WriteLine($"{Symbols.Dead}  - dead body");

            Console.ForegroundColor = DarkYellow;
            Console.Write(Symbols.Item);
            Console.ForegroundColor = White;
            Console.WriteLine("  - item that may be picked up");

            Console.ForegroundColor = DarkYellow;
            Console.Write(Symbols.Key);
            Console.ForegroundColor = White;
            Console.WriteLine("  - key used to open doors");

            Console.ForegroundColor = DarkGray;
            Console.Write(Symbols.Barrier);
            Console.ForegroundColor = White;
            Console.WriteLine("  - wall or other barrier");

            Console.WriteLine($"{Symbols.Door}  - door to access the next floor");

            WaitForInput();
            Console.ResetColor();
        }
    }
}