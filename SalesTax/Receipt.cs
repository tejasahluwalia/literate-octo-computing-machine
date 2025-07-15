using System.Text;

namespace SalesTax
{
    public class ReceiptLine(int Quantity, string ProductName, decimal Amount, decimal Tax, decimal Total)
    {
        public int Quantity { get; set; } = Quantity;
        public string ProductName { get; set; } = ProductName;
        public decimal Amount { get; set; } = Amount;
        public decimal Tax { get; set; } = Tax;
        public decimal Total { get; set; } = Total;

        public override string ToString() => $"{Quantity} {ProductName}: {Total:0.00}";
    };

    public class Receipt(List<ReceiptLine> Lines, decimal TotalTax, decimal TotalAmount)
    {
        public List<ReceiptLine> Lines { get; set; } = Lines;
        public decimal TotalTax { get; set; } = TotalTax;
        public decimal TotalAmount { get; set; } = TotalAmount;

        public override string ToString()
        {
            var result = new StringBuilder();
            foreach (var line in Lines)
                result.AppendLine(line.ToString());

            result.AppendLine($"Sales Taxes: {TotalTax:F2}");
            result.AppendLine($"Total: {TotalAmount:F2}");
            return result.ToString();
        }
    };

    public class ReceiptService(TaxCalculator taxCalculator)
    {
        private readonly TaxCalculator _taxCalculator = taxCalculator;

        public Receipt GenerateReceipt(Basket basket)
        {
            var lines = new List<ReceiptLine>();
            decimal totalTax = 0;
            decimal totalAmount = 0;

            foreach (var (product, quantity) in basket.GetItems())
            {
                decimal baseAmount = product.CalculateBaseAmount(quantity);
                decimal tax = _taxCalculator.CalculateTax(product, quantity);
                decimal lineTotal = baseAmount + tax;

                lines.Add(new ReceiptLine(quantity, product.Name, baseAmount, tax, lineTotal));
                totalAmount += baseAmount;
                totalTax += tax;
            }

            return new Receipt(lines, totalTax, totalAmount + totalTax);
        }
    }
}