using SalesTax;

Console.WriteLine("Running SalesTax");

double BASIC_SALES_TAX_RATE = 10.0/100;
double IMPORT_TAX_RATE = 5.0/100;

TaxRateCalculator taxRateCalculator = new(BASIC_SALES_TAX_RATE, IMPORT_TAX_RATE);

var filename = args[0];
var input = Utils.ReadFrom(filename);
var basket = OrderParser.ParseOrder(input);

ReceiptGenerator.PrintTotal(basket, taxRateCalculator);