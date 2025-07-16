namespace SalesTax
{
    public enum ProductCategory
    {
        Food,
        Book,
        Medical,
        Other
    }

    public class Product(string name, ProductCategory category, decimal price, bool isImported)
    {
        public string Name { get; } = name;
        public ProductCategory Category { get; } = category;
        public decimal Price { get; } = price;
        public bool IsImported { get; } = isImported;
        
        public bool IsExemptFromSalesTax() => Category is ProductCategory.Food or ProductCategory.Book or ProductCategory.Medical;

        // Override Equals and GetHashCode for proper dictionary usage
        public override bool Equals(object? obj)
        {
            if (obj is not Product other) return false;
            return Name == other.Name && 
                   Category == other.Category && 
                   Price == other.Price && 
                   IsImported == other.IsImported;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Category, Price, IsImported);
        }
    }
}