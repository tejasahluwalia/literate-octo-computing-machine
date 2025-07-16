using SalesTax;

Console.WriteLine("Running Sales Tax\n");
Console.WriteLine("Enter products in the format '1 book at 12.49' to add them to the basket.");
Console.WriteLine("When finished, press 'Enter' to generate the receipt.\n");
Console.WriteLine("ORDER INPUT:");

decimal BASIC_SALES_TAX_RATE = 10.0m/100;
decimal IMPORT_TAX_RATE = 5.0m/100;

Basket basket = new();
BasicSalesTax basicSalesTax = new(BASIC_SALES_TAX_RATE);
ImportSalesTax importSalesTax = new(IMPORT_TAX_RATE);
TaxStrategy taxStrategy = new([basicSalesTax, importSalesTax]);

while (true)
{
    var input = Console.ReadLine();
    
    if (string.IsNullOrWhiteSpace(input))
        break;
        
    if (string.Equals(input.ToLower().Trim(), "done"))
        break;
    
    try
    {
        var product = OrderParser.ParseProductLine(input);
        var quantity = OrderParser.ParseQuantityFromLine(input);
        basket.AddProduct(product, quantity);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error parsing line '{input}': {ex.Message}");
        continue;
    }
}

Console.WriteLine("\n--- RECEIPT ---");
var receipt = new Receipt(basket, taxStrategy);
Console.WriteLine(receipt);