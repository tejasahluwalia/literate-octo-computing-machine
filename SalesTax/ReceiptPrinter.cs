namespace SalesTax
{
        public static class ReceiptPrinter
    {
        public static void PrintReceipt(Receipt receipt)
        {
            foreach (var line in receipt.Lines)
            {
                Console.WriteLine($"{line.Quantity} {line.ProductName}: {line.Total:0.00}");
            }
            Console.WriteLine($"Sales Taxes: {receipt.TotalTax:0.00}");
            Console.WriteLine($"Total: {receipt.TotalAmount:0.00}");
        }
    }
}