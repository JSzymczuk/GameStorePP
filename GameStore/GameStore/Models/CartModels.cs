using System.Collections;
using System.Collections.Generic;

namespace GameStore.Models
{
    public class CartPosition
    {
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }

    public class Cart : IEnumerable<CartPosition>
    {
        private Dictionary<int, CartPosition> items;

        public Cart() { items = new Dictionary<int, CartPosition>(); }

        public CartPosition this[int index]
        {
            get { return items.ContainsKey(index) ? items[index] : null; }
        }

        public void AddAmount(Product product, int quantity)
        {
            if (quantity != 0 && product.Price.HasValue)
            {
                int id = product.Id;
                if (items.ContainsKey(id))
                {
                    int q = items[id].Quantity + quantity;
                    items[id].Quantity = q > 1 ? q : 1;
                }
                else if (quantity > 0)
                {
                    items.Add(id, new CartPosition
                    {
                        Product = product,
                        Quantity = quantity,
                        UnitPrice = product.Price.Value
                    });
                }
            }
        }

        public void Remove(Product product)
        {
            items.Remove(product.Id);
        }

        public void Clear()
        {
            items.Clear();
        }

        public int Count { get { return items.Count; } }

        public bool IsEmpty { get { return items.Count == 0; } }

        public int TotalProducts
        {
            get
            {
                int res = 0;
                foreach (var entry in items)
                { res += entry.Value.Quantity; }
                return res;
            }
        }

        public decimal TotalPrice
        {
            get
            {
                decimal res = 0;
                foreach (var entry in items)
                { res += entry.Value.Quantity * entry.Value.UnitPrice; }
                return res;
            }
        }

        public IEnumerator<CartPosition> GetEnumerator()
        {
            return items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.Values.GetEnumerator();
        }
    }
}