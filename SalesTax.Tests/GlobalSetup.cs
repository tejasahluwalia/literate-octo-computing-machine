[assembly: Retry(3)]
[assembly: System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]

namespace SalesTax.Tests;
public class GlobalHooks
{
    [Before(TestSession)]
    public static void SetUp()
    {
        Console.WriteLine(@"Or you can define methods that do stuff before...");
    }

    [After(TestSession)]
    public static void CleanUp()
    {
        Console.WriteLine(@"...and after!");
    }
}