namespace SalesTax

{
    public static class ReceiptGenerator
    {
        public static void PrintTotal(Basket basket, TaxRateCalculator taxRateCalculator)
        {
            double totalAmount = 0.0;
            double totalTax = 0.0;

            foreach ((Product product, int qty) in basket.Products)
            {
                double taxRate = taxRateCalculator.GetProductTaxRate(product);
                double amount = (double)(product.Price * qty);
                double tax = amount * taxRate; // TODO: Round-up to 0.05
                Console.WriteLine($"{qty} {product.Name}: {amount + tax}");
                totalAmount += amount;
                totalTax += tax;
            }

            Console.WriteLine($"Sales Taxes: {totalTax}");
            Console.WriteLine($"Total: {totalTax + totalAmount}");
        }
    }
}