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
        public List<Item> Items => Assets.Where(a => a is Item).Cast<Item>().ToList();
        public List<IContainer> Containers => Assets.Where(a => a is IContainer).Cast<IContainer>().ToList();
        public List<Item> AllItems
        {
            get
            {
                var itemsInInventories = new List<Item>();
                foreach (var c in Containers) itemsInInventories.AddRange(c.Inventory);
                return Items.Concat(itemsInInventories).ToList();
            }
        }        
        public List<IMappable> AllAssets => Assets.Concat(AllItems.Cast<IMappable>()).Distinct().ToList();
        public List<Repellant> Repellants => AllItems.Where(i => i is Repellant).Cast<Repellant>().ToList();
        public List<Creature> Creatures => Assets.Where(a => a is Creature).Cast<Creature>().ToList();
        public List<Npc> Npcs => Assets.Where(a => a is Npc).Cast<Npc>().ToList();
        public List<Chest> Chests => Assets.Where(a => a is Chest).Cast<Chest>().ToList();
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
        public bool HasPlayerExited {get; set;} = false;
        
        public Map()
        {
        }

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public Map(MapSize size, Player player, Difficulty difficulty, int floor)
        {
            var _rand = new Random();

            int sizeMin = (int)size - 6;
            int sizeMax = (int)size + 6;
            Width = _rand.Next(sizeMin, sizeMax);
            Height = _rand.Next(sizeMin, sizeMax);

            int numTiles = Width < Height ? Width/2 : Height/2;
            int numAttempts = numTiles * 3;
            generateTiles(numTiles, numAttempts);
            fillSpaceBetweenTiles();
            AddPlayer(player);
            generateDoor();
            generateKey();
            generateItemsUsingDifficulty(difficulty, floor);
            generateChestsUsingDifficulty(difficulty, floor);
            generateNpcsUsingDifficulty(difficulty, floor);
            validateAssets(Assets);
        }

        public void generateCentralTile()
        {
            _centralTile = new Tile(TileSize.Large, TileSize.Large, 1, 1, 1, 1, new TileInterior(TileSize.Large, TileSize.Large, InteriorPreset.IndentedCorners)); 
            var centralTileStart = new Point(Width/2 - (int)TileSize.Large/2, Height/2 - (int)TileSize.Large/2);
            _centralTile.TranslateAssets(centralTileStart);
            AddTile(_centralTile);
        }
        
        public static Map CreateMerchantMap(Player player, Difficulty difficulty, int floor)
        {
            var merchantMap = new Map(14, 14);
            merchantMap.generateCentralTile();
            merchantMap.fillSpaceBetweenTiles();
            merchantMap.AddPlayer(player);
            var merchant = Merchant.GenerateUsingDifficulty(difficulty, floor, player);
            merchant.Location = merchantMap.EmptyPoints.RandomElement();
            merchantMap.AddAsset(merchant);
            var door = new Door(merchantMap, merchantMap.EmptyPoints.RandomElement(), false);
            merchantMap.AddAsset(door);
            player.AddToMemory(merchant);
            player.AddToMemory(door);
            return merchantMap;
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
                        if (WithinBorder(tileToAdd, 2) &&
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
            generateCentralTile();
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
            // If no points meet above conditions, then set location to random empty point:
            var spawnPoint = validPoints.Count() > 0 ? validPoints.RandomElement() : EmptyPoints.RandomElement();
            AddAsset(new Door(this, spawnPoint));
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
            // If no points meet above conditions, then set location to random empty point:
            var spawnPoint = validPoints.Count() > 0 ? validPoints.RandomElement() : EmptyPoints.RandomElement();
            AddAsset(new Key(spawnPoint));
        }

        private void generateItem(Item item)
        {
            var clonedItem = item.GetClone();
            var validPoints = EmptyPoints.Where(p =>
                // Central tile does not contain items:
                !_centralTile.OnMap(p) && 
                // Tiles can only have up to 3 items:
                _tiles.Where(t => t.Items.Count > 3).All(tA => !tA.OnMap(p)));
            // If no points meet above conditions, then set location to random empty point:
            clonedItem.Location = validPoints.Count() > 0 ? validPoints.RandomElement() : EmptyPoints.RandomElement();
            AddAsset(clonedItem);
        }

        private void generateNpc(Npc npc, int npcsPerTile)
        {
            var clonedNpc = npc.GetClone();
            var validPoints = EmptyPoints.Where(p =>
                // Central tile does not contain npcs:
                !_centralTile.OnMap(p) && 
                // Tiles have limited npcs based on difficulty:
                _tiles.Where(t => t.Creatures.Count() > npcsPerTile).All(tA => !tA.OnMap(p)));
            // If no points meet above conditions, then set location to random empty point:
            clonedNpc.Location = validPoints.Count() > 0 ? validPoints.RandomElement() : EmptyPoints.RandomElement();
            AddAsset(clonedNpc);
        }

        private void generateItemsUsingDifficulty(Difficulty difficulty, int floor)
        {
            var totalItemMax = difficulty.ItemToTileRatio * _tiles.Count;
            var itemValueAverage = difficulty.AverageItemValue + (floor * 2);
            for (int i = 0; i < totalItemMax; i++)
            {
                Item newItem = null;
                if (i < totalItemMax / 6)
                {
                    newItem = ItemsRepository.All.Where(i => 
                        i.Value.IsBetween(itemValueAverage-40, itemValueAverage+40)).RandomElement();
                }
                else
                {
                    double currentItemValueSum = Items.Sum(i => i.Value);
                    double iVVariance = 15;

                    while (newItem == null)
                    {
                        var potentialItems = ItemsRepository.All.Where(i =>
                            ((currentItemValueSum + i.Value) / (Items.Count + 1))
                            .IsBetween(itemValueAverage-iVVariance, itemValueAverage+iVVariance));
                        if (potentialItems.Any())
                        {
                            newItem = potentialItems.RandomElement();
                        }
                        else
                        {
                            iVVariance += 2;
                        }
                    }
                }
                generateItem(newItem);
            }
            // System.Console.WriteLine();
            // System.Console.WriteLine("Max items: " + totalItemMax);
            // System.Console.WriteLine("Average item value: " + itemValueAverage);
            // System.Console.WriteLine("Actual items: " + Items.Count);
            // System.Console.WriteLine("Actual average item value: " + Items.Average(i => i.Value));
            // System.Console.WriteLine("All items: " + Items.Select(i => i.Name + " - " + i.Value).ToString("and"));
            // System.Console.WriteLine();
        }

        private void generateNpcsUsingDifficulty(Difficulty difficulty, int floor)
        {
            double totalNpcMax = difficulty.NpcToTileRatio * _tiles.Count + (floor * .2);
            double npcChallengeAverage = difficulty.AverageNpcChallenge + (floor * .5);
            for (int i = 0; i < totalNpcMax; i++)
            {
                Npc newNpc = null;
                // For first fifth of npcs, add npcs within a range of -4/+4 of average challenge:
                if (i < totalNpcMax / 5)
                {
                    newNpc = NpcsRepository.All.Where(n => 
                        ((double)n.ChallengeLevel).IsBetween(npcChallengeAverage-4, npcChallengeAverage+4)).RandomElement();
                }
                // Then add npcs where total average will be within acceptable variance:
                else
                {
                    double currentNpcChallengeSum = Npcs.Sum(n => n.ChallengeLevel);
                    double cAVariance = .5;

                    // If no suitable npcs, increase variance until one matches:
                    while (newNpc == null)
                    {
                        var potentialNpcs = NpcsRepository.All.Where(n =>
                            ((currentNpcChallengeSum + n.ChallengeLevel) / (Npcs.Count + 1))
                            .IsBetween(npcChallengeAverage-cAVariance, npcChallengeAverage+cAVariance));
                        if (potentialNpcs.Any())
                        {
                            newNpc = potentialNpcs.RandomElement();
                        }
                        else
                        {
                            cAVariance += .5;
                        }
                    }
                }
                generateNpc(newNpc, difficulty.MaxNpcsPerTile + (int)(floor * .25));
            }
            // System.Console.WriteLine("Max npcs: " + totalNpcMax);
            // System.Console.WriteLine("Average challenge level: " + npcChallengeAverage);
            // System.Console.WriteLine("Actual npcs: " + Npcs.Count);
            // System.Console.WriteLine("Actual average challenge level: " + Npcs.Average(n => n.ChallengeLevel));
            // System.Console.WriteLine("All npcs: " + Npcs.Select(n => n.Name + " - " + n.ChallengeLevel).ToString("and"));
        }

        public void generateChestsUsingDifficulty(Difficulty difficulty, int floor)
        {
            var rand = new Random();
            if (rand.NextDouble() < difficulty.ChestSpawnChance + floor *.05)
            {
                int totalChestValueMax = difficulty.ChestValuePerTile * _tiles.Count;
                var validSpawns = EmptyPoints.Where(eP => GetAssetsInRangeOf(eP, 1).Count() < 2);
                while (Chests.Sum(c => c.TotalValue) < totalChestValueMax)
                {
                    var newChest = ChestsRepository.All.RandomElement();
                    if (newChest.TotalValue + Chests.Sum(c => c.TotalValue) <= totalChestValueMax + 15)
                    {
                        newChest.Location = validSpawns.RandomElement();
                        AddAsset(newChest);
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

        public void ManageIDegradables()
        {
            var allIDegradables = AllItems.Where(i => i is IDegradable).Cast<IDegradable>();
            foreach (var d in allIDegradables)
            {
                if ((d is Repellant && (d as Repellant).IsActive) || !(d is Repellant))
                {
                    d.DecrementDuration();
                }
            }
        }
        public void PurgeDestroyedAssets()
        {
            Assets.RemoveAll(a => a is IDestroyable && (a as IDestroyable).IsDestroyed);
            foreach (var c in Containers) c.Inventory.RemoveAll(i => i.IsDestroyed);
            foreach (var c in Creatures) c.EquippedItems.RemoveAll(i => (i as Item).IsDestroyed);
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
        public void PrintMapFromPerspective(Creature creature)
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

                    if (thisAsset != null && !((thisAsset is INameable || thisAsset is Door) && !inSearchRange))
                    {
                        if (thisAsset is Door || thisAsset is Npc) Console.ForegroundColor = White;
                        if (thisAsset is Item || thisAsset is Chest) Console.ForegroundColor = DarkYellow;
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

        // Prints only section of map within viewport:
        public void PrintMapFromViewport(Creature creature)
        {
            int searchRadius = creature.SearchRange + 4;
            int searchDiameter = searchRadius*2 + 1;

            int getOriginCoord(int creatureCoord, int upperLimit)
            {
                int tempOriginCoord = creatureCoord - searchRadius;
                if (tempOriginCoord < 0)
                {
                    return 0;
                }
                else if (tempOriginCoord + searchDiameter > upperLimit)
                {
                    return upperLimit - searchDiameter < 0 ? 0 : upperLimit - searchDiameter;
                }
                else
                {
                    return tempOriginCoord;
                }
            }

            Point origin = new Point(getOriginCoord(creature.Location.X, Width), getOriginCoord(creature.Location.Y, Height));
            int viewportHeight = origin.Y + searchDiameter > Height ? Height : origin.Y + searchDiameter;
            int viewportWidth = origin.X + searchDiameter > Width ? Width : origin.X + searchDiameter;

            for (int y = origin.Y; y < viewportHeight; y++)
            {
                for (int x = origin.X; x < viewportWidth; x++)
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

                    if (thisAsset != null && !((thisAsset is INameable || thisAsset is Door) && !inSearchRange))
                    {
                        if (thisAsset is Door || thisAsset is Npc) Console.ForegroundColor = White;
                        if (thisAsset is Item || thisAsset is Chest) Console.ForegroundColor = DarkYellow;
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

        // Prints map only showing highlighted assets:
        public void PrintMapWithOnlyHighlightedAssets(IEnumerable<IMappable> highlightedAssets)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Console.ForegroundColor = DarkGray;
                    
                    var thisAsset = Assets.FirstOrDefault(a => 
                        a.Location.X == x && a.Location.Y == y || a is IRectangular && 
                        Rectangle.DoesRectContainPoint(new Point(x, y), (a as IRectangular).Rect));
                    if (thisAsset != null && !(thisAsset is Npc) && 
                        !((thisAsset is Item || thisAsset is Door) && 
                        !highlightedAssets.Contains(thisAsset)))
                    {
                        if (thisAsset is Door || thisAsset is Item) Console.BackgroundColor = Yellow;
                        if (thisAsset is Door) Console.ForegroundColor = White;
                        if (thisAsset is Item || thisAsset is Chest) Console.ForegroundColor = DarkYellow;
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
        
        // Prints only section of map within viewport, highlighting the selected item:
        // (For use in menu class)
        public void PrintMapFromViewportWithHighlightedAsset(Creature creature, IMappable highlightedAsset)
        {
            int searchRadius = creature.SearchRange + 4;
            int searchDiameter = searchRadius*2 + 1;

            int getOriginCoord(int creatureCoord, int upperLimit)
            {
                int tempOriginCoord = creatureCoord - searchRadius;
                if (tempOriginCoord < 0)
                {
                    return 0;
                }
                else if (tempOriginCoord + searchDiameter > upperLimit)
                {
                    return upperLimit - searchDiameter;
                }
                else
                {
                    return tempOriginCoord;
                }
            }

            Point origin = new Point(getOriginCoord(creature.Location.X, Width), getOriginCoord(creature.Location.Y, Height));
            int viewportHeight = origin.Y + searchDiameter;
            int viewportWidth = origin.X + searchDiameter;

            for (int y = origin.Y; y < viewportHeight; y++)
            {
                for (int x = origin.X; x < viewportWidth; x++)
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

                    if (thisAsset != null && !((thisAsset is INameable || thisAsset is Door) && !inSearchRange))
                    {
                        if (highlightedAsset != null && thisAsset == highlightedAsset) Console.BackgroundColor = Yellow;
                        if (thisAsset is Door || thisAsset is Npc) Console.ForegroundColor = White;
                        if (thisAsset is Item || thisAsset is Chest) Console.ForegroundColor = DarkYellow;
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
        public enum MapSize
        {
            XSmall = 24,
            Small = 36,
            Medium = 48,
            Large = 60,
            XLarge = 72
        }
}