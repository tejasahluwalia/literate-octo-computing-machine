namespace SalesTax.Tests.Data;

public class DataGenerator : DataSourceGeneratorAttribute<Product>
{
    public override IEnumerable<Func<Product>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        yield return () => new Product("", ProductCategory.Book, 20, false);
    }
}