using SalesTax;
using SalesTax.Tests.Data;

namespace SalesTax.Tests;

public class Tests
{
    [Test]
    [Arguments(1, 2, 3)]
    [Arguments(2, 3, 5)]
    public async Task DataDrivenArguments(int a, int b, int c)
    {
        Console.WriteLine("This one can accept arguments from an attribute");

        var result = a + b;

        await Assert.That(result).IsEqualTo(c);
    }

}