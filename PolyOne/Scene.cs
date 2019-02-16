using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using PolyOne.Utility;


namespace PolyOne.Scenes
{
    public class Scene : IEnumerable<Entity>, IEnumerable
    {
        public float TimeActive { get; private set; }

        public EntityList Entities { get; private set; }

        public TagList TagLists { get; private set; }

        public Entity HelperEntity { get; private set; }

        public Tracker Tracker { get; private set; }

        private Dictionary<int, double> actualDepthLookup;

        public virtual void LoadContent()
        {
            Tracker = new Tracker();
            Entities = new EntityList(this);
            TagLists = new TagList();

            actualDepthLookup = new Dictionary<int, double>();
            HelperEntity = new Entity();
            Entities.Add(HelperEntity);
        }

        public virtual void UnloadContent() { }

        private void BeforeUpdate()
        {
            TimeActive += Engine.Engine.DeltaTime;

            Entities.UpdateLists();
            TagLists.UpdateLists();
        }

        public virtual void Update()
        {
            BeforeUpdate();
            Entities.Update();
        }

        public virtual void Draw()
        {
            Entities.Draw();
        }

        public bool CollideCheck(Vector2 point, int tag)
        {
            var list = TagLists[(int)tag];

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Collidable == true && list[i].CollidePoint(point) == true)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CollideCheck(Vector2 from, Vector2 to, int tag)
        {
            var list = TagLists[(int)tag];

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Collidable == true && list[i].CollideLine(from, to) == true)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CollideCheck(Rectangle rect, int tag)
        {
            var list = TagLists[(int)tag];

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Collidable == true && list[i].CollideRect(rect) == true)
                {
                    return true;
                }
            }
            return false;
        }

        public Entity CollideFirst(Vector2 point, int tag)
        {
            var list = TagLists[(int)tag];

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Collidable == true && list[i].CollidePoint(point) == true)
                {
                    return list[i];
                }
            }
            return null;
        }

        public Entity CollideFirst(Vector2 from, Vector2 to, int tag)
        {
            var list = TagLists[(int)tag];

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Collidable == true && list[i].CollideLine(from, to) == true)
                {
                    return list[i];
                }
            }
            return null;
        }

        public Entity CollideFirst(Rectangle rect, int tag)
        {
            var list = TagLists[(int)tag];

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Collidable == true && list[i].CollideRect(rect) == true)
                {
                    return list[i];
                }
            }
            return null;
        }

        public void CollideInto(Vector2 point, int tag, List<Entity> hits)
        {
            var list = TagLists[(int)tag];

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Collidable == true && list[i].CollidePoint(point) == true)
                {
                    hits.Add(list[i]);
                }
            }
        }

        public void CollideInto(Vector2 from, Vector2 to, int tag, List<Entity> hits)
        {
            var list = TagLists[(int)tag];

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Collidable == true && list[i].CollideLine(from, to) == true)
                {
                    hits.Add(list[i]);
                }
            }
        }

        public void CollideInto(Rectangle rect, int tag, List<Entity> hits)
        {
            var list = TagLists[(int)tag];

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Collidable == true && list[i].CollideRect(rect) == true)
                {
                    list.Add(list[i]);
                }
            }
        }

        public List<Entity> CollideAll(Vector2 point, int tag)
        {
            List<Entity> list = new List<Entity>();
            CollideInto(point, tag, list);
            return list;
        }

        public List<Entity> CollideAll(Vector2 from, Vector2 to, int tag)
        {
            List<Entity> list = new List<Entity>();
            CollideInto(from, to, tag, list);
            return list;
        }

        public List<Entity> CollideAll(Rectangle rect, int tag)
        {
            List<Entity> list = new List<Entity>();
            CollideInto(rect, tag, list);
            return list;
        }

        public void CollideDo(Vector2 point, int tag, Action<Entity> action)
        {
            var list = TagLists[(int)tag];

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Collidable == true && list[i].CollidePoint(point) == true)
                {
                    action(list[i]);
                }
            }
        }

        public void CollideDo(Vector2 from, Vector2 to, int tag, Action<Entity> action)
        {
            var list = TagLists[(int)tag];

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Collidable == true && list[i].CollideLine(from, to) == true)
                {
                    action(list[i]);
                }
            }
        }

        public void CollideDo(Rectangle rect, int tag, Action<Entity> action)
        {
            var list = TagLists[(int)tag];

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Collidable == true && list[i].CollideRect(rect) == true)
                {
                    action(list[i]);
                }
            }
        }

        public Vector2 LineWalkCheck(Vector2 from, Vector2 to, int tag, float precision)
        {
            Vector2 add = to - from;
            add.Normalize();
            add *= precision;

            int amount = (int)Math.Floor((from - to).Length() / precision);
            Vector2 prev = from;
            Vector2 at = from + add;

            for (int i = 0; i <= amount; i++)
            {
                if (CollideCheck(at, tag) == true)
                {
                    return prev;
                }
                prev = at;
                at += add;
            }
            return to;
        }

        internal void SetActualDepth(Entity entity)
        {
            const double theta = .000001f;

            double add = 0;
            if (actualDepthLookup.TryGetValue(entity.depth, out add) == true)
            {
                actualDepthLookup[entity.depth] += theta;
            }
            else
            {
                actualDepthLookup.Add(entity.depth, theta);
            }
            entity.actualDepth = entity.depth - add;

            Entities.MarkUnsorted();
            foreach (int tag in entity.Tags)
            {
                TagLists.MarkUnsorted(tag);
            }
        }

        public List<Entity> this[int tag]
        {
            get
            {
                return TagLists[(int)tag];
            }
        }

        public void Add(Entity entity)
        {
            Entities.Add(entity);
        }

        public void Remove(Entity entity)
        {
            Entities.Remove(entity);
        }

        public void Add(IEnumerable<Entity> Entities)
        {
            this.Entities.Add(Entities);
        }

        public void Remove(IEnumerable<Entity> Entities)
        {
            this.Entities.Remove(Entities);
        }

        public void Add(params Entity[] Entities)
        {
            this.Entities.Add(Entities);
        }

        public void Remove(params Entity[] Entities)
        {
            this.Entities.Remove(Entities);
        }

        public IEnumerator<Entity> GetEnumerator()
        {
            return this.Entities.GetEnumerator();
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
