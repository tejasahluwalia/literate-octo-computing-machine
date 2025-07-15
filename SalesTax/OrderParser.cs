namespace SalesTax
{
    public static class OrderParser
    {
        public static Basket ParseOrder(IEnumerable<string> lines)
        {
            var basket = new Basket();
            
            foreach (var line in lines)
            {
                var product = ParseProductLine(line);
                var quantity = ParseQuantityFromLine(line);
                basket.AddProduct(product, quantity);
            }
            
            return basket;
        }
        
        public static Product ParseProductLine(string line)
        {
            string[] substrings = line.Split([" at "], 2, StringSplitOptions.TrimEntries);
            bool canParsePrice = decimal.TryParse(substrings[1], out decimal price);
            
            if (!canParsePrice)
                throw new ArgumentException($"Could not parse price from line: {line}");
                
            int indexOfSpace = substrings[0].IndexOf(" ");
            string name = substrings[0][indexOfSpace..].Trim();
            bool isImported = name.Contains("imported");
            
            ProductCategory category = name.Contains("book") ? ProductCategory.Book :
                name.Contains("pills") ? ProductCategory.Medical :
                name.Contains("chocolate") ? ProductCategory.Food :
                ProductCategory.Other;
                
            return new Product(name, category, price, isImported);
        }
        
        public static int ParseQuantityFromLine(string line)
        {
            int indexOfSpace = line.IndexOf(' ');
            bool canParseQuantity = int.TryParse(line[..indexOfSpace], out int qty);
            
            if (!canParseQuantity)
                throw new ArgumentException($"Could not parse quantity from line: {line}");
                
            return qty;
        }
    }
}
