namespace SalesTax

{
    public class Basket
    {
        public Dictionary<Product, int> Products = [];

        public void AddProduct(Product product, int quantity)
        {
            if (Products.ContainsKey(product))
            {
                Products[product] += quantity;
            }
            else
            {
                Products.TryAdd(product, quantity);
            }
            return;
        }
    }
}

