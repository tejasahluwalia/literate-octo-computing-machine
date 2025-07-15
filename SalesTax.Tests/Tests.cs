using SalesTax;
using SalesTax.Tests.Data;

namespace SalesTax.Tests;

public class SalesTaxIntegrationTests
{
    private const decimal BASIC_SALES_TAX_RATE = 10.0m / 100;
    private const decimal IMPORT_TAX_RATE = 5.0m / 100;
    private readonly TaxCalculator _taxCalculator = new(BASIC_SALES_TAX_RATE, IMPORT_TAX_RATE);
    private readonly ReceiptService _receiptService;

    public SalesTaxIntegrationTests()
    {
        _receiptService = new ReceiptService(_taxCalculator);
    }

    [Test]
    public async Task TestCase1_BasicItems_CalculatesCorrectTaxes()
    {
        // Arrange
        var inputLines = new[]
        {
            "1 book at 12.49",
            "1 music CD at 14.99", 
            "1 chocolate bar at 0.85"
        };

        // Act
        var basket = OrderParser.ParseOrder(inputLines);
        var receipt = _receiptService.GenerateReceipt(basket);

        // Assert
        await Assert.That(receipt.Lines).HasCount().EqualTo(3);
        
        // Book (exempt from sales tax)
        var bookLine = receipt.Lines.First(l => l.ProductName.Contains("book"));
        await Assert.That(bookLine.Amount).IsEqualTo(12.49m);
        await Assert.That(bookLine.Tax).IsEqualTo(0.0m);
        await Assert.That(bookLine.Total).IsEqualTo(12.49m);

        // Music CD (10% sales tax)
        var cdLine = receipt.Lines.First(l => l.ProductName.Contains("music CD"));
        await Assert.That(cdLine.Amount).IsEqualTo(14.99m);
        await Assert.That(cdLine.Tax).IsEqualTo(1.50m); // 14.99 * 0.10 = 1.499, rounded up to 1.50
        await Assert.That(cdLine.Total).IsEqualTo(16.49m);

        // Chocolate bar (food, exempt from sales tax)
        var chocolateLine = receipt.Lines.First(l => l.ProductName.Contains("chocolate bar"));
        await Assert.That(chocolateLine.Amount).IsEqualTo(0.85m);
        await Assert.That(chocolateLine.Tax).IsEqualTo(0.0m);
        await Assert.That(chocolateLine.Total).IsEqualTo(0.85m);

        // Totals
        await Assert.That(receipt.TotalTax).IsEqualTo(1.50m);
        await Assert.That(receipt.TotalAmount).IsEqualTo(29.83m);
    }

    [Test]
    public async Task TestCase2_ImportedItems_CalculatesCorrectTaxes()
    {
        // Arrange
        var inputLines = new[]
        {
            "1 imported box of chocolates at 10.00",
            "1 imported bottle of perfume at 47.50"
        };

        // Act
        var basket = OrderParser.ParseOrder(inputLines);
        var receipt = _receiptService.GenerateReceipt(basket);

        // Assert
        await Assert.That(receipt.Lines).HasCount().EqualTo(2);

        // Imported chocolates (food + import tax = 5%)
        var chocolateLine = receipt.Lines.First(l => l.ProductName.Contains("chocolates"));
        await Assert.That(chocolateLine.Amount).IsEqualTo(10.00m);
        await Assert.That(chocolateLine.Tax).IsEqualTo(0.50m); // 10.00 * 0.05 = 0.50
        await Assert.That(chocolateLine.Total).IsEqualTo(10.50m);

        // Imported perfume (other + import tax = 15%)
        var perfumeLine = receipt.Lines.First(l => l.ProductName.Contains("perfume"));
        await Assert.That(perfumeLine.Amount).IsEqualTo(47.50m);
        await Assert.That(perfumeLine.Tax).IsEqualTo(7.15m); // 47.50 * 0.15 = 7.125, rounded up to 7.15
        await Assert.That(perfumeLine.Total).IsEqualTo(54.65m);

        // Totals
        await Assert.That(receipt.TotalTax).IsEqualTo(7.65m);
        await Assert.That(receipt.TotalAmount).IsEqualTo(65.15m);
    }

    [Test]
    public async Task TestCase3_MixedItems_CalculatesCorrectTaxes()
    {
        // Arrange
        var inputLines = new[]
        {
            "1 imported bottle of perfume at 27.99",
            "1 bottle of perfume at 18.99",
            "1 packet of headache pills at 9.75",
            "1 box of imported chocolates at 11.25"
        };

        // Act
        var basket = OrderParser.ParseOrder(inputLines);
        var receipt = _receiptService.GenerateReceipt(basket);

        // Assert
        await Assert.That(receipt.Lines).HasCount().EqualTo(4);

        // Imported perfume (other + import tax = 15%)
        var importedPerfumeLine = receipt.Lines.First(l => l.ProductName.Contains("imported") && l.ProductName.Contains("perfume"));
        await Assert.That(importedPerfumeLine.Amount).IsEqualTo(27.99m);
        await Assert.That(importedPerfumeLine.Tax).IsEqualTo(4.20m); // 27.99 * 0.15 = 4.1985, rounded up to 4.20
        await Assert.That(importedPerfumeLine.Total).IsEqualTo(32.19m);

        // Regular perfume (other + sales tax = 10%)
        var perfumeLine = receipt.Lines.First(l => !l.ProductName.Contains("imported") && l.ProductName.Contains("perfume"));
        await Assert.That(perfumeLine.Amount).IsEqualTo(18.99m);
        await Assert.That(perfumeLine.Tax).IsEqualTo(1.90m); // 18.99 * 0.10 = 1.899, rounded up to 1.90
        await Assert.That(perfumeLine.Total).IsEqualTo(20.89m);

        // Headache pills (medical, exempt from sales tax)
        var pillsLine = receipt.Lines.First(l => l.ProductName.Contains("pills"));
        await Assert.That(pillsLine.Amount).IsEqualTo(9.75m);
        await Assert.That(pillsLine.Tax).IsEqualTo(0.0m);
        await Assert.That(pillsLine.Total).IsEqualTo(9.75m);

        // Imported chocolates (food + import tax = 5%)
        var chocolateLine = receipt.Lines.First(l => l.ProductName.Contains("chocolates"));
        await Assert.That(chocolateLine.Amount).IsEqualTo(11.25m);
        await Assert.That(chocolateLine.Tax).IsEqualTo(0.60m); // 11.25 * 0.05 = 0.5625, rounded up to 0.60
        await Assert.That(chocolateLine.Total).IsEqualTo(11.85m);

        // Totals
        await Assert.That(receipt.TotalTax).IsEqualTo(6.70m);
        await Assert.That(receipt.TotalAmount).IsEqualTo(74.68m);
    }

    [Test]
    public async Task TestFromGeneratedData()
    {
        // Arrange
        var inputData = DataGenerator.GetInput1();

        // Act
        var basket = OrderParser.ParseOrder(inputData);
        var receipt = _receiptService.GenerateReceipt(basket);

        // Assert
        await Assert.That(receipt.Lines).HasCount().GreaterThan(0);
        await Assert.That(receipt.TotalAmount).IsGreaterThan(0);
    }
}
