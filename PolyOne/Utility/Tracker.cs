using System;
using System.Collections.Generic;
using System.Reflection;

using PolyOne.Components;

namespace PolyOne.Utility
{
    public class Tracker
    {
        static public Dictionary<Type, List<Type>> TrackEntityTypes { get; private set; }
        static public Dictionary<Type, List<Type>> TrackedComponentTypes { get; private set; }
        static public HashSet<Type> StoredEntityTypes { get; private set; }
        static public HashSet<Type> StoreComponentTypes { get; private set; }

        static public void Initialize()
        {
            TrackEntityTypes = new Dictionary<Type, List<Type>>();
            TrackedComponentTypes = new Dictionary<Type, List<Type>>();
            StoredEntityTypes = new HashSet<Type>();
            StoreComponentTypes = new HashSet<Type>();

            foreach (var type in Assembly.GetEntryAssembly().GetTypes())
            {
                var attrs = type.GetCustomAttributes(typeof(Tracked), false);

                if (attrs.Length > 0)
                {
                    bool inherited = (attrs[0] as Tracked).Inherited;

                    if (typeof(Entity).IsAssignableFrom(type) == true)
                    {
                        if (type.IsAbstract == false)
                        {
                            if (TrackEntityTypes.ContainsKey(type) == false)
                            {
                                TrackEntityTypes.Add(type, new List<Type>());
                                TrackEntityTypes[type].Add(type);
                            }
                        }

                        StoredEntityTypes.Add(type);

                        if (inherited == true)
                        {
                            foreach (var subclass in GetSubclasses(type))
                            {
                                if (subclass.IsAbstract == false)
                                {
                                    if (TrackEntityTypes.ContainsKey(subclass) == false)
                                    {
                                        TrackEntityTypes.Add(subclass, new List<Type>());
                                    }
                                    TrackEntityTypes[subclass].Add(type);
                                }
                            }
                        }
                    }
                    else if (typeof(Component).IsAssignableFrom(type) == true)
                    {
                        if (type.IsAbstract == false)
                        {
                            if (TrackedComponentTypes.ContainsKey(type) == false)
                            {
                                TrackedComponentTypes.Add(type, new List<Type>());
                            }
                            TrackedComponentTypes[type].Add(type);
                        }

                        StoreComponentTypes.Add(type);

                        if (inherited == true)
                        {
                            foreach (var subclass in GetSubclasses(type))
                            {
                                if (subclass.IsAbstract == false)
                                {
                                    if (TrackedComponentTypes.ContainsKey(subclass) == false)
                                    {
                                        TrackedComponentTypes.Add(subclass, new List<Type>());
                                    }
                                    TrackedComponentTypes[subclass].Add(type);
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Type '" + type.Name + "' cannot be Tracked because it does not dervive from Entity or Component");
                    }
                }
            }
        }

        static private List<Type> GetSubclasses(Type type)
        {
            List<Type> matches = new List<Type>();

            foreach (var check in Assembly.GetEntryAssembly().GetTypes())
            {
                if (type != check && type.IsAssignableFrom(check) == true)
                {
                    matches.Add(check);
                }
            }
            return matches;
        }

        public Dictionary<Type, List<Entity>> Entities { get; private set; }
        public Dictionary<Type, List<Component>> Components { get; private set; }

        public Tracker()
        {
            Entities = new Dictionary<Type, List<Entity>>(TrackEntityTypes.Count);
            foreach (var type in StoredEntityTypes)
            {
                Entities.Add(type, new List<Entity>());
            }

            Components = new Dictionary<Type, List<Component>>(TrackedComponentTypes.Count);
            foreach (var type in StoreComponentTypes)
            {
                Components.Add(type, new List<Component>());
            }
        }

        public T GetEntity<T>() where T : Entity
        {
#if DEBUG
            if (Entities.ContainsKey(typeof(T)) == false)
            {
                throw new Exception("Provided Entity type is not marked with the Tracked arrtribute");
            }
#endif

            var list = Entities[typeof(T)];
            if (list.Count == 0)
            {
                return null;
            }
            else
            {
                return list[0] as T;
            }
        }

        public List<Entity> GetEntites<T>() where T : Entity
        {
#if DEBUG
            if (Entities.ContainsKey(typeof(T)) == false)
            {
                throw new Exception("Provided Entity type is not marked with the Tracked arrtribute");
            }
#endif
            return Entities[typeof(T)];

        }

        public List<Entity> GetEntitesCopy<T>() where T : Entity
        {
            return new List<Entity>(GetEntites<T>());
        }

        public IEnumerator<T> EnumerateEntities<T>() where T : Entity
        {
#if DEBUG
            if (Entities.ContainsKey(typeof(T)) == false)
            {
                throw new Exception("Provided Entity type is not marked with the Tracked arrtribute");
            }
#endif
            foreach (var e in Entities[typeof(T)])
            {
                yield return e as T;
            }
        }

        public T GetComponent<T>() where T : Component
        {
#if DEBUG
            if (Components.ContainsKey(typeof(T)) == false)
            {
                throw new Exception("Provided Entity type is not marked with the Tracked arrtribute");
            }
#endif

            var list = Components[typeof(T)];
            if (list.Count == 0)
            {
                return null;
            }
            else
            {
                return list[0] as T;
            }
        }

        public List<Component> GetComponents<T>() where T : Component
        {
#if DEBUG
            if (Components.ContainsKey(typeof(T)) == false)
            {
                throw new Exception("Provided Entity type is not marked with the Tracked arrtribute");
            }
#endif
            return Components[typeof(T)];
        }

        public List<Component> GetComponentsCopy<T>() where T : Component
        {
            return new List<Component>(GetComponents<T>());
        }

        public IEnumerator<T> EnumerateComponents<T>() where T : Component
        {
#if DEBUG
            if (Components.ContainsKey(typeof(T)) == false)
            {
                throw new Exception("Provided Entity type is not marked with the Tracked arrtribute");
            }
#endif
            foreach (var c in Components[typeof(T)])
            {
                yield return c as T;
            }
        }

        internal void EntityAdded(Entity entity)
        {
            var type = entity.GetType();
            List<Type> trackAs;

            if (TrackEntityTypes.TryGetValue(type, out trackAs) == true)
            {
                foreach (var track in trackAs)
                {
                    Entities[track].Add(entity);
                }
            }
        }

        internal void EntityRemoved(Entity entity)
        {
            var type = entity.GetType();
            List<Type> trackAs;

            if (TrackEntityTypes.TryGetValue(type, out trackAs) == true)
            {
                foreach (var track in trackAs)
                {
                    Entities[track].Remove(entity);
                }
            }
        }

        internal void ComponentAdded(Component component)
        {
            var type = component.GetType();
            List<Type> trackAs;

            if (TrackedComponentTypes.TryGetValue(type, out trackAs) == true)
            {
                foreach (var track in trackAs)
                {
                    Components[track].Add(component);
                }
            }
        }

        internal void ComponentRemoved(Component component)
        {
            var type = component.GetType();
            List<Type> trackAs;

            if (TrackedComponentTypes.TryGetValue(type, out trackAs) == true)
            {
                foreach (var track in trackAs)
                {
                    Components[track].Remove(component);
                }
            }
        }
    }

    public class Tracked : Attribute
    {
        public bool Inherited;

        public Tracked(bool inherited = false)
        {
            Inherited = inherited;
        }
    }
}
