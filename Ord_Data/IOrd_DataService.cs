using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ord_Common;

namespace Ord_Data
{
    public interface IOrd_DataService
    {
      public  void AddItem(items item);
        public List<items> GetAllItems();
        public items GetItemById(int id);
        public void UpdateItem(items item);
        public void DeleteItem(int id);
    }
}
