using System.Collections.Generic;

namespace PolyOne.Utility
{
    public class TagList
    {
        private List<List<Entity>> lists;
        private List<bool> unsorted;
        private bool areAnyUnsorted;

        internal TagList()
        {
            lists = new List<List<Entity>>();
            unsorted = new List<bool>();
        }

        public List<Entity> this[int index]
        {
            get
            {
                while (index >= lists.Count)
                {
                    lists.Add(new List<Entity>());
                    unsorted.Add(false);
                }

                if (lists[index] == null)
                    lists[index] = new List<Entity>();

                return lists[index];
            }
        }

        internal void MarkUnsorted(int tag)
        {
            areAnyUnsorted = true;
            unsorted[tag] = true;
        }

        internal void UpdateLists()
        {
            if (areAnyUnsorted == true)
            {
                for (int i = 0; i < lists.Count; i++)
                {
                    lists[i].Sort(EntityList.CompareDepth);
                    unsorted[i] = false;
                }
            }

            areAnyUnsorted = false;
        }

        internal void EntityAdded(Entity entity)
        {
            foreach (int tag in entity.Tags)
            {
                this[tag].Add(entity);
                areAnyUnsorted = true;
                unsorted[tag] = true;
            }
        }

        internal void EntityRemoved(Entity entity)
        {
            foreach (var tag in entity.Tags)
            {
                lists[tag].Remove(entity);
            }
        }
    }
}
