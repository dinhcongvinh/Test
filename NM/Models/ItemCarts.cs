using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NM.Models
{
    public class ItemCarts
    {
        public int idProduct { get; set; }
        public string namProduct { get; set; }
        public int numberProduct { get; set; }
        public double priceProduct { get; set; }
        public string imageProduct { get; set; }
        public double intoMoney { get; set; }

        public ItemCarts() { }
    }
}