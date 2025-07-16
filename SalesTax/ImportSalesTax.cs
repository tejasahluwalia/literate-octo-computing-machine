namespace SalesTax
{
    public record ImportSalesTax : Tax
    {
        public ImportSalesTax(decimal rate) : base(rate) { }

        public override bool IsProductExempt(Product product)
        {
            return !product.IsImported;
        }
    }
}