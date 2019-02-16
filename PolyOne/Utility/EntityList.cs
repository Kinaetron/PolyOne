using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using PolyOne.Scenes;

namespace PolyOne.Utility
{
    public class EntityList : IEnumerable<Entity>, IEnumerable
    {
        public Scene Scene { get; private set; }

        private List<Entity> entities;
        private List<Entity> toAdd;
        private List<Entity> toAwake;
        private List<Entity> toRemove;
        private bool unsorted;

        internal EntityList(Scene scene)
        {
            Scene = scene;

            entities = new List<Entity>();
            toAdd = new List<Entity>();
            toAwake = new List<Entity>();
            toRemove = new List<Entity>();
        }

        internal void MarkUnsorted()
        {
            unsorted = true;
        }

        public void UpdateLists()
        {
            if (toAdd.Count > 0)
            {
                foreach (Entity entity in toAdd)
                {
                    if (entities.Contains(entity) == false)
                    {
                        entities.Add(entity);

                        if (Scene != null)
                        {
                            Scene.TagLists.EntityAdded(entity);
                        }
                    }
                }
                unsorted = true;
            }

            if (toRemove.Count > 0)
            {
                foreach (Entity entity in toRemove)
                {
                    if (entities.Contains(entity))
                    {
                        entities.Remove(entity);

                        if (Scene != null)
                        {
                            Scene.TagLists.EntityRemoved(entity);
                        }
                    }
                }

                toRemove.Clear();
            }

            if (unsorted == true)
            {
                unsorted = false;
                entities.Sort(CompareDepth);
            }

            if (toAdd.Count > 0)
            {
                toAwake.AddRange(toAdd);
                toAdd.Clear();

                toAwake.Clear();
            }
        }

        public void Add(Entity entity)
        {
            if (toAdd.Contains(entity) == false && entities.Contains(entity) == false)
            {
                toAdd.Add(entity);
            }
        }

        public void Remove(Entity entity)
        {
            if (toRemove.Contains(entity) == false && entities.Contains(entity) == true)
            {
                toRemove.Add(entity);
            }
        }

        public void Add(IEnumerable<Entity> entites)
        {
            foreach (Entity entity in entites)
            {
                Add(entity);
            }
        }

        public void Remove(IEnumerable<Entity> entites)
        {
            foreach (Entity entity in entites)
            {
                Remove(entity);
            }
        }

        public void Add(params Entity[] entites)
        {
            for (int i = 0; i < entites.Length; i++)
            {
                Add(entites[i]);
            }
        }

        public void Remove(params Entity[] entites)
        {
            for (int i = 0; i < entites.Length; i++)
            {
                Remove(entites[i]);
            }
        }

        public int Count
        {
            get
            {
                return entities.Count;
            }
        }

        public Entity this[int index]
        {
            get
            {
                if (index < 0 || index >= entities.Count)
                {
                    throw new IndexOutOfRangeException();
                }
                else
                {
                    return entities[index];
                }
            }
        }

        public IEnumerator<Entity> GetEnumerator()
        {
            return entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Entity[] ToArray()
        {
            return entities.ToArray<Entity>();
        }

        internal void Update()
        {
            foreach (Entity entity in entities)
            {
                if (entity.Active == true)
                {
                    entity.Update();
                }
            }
        }

        internal void Draw()
        {
            foreach (Entity entity in entities)
            {
                if (entity.Visible == true)
                {
                    entity.Draw();
                }
            }
        }

        static public Comparison<Entity> CompareDepth = (a, b) => { return Math.Sign(b.actualDepth - a.actualDepth); };
    }
}
