using System.Text;

namespace SalesTax
{
    public class Receipt
    {
        private readonly List<ReceiptLine> _lines = [];

        public Receipt(Basket basket, TaxPolicy taxPolicy)
        {
            foreach (var (product, quantity) in basket.GetItems())
            {
                _lines.Add(new ReceiptLine(quantity, product, taxPolicy));
            }
        }

        public IReadOnlyList<ReceiptLine> Lines => _lines.AsReadOnly();

        public decimal GetTotalTax() {
            return _lines.Sum(line => line.Tax);
        }

        public decimal GetTotal() {
            return _lines.Sum(line => line.Total);
        }   

        public override string ToString()
        {
            var result = new StringBuilder();
            foreach (var line in _lines)
                result.AppendLine(line.ToString());

            result.AppendLine($"Sales Taxes: {GetTotalTax():F2}");
            result.AppendLine($"Total: {GetTotal():F2}");
            return result.ToString();
        }
    }
}