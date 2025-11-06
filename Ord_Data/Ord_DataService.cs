using System;
using System.Collections.Generic;
using Ord_Common;

namespace Ord_Data
{
    public class Ord_DataService
    {
        private IOrd_DataService dataLogic;

        public Ord_DataService()
        {
            // Easily switch between data sources
            //dataLogic = new Ord_InMemory();
            dataLogic = new Ord_TextMem();
            // dataLogic = new Ord_JSONDataService();
            // dataLogic = new Ord_DatabaseService();
        }

        public void AddItem(object item)
        {
            dataLogic.AddItem((Ord_Common.items)item);
        }

        public List<items> GetAllItems()
        {
            var list = new List<items>();
            foreach (var i in dataLogic.GetAllItems())
                list.Add(i);
            return list;
        }

        public items GetItemById(int id)
        {
            return dataLogic.GetItemById(id);
        }

        public void UpdateItem(items item)
        {
            dataLogic.UpdateItem(item);
        }

        public void DeleteItem(int id)
        {
            dataLogic.DeleteItem(id);
        }
    }
}
