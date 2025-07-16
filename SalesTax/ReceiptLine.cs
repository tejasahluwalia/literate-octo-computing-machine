namespace SalesTax
{
    public class ReceiptLine
    {
        public readonly string ProductName;
        public readonly decimal Amount;
        public readonly decimal Tax;
        public readonly decimal Total;
        public readonly int Quantity;

        public ReceiptLine(int quantity, Product product, TaxStrategy taxStrategy)
        {
            Quantity = quantity;
            ProductName = product.Name;
            Amount = product.Price * quantity;
            var taxAmount = Amount * product.GetTaxRate(taxStrategy);
            Tax = Math.Ceiling(taxAmount / 0.05m) * 0.05m;
            
            Total = Amount + Tax;
        }

        public override string ToString() => $"{Quantity} {ProductName}: {Total:0.00}";
    };
}