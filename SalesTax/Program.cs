using SalesTax;

Console.WriteLine("Running Sales Tax");
Console.WriteLine("Enter products one by one (format: '1 book at 12.49')");
Console.WriteLine("Enter 'done' to checkout");

decimal BASIC_SALES_TAX_RATE = 10.0m/100;
decimal IMPORT_TAX_RATE = 5.0m/100;

var taxCalculator = new TaxCalculator(BASIC_SALES_TAX_RATE, IMPORT_TAX_RATE);
var receiptGenerator = new ReceiptGenerator(taxCalculator);
var basket = new Basket();

while (true)
{
    Console.Write("Enter product: ");
    var input = Console.ReadLine();
    
    if (string.IsNullOrWhiteSpace(input))
        continue;

    if (string.Equals(input, "done"))
        break;

    try
    {
        var product = OrderParser.ParseProductLine(input);
        var quantity = OrderParser.ParseQuantityFromLine(input);
        basket.AddProduct(product, quantity);
        Console.WriteLine($"Added {quantity} x {product.Name} to basket");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        Console.WriteLine("Please try again with format: '1 book at 12.49'");
        continue;
    }
    
    Console.WriteLine("\nCurrent basket:");
    foreach (var (product, quantity) in basket.GetItems())
    {
        Console.WriteLine($"  {quantity} x {product.Name} - {product.Price:0.00} each");
    }
    
    Console.Write("\nAdd more products? (y/n): ");
    var response = Console.ReadLine();
    if (response?.ToLower() == "n")
        break;
        
    Console.WriteLine();
}

Console.WriteLine("\n--- RECEIPT ---");
var receipt = receiptGenerator.GenerateReceipt(basket);
ReceiptPrinter.PrintReceipt(receipt);