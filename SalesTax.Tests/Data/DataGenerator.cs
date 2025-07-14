namespace SalesTax.Tests.Data;

public class DataGenerator : DataSourceGeneratorAttribute<Product>
{
    public override IEnumerable<Func<Product>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        yield return () => new Product("book", ProductCategory.Book, 12.49, false);
    }
}