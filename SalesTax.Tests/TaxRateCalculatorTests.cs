using SalesTax;

namespace SalesTax.Tests;

public class TaxRateCalculatorTests
{
    private const double BASIC_SALES_TAX_RATE = 10.0 / 100;
    private const double IMPORT_TAX_RATE = 5.0 / 100;
    private readonly TaxRateCalculator _calculator = new(BASIC_SALES_TAX_RATE, IMPORT_TAX_RATE);

    [Test]
    public async Task GetProductTaxRate_BookProduct_ReturnsZeroTax()
    {
        // Arrange
        var book = new Product("book", ProductCategory.Book, 12.49, false);

        // Act
        var taxRate = _calculator.GetProductTaxRate(book);

        // Assert
        await Assert.That(taxRate).IsEqualTo(0.0);
    }

    [Test]
    public async Task GetProductTaxRate_ImportedBookProduct_ReturnsImportTaxOnly()
    {
        // Arrange
        var importedBook = new Product("imported book", ProductCategory.Book, 12.49, true);

        // Act
        var taxRate = _calculator.GetProductTaxRate(importedBook);

        // Assert
        await Assert.That(taxRate).IsEqualTo(IMPORT_TAX_RATE);
    }

    [Test]
    public async Task GetProductTaxRate_MedicalProduct_ReturnsZeroTax()
    {
        // Arrange
        var pills = new Product("headache pills", ProductCategory.Medical, 9.75, false);

        // Act
        var taxRate = _calculator.GetProductTaxRate(pills);

        // Assert
        await Assert.That(taxRate).IsEqualTo(0.0);
    }

    [Test]
    public async Task GetProductTaxRate_ImportedMedicalProduct_ReturnsImportTaxOnly()
    {
        // Arrange
        var importedPills = new Product("imported headache pills", ProductCategory.Medical, 9.75, true);

        // Act
        var taxRate = _calculator.GetProductTaxRate(importedPills);

        // Assert
        await Assert.That(taxRate).IsEqualTo(IMPORT_TAX_RATE);
    }

    [Test]
    public async Task GetProductTaxRate_FoodProduct_ReturnsZeroTax()
    {
        // Arrange
        var chocolate = new Product("chocolate bar", ProductCategory.Food, 0.85, false);

        // Act
        var taxRate = _calculator.GetProductTaxRate(chocolate);

        // Assert
        await Assert.That(taxRate).IsEqualTo(0.0);
    }

    [Test]
    public async Task GetProductTaxRate_ImportedFoodProduct_ReturnsImportTaxOnly()
    {
        // Arrange
        var importedChocolate = new Product("imported chocolates", ProductCategory.Food, 10.00, true);

        // Act
        var taxRate = _calculator.GetProductTaxRate(importedChocolate);

        // Assert
        await Assert.That(taxRate).IsEqualTo(IMPORT_TAX_RATE);
    }

    [Test]
    public async Task GetProductTaxRate_OtherProduct_ReturnsSalesTaxOnly()
    {
        // Arrange
        var cd = new Product("music CD", ProductCategory.Other, 14.99, false);

        // Act
        var taxRate = _calculator.GetProductTaxRate(cd);

        // Assert
        await Assert.That(taxRate).IsEqualTo(BASIC_SALES_TAX_RATE);
    }

    [Test]
    public async Task GetProductTaxRate_ImportedOtherProduct_ReturnsBothTaxes()
    {
        // Arrange
        var importedPerfume = new Product("imported perfume", ProductCategory.Other, 47.50, true);

        // Act
        var taxRate = _calculator.GetProductTaxRate(importedPerfume);

        // Assert
        await Assert.That(taxRate).IsEqualTo(BASIC_SALES_TAX_RATE + IMPORT_TAX_RATE);
    }
}
