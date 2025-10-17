using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ord_Common;
using Ord_Data;

namespace Ord_Business
{
    public class Ord_BusinessServ
    {
        private readonly IOrd_DataService _dataService;

        public Ord_BusinessServ(IOrd_DataService dataService)
        {
            _dataService = dataService;
        }

        public void AddNewItem(items item)
        {
            if (string.IsNullOrWhiteSpace(item.ItemName))
                throw new Exception("Item name cannot be empty.");

            _dataService.AddItem(item);
        }

        public List<items> GetAllItems()
        {
            return _dataService.GetAllItems();
        }

        public items GetItemById(int id)
        {
            return _dataService.GetItemById(id);
        }

        public void UpdateItem(items item)
        {
            _dataService.UpdateItem(item);
        }

        public void DeleteItem(int id)
        {
            _dataService.DeleteItem(id);
        }

        public items GetBestDeal(string itemName)
        {
            var allItems = _dataService.GetAllItems();
            var match = allItems.FirstOrDefault(i => i.ItemName.Equals(itemName, StringComparison.OrdinalIgnoreCase));

            if (match == null || match.Platforms == null || !match.Platforms.Any())
                return null;

            // Sort platforms by price and return item with lowest
            match.Platforms = match.Platforms.OrderBy(p => p.PlatformId).ToList();
            return match;
        }
    }
}
