using System;
using System.Linq;
using System.Collections.Generic;

namespace ProceduralDungeon
{
    public class Rectangle
    {
        public Point StartLocation {get; set;} // upper-left corner
        public int Width {get; set;}
        public int Height {get; set;}
        public int YMin => StartLocation.Y;
        public int YMax => StartLocation.Y + Height;
        public int XMin => StartLocation.X;
        public int XMax => StartLocation.X + Width;
        public Point NwCorner => new Point(XMin, YMin);
        public Point NeCorner => new Point(XMax, YMin);
        public Point SwCorner => new Point(XMin, YMax);
        public Point SeCorner => new Point(XMax, YMax);

        public bool Completed {get; set;} = false;

        public Rectangle(Point startLocation, int width, int height)
        {
            StartLocation = startLocation;
            Width = width;
            Height = height;
        }

        public Rectangle(int start, int end, Orientation borderOrientation)
        {
            StartLocation = new Point(borderOrientation, start);
            if (borderOrientation == Orientation.Horizontal)
            {
                Width = end - start;
                Height = 0;
            }
            else if (borderOrientation == Orientation.Vertical)
            {
                Width = 0;
                Height = end - start;
            }
            else
            {
                throw new Exception("Unable to create point due to invalid orientation.");
            }
        }
        public static bool DoesRectContainPoint(Point point, Rectangle shape)
        {
            if (point.X >= shape.XMin && point.X <= shape.XMax &&
                point.Y >= shape.YMin && point.Y <= shape.YMax)
            {
                return true;
            }
            else
            {
                return false;
            }
        
        }

        public static bool DoesLineIntersectRect(Point lineStart, Point lineEnd, Rectangle rect)
        {
            if (DoesRectContainPoint(lineStart, rect) ||
                DoesRectContainPoint(lineEnd, rect))
            {
                return true;
            }
            else if (Point.DoLinesIntersect(lineStart, lineEnd, rect.NeCorner, rect.NwCorner) ||
                Point.DoLinesIntersect(lineStart, lineEnd, rect.NwCorner, rect.SwCorner) ||
                Point.DoLinesIntersect(lineStart, lineEnd, rect.SwCorner, rect.SeCorner) ||
                Point.DoLinesIntersect(lineStart, lineEnd, rect.SeCorner, rect.NeCorner))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool DoRectsIntersect(Rectangle rect1, Rectangle rect2)
        {
            if (DoesRectContainPoint(rect1.NeCorner, rect2) ||
                DoesRectContainPoint(rect1.NwCorner, rect2) ||
                DoesRectContainPoint(rect1.SwCorner, rect2) ||
                DoesRectContainPoint(rect1.SeCorner, rect2))
            {
                return true;
            }
            {
                return false;
            }
        }     
    }
}