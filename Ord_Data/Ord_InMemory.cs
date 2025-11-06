using Ord_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ord_Common;

namespace Ord_Data
{
    public class Ord_InMemory : IOrd_DataService
    {
        private readonly List<items> _items = new List<items>();
        private int _nextId = 1;

        public void AddItem(items item)
        {
            item.ItemId = _nextId++;
            _items.Add(item);
        }

        public List<items> GetAllItems()
        {
            return _items;
        }

        public items GetItemById(int id)
        {
            return _items.FirstOrDefault(i => i.ItemId == id);
        }

        public void UpdateItem(items updatedItem)
        {
            var existing = _items.FirstOrDefault(i => i.ItemId == updatedItem.ItemId);
            if (existing != null)
            {
                existing.ItemName = updatedItem.ItemName;
                existing.ItemStatus = updatedItem.ItemStatus;
                existing.Price = updatedItem.Price;
                existing.Description = updatedItem.Description;
                existing.Platforms = updatedItem.Platforms;
            }
        }

        public void DeleteItem(int id)
        {
            var existing = _items.FirstOrDefault(i => i.ItemId == id);
            if (existing != null)
            {
                _items.Remove(existing);
            }
        }
    }
}
