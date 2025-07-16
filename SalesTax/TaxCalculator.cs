namespace SalesTax
{
    public class TaxCalculator(decimal salesTaxRate, decimal importTaxRate)
    {
        private readonly decimal _salesTaxRate = salesTaxRate;
        private readonly decimal _importTaxRate = importTaxRate;

        public decimal CalculateTax(Product product, int quantity)
        {
            decimal baseAmount = product.Price * quantity;
            decimal taxRate = GetTaxRate(product);
            decimal tax = baseAmount * taxRate;
            
            // Round up to nearest 0.05
            return Math.Ceiling(tax * 20) / 20;
        }

        private decimal GetTaxRate(Product product)
        {
            decimal salesTax = product.IsExemptFromSalesTax() ? 0 : _salesTaxRate;
            decimal importTax = product.IsImported ? _importTaxRate : 0;
            return salesTax + importTax;
        }
    }
}