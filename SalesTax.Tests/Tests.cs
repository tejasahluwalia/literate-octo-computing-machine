using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;
using SalesTax;
using SalesTax.Tests.Data;

namespace SalesTax.Tests;

public class SalesTaxIntegrationTests
{
    private const decimal BASIC_SALES_TAX_RATE = 10.0m / 100;
    private const decimal IMPORT_TAX_RATE = 5.0m / 100;

    private static readonly BasicSalesTax _basicSalesTax = new(BASIC_SALES_TAX_RATE);
    private static readonly ImportSalesTax _importSalesTax = new(IMPORT_TAX_RATE);
    private static readonly TaxPolicy _taxPolicy = new([_basicSalesTax, _importSalesTax]);

    [Test]
    [MethodDataSource(typeof(DataGenerator), nameof(DataGenerator.GetSalesTaxTestData))]
    public async Task TestSalesTaxCalculation(SalesTaxTestData testData)
    {
        // Arrange & Act
        var basket = OrderParser.ParseOrder(testData.InputLines);
        var receipt = new Receipt(basket, _taxPolicy);

        // Assert
        await Assert.That(receipt.Lines).HasCount().EqualTo(testData.ExpectedLines);
        
        // Check sales tax
        await Assert.That(receipt.GetTotalTax()).IsEqualTo(testData.ExpectedSalesTax);
        
        // Check total
        await Assert.That(receipt.GetTotal()).IsEqualTo(testData.ExpectedTotal);
    }
}
