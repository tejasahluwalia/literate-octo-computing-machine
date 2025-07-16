namespace SalesTax
{
    public class ReceiptLine
    {
        public readonly string ProductName;
        public readonly decimal Amount;
        public readonly decimal Tax;
        public readonly decimal Total;
        public readonly int Quantity;

        public ReceiptLine(int quantity, Product product, TaxPolicy taxPolicy)
        {
            Quantity = quantity;
            ProductName = product.Name;

            decimal unitTax = TaxCalculator.CalculateTaxForUnit(product, taxPolicy);
            Tax = unitTax * quantity;

            Amount = product.Price * quantity;

            Total = Amount + Tax;
        }

        public override string ToString() => $"{Quantity} {ProductName}: {Total:0.00}";
    };
}