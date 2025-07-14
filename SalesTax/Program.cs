using SalesTax;

Console.WriteLine("Running SalesTax");

double BASIC_SALES_TAX_RATE = 10.0/100;
double IMPORT_TAX_RATE = 5.0/100;

TaxRateCalculator taxRateCalculator = new(BASIC_SALES_TAX_RATE, IMPORT_TAX_RATE);
Basket basket = new();

var filename = args[0];

var input = Utils.ReadFrom(filename);

foreach (var line in input)
{
    string[] substrings = line.Split([" at "], 2, StringSplitOptions.TrimEntries);
    bool canParsePrice = double.TryParse(substrings[1], out double price);
    int indexOfSpace = substrings[0].IndexOf(" ");
    bool canParseQuantity = int.TryParse(substrings[0][..indexOfSpace], out int qty);
    string name = substrings[0][indexOfSpace..].Trim();
    bool isImported = name.Contains("imported");
    ProductCategory category = name.Contains("book") ? ProductCategory.Book :
        name.Contains("pills") ? ProductCategory.Medical :
        name.Contains("chocolate") ? ProductCategory.Food :
        ProductCategory.Other;
    Product product = new(name, category, price, isImported);
    basket.AddProduct(product, qty);
}

ReceiptGenerator.PrintTotal(basket, taxRateCalculator);