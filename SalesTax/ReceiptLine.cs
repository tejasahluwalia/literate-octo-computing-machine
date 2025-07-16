namespace SalesTax
{
    public class ReceiptLine(int quantity, string productName, decimal amount, decimal tax, decimal total)
    {
        public readonly int Quantity = quantity;
        public readonly string ProductName = productName;
        public readonly decimal Amount = amount;
        public readonly decimal Tax = tax;
        public readonly decimal Total = total;

        public override string ToString() => $"{Quantity} {ProductName}: {Total:0.00}";
    };
}