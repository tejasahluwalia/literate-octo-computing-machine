using SalesTax;

namespace SalesTax.Tests;

public class TaxCalculatorTests
{
    private const decimal BASIC_SALES_TAX_RATE = 10.0m / 100;
    private const decimal IMPORT_TAX_RATE = 5.0m / 100;
    private readonly TaxCalculator _calculator = new(BASIC_SALES_TAX_RATE, IMPORT_TAX_RATE);

    [Test]
    public async Task CalculateTax_BookProduct_ReturnsZeroTax()
    {
        // Arrange
        var book = new Product("book", ProductCategory.Book, 12.49m, false);

        // Act
        var tax = _calculator.CalculateTax(book, 1);

        // Assert
        await Assert.That(tax).IsEqualTo(0.0m);
    }

    [Test]
    public async Task CalculateTax_ImportedBookProduct_ReturnsImportTaxOnly()
    {
        // Arrange
        var importedBook = new Product("imported book", ProductCategory.Book, 12.49m, true);

        // Act
        var tax = _calculator.CalculateTax(importedBook, 1);

        // Assert
        // Expected: 12.49 * 0.05 = 0.6245, rounded up to nearest 0.05 = 0.65
        await Assert.That(tax).IsEqualTo(0.65m);
    }

    [Test]
    public async Task CalculateTax_MedicalProduct_ReturnsZeroTax()
    {
        // Arrange
        var pills = new Product("headache pills", ProductCategory.Medical, 9.75m, false);

        // Act
        var tax = _calculator.CalculateTax(pills, 1);

        // Assert
        await Assert.That(tax).IsEqualTo(0.0m);
    }

    [Test]
    public async Task CalculateTax_ImportedMedicalProduct_ReturnsImportTaxOnly()
    {
        // Arrange
        var importedPills = new Product("imported headache pills", ProductCategory.Medical, 9.75m, true);

        // Act
        var tax = _calculator.CalculateTax(importedPills, 1);

        // Assert
        // Expected: 9.75 * 0.05 = 0.4875, rounded up to nearest 0.05 = 0.50
        await Assert.That(tax).IsEqualTo(0.50m);
    }

    [Test]
    public async Task CalculateTax_FoodProduct_ReturnsZeroTax()
    {
        // Arrange
        var chocolate = new Product("chocolate bar", ProductCategory.Food, 0.85m, false);

        // Act
        var tax = _calculator.CalculateTax(chocolate, 1);

        // Assert
        await Assert.That(tax).IsEqualTo(0.0m);
    }

    [Test]
    public async Task CalculateTax_ImportedFoodProduct_ReturnsImportTaxOnly()
    {
        // Arrange
        var importedChocolate = new Product("imported chocolates", ProductCategory.Food, 10.00m, true);

        // Act
        var tax = _calculator.CalculateTax(importedChocolate, 1);

        // Assert
        // Expected: 10.00 * 0.05 = 0.50, already aligned to 0.05
        await Assert.That(tax).IsEqualTo(0.50m);
    }

    [Test]
    public async Task CalculateTax_OtherProduct_ReturnsSalesTaxOnly()
    {
        // Arrange
        var cd = new Product("music CD", ProductCategory.Other, 14.99m, false);

        // Act
        var tax = _calculator.CalculateTax(cd, 1);

        // Assert
        // Expected: 14.99 * 0.10 = 1.499, rounded up to nearest 0.05 = 1.50
        await Assert.That(tax).IsEqualTo(1.50m);
    }

    [Test]
    public async Task CalculateTax_ImportedOtherProduct_ReturnsBothTaxes()
    {
        // Arrange
        var importedPerfume = new Product("imported perfume", ProductCategory.Other, 47.50m, true);

        // Act
        var tax = _calculator.CalculateTax(importedPerfume, 1);

        // Assert
        // Expected: 47.50 * 0.15 = 7.125, rounded up to nearest 0.05 = 7.15
        await Assert.That(tax).IsEqualTo(7.15m);
    }

    [Test]
    public async Task CalculateTax_MultipleQuantity_CalculatesCorrectly()
    {
        // Arrange
        var cd = new Product("music CD", ProductCategory.Other, 14.99m, false);

        // Act
        var tax = _calculator.CalculateTax(cd, 2);

        // Assert
        // Expected: (14.99 * 2) * 0.10 = 2.998, rounded up to nearest 0.05 = 3.00
        await Assert.That(tax).IsEqualTo(3.00m);
    }
}
