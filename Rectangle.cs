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
        public Point NeCorner => new Point(XMax-1, YMin);
        public Point SwCorner => new Point(XMin, YMax-1);
        public Point SeCorner => new Point(XMax-1, YMax-1);

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
        public static bool DoesRectContainPoint(Point point, Rectangle rect)
        {
            if (point.X >= rect.XMin && point.X <= rect.XMax &&
                point.Y >= rect.YMin && point.Y <= rect.YMax)
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
            else if (rect.Height == 0)
            {
                return Point.DoLineSegmentsIntersect(lineStart, lineEnd, rect.NeCorner, rect.NwCorner);
            }
            else if (rect.Width == 0)
            {
                return Point.DoLineSegmentsIntersect(lineStart, lineEnd, rect.NeCorner, rect.SeCorner);
            }
            else if (Point.DoLineSegmentsIntersect(lineStart, lineEnd, rect.NeCorner, rect.NwCorner) ||
                Point.DoLineSegmentsIntersect(lineStart, lineEnd, rect.NwCorner, rect.SwCorner) ||
                Point.DoLineSegmentsIntersect(lineStart, lineEnd, rect.SwCorner, rect.SeCorner) ||
                Point.DoLineSegmentsIntersect(lineStart, lineEnd, rect.SeCorner, rect.NeCorner))
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
            for (int y = rect1.YMin; y <= rect1.YMax; y++)
            {
                for (int x = rect1.XMin; x <= rect1.XMax; x++)
                {
                    if (DoesRectContainPoint(new Point(x, y), rect2)) return true;
                }
            }
            return false;
        }     
    }
}