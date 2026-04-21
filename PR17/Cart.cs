using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR17
{
    public class CartItem
    {
        public Products Product { get; set; }
        public int Count { get; set; }
    }

    public static class Cart
    {
        public static List<CartItem> Items = new List<CartItem>();
    }
}