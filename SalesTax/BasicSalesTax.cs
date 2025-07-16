namespace SalesTax
{
    public record BasicSalesTax : Tax
    {
        public BasicSalesTax(decimal rate) : base(rate) { }

        public override bool IsProductExempt(Product product)
        {
            List<ProductCategory> exemptCategories =
            [
                ProductCategory.Food,
                ProductCategory.Book,
                ProductCategory.Medical
            ];
            return exemptCategories.Contains(product.Category);
        }
    }
}