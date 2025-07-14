namespace SalesTax
{
    public readonly record struct ReceiptLine(int Quantity, string ProductName, double Amount, double Tax, double Total);
    
    public readonly record struct Receipt(List<ReceiptLine> Lines, double TotalTax, double TotalAmount);

    public static class ReceiptGenerator
    {
        public static Receipt GenerateReceipt(Basket basket, TaxRateCalculator taxRateCalculator)
        {
            var lines = new List<ReceiptLine>();
            double totalAmount = 0.0;
            double totalTax = 0.0;

            foreach ((Product product, int qty) in basket.Products)
            {
                double taxRate = taxRateCalculator.GetProductTaxRate(product);
                double amount = (double)(product.Price * qty);
                double tax = Math.Ceiling(amount * taxRate * 20) / 20.0; // Round up to nearest 0.05
                double lineTotal = amount + tax;
                
                lines.Add(new ReceiptLine(qty, product.Name, amount, tax, lineTotal));
                totalAmount += amount;
                totalTax += tax;
            }

            return new Receipt(lines, totalTax, totalAmount + totalTax);
        }

        public static void PrintTotal(Basket basket, TaxRateCalculator taxRateCalculator)
        {
            var receipt = GenerateReceipt(basket, taxRateCalculator);
            
            foreach (var line in receipt.Lines)
            {
                Console.WriteLine($"{line.Quantity} {line.ProductName}: {line.Total.ToString("0.00")}");
            }

            Console.WriteLine($"Sales Taxes: {receipt.TotalTax.ToString("0.00")}");
            Console.WriteLine($"Total: {receipt.TotalAmount.ToString("0.00")}");
        }
    }
}
