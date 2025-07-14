namespace SalesTax
{
    public class TaxRateCalculator(double salesTaxRate, double importTaxRate)
    {
        private readonly double _salesTaxRate = salesTaxRate;
        private readonly double _importTaxRate = importTaxRate;

        public double GetProductTaxRate(Product product)
        {
            double taxRate = (product.Category, product.IsImported) switch
            {
                (ProductCategory.Other, true) => _salesTaxRate + _importTaxRate,
                (ProductCategory.Other, false) => _salesTaxRate,
                (_, true) => _importTaxRate,
                (_, false) => 0.0
            };

            return taxRate;
        }

    }
}