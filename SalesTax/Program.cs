using SalesTax;

Console.WriteLine("Running SalesTax");

double BASIC_SALES_TAX_RATE = 10.0;
double IMPORT_TAX_RATE = 5.0;

TaxRateCalculator taxRateCalculator = new(BASIC_SALES_TAX_RATE, IMPORT_TAX_RATE);
Basket basket = new();

var filename = args[0];

var input = Utils.ReadFrom(filename);

foreach (var line in input)
{
    // TODO: Parse input -> Products -> AddToBasket
    Console.WriteLine(line);
}

ReceiptGenerator.PrintTotal(basket, taxRateCalculator);