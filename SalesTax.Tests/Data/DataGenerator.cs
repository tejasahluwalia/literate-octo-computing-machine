using TUnit.Core;

namespace SalesTax.Tests.Data;

public record SalesTaxTestData(IEnumerable<string> InputLines, int ExpectedLines, decimal ExpectedSalesTax, decimal ExpectedTotal);

public static class DataGenerator
{
    public static IEnumerable<Func<SalesTaxTestData>> GetSalesTaxTestData()
    {
        // Test case 1
        yield return () => new SalesTaxTestData(
            [
                "1 book at 12.49",
                "1 music CD at 14.99", 
                "1 chocolate bar at 0.85"
            ],
            3,
            1.50m,
            29.83m
        );

        // Test case 2
        yield return () => new SalesTaxTestData(
            [
                "1 imported box of chocolates at 10.00",
                "1 imported bottle of perfume at 47.50"
            ],
            2,
            7.65m,
            65.15m
        );

        // Test case 3
        yield return () => new SalesTaxTestData(
            [
                "1 imported bottle of perfume at 27.99",
                "1 bottle of perfume at 18.99",
                "1 packet of headache pills at 9.75",
                "1 box of imported chocolates at 11.25"
            ],
            4,
            6.70m,
            74.68m
        );
    }
}