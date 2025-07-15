namespace SalesTax
{
    public class Basket
    {
        private readonly Dictionary<Product, int> _products = [];

        public void AddProduct(Product product, int quantity)
        {
            if (_products.ContainsKey(product))
                _products[product] += quantity;
            else
                _products[product] = quantity;
        }

        public IEnumerable<(Product Product, int Quantity)> GetItems()
            => _products.Select(kvp => (kvp.Key, kvp.Value));

        public bool IsEmpty => _products.Count == 0;

        public int ItemCount => _products.Count;
    }
}

