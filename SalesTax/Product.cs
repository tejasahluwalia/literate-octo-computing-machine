namespace SalesTax

{
    public enum ProductCategory
    {
        Food,
        Book,
        Medical,
        Other
    };

    public readonly record struct Product(string Name, ProductCategory Category, double Price, bool IsImported);

}