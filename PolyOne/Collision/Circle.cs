﻿using Microsoft.Xna.Framework;

namespace PolyOne.Collision
{
    public class Circle : Collider
    {
        public float Radius;

        public Circle(float radius, float x = 0, float y = 0)
        {
            Radius = radius;
            Position.X = x;
            Position.Y = y;
        }

        public override float Width
        {
            get { return Radius * 2; }
            set { Radius = value / 2; }
        }

        public override float Height
        {
            get { return Radius * 2; }
            set { Radius = value / 2; }
        }

        public override float Left
        {
            get { return Position.X - Radius; }
            set { Position.X = value + Radius; }
        }

        public override float Top
        {
            get { return Position.Y - Radius; }
            set { Position.Y = value + Radius; }
        }

        public override float Right
        {
            get { return Position.X + Radius; }
            set { Position.X = value - Radius; }
        }

        public override float Bottom
        {
            get { return Position.Y + Radius; }
            set { Position.Y = value - Radius; }
        }

        public override Collider Clone()
        {
            return new Circle(Radius, Position.X, Position.Y);
        }

        public override bool Collide(Vector2 point)
        {
            return Collision.Collide.CircleToPoint(AbsolutePostion, Radius, point);
        }

        public override bool Collide(Rectangle rect)
        {
            return Collision.Collide.RectToCircle(rect, AbsolutePostion, Radius);
        }

        public override bool Collide(Vector2 from, Vector2 to)
        {
            return Collision.Collide.CircleToLine(AbsolutePostion, Radius, from, to);
        }

        public override bool Collide(Circle circle)
        {
            return Vector2.DistanceSquared(AbsolutePostion, circle.AbsolutePostion) < (Radius + circle.Radius) * (Radius + circle.Radius);
        }

        public override bool Collide(Hitbox hitbox)
        {
            return hitbox.Collide(this);
        }

        public override bool Collide(Grid grid)
        {
            return grid.Collide(this);
        }

        public override bool Collide(ColliderList list)
        {
            return list.Collide(this);
        }
    }
}
