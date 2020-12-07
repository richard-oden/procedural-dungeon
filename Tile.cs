using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon
{
    public class Tile : Map
    {
        public Point startLocation {get; private set;} = new Point(0,0);
        public Dictionary<char, TileBorder> Borders {get; private set;}
        public List<Point> OpeningPoints {get; private set;} = new List<Point>();
        // public TileInterior Interior {get; private set;}

        public Tile(TileSize width, TileSize height, 
            int northGates, int southGates, int eastGates, int westGates) : base()
        {
            Width = (int)width;
            Height = (int)height;
            Borders = new Dictionary<char, TileBorder>();

            Borders.Add('N', new TileBorder(width, Orientation.Horizontal, northGates));
            Borders.Add('S', new TileBorder(width, Orientation.Horizontal, southGates));
            Borders.Add('E', new TileBorder(height, Orientation.Vertical, eastGates));
            Borders.Add('W', new TileBorder(height, Orientation.Vertical, westGates));
            AddBorderAssets(Borders['N'], new Point(0, 0));
            AddBorderAssets(Borders['S'], new Point(0, Height-1));
            AddBorderAssets(Borders['E'], new Point(Width-1, 0));
            AddBorderAssets(Borders['W'], new Point(0, 0));
            fillCorners();
        }

        public static Tile CreateRandom()
        {
            var width = (TileSize)(Dice.D4.Roll()*3);
            var height = (TileSize)(Dice.D4.Roll()*3);
            var northGates = (width > TileSize.Tiny ? Dice.D3 : Dice.Coin).RollBaseZero();
            var southGates = (width > TileSize.Tiny ? Dice.D3 : Dice.Coin).RollBaseZero();
            var eastGates = (height > TileSize.Tiny ? Dice.D3 : Dice.Coin).RollBaseZero();
            var westGates = (height > TileSize.Tiny ? Dice.D3 : Dice.Coin).RollBaseZero();
            return new Tile(width, height, northGates, southGates, eastGates, westGates);
        }

        private void fillCorners()
        {
            Barrier getBarrierThatIncludes(Point point)
            {
                var iMappable = Assets.SingleOrDefault(a => a is Barrier && 
                Rectangle.DoesRectContainPoint(point, (a as IRectangular).Rect));
                return (Barrier)iMappable;
            }

            void createOrExpandBarrierAt(Point point)
            {
                Barrier cornerBarrier = null;

                if (point.X == 0)
                {
                    cornerBarrier = getBarrierThatIncludes(new Point(1, point.Y));
                }
                else if (point.X == Width-1)
                {
                    cornerBarrier = getBarrierThatIncludes(new Point(point.X-1, point.Y));
                }

                if (cornerBarrier != null)
                {
                    if (point.X == 0) cornerBarrier.Rect.StartLocation.X--;
                    cornerBarrier.Rect.Width++;
                }
                else
                {
                    if (point.Y == 0)
                    {
                        cornerBarrier = getBarrierThatIncludes(new Point(point.X, 1));
                    }
                    else if (point.Y == Height-1)
                    {
                        cornerBarrier = getBarrierThatIncludes(new Point(point.X, point.Y-1));
                    }

                    if (cornerBarrier != null)
                    {
                        if (point.Y == 0) cornerBarrier.Rect.StartLocation.Y--;
                        cornerBarrier.Rect.Height++;
                    }
                    else
                    {
                        Assets.Add(new Barrier(new Rectangle(point, 0, 0)));
                    }
                }
            }
            
            createOrExpandBarrierAt(new Point(0, 0));
            createOrExpandBarrierAt(new Point(Width-1, 0));
            createOrExpandBarrierAt(new Point(0, Height-1));
            createOrExpandBarrierAt(new Point(Width-1, Height-1));
        }
        public override bool OnMap(Point point)
        {
            return point.X >= startLocation.X && point.X < startLocation.X + Width && 
                   point.Y >= startLocation.Y && point.Y < startLocation.Y + Height;
        }
        public void TranslateAssets(Point start)
        {
            startLocation = start;
            foreach (var asset in Assets)
            {
                asset.Location.X += start.X;
                asset.Location.Y += start.Y;
            }
            foreach (var point in OpeningPoints)
            {
                point.X += start.X;
                point.Y += start.Y;
            }
        }
        public void AddBorderAssets(TileBorder border, Point start)
        {
            foreach (var asset in border.Assets)
            {
                asset.Location.X += start.X;
                asset.Location.Y += start.Y;
            }
            foreach (var point in border.OpeningPoints)
            {
                point.X += start.X;
                point.Y += start.Y;
            }
            AddAssets(border.Assets);
            OpeningPoints.AddRange(border.OpeningPoints);
        }
        public bool DoOpeningPointsMatch(Tile tileToMatch)
        {
            bool canConnectToSouth = this.Borders['S'].OpeningPoints.Any(p1 =>
                tileToMatch.Borders['N'].OpeningPoints.Any(p2 => p2.Y - p1.Y == 1 && p2.X == p1.X));
            bool canConnectToNorth = this.Borders['N'].OpeningPoints.Any(p1 =>
                tileToMatch.Borders['S'].OpeningPoints.Any(p2 => p1.Y - p2.Y == 1 && p1.X == p2.X));
            bool canConnectToEast = this.Borders['E'].OpeningPoints.Any(p1 =>
                tileToMatch.Borders['W'].OpeningPoints.Any(p2 => p2.X - p1.X == 1 && p2.Y == p1.Y));
            bool canConnectToWest = this.Borders['W'].OpeningPoints.Any(p1 =>
                tileToMatch.Borders['E'].OpeningPoints.Any(p2 => p1.X - p2.X == 1 && p1.Y == p2.Y));

            return canConnectToSouth || canConnectToNorth || canConnectToEast || canConnectToWest;
        }
        public bool DoesIntersectTile(Tile tileToTest)
        {
            bool thisTileContainsPoint = tileToTest.Assets.Any(a => this.OnMap(a.Location));
            bool thisTileContainsRect = tileToTest.Assets.Any(a => a is IRectangular &&
                this.OnMap((a as IRectangular).Rect));
            var rects = Assets.Where(a => a is IRectangular).Select(a => (a as IRectangular).Rect);
            var tileToTestRects = tileToTest.Assets.Where(a => a is IRectangular).Select(a => (a as IRectangular).Rect);
            bool intersectingRects = rects.Any(r1 => tileToTestRects.Any(r2 => Rectangle.DoRectsIntersect(r1, r2)));

            return thisTileContainsPoint || thisTileContainsRect || intersectingRects;
        }
    }
    public enum TileSize
    {
        Tiny = 3,
        Small = 6,
        Medium = 9,
        Large = 12
    }
}