using System;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Xna.Framework;

using PolyOne.Components;
using PolyOne.Utility;
using PolyOne.Collision;
using PolyOne.Scenes;

namespace PolyOne
{
    public class Entity : IEnumerable<Component>, IEnumerable
    {
        public Vector2 Position;

        public Scene Scene { get; private set; }

        public bool Active { get; set; } = true;

        public bool Visible { get; set; }

        public ComponentList Components { get; set; }

        private Collider collider;
        internal int depth = 0;
        internal double actualDepth = 0;

        public bool Collidable { get; set; } = true;

        public List<int> Tags { get; private set; }

        public Entity(Vector2 position)
        {
            Position = position;

            Components = new ComponentList(this);
            Tags = new List<int>();
        }

        public Entity() : this(Vector2.Zero) { }

        public virtual void Added(Scene Scene)
        {
            this.Scene = Scene;

            if (Components != null)
            {
                foreach (Component c in Components)
                {
                    c.EntityAdded();
                }
            }
        }

        public virtual void Removed(Scene Scene)
        {
            if (Components != null)
            {
                foreach (Component c in Components)
                {
                    c.EntityRemoved();
                }
                Scene = null;
            }
        }

        public virtual void Update()
        {
            Components.Update();
        }

        public virtual void Draw()
        {
            Components.Draw();
        }

        public void RemoveSelf()
        {
            if (Scene != null)
            {
                Scene.Entities.Remove(this);
            }
        }

        public Collider Collider
        {
            get { return collider; }

            set
            {
                if (value == collider)
                {
                    return;
                }

#if DEBUG
                if (value.Entity != null)
                {
                    throw new Exception("Setting an Entity's Collider to a Collider already in use by another Entity");
                }
#endif
                if (collider != null)
                {
                    collider.Removed();
                }
                collider = value;
                if (collider != null)
                {
                    collider.Added(this);
                }
            }
        }

        public float X
        {
            get { return Position.X; }
            set { Position.X = value; }
        }

        public float Y
        {
            get { return Position.Y; }
            set { Position.Y = value; }
        }

        public float Width
        {
            get
            {
                if (Collider == null)
                {
                    return 0;
                }
                else
                {
                    return Collider.Width;
                }
            }
        }

        public float Height
        {
            get
            {
                if (Collider == null)
                {
                    return 0;
                }
                else
                {
                    return Collider.Height;
                }
            }
        }

        public float Left
        {
            get
            {
                if (Collider == null)
                {
                    return X;
                }
                else
                {
                    return Position.X + Collider.Left;
                }
            }

            set
            {
                if (Collider == null)
                {
                    Position.X = value;
                }
                else
                {
                    Position.X = value - Collider.Left;
                }
            }
        }

        public float Right
        {
            get
            {
                if (Collider == null)
                {
                    return Position.X;
                }
                else
                {
                    return Position.X + Collider.Right;
                }
            }

            set
            {
                if (Collider == null)
                {
                    Position.X = value;
                }
                else
                {
                    Position.X = value - Collider.Right;
                }
            }
        }

        public float Top
        {
            get
            {
                if (Collider == null)
                {
                    return Position.Y;
                }
                else
                {
                    return Position.Y + Collider.Top;
                }
            }

            set
            {
                if (Collider == null)
                {
                    Position.Y = value;
                }
                else
                {
                    Position.Y = value - Collider.Top;
                }
            }
        }

        public float Bottom
        {
            get
            {
                if (Collider == null)
                {
                    return Position.Y;
                }
                else
                {
                    return Position.Y + Collider.Bottom;
                }
            }

            set
            {
                if (Collider == null)
                {
                    Position.Y = value;
                }
                else
                {
                    Position.Y = value - Collider.Bottom;
                }
            }
        }

        public float CentreX
        {
            get
            {
                if (Collider == null)
                {
                    return Position.X;
                }
                else
                {
                    return Position.X + Collider.Left + Collider.Width / 2.0f;
                }
            }

            set
            {
                if (Collider == null)
                {
                    Position.X = value;
                }
                else
                {
                    Position.X = value - (Collider.Left + Collider.Width / 2.0f);
                }
            }
        }

