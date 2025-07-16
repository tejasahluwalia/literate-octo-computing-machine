namespace SalesTax
{
    public abstract record Tax(decimal Rate)
    {
        public abstract bool IsProductExempt(Product product);
    };
}