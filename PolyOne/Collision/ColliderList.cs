using System;
using System.Linq;

using Microsoft.Xna.Framework;

namespace PolyOne.Collision
{
    public class ColliderList : Collider
    {
        public Collider[] colliders { get; private set; }

        public ColliderList(params Collider[] colliders)
        {
#if DEBUG
            foreach (Collider c in colliders)
                if (c == null)
                    throw new Exception("Cannot add a null Collider to a ColliderList.");
#endif
            this.colliders = colliders;
        }

        public void Add(params Collider[] toAdd)
        {
#if DEBUG
            foreach (Collider c in toAdd)
            {
                if (colliders.Contains(c))
                    throw new Exception("Adding a Collider to a ColliderList that already contains it!");
                else if (c == null)
                    throw new Exception("Cannot add a null Collider to a ColliderList");
            }

#endif

            Collider[] newColliders = new Collider[colliders.Length + toAdd.Length];
            for (int i = 0; i < colliders.Length; i++)
            {
                newColliders[i] = colliders[i];
            }

            for (int i = 0; i < toAdd.Length; i++)
            {
                newColliders[i + colliders.Length] = toAdd[i];
                toAdd[i].Added(Entity);
            }
            colliders = newColliders;
        }

        public void Remove(params Collider[] toRemove)
        {
#if DEBUG
            foreach (Collider c in toRemove)
            {
                if (colliders.Contains(c) == false)
                    throw new Exception("Removing a Collider from a ColliderList that does not containt it!");
                else if (c == null)
                    throw new Exception("Cannot remove a null Collider from a ColliderList");
            }
#endif
            Collider[] newColliders = new Collider[colliders.Length - toRemove.Length];
            int at = 0;

            foreach (Collider c in colliders)
            {
                if (toRemove.Contains(c) == false)
                {
                    newColliders[at] = c;
                    at++;
                }
            }
            colliders = newColliders;
        }

        internal override void Added(Entity entity)
        {
            base.Added(entity);
            foreach (Collider c in colliders)
            {
                c.Added(entity);
            }
        }

        internal override void Removed()
        {
            base.Removed();
            foreach (Collider c in colliders)
            {
                c.Removed();
            }
        }

        public override float Width
        {
            get
            {
                return Right - Left;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override float Height
        {
            get
            {
                return Bottom - Top;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override float Left
        {
            get
            {
                float left = colliders[0].Left;
                for (int i = 1; i < colliders.Length; i++)
                {
                    if (colliders[i].Left < left)
                        left = colliders[i].Left;
                }
                return left;
            }
            set
            {
                float changeX = value - Left;
                foreach (Collider c in colliders)
                {
                    Position.X = changeX;
                }
            }
        }

        public override float Right
        {
            get
            {
                float right = colliders[0].Right;
                for (int i = 1; i < colliders.Length; i++)
                {
                    if (colliders[i].Right > right)
                        right = colliders[i].Right;
                }
                return right;
            }
            set
            {
                float changeX = value - Right;
                foreach (Collider item in colliders)
                {
                    Position.X += changeX;
                }
            }
        }

        public override float Top
        {
            get
            {
                float top = colliders[0].Top;
                for (int i = 1; i < colliders.Length; i++)
                {
                    if (colliders[i].Top < top)
                        top = colliders[i].Top;
                }
                return top;
            }
            set
            {
                float changeY = value - Top;
                foreach (Collider c in colliders)
                {
                    Position.Y += changeY;
                }
            }
        }

        public override float Bottom
        {
            get
            {
                float bottom = colliders[0].Bottom;
                for (int i = 1; i < colliders.Length; i++)
                {
                    if (colliders[i].Bottom > bottom)
                        bottom = colliders[i].Bottom;
                }
                return bottom;
            }
            set
            {
                float changeY = value - Bottom;
                foreach (Collider c in colliders)
                {
                    Position.Y += changeY;
                }
            }
        }

        public override Collider Clone()
        {
            Collider[] clones = new Collider[colliders.Length];
            for (int i = 0; i < colliders.Length; i++)
            {
                clones[i] = colliders[i].Clone();
            }

            return new ColliderList(clones);
        }

        public override bool Collide(Vector2 point)
        {
            foreach (Collider c in colliders)
            {
                if (c.Collide(point) == true)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool Collide(Rectangle rect)
        {
            foreach (Collider c in colliders)
            {
                if (c.Collide(rect) == true)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool Collide(Vector2 from, Vector2 to)
        {
            foreach (Collider c in colliders)
            {
                if (c.Collide(from, to) == true)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool Collide(Hitbox hitbox)
        {
            foreach (Collider c in colliders)
            {
                if (c.Collide(hitbox) == true)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool Collide(Grid grid)
        {
            foreach (Collider c in colliders)
            {
                if (c.Collide(grid) == true)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool Collide(Circle circle)
        {
            foreach (Collider c in colliders)
            {
                if (c.Collide(circle) == true)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool Collide(ColliderList list)
        {
            foreach (Collider c in colliders)
            {
                if (c.Collide(list) == true)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
