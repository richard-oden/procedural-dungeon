using System;
using System.Collections.Generic;

namespace ProceduralDungeon
{
    public class TileBorder : Map
    {
        public int NumGates {get; private set;}
        public string Structure {get; private set;}

        public TileBorder(TileSize length, Orientation orientation, int numGates) : base()
        {
            NumGates = numGates;
            Width = 0;
            Height = 0;
            if (orientation == Orientation.Vertical)
            {
                Height = (int)length;
            }
            else if (orientation == Orientation.Horizontal)
            {
                Width = (int)length;
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
            var rectangles = createRects((int)length, orientation);
            foreach (var r in rectangles)
            {
                Assets.Add(new Barrier(r));
            }
        }

        private List<Rectangle> createRects(int length, Orientation orientation)
        {
            var rects = new List<Rectangle>();
            switch (NumGates)
            {
                case 0:
                    rects.Add(new Rectangle(1, length-2, orientation));
                    break;
                case 1:
                    rects.Add(new Rectangle(1, length/3-1, orientation));
                    rects.Add(new Rectangle(length - length/3, length-2, orientation));
                    break;
                case 2:
                    if (length == (int)TileSize.Small)
                    {
                        rects.Add(new Rectangle(2, 3, orientation));
                    }
                    else if (length == (int)TileSize.Medium)
                    {
                        rects.Add(new Rectangle(3, 5, orientation));
                    }
                    else if (length == (int)TileSize.Large)
                    {
                        rects.Add(new Rectangle(1, 1, orientation));
                        rects.Add(new Rectangle(4, 7, orientation));
                        rects.Add(new Rectangle(10, 10, orientation));
                    }
                    break;
                default:
                    throw new Exception("Invalid number of gates. Must be 0, 1, or 2.");
            }
            return rects;
        }
    }

    public enum Orientation
    {
        Vertical,
        Horizontal
    }
}