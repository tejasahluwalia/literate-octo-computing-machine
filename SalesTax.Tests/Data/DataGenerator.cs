namespace SalesTax.Tests.Data;

public class DataGenerator : DataSourceGeneratorAttribute<Product>
{
    public override IEnumerable<Func<Product>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        yield return () => new Product("book", ProductCategory.Book, 12.49m, false);
    }
    
    public static IEnumerable<string> GetInput1()
    {
        return new[]
        {
            "1 book at 12.49",
            "1 music CD at 14.99",
            "1 chocolate bar at 0.85"
        };
    }
}