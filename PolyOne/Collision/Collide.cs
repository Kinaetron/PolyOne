using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace PolyOne.Collision
{
    [Flags]
    public enum PointSectors
    {
        Centre = 0,
        Top = 1,
        Bottom = 2,
        TopLeft = 9,
        TopRight = 5,
        Left = 8,
        Right = 4,
        BottomLeft = 10,
        BottomRight = 6
    };

    static public class Collide
    {
        static public bool Check(Entity a, Entity b)
        {
            if (a.Collider == null || b.Collider == null)
            {
                return false;
            }
            else
            {
                return a != b && b.Collidable && a.Collider.Collide(b);
            }
        }

        static public bool Check(Entity a, Entity b, Vector2 at)
        {
            Vector2 old = a.Position;
            a.Position = at;
            bool ret = Check(a, b);
            a.Position = old;
            return ret;
        }

        static public bool Check(Entity a, IEnumerable<Entity> b)
        {
            foreach (Entity e in b)
            {
                if (Check(a, e) == true)
                {
                    return true;
                }
            }
            return false;
        }

        static public bool Check(Entity a, IEnumerable<Entity> b, Vector2 at)
        {
            Vector2 old = a.Position;
            a.Position = at;
            bool ret = Check(a, b);
            a.Position = old;
            return ret;
        }

        static public Entity First(Entity a, IEnumerable<Entity> b)
        {
            foreach (Entity e in b)
            {
                if (Check(a, e) == true)
                {
                    return e;
                }
            }
            return null;
        }

        static public Entity First(Entity a, IEnumerable<Entity> b, Vector2 at)
        {
            Vector2 old = a.Position;
            a.Position = at;
            Entity ret = First(a, b);
            a.Position = old;
            return ret;
        }

        static public List<Entity> All(Entity a, IEnumerable<Entity> b, List<Entity> into)
        {
            foreach (Entity e in b)
            {
                if (Check(a, e) == true)
                {
                    into.Add(e);
                }
            }
            return into;
        }

        static public List<Entity> All(Entity a, IEnumerable<Entity> b, List<Entity> into, Vector2 at)
        {
            Vector2 old = a.Position;
            a.Position = at;
            List<Entity> ret = All(a, b, into);
            a.Position = old;
            return ret;
        }

        static public List<Entity> All(Entity a, IEnumerable<Entity> b)
        {
            return All(a, b, new List<Entity>());
        }

        static public List<Entity> All(Entity a, IEnumerable<Entity> b, Vector2 at)
        {
            return All(a, b, new List<Entity>(), at);
        }

        static public bool CheckPoint(Entity a, Vector2 point)
        {
            if (a.Collider == null)
            {
                return false;
            }
            else
            {
                return a.Collider.Collide(point);
            }
        }

        static public bool CheckPoint(Entity a, Vector2 point, Vector2 at)
        {
            Vector2 old = a.Position;
            a.Position = at;
            bool ret = CheckPoint(a, point);
            a.Position = old;
            return ret;
        }

        static public bool CheckLine(Entity a, Vector2 from, Vector2 to)
        {
            if (a.Collider == null)
            {
                return false;
            }
            else
            {
                return a.Collider.Collide(from, to);
            }
        }

        static public bool CheckLine(Entity a, Vector2 from, Vector2 to, Vector2 at)
        {
            Vector2 old = a.Position;
            a.Position = at;
            bool ret = CheckLine(a, from, to);
            a.Position = old;
            return ret;
        }

        static public bool CheckRect(Entity a, Rectangle rect)
        {
            if (a.Collider == null)
            {
                return false;
            }
            else
            {
                return a.Collider.Collide(rect);
            }
        }

        static public bool CheckRect(Entity a, Rectangle rect, Vector2 at)
        {
            Vector2 old = a.Position;
            a.Position = at;
            bool ret = CheckRect(a, rect);
            a.Position = old;
            return ret;
        }

        static bool LineCheck(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
        {
            Vector2 b = a2 - a1;
            Vector2 d = b2 - b1;
            float bDotPerp = b.X * d.Y - b.Y * d.X;

            if (bDotPerp == 0)
            {
                return false;
            }

            Vector2 c = b1 - a1;
            float t = (c.X * d.Y - c.Y * d.X) / bDotPerp;
            if (t < 0 || t > 1)
            {
                return false;
            }

            float u = (c.X * b.Y - c.Y * b.X) / bDotPerp;
            if (u < 0 || u > 1)
            {
                return false;
            }

            return true;
        }

        static bool LineCheck(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, out Vector2 intersection)
        {
            intersection = Vector2.Zero;

            Vector2 b = a2 - a1;
            Vector2 d = b2 - b1;
            float bDotPerp = b.X * d.Y - b.Y * d.X;

            if (bDotPerp == 0)
            {
                return false;
            }

            Vector2 c = b1 - a1;
            float t = (c.X * d.Y - c.Y * d.X) / bDotPerp;
            if (t < 0 || t > 1)
            {
                return false;
            }

            float u = (c.X * b.Y - c.Y * b.X) / bDotPerp;
            if (u < 0 || u > 1)
            {
                return false;
            }

            intersection = a1 + t * b;

            return true;
        }

        static public bool CircleToLine(Vector2 cPosition, float cRadius, Vector2 lineFrom, Vector2 lineTo)
        {
            return Vector2.DistanceSquared(cPosition, ClosestPointOnLine(lineFrom, lineTo, cPosition)) < cRadius * cRadius;
        }

        static public bool CircleToPoint(Vector2 cPosition, float cRadius, Vector2 point)
        {
            return Vector2.DistanceSquared(cPosition, point) < cRadius * cRadius;
        }

        static public bool RectToCircle(float rX, float rY, float rW, float rH, Vector2 cPosition, float cRadius)
        {
            if (CircleToPoint(cPosition, cRadius, new Vector2(rX + rW / 2, rY + rH / 2)) == true)
            {
                return true;
            }

            Vector2 edgeFrom;
            Vector2 edgeTo;
            PointSectors sector = GetSector(rX, rY, rW, rH, cPosition);

            if ((sector & PointSectors.Top) != 0)
            {
                edgeFrom = new Vector2(rX, rY);
                edgeTo = new Vector2(rX + rW, rY);
                if (CircleToLine(cPosition, cRadius, edgeFrom, edgeTo) == true)
                {
                    return true;
                }
            }

            if ((sector & PointSectors.Bottom) != 0)
            {
                edgeFrom = new Vector2(rX, rY + rH);
                edgeTo = new Vector2(rX + rW, rY + rH);
                if (CircleToLine(cPosition, cRadius, edgeFrom, edgeTo) == true)
                {
                    return true;
                }
            }

            if ((sector & PointSectors.Left) != 0)
            {
                edgeFrom = new Vector2(rX, rY);
                edgeTo = new Vector2(rX, rY + rH);
                if (CircleToLine(cPosition, cRadius, edgeFrom, edgeTo) == true)
                {
                    return true;
                }
            }

            if ((sector & PointSectors.Right) != 0)
            {
                edgeFrom = new Vector2(rX + rW, rY);
                edgeTo = new Vector2(rX + rW, rY + rH);
                if (CircleToLine(cPosition, cRadius, edgeFrom, edgeTo) == true)
                {
                    return true;
                }
            }
            return false;
        }

        static public bool RectToCircle(Rectangle rect, Vector2 cPosition, float cRadius)
        {
            return RectToCircle(rect.X, rect.Y, rect.Width, rect.Height, cPosition, cRadius);
        }

        static public bool RectToLine(float rX, float rY, float rW, float rH, Vector2 lineFrom, Vector2 lineTo)
        {
            PointSectors fromSector = GetSector(rX, rY, rW, rH, lineFrom);
            PointSectors toSector = GetSector(rX, rY, rW, rH, lineTo);

            if (fromSector == PointSectors.Centre || toSector == PointSectors.Centre)
            {
                return true;
            }
            else if ((fromSector & toSector) != 0)
            {
                return false;
            }
            else
            {
                PointSectors both = fromSector | toSector;

                Vector2 edgeFrom;
                Vector2 edgeTo;

                if ((both & PointSectors.Top) != 0)
                {
                    edgeFrom = new Vector2(rX, rY);
                    edgeTo = new Vector2(rX + rW, rY);
                    if (LineCheck(edgeFrom, edgeTo, lineFrom, lineTo) == true)
                    {
                        return true;
                    }
                }

                if ((both & PointSectors.Bottom) != 0)
                {
                    edgeFrom = new Vector2(rX, rY + rH);
                    edgeTo = new Vector2(rX + rW, rY + rH);
                    if (LineCheck(edgeFrom, edgeTo, lineFrom, lineTo) == true)
                    {
                        return true;
                    }
                }

                if ((both & PointSectors.Left) != 0)
                {
                    edgeFrom = new Vector2(rX, rY);
                    edgeTo = new Vector2(rX, rY + rH);
                    if (LineCheck(edgeFrom, edgeTo, lineFrom, lineTo) == true)
                    {
                        return true;
                    }
                }

                if ((both & PointSectors.Right) != 0)
                {
                    edgeFrom = new Vector2(rX + rW, rY);
                    edgeTo = new Vector2(rX + rW, rY + rH);
                    if (LineCheck(edgeFrom, edgeTo, lineFrom, lineTo) == true)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        static public bool RectToLine(Rectangle rect, Vector2 lineFrom, Vector2 lineTo)
        {
            return RectToLine(rect.X, rect.Y, rect.Width, rect.Height, lineFrom, lineTo);
        }

        static public bool RectToPoint(float rX, float rY, float rW, float rH, Vector2 point)
        {
            return point.X >= rX && point.Y >= rY && point.X < rX + rW && point.Y < rY + rH;
        }

        static public bool RectToPoint(Rectangle rect, Vector2 point)
        {
            return RectToPoint(rect.X, rect.Y, rect.Width, rect.Height, point);
        }

        static public Vector2 ClosestPointOnLine(Vector2 lineA, Vector2 lineB, Vector2 closestTo)
        {
            Vector2 v = lineB - lineA;
            Vector2 w = closestTo - lineA;
            float t = Vector2.Dot(w, v) / Vector2.Dot(v, v);
            t = MathHelper.Clamp(t, 0, 1);

            return lineA + v * t;
        }

        static PointSectors GetSector(Rectangle rect, Vector2 point)
        {
            PointSectors sector = PointSectors.Centre;

            if (point.X < rect.Left)
            {
                sector |= PointSectors.Left;
            }
            else if (point.X >= rect.Right)
            {
                sector |= PointSectors.Right;
            }

            if (point.Y < rect.Top)
            {
                sector |= PointSectors.Top;
            }
            else if (point.Y >= rect.Bottom)
            {
                sector |= PointSectors.Bottom;
            }

            return sector;
        }

        static public PointSectors GetSector(float rX, float rY, float rW, float rH, Vector2 point)
        {
            PointSectors sector = PointSectors.Centre;

            if (point.X < rX)
            {
                sector |= PointSectors.Left;
            }
            else if (point.X >= rX + rW)
            {
                sector |= PointSectors.Right;
            }

            if (point.Y < rY)
            {
                sector |= PointSectors.Top;
            }
            else if (point.Y >= rY + rH)
            {
                sector |= PointSectors.Bottom;
            }

            return sector;
        }
    }
}
