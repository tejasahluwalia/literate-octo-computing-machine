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
    private readonly TaxCalculator _taxCalculator = new(BASIC_SALES_TAX_RATE, IMPORT_TAX_RATE);

    [Test]
    [MethodDataSource(typeof(DataGenerator), nameof(DataGenerator.GetSalesTaxTestData))]
    public async Task TestSalesTaxCalculation(SalesTaxTestData testData)
    {
        // Arrange & Act
        var basket = OrderParser.ParseOrder(testData.InputLines);
        var receipt = new Receipt(basket, _taxCalculator);

        // Assert
        await Assert.That(receipt.Lines).HasCount().EqualTo(testData.ExpectedLines);
        
        // Check sales tax
        await Assert.That(receipt.TotalSalesTax).IsEqualTo(testData.ExpectedSalesTax);
        
        // Check total
        await Assert.That(receipt.Total).IsEqualTo(testData.ExpectedTotal);
    }
}
