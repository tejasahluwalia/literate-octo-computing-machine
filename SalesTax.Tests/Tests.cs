using SalesTax;
using SalesTax.Tests.Data;

namespace SalesTax.Tests;

public class SalesTaxIntegrationTests
{
    private const double BASIC_SALES_TAX_RATE = 10.0 / 100;
    private const double IMPORT_TAX_RATE = 5.0 / 100;
    private readonly TaxRateCalculator _taxRateCalculator = new(BASIC_SALES_TAX_RATE, IMPORT_TAX_RATE);

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
        var receipt = ReceiptGenerator.GenerateReceipt(basket, _taxRateCalculator);

        // Assert
        await Assert.That(receipt.Lines).HasCount().EqualTo(3);
        
        // Book (exempt from sales tax)
        var bookLine = receipt.Lines.First(l => l.ProductName.Contains("book"));
        await Assert.That(bookLine.Amount).IsEqualTo(12.49);
        await Assert.That(bookLine.Tax).IsEqualTo(0.0);
        await Assert.That(bookLine.Total).IsEqualTo(12.49);

        // Music CD (10% sales tax)
        var cdLine = receipt.Lines.First(l => l.ProductName.Contains("music CD"));
        await Assert.That(cdLine.Amount).IsEqualTo(14.99);
        await Assert.That(cdLine.Tax).IsEqualTo(1.50); // 14.99 * 0.10 = 1.499, rounded up to 1.50
        await Assert.That(Math.Round(cdLine.Total, 2)).IsEqualTo(16.49);

        // Chocolate bar (food, exempt from sales tax)
        var chocolateLine = receipt.Lines.First(l => l.ProductName.Contains("chocolate bar"));
        await Assert.That(chocolateLine.Amount).IsEqualTo(0.85);
        await Assert.That(chocolateLine.Tax).IsEqualTo(0.0);
        await Assert.That(chocolateLine.Total).IsEqualTo(0.85);

        // Totals
        await Assert.That(receipt.TotalTax).IsEqualTo(1.50);
        await Assert.That(Math.Round(receipt.TotalAmount, 2)).IsEqualTo(29.83);
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
        var receipt = ReceiptGenerator.GenerateReceipt(basket, _taxRateCalculator);

        // Assert
        await Assert.That(receipt.Lines).HasCount().EqualTo(2);

        // Imported chocolates (food + import tax = 5%)
        var chocolateLine = receipt.Lines.First(l => l.ProductName.Contains("chocolates"));
        await Assert.That(chocolateLine.Amount).IsEqualTo(10.00);
        await Assert.That(chocolateLine.Tax).IsEqualTo(0.50); // 10.00 * 0.05 = 0.50
        await Assert.That(chocolateLine.Total).IsEqualTo(10.50);

        // Imported perfume (other + import tax = 15%)
        var perfumeLine = receipt.Lines.First(l => l.ProductName.Contains("perfume"));
        await Assert.That(perfumeLine.Amount).IsEqualTo(47.50);
        await Assert.That(perfumeLine.Tax).IsEqualTo(7.15); // 47.50 * 0.15 = 7.125, rounded up to 7.15
        await Assert.That(perfumeLine.Total).IsEqualTo(54.65);

        // Totals
        await Assert.That(Math.Round(receipt.TotalTax, 2)).IsEqualTo(7.65);
        await Assert.That(Math.Round(receipt.TotalAmount, 2)).IsEqualTo(65.15);
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
        var receipt = ReceiptGenerator.GenerateReceipt(basket, _taxRateCalculator);

        // Assert
        await Assert.That(receipt.Lines).HasCount().EqualTo(4);

        // Imported perfume (other + import tax = 15%)
        var importedPerfumeLine = receipt.Lines.First(l => l.ProductName.Contains("imported bottle of perfume"));
        await Assert.That(importedPerfumeLine.Amount).IsEqualTo(27.99);
        await Assert.That(importedPerfumeLine.Tax).IsEqualTo(4.20); // 27.99 * 0.15 = 4.1985, rounded up to 4.20
        await Assert.That(importedPerfumeLine.Total).IsEqualTo(32.19);

        // Domestic perfume (other + sales tax = 10%)
        var domesticPerfumeLine = receipt.Lines.First(l => l.ProductName.Contains("bottle of perfume") && !l.ProductName.Contains("imported"));
        await Assert.That(domesticPerfumeLine.Amount).IsEqualTo(18.99);
        await Assert.That(domesticPerfumeLine.Tax).IsEqualTo(1.90); // 18.99 * 0.10 = 1.899, rounded up to 1.90
        await Assert.That(Math.Round(domesticPerfumeLine.Total, 2)).IsEqualTo(20.89);

        // Headache pills (medical, exempt)
        var pillsLine = receipt.Lines.First(l => l.ProductName.Contains("pills"));
        await Assert.That(pillsLine.Amount).IsEqualTo(9.75);
        await Assert.That(pillsLine.Tax).IsEqualTo(0.0);
        await Assert.That(pillsLine.Total).IsEqualTo(9.75);

        // Imported chocolates (food + import tax = 5%)
        var importedChocolateLine = receipt.Lines.First(l => l.ProductName.Contains("imported chocolates"));
        await Assert.That(importedChocolateLine.Amount).IsEqualTo(11.25);
        await Assert.That(importedChocolateLine.Tax).IsEqualTo(0.60); // 11.25 * 0.05 = 0.5625, rounded up to 0.60
        await Assert.That(importedChocolateLine.Total).IsEqualTo(11.85);

        // Totals
        await Assert.That(Math.Round(receipt.TotalTax, 2)).IsEqualTo(6.70);
        await Assert.That(Math.Round(receipt.TotalAmount, 2)).IsEqualTo(74.68);
    }

    [Test]
    [Arguments("input1.txt")]
    [Arguments("input2.txt")]
    [Arguments("input3.txt")]
    public async Task TestCase_FromFile_ParsesAndCalculatesCorrectly(string filename)
    {
        // Arrange
        var basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        var projectPath = Path.GetFullPath(Path.Combine(basePath!, "..", "..", ".."));
        var filePath = Path.Combine(projectPath, "Data", filename);
        var input = File.ReadAllLines(filePath);

        // Act
        var basket = OrderParser.ParseOrder(input);
        var receipt = ReceiptGenerator.GenerateReceipt(basket, _taxRateCalculator);

        // Assert - Basic validation that parsing worked
        await Assert.That(receipt.Lines).HasCount().GreaterThan(0);
        await Assert.That(receipt.TotalAmount).IsGreaterThan(0);
        
        // All line totals should equal sum of amount + tax
        foreach (var line in receipt.Lines)
        {
            await Assert.That(Math.Round(line.Total, 2)).IsEqualTo(Math.Round(line.Amount + line.Tax, 2));
        }
    }
}