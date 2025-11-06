using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ord_Common
{
    public class items
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemStatus { get; set; } 
        public decimal Price { get; set; }
        public string Description { get; set; }
        public List<platform> Platforms { get; set; }


    }
}
