using PolyOne.Scenes;

namespace PolyOne.Components
{
    public class Component
    {
        public Entity Entity { get; private set; }

        public bool Active { get; set; }

        public bool Visible { get; set; }

        public Component(bool active, bool visible)
        {
            Active = active;
            Visible = visible;
        }

        public virtual void Added(Entity entity)
        {
            Entity = entity;

            if (Scene != null)
            {
                Scene.Tracker.ComponentAdded(this);
            }
        }

        public virtual void Removed(Entity entity)
        {
            Entity = null;

            if (Scene != null)
            {
                Scene.Tracker.ComponentRemoved(this);
            }
        }

        public virtual void EntityAdded()
        {
            if (Scene != null)
            {
                Scene.Tracker.ComponentRemoved(this);
            }
        }

        public virtual void EntityRemoved()
        {
            if (Scene != null)
            {
                Scene.Tracker.ComponentRemoved(this);
            }
        }

        public virtual void Update()
        {

        }

        public virtual void Draw()
        {

        }

        public void RemoveSelf()
        {
            if (Entity != null)
            {
                Entity.Remove(this);
            }
        }

        public Scene Scene
        {
            get { return Entity != null ? Entity.Scene : null; }
        }
    }
}
