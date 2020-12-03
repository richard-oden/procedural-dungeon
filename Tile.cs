using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon
{
    public class Tile : Map
    {
        public Dictionary<char, TileBorder> Borders {get; private set;}
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
            AddAssets(Borders['N'].Assets, new Point(0, 0));
            AddAssets(Borders['S'].Assets, new Point(0, Height-1));
            AddAssets(Borders['E'].Assets, new Point(Width-1, 0));
            AddAssets(Borders['W'].Assets, new Point(0, 0));
            fillCorners();
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

    }
    public enum TileSize
    {
        Tiny = 3,
        Small = 6,
        Medium = 9,
        Large = 12
    }
}