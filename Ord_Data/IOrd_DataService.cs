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
        void AddItem(items item);
        List<items> GetAllItems();
        items GetItemById(int id);
        void UpdateItem(items item);
        void DeleteItem(int id);
    }
}
