using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

using PolyOne.Components;

namespace PolyOne.Utility
{
    public class ComponentList : IEnumerable<Component>, IEnumerable
    {
        public enum LockModes { Open, Locked, Error };
        public Entity Entity { get; internal set; }

        private List<Component> components;
        private List<Component> toAdd;
        private List<Component> toRemove;
        private LockModes lockMode;

        internal ComponentList(Entity entity)
        {
            Entity = entity;

            components = new List<Component>();
            toAdd = new List<Component>();
            toRemove = new List<Component>();
        }

        internal LockModes LockMode
        {
            get
            {
                return lockMode;
            }

            set
            {
                lockMode = value;

                if (toAdd.Count > 0)
                {
                    foreach (Component component in toAdd)
                    {
                        if (components.Contains(component) == false)
                        {
                            components.Add(component);
                            component.Added(Entity);
                        }
                    }

                    toAdd.Clear();
                }

                if (toRemove.Count > 0)
                {
                    foreach (Component component in toRemove)
                    {
                        if (components.Contains(component) == true)
                        {
                            components.Remove(component);
                            component.Removed(Entity);
                        }
                    }

                    toRemove.Clear();
                }
            }
        }

        public void Add(Component component)
        {
            switch (lockMode)
            {
                case LockModes.Open:
                    if (components.Contains(component) == false)
                    {
                        components.Add(component);
                        component.Added(Entity);
                    }
                    break;

                case LockModes.Locked:
                    if (toAdd.Contains(component) == false && components.Contains(component) == false)
                    {
                        toAdd.Add(component);
                    }
                    break;

                case LockModes.Error:
                    throw new Exception("Cannot add or remove Entities at this time");
            }
        }

        public void Remove(Component component)
        {
            switch (lockMode)
            {
                case LockModes.Open:
                    if (components.Contains(component) == true)
                    {
                        components.Remove(component);
                        component.Removed(Entity);
                    }
                    break;

                case LockModes.Locked:
                    if (toRemove.Contains(component) == false && components.Contains(component) == true)
                    {
                        toRemove.Add(component);
                    }
                    break;

                case LockModes.Error:
                    throw new Exception("Cannot add or remove Entites at this time");
            }
        }

        public void Add(IEnumerable<Component> components)
        {
            foreach (Component component in components)
            {
                Add(component);
            }
        }

        public void Remove(IEnumerable<Component> components)
        {
            foreach (Component component in components)
            {
                Remove(component);
            }
        }


        public void Add(params Component[] components)
        {
            foreach (Component component in components)
            {
                Add(component);
            }
        }

        public void Remove(params Component[] components)
        {
            foreach (Component component in components)
            {
                Remove(component);
            }
        }

        public int Count
        {
            get { return components.Count; }
        }

        public Component this[int index]
        {
            get
            {
                if (index < 0 || index >= components.Count)
                {
                    throw new IndexOutOfRangeException();
                }
                else
                {
                    return components[index];
                }
            }
        }

        public IEnumerator<Component> GetEnumerator()
        {
            return components.GetEnumerator();
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Component[] ToArray()
        {
            return components.ToArray<Component>();
        }

        internal void Update()
        {
            lockMode = ComponentList.LockModes.Locked;
            foreach (Component component in components)
            {
                if (component.Active == true)
                {
                    component.Update();
                }
            }
            lockMode = ComponentList.LockModes.Open;
        }

        internal void Draw()
        {
            lockMode = ComponentList.LockModes.Error;
            foreach (Component component in components)
            {
                if (component.Visible == true)
                {
                    component.Draw();
                }
            }
            lockMode = ComponentList.LockModes.Open;
        }

        public T Get<T>() where T : Component
        {
            foreach (var component in components)
                if (component is T)
                    return component as T;
            return null;
        }
    }
}
