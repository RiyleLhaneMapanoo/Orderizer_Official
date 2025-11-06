using Ord_Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;


namespace Ord_Data
{
    public class Ord_TextMem : IOrd_DataService
    {
        private readonly string _filePath = "textDB.txt";
        private List<items> _items = new List<items>();
        private int _nextId = 1;

        public Ord_TextMem()
        {
            GetDataFromFile();
        }

        private void GetDataFromFile()
        {
            if (!File.Exists(_filePath))
            {
                File.Create(_filePath).Close();
                return;
            }

            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split('|');
                var item = new items
                {
                    ItemId = Convert.ToInt32(parts[0]),
                    ItemName = parts[1],
                    ItemStatus = parts[2],
                    Price = Convert.ToDecimal(parts[3]),
                    Description = parts[4]
                };

              
                if (parts.Length > 5 && !string.IsNullOrWhiteSpace(parts[5]))
                {
                    item.Platforms = DeserializePlatforms(parts[5]);
                }

                _items.Add(item);

                
                if (item.ItemId >= _nextId)
                {
                    _nextId = item.ItemId + 1;
                }
            }
        }

        private void WriteDataToFile()
        {
            var lines = new string[_items.Count];
            for (int i = 0; i < _items.Count; i++)
            {
                var platformsData = SerializePlatforms(_items[i].Platforms);
                lines[i] = $"{_items[i].ItemId}|{_items[i].ItemName}|{_items[i].ItemStatus}|{_items[i].Price}|{_items[i].Description}|{platformsData}";
            }
            File.WriteAllLines(_filePath, lines);
        }

        private string SerializePlatforms(List<platform> platforms)
        {
            if (platforms == null || platforms.Count == 0)
                return "";

            var serialized = new StringBuilder();
            foreach (var plat in platforms)
            {
                serialized.Append($"{plat.PlatformId}:{plat.PlatformName}:{plat.ShopName}:{plat.Price};");
            }
            return serialized.ToString().TrimEnd(';');
        }

        private List<platform> DeserializePlatforms(string platformsData)
        {
            var platforms = new List<platform>();
            if (string.IsNullOrWhiteSpace(platformsData))
                return platforms;

            var platformParts = platformsData.Split(';');
            foreach (var part in platformParts)
            {
                if (string.IsNullOrWhiteSpace(part))
                    continue;

                var details = part.Split(':');
                platforms.Add(new platform
                {
                    PlatformId = Convert.ToInt32(details[0]),
                    PlatformName = details[1],
                    ShopName = details[2],
                    Price = Convert.ToDecimal(details[3])
                });
            }
            return platforms;
        }

        private int FindIndex(int id)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].ItemId == id)
                {
                    return i;
                }
            }
            return -1;
        }

        public void AddItem(items item)
        {
            item.ItemId = _nextId++;
            _items.Add(item);
            WriteDataToFile();
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
            int index = FindIndex(updatedItem.ItemId);
            if (index != -1)
            {
                _items[index].ItemName = updatedItem.ItemName;
                _items[index].ItemStatus = updatedItem.ItemStatus;
                _items[index].Price = updatedItem.Price;
                _items[index].Description = updatedItem.Description;
                _items[index].Platforms = updatedItem.Platforms;
                WriteDataToFile();
            }
        }

        public void DeleteItem(int id)
        {
            int index = FindIndex(id);
            if (index != -1)
            {
                _items.RemoveAt(index);
                WriteDataToFile();
            }
        }
    }
}
