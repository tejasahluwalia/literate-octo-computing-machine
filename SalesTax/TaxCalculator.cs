namespace SalesTax
{
public class TaxCalculator
  {
      public static decimal CalculateTaxForUnit(Product product, TaxPolicy taxPolicy)
      {
          decimal taxRate = product.GetTaxRate(taxPolicy);
          decimal taxAmount = product.Price * taxRate;
          return Math.Ceiling(taxAmount / 0.05m) * 0.05m;
      }
  }
}