using Microsoft.Xna.Framework;
using System;

namespace PolyOne.Collision
{
    public abstract class Collider
    {
        public Entity Entity { get; private set; }
        public Vector2 Position;

        internal virtual void Added(Entity entity)
        {
            Entity = entity;
        }

        internal virtual void Removed()
        {
            Entity = null;
        }

        public bool Collide(Entity entity)
        {
            return Collide(entity.Collider);
        }

        public bool Collide(Collider collider)
        {
            if (collider is Hitbox)
            {
                return Collide(collider as Hitbox);
            }
            else if (collider is Grid)
            {
                return Collide(collider as Grid);
            }
            else if (collider is ColliderList)
            {
                return Collide(collider as ColliderList);
            }
            else if (collider is Circle)
            {
                return Collide(collider as Circle);
            }
            else
                throw new Exception("Collisions against the collider type are not implemented");

        }

        public abstract bool Collide(Vector2 point);
        public abstract bool Collide(Rectangle rect);
        public abstract bool Collide(Vector2 from, Vector2 to);
        public abstract bool Collide(Hitbox hitbox);
        public abstract bool Collide(Grid grid);
        public abstract bool Collide(Circle circle);
        public abstract bool Collide(ColliderList list);
        public abstract Collider Clone();
        public abstract float Width { get; set; }
        public abstract float Height { get; set; }
        public abstract float Top { get; set; }
        public abstract float Bottom { get; set; }
        public abstract float Left { get; set; }
        public abstract float Right { get; set; }

        public float CentreX
        {
            get
            {
                return Left + Left / 2.0f;
            }

            set
            {
                Left = value - Width / 2.0f;
            }
        }

        public float CentreY
        {
            get
            {
                return Top + Height / 2.0f;
            }

            set
            {
                Top = value - Height / 2.0f;
            }
        }

        public Vector2 TopLeft
        {
            get
            {
                return new Vector2(Left, Top);
            }

            set
            {
                Left = value.X;
                Top = value.Y;
            }
        }

        public Vector2 TopCentre
        {
            get
            {
                return new Vector2(CentreX, Top);
            }

            set
            {
                CentreX = value.X;
                Top = value.Y;
            }
        }

        public Vector2 TopRight
        {
            get
            {
                return new Vector2(Right, Top);
            }

            set
            {
                Right = value.X;
                Top = value.Y;
            }
        }

        public Vector2 CentreLeft
        {
            get
            {
                return new Vector2(Left, CentreY);
            }

            set
            {
                Left = value.X;
                CentreY = value.Y;
            }
        }

        public Vector2 Centre
        {
            get
            {
                return new Vector2(CentreX, CentreY);
            }

            set
            {
                CentreX = value.X;
                CentreY = value.Y;
            }
        }

        public Vector2 CentreRight
        {
            get
            {
                return new Vector2(Right, CentreY);
            }

            set
            {
                Right = value.X;
                CentreY = value.Y;
            }
        }

        public Vector2 BottomLeft
        {
            get
            {
                return new Vector2(Left, Bottom);
            }

            set
            {
                Left = value.X;
                Bottom = value.Y;
            }
        }

        public Vector2 BottomCentre
        {
            get
            {
                return new Vector2(CentreX, Bottom);
            }

            set
            {
                CentreX = value.X;
                Bottom = value.Y;
            }
        }

        public Vector2 BottomRight
        {
            get
            {
                return new Vector2(Right, Bottom);
            }

            set
            {
                Right = value.X;
                Bottom = value.Y;
            }
        }

        public Vector2 AbsolutePostion
        {
            get
            {
                if (Entity != null)
                    return Entity.Position + Position;
                else
                    return Position;
            }
        }

        public float AbsoluteY
        {
            get
            {
                if (Entity != null)
                    return Entity.Position.Y + Position.Y;
                else
                    return Position.Y;
            }
        }

        public float AbsoluteTop
        {
            get
            {
                if (Entity != null)
                    return Top + Entity.Position.Y;
                else
                    return Top;
            }
        }

        public float AbsoluteBottom
        {
            get
            {
                if (Entity != null)
                    return Bottom + Entity.Position.Y;
                else
                    return Bottom;
            }
        }

        public float AbsoluteLeft
        {
            get
            {
                if (Entity != null)
                    return Left + Entity.Position.X;
                else
                    return Left;
            }
        }

        public float AbsoluteRight
        {
            get
            {
                if (Entity != null)
                    return Right + Entity.Position.X;
                else
                    return Right;
            }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)AbsoluteLeft, (int)AbsoluteTop, (int)Width, (int)Height);
            }
        }
    }
}
