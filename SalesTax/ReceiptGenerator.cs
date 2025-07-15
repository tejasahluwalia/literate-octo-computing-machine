namespace SalesTax
{
    public readonly record struct ReceiptLine(int Quantity, string ProductName, decimal Amount, decimal Tax, decimal Total);
    
    public readonly record struct Receipt(List<ReceiptLine> Lines, decimal TotalTax, decimal TotalAmount);

    public class ReceiptGenerator(TaxCalculator taxCalculator)
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
