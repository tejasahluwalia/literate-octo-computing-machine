using SalesTax;

namespace SalesTax.Tests;

public class OrderParserTests
{
    [Test]
    public async Task ParseProductLine_BasicProduct_ParsesCorrectly()
    {
        // Arrange
        var line = "1 book at 12.49";

        // Act
        var product = OrderParser.ParseProductLine(line);

        // Assert
        await Assert.That(product.Name).IsEqualTo("book");
        await Assert.That(product.Category).IsEqualTo(ProductCategory.Book);
        await Assert.That(product.Price).IsEqualTo(12.49);
        await Assert.That(product.IsImported).IsFalse();
    }

    [Test]
    public async Task ParseProductLine_ImportedProduct_ParsesCorrectly()
    {
        // Arrange
        var line = "1 imported bottle of perfume at 47.50";

        // Act
        var product = OrderParser.ParseProductLine(line);

        // Assert
        await Assert.That(product.Name).IsEqualTo("imported bottle of perfume");
        await Assert.That(product.Category).IsEqualTo(ProductCategory.Other);
        await Assert.That(product.Price).IsEqualTo(47.50);
        await Assert.That(product.IsImported).IsTrue();
    }

    [Test]
    public async Task ParseProductLine_MedicalProduct_ParsesCorrectly()
    {
        // Arrange
        var line = "1 packet of headache pills at 9.75";

        // Act
        var product = OrderParser.ParseProductLine(line);

        // Assert
        await Assert.That(product.Name).IsEqualTo("packet of headache pills");
        await Assert.That(product.Category).IsEqualTo(ProductCategory.Medical);
        await Assert.That(product.Price).IsEqualTo(9.75);
        await Assert.That(product.IsImported).IsFalse();
    }

    [Test]
    public async Task ParseProductLine_FoodProduct_ParsesCorrectly()
    {
        // Arrange
        var line = "1 chocolate bar at 0.85";

        // Act
        var product = OrderParser.ParseProductLine(line);

        // Assert
        await Assert.That(product.Name).IsEqualTo("chocolate bar");
        await Assert.That(product.Category).IsEqualTo(ProductCategory.Food);
        await Assert.That(product.Price).IsEqualTo(0.85);
        await Assert.That(product.IsImported).IsFalse();
    }

    [Test]
    public async Task ParseQuantityFromLine_SingleQuantity_ParsesCorrectly()
    {
        // Arrange
        var line = "1 book at 12.49";

        // Act
        var quantity = OrderParser.ParseQuantityFromLine(line);

        // Assert
        await Assert.That(quantity).IsEqualTo(1);
    }

    [Test]
    public async Task ParseQuantityFromLine_MultipleQuantity_ParsesCorrectly()
    {
        // Arrange
        var line = "5 chocolate bars at 0.85";

        // Act
        var quantity = OrderParser.ParseQuantityFromLine(line);

        // Assert
        await Assert.That(quantity).IsEqualTo(5);
    }

    [Test]
    public async Task ParseOrder_MultipleLines_CreatesBasketCorrectly()
    {
        // Arrange
        var lines = new[]
        {
            "1 book at 12.49",
            "2 music CD at 14.99",
            "1 chocolate bar at 0.85"
        };

        // Act
        var basket = OrderParser.ParseOrder(lines);

        // Assert
        await Assert.That(basket.Products).HasCount().EqualTo(3);
        
        var bookProduct = basket.Products.Keys.First(p => p.Name == "book");
        await Assert.That(basket.Products[bookProduct]).IsEqualTo(1);
        
        var cdProduct = basket.Products.Keys.First(p => p.Name == "music CD");
        await Assert.That(basket.Products[cdProduct]).IsEqualTo(2);
        
        var chocolateProduct = basket.Products.Keys.First(p => p.Name == "chocolate bar");
        await Assert.That(basket.Products[chocolateProduct]).IsEqualTo(1);
    }
}
