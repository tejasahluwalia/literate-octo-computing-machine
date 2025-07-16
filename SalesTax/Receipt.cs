using System.Text;

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

    public class Receipt
    {
        private readonly List<ReceiptLine> _lines = [];
        private readonly decimal _totalTax = 0;
        private readonly decimal _totalAmount = 0;

        public Receipt(Basket basket, TaxCalculator taxCalculator)
        {
            foreach (var (product, quantity) in basket.GetItems())
            {
                decimal baseAmount = product.Price * quantity;
                decimal tax = taxCalculator.CalculateTax(product, quantity);
                decimal lineTotal = baseAmount + tax;

                _lines.Add(new ReceiptLine(quantity, product.Name, baseAmount, tax, lineTotal));
                _totalAmount += baseAmount;
                _totalTax += tax;
            }
        }

        public IReadOnlyList<ReceiptLine> Lines => _lines.AsReadOnly();

        public decimal TotalSalesTax => _totalTax;
        
        public decimal Total => _totalAmount + _totalTax;

        public override string ToString()
        {
            var result = new StringBuilder();
            foreach (var line in _lines)
                result.AppendLine(line.ToString());

            result.AppendLine($"Sales Taxes: {TotalSalesTax:F2}");
            result.AppendLine($"Total: {Total:F2}");
            return result.ToString();
        }
    }
}