        public float CentreY
        {
            get
            {
                if (Collider == null)
                {
                    return Position.Y;
                }
                else
                {
                    return Position.Y + Collider.Top + Collider.Height / 2.0f;
                }
            }

            set
            {
                if (Collider == null)
                {
                    Position.Y = value;
                }
                else
                {
                    Position.Y = value - (Collider.Top + Collider.Height / 2.0f);
                }
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

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)Width, (int)Height);
            }
        }

        public void Tag(int tag)
        {
            if (Tags.Contains(tag) == false)
            {
                Tags.Add(tag);
                if (Scene != null)
                {
                    Scene.TagLists[tag].Add(this);
                }
            }
        }

        public void UnTag(int tag)
        {
            if (Tags.Contains(tag) == true)
            {
                Tags.Add(tag);
                if (Scene != null)
                {
                    Scene.TagLists[tag].Remove(this);
                }
            }
        }

        public bool CollideCheck(Entity other)
        {
            return Collide.Check(this, other);
        }

        public bool CollideCheck(Entity other, Vector2 at)
        {
            return Collide.Check(this, other, at);
        }

        public bool CollideCheck(int tag)
        {
#if DEBUG
            if (Scene == null)
            {
                throw new Exception("Can't collide check an Entity against a tag list when it is not a member of Scene");
            }
#endif
            return Collide.Check(this, Scene[tag]);
        }

        public bool CollideCheck(int tag, Vector2 at)
        {
#if DEBUG
            if (Scene == null)
            {
                throw new Exception("Can't collide check an Entity against a tag list when it is not a member of Scene");
            }
#endif
            return Collide.Check(this, Scene[tag], at);
        }

        public bool CollideCheck<T>() where T : Entity
        {
#if DEBUG
            if (Scene == null)
            {
                throw new Exception("Can't collide check an Entity against tracked Entites when it is not a member of Scene");
            }
            else if (Scene.Tracker.Entities.ContainsKey(typeof(T)) == false)
            {
                throw new Exception("Can't collide check an Entity against an untracked Entity type");
            }
#endif
            return Collide.Check(this, Scene.Tracker.Entities[typeof(T)]);
        }

        public bool CollideCheckByComponent<T>() where T : Component
        {
#if DEBUG 
            if (Scene == null)
            {
                throw new Exception("Can't collide check an Entity against tracked Entites when it is not a member of Scene");
            }
            else if (Scene.Tracker.Entities.ContainsKey(typeof(T)) == false)
            {
                throw new Exception("Can't collide check an Entity against an untracked Entity type");
            }
#endif
            foreach (Component c in Scene.Tracker.Components[typeof(T)])
            {
                if (Collide.Check(this, c.Entity) == true)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CollideCheckByComponent<T>(Vector2 at) where T : Component
        {
            Vector2 old = Position;
            Position = at;
            bool ret = CollideCheckByComponent<T>();
            Position = old;
            return ret;
        }

        public bool CollideCheckOutSide(Entity other, Vector2 at)
        {
            return !Collide.Check(this, other) && Collide.Check(this, other, at);
        }

        public bool CollideCheckOutside(int tag, Vector2 at)
        {
#if DEBUG 
            if (Scene == null)
            {
                throw new Exception("Can't collide check an Entity against tracked Entites when it is not a member of Scene");
            }
#endif
            foreach (Entity entity in Scene[tag])
            {
                if (Collide.Check(this, entity) && Collide.Check(this, entity, at) == false)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CollideCheckOutside<T>(Vector2 at) where T : Entity
        {
#if DEBUG
            if (Scene == null)
            {
                throw new Exception("Can't collide check an Entity against tracked Entites when it is not a member of Scene");
            }
            else if (Scene.Tracker.Entities.ContainsKey(typeof(T)) == false)
            {
                throw new Exception("Can't collide check an Entity against an untracked Entity type");
            }
#endif
            foreach (Entity entity in Scene.Tracker.Entities[typeof(T)])
            {
                if (Collide.Check(this, entity) == false && Collide.Check(this, entity, at) == true)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CollideCheckOutsideByComponent<T>(Vector2 at) where T : Component
        {
#if DEBUG
            if (Scene == null)
            {
                throw new Exception("Can't collide check an Entity against tracked Entites when it is not a member of Scene");
            }
            else if (Scene.Tracker.Entities.ContainsKey(typeof(T)) == false)
            {
                throw new Exception("Can't collide check an Entity against an untracked Entity type");
            }
#endif

            foreach (Component component in Scene.Tracker.Components[typeof(T)])
            {
                if (Collide.Check(this, component.Entity) == false && Collide.Check(this, component.Entity, at) == true)
                {
                    return true;
                }
            }
            return false;
        }

        public Entity CollideFirst(int tag)
        {
#if DEBUG
            if (Scene == null)
            {
                throw new Exception("Can't collide check an Entity against tracked Entites when it is not a member of Scene");
            }
#endif
            return Collide.First(this, Scene[tag]);
        }

        public Entity CollideFirst(int tag, Vector2 at)
        {
#if DEBUG
            if (Scene == null)
            {
                throw new Exception("Can't collide check an Entity against tracked Entites when it is not a member of Scene");
            }
#endif
            return Collide.First(this, Scene[tag], at);
        }

        public T CollideFirst<T>() where T : Entity
        {
#if DEBUG
            if (Scene == null)
            {
                throw new Exception("Can't collide check an Entity against tracked Entites when it is not a member of Scene");
            }
            else if (Scene.Tracker.Entities.ContainsKey(typeof(T)) == false)
            {
                throw new Exception("Can't collide check an Entity against an untracked Entity type");
            }
#endif
            return Collide.First(this, Scene.Tracker.Entities[typeof(T)]) as T;
        }

        public T CollideFirst<T>(Vector2 at) where T : Entity
        {
#if DEBUG
            if (Scene == null)
            {
                throw new Exception("Can't collide check an Entity against tracked Entites when it is not a member of Scene");
            }
            else if (Scene.Tracker.Entities.ContainsKey(typeof(T)) == false)
            {
                throw new Exception("Can't collide check an Entity against an untracked Entity type");
            }
#endif
            return Collide.First(this, Scene.Tracker.Entities[typeof(T)], at) as T;
        }

        public T CollideFirstByComponent<T>() where T : Component
        {
#if DEBUG
            if (Scene == null)
            {
                throw new Exception("Can't collide check an Entity against tracked Entites when it is not a member of Scene");
            }
            else if (Scene.Tracker.Entities.ContainsKey(typeof(T)) == false)
            {
                throw new Exception("Can't collide check an Entity against an untracked Entity type");
            }
#endif

            foreach (Component component in Scene.Tracker.Components[typeof(T)])
            {
                if (Collide.Check(this, component.Entity) == true)
                {
                    return component as T;
                }
            }
            return null;
        }

        public T CollideFirstByComponent<T>(Vector2 at) where T : Component
        {
#if DEBUG
            if (Scene == null)
            {
                throw new Exception("Can't collide check an Entity against tracked Entites when it is not a member of Scene");
            }
            else if (Scene.Tracker.Entities.ContainsKey(typeof(T)) == false)
            {
                throw new Exception("Can't collide check an Entity against an untracked Entity type");
            }
#endif
            foreach (Component component in Scene.Tracker.Components[typeof(T)])
            {
                if (Collide.Check(this, component.Entity, at) == true)
                {
                    return component as T;
                }
            }
            return null;
        }

        public Entity CollideFirstOutside(int tag, Vector2 at)
        {
#if DEBUG
            if (Scene == null)
            {
                throw new Exception("Can't collide check an Entity against tracked Entites when it is not a member of Scene");
            }
#endif
            foreach (Entity entity in Scene[tag])
            {
                if (Collide.Check(this, entity) == false && Collide.Check(this, entity, at) == true)
                {
                    return entity;
                }
            }
            return null;
        }

        public T CollideFirstOutside<T>(Vector2 at) where T : Entity
        {
#if DEBUG
            if (Scene == null)
            {
                throw new Exception("Can't collide check an Entity against tracked Entites when it is not a member of Scene");
            }
            else if (Scene.Tracker.Entities.ContainsKey(typeof(T)) == false)
            {
                throw new Exception("Can't collide check an Entity against an untracked Entity type");
            }
#endif
            foreach (Entity entity in Scene.Tracker.Entities[typeof(T)])
            {
                if (Collide.Check(this, entity) == false && Collide.Check(this, entity, at) == true)
                {
                    return entity as T;
                }
            }
            return null;
        }

        public T CollideFirstOutsideByComponent<T>(Vector2 at) where T : Component
        {
#if DEBUG
            if (Scene == null)
            {
                throw new Exception("Can't collide check an Entity against tracked Entites when it is not a member of Scene");
            }
            else if (Scene.Tracker.Entities.ContainsKey(typeof(T)) == false)
            {
                throw new Exception("Can't collide check an Entity against an untracked Entity type");
            }
#endif
            foreach (Component component in Scene.Tracker.Components[typeof(T)])
            {
                if (Collide.Check(this, component.Entity) == false && Collide.Check(this, component.Entity, at) == true)
                {
                    return component as T;
                }
            }
            return null;
        }

        public List<Entity> CollideAll(int tag)
        {
#if DEBUG 
            if (Scene == null)
            {
                throw new Exception("Can't collide check an Entity against tracked Entites when it is not a member of Scene");
            }
#endif
            return Collide.All(this, Scene[tag]);
        }

        public List<Entity> CollideAll(int tag, Vector2 at)
        {
#if DEBUG
            if (Scene == null)
            {
                throw new Exception("Can't collide check an Entity against tracked Entites when it is not a member of Scene");
            }
#endif
            return Collide.All(this, Scene[tag], at);
        }

        public List<T> CollideAll<T>() where T : Entity
        {
#if DEBUG
            if (Scene == null)
            {
                throw new Exception("Can't collide check an Entity against tracked Entites when it is not a member of Scene");
            }
            else if (Scene.Tracker.Entities.ContainsKey(typeof(T)) == false)
            {
                throw new Exception("Can't collide check an Entity against an untracked Entity type");
            }
#endif
            return Collide.All(this, Scene.Tracker.Entities[typeof(T)]) as List<T>;
        }

        public List<T> CollideAll<T>(Vector2 at) where T : Entity
        {
#if DEBUG
            if (Scene == null)
            {
                throw new Exception("Can't collide check an Entity against tracked Entites when it is not a member of Scene");
            }
            else if (Scene.Tracker.Entities.ContainsKey(typeof(T)) == false)
            {
                throw new Exception("Can't collide check an Entity against an untracked Entity type");
            }
#endif
            return Collide.All(this, Scene.Tracker.Entities[typeof(T)], at) as List<T>;
        }

        public List<T> CollideAllByComponent<T>() where T : Component
        {
#if DEBUG
            if (Scene == null)
            {
                throw new Exception("Can't collide check an Entity against tracked Entites when it is not a member of Scene");
            }
            else if (Scene.Tracker.Entities.ContainsKey(typeof(T)) == false)
            {
                throw new Exception("Can't collide check an Entity against an untracked Entity type");
            }
#endif
            List<T> list = new List<T>();
            foreach (Component component in Scene.Tracker.Components[typeof(T)])
            {
                if (Collide.Check(this, component.Entity) == true)
                {
                    list.Add(component as T);
                }
            }
            return list;
        }

        public List<T> CollideAllByComponent<T>(Vector2 at) where T : Component
        {
            Vector2 old = Position;
            Position = at;
            var ret = CollideAllByComponent<T>();
            Position = old;
            return ret;
        }

        public bool CollideDo(int tag, Action<Entity> action)
        {
#if DEBUG
            if (Scene == null)
            {
                throw new Exception("Can't collide check an Entity against tracked Entites when it is not a member of Scene");
            }
#endif
            bool hit = false;
            foreach (var other in Scene[tag])
            {
                if (CollideCheck(other) == true)
                {
                    action(other);
                    hit = true;
                }
            }
            return hit;
        }

        public bool CollideDo(int tag, Action<Entity> action, Vector2 at)
        {
#if DEBUG
            if (Scene == null)
            {
                throw new Exception("Can't collide check an Entity against tracked Entites when it is not a member of Scene");
            }
#endif
            bool hit = false;
            var was = Position;
            Position = at;

            foreach (var other in Scene[tag])
            {
                if (CollideCheck(other) == true)
                {
                    action(other);
                    hit = true;
                }
            }
            Position = was;
            return hit;
        }

        public bool CollideDo<T>(Action<T> action) where T : Entity
        {
#if DEBUG
            if (Scene == null)
            {
                throw new Exception("Can't collide check an Entity against tracked Entites when it is not a member of Scene");
            }
            else if (Scene.Tracker.Entities.ContainsKey(typeof(T)) == false)
            {
                throw new Exception("Can't collide check an Entity against an untracked Entity type");
            }
#endif

            bool hit = false;
            foreach (var other in Scene.Tracker.Entities[typeof(T)])
            {
                if (CollideCheck(other) == true)
                {
                    action(other as T);
                    hit = true;
                }
            }
            return hit;
        }

        public bool CollideDo<T>(Action<T> action, Vector2 at) where T : Entity
        {
#if DEBUG
            if (Scene == null)
            {
                throw new Exception("Can't collide check an Entity against tracked Entites when it is not a member of Scene");
            }
            else if (Scene.Tracker.Entities.ContainsKey(typeof(T)) == false)
            {
                throw new Exception("Can't collide check an Entity against an untracked Entity type");
            }
#endif

            bool hit = false;
            var was = Position;
            Position = at;

            foreach (var other in Scene.Tracker.Entities[typeof(T)])
            {
                if (CollideCheck(other) == true)
                {
                    action(other as T);
                    hit = true;
                }
            }
            Position = was;
            return hit;
        }

        public bool CollideDoByComponent<T>(Action<T> action) where T : Component
        {
#if DEBUG
            if (Scene == null)
            {
                throw new Exception("Can't collide check an Entity against tracked Entites when it is not a member of Scene");
            }
            else if (Scene.Tracker.Entities.ContainsKey(typeof(T)) == false)
            {
                throw new Exception("Can't collide check an Entity against an untracked Entity type");
            }
#endif

            bool hit = false;
            foreach (Component component in Scene.Tracker.Components[typeof(T)])
            {
                if (CollideCheck(component.Entity) == true)
                {
                    action(component as T);
                    hit = true;
                }
            }
            return hit;
        }

        public bool CollideDoByComponent<T>(Action<T> action, Vector2 at) where T : Component
        {
#if DEBUG
            if (Scene == null)
            {
                throw new Exception("Can't collide check an Entity against tracked Entites when it is not a member of Scene");
            }
            else if (Scene.Tracker.Entities.ContainsKey(typeof(T)) == false)
            {
                throw new Exception("Can't collide check an Entity against an untracked Entity type");
            }
#endif

            bool hit = false;
            var was = Position;
            Position = at;

            foreach (Component component in Scene.Tracker.Components[typeof(T)])
            {
                if (CollideCheck(component.Entity) == true)
                {
                    action(component as T);
                    hit = true;
                }
            }
            Position = was;
            return hit;
        }

        public bool CollidePoint(Vector2 point)
        {
            return Collide.CheckPoint(this, point);
        }

        public bool CollidePoint(Vector2 point, Vector2 at)
        {
            return Collide.CheckPoint(this, point, at);
        }

        public bool CollideLine(Vector2 from, Vector2 to)
        {
            return Collide.CheckLine(this, from, to);
        }

        public bool CollideLine(Vector2 from, Vector2 to, Vector2 at)
        {
            return Collide.CheckLine(this, from, to, at);
        }

        public bool CollideRect(Rectangle rect)
        {
            return Collide.CheckRect(this, rect);
        }

        public bool CollideRect(Rectangle rect, Vector2 at)
        {
            return Collide.CheckRect(this, rect, at);
        }

        public void Add(Component component)
        {
            Components.Add(component);
        }

        public void Remove(Component component)
        {
            Components.Remove(component);
        }

        public void Add(params Component[] components)
        {
            Components.Add(components);
        }

        public void Remove(params Component[] components)
        {
            Components.Remove(components);
        }

        public IEnumerator<Component> GetEnumerator()
        {
            return Components.GetEnumerator();
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Entity Closest(params Entity[] entities)
        {
            Entity closest = entities[0];
            float dist = Vector2.DistanceSquared(Position, closest.Position);

            for (int i = 1; i < entities.Length; i++)
            {
                float current = Vector2.DistanceSquared(Position, entities[i].Position);
                if (current < dist)
                {
                    closest = entities[i];
                    dist = current;
                }
            }
            return closest;
        }

        public Entity Closest(int tag)
        {
            var list = Scene[tag];
            Entity closest = null;
            float dist;

            if (list.Count >= 1)
            {
                closest = list[0];
                dist = Vector2.DistanceSquared(Position, closest.Position);

                for (int i = 1; i < list.Count; i++)
                {
                    float current = Vector2.DistanceSquared(Position, list[i].Position);
                    if (current < dist)
                    {
                        closest = list[i];
                        dist = current;
                    }
                }
            }
            return closest;
        }
    }
}
