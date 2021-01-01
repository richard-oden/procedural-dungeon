using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon
{
    public class TileBorder : Map
    {
        public int NumGates {get; private set;}
        public Orientation Orientation {get; private set;}
        public int Length {get; private set;}
        public List<Point> OpeningPoints {get; private set;} = new List<Point>();

        public TileBorder(TileSize length, Orientation orientation, int numGates) : base()
        {
            Length = (int)length;
            Orientation = orientation;
            NumGates = numGates;
            Width = 0;
            Height = 0;
            if (Orientation == Orientation.Vertical)
            {
                Height = Length;
            }
            else if (Orientation == Orientation.Horizontal)
            {
                Width = Length;
            }
            // if (gateTypes != null) GateTypes = gateTypes;
            // if (gateTypes.Length != (int)numGates)
            // {
            //     throw new Exception($"A border with {(int)NumGates} cannot have {gateTypes.Length} gate types.");
            // }
            // else if (Length == TileSize.Tiny && (int)NumGates > 1)
            // {
            //     throw new Exception("A border with a length of tiny cannot have more than one gate.");
            // }
            // else if (Length < TileSize.Large && (int)NumGates > 2)
            // {
            //     throw new Exception("Only large and extra large borders may have three gates.");
            // }
            createBarriers();
            createOpeningPoints();
        }

        private void createBarriers()
        {
            var rects = new List<Rectangle>();
            switch (NumGates)
            {
                case 0:
                    rects.Add(new Rectangle(1, Length-2, Orientation));
                    break;
                case 1:
                    rects.Add(new Rectangle(1, Length/3-1, Orientation));
                    rects.Add(new Rectangle(Length - Length/3, Length-2, Orientation));
                    break;
                case 2:
                    if (Length == (int)TileSize.Small)
                    {
                        rects.Add(new Rectangle(2, 3, Orientation));
                    }
                    else if (Length == (int)TileSize.Medium)
                    {
                        rects.Add(new Rectangle(3, 5, Orientation));
                    }
                    else if (Length == (int)TileSize.Large)
                    {
                        rects.Add(new Rectangle(1, 1, Orientation));
                        rects.Add(new Rectangle(4, 7, Orientation));
                        rects.Add(new Rectangle(10, 10, Orientation));
                    }
                    break;
                default:
                    throw new Exception("Invalid number of gates. Must be 0, 1, or 2.");
            }
            foreach (var r in rects) Assets.Add(new Barrier(r));
        }
        private void createOpeningPoints()
        {
            for (int i = 0; i < Length; i++)
            {
                var tempPoint = new Point(Orientation, i);
                var barrierRects = from a in Assets where a is Barrier select (a as Barrier).Rect;
                if (barrierRects.All(bR => !Rectangle.DoesRectContainPoint(tempPoint, bR)) &&
                    // Don't include corners:
                    i != 0 && i != Length-1)
                {
                    OpeningPoints.Add(tempPoint);
                }
            }
        }
    }

    public enum Orientation
    {
        Vertical,
        Horizontal
    }
}