using Lab3;
namespace TestLogicalFunctionMinimizator
{
    public class IsSKNFTests
    {
        [Fact]
        public void TestIsSKNF()
        {
            Composite expr = (Composite)Parser.Parse("(a|b|c)&(a|!b|c)&(!a|!b|!c)");
            LogicalFunctionsMinimizator l = new(expr);
            Assert.True(l.IsSKNF(expr));
        }

        [Fact]
        public void TestNotSKNF()
        {
            Composite expr = (Composite)Parser.Parse("(a|b|(a|b))&(a|b)");
            LogicalFunctionsMinimizator l = new(expr);
            Assert.False(l.IsSKNF(expr));
        }

        [Fact]
        public void TestNotSKNF11()
        {
            Composite expr = (Composite)Parser.Parse("(a&b)|(a&b&c)");
            LogicalFunctionsMinimizator l = new(expr);
            Assert.False(l.IsSKNF(expr));
        }

        [Fact]
        public void TestNotSKNF1()
        {
            Composite expr = Parser.Parse("(a|b)&(a|b)");
            LogicalFunctionsMinimizator l = new(expr);
            Assert.False(l.IsSKNF(expr));
        }

        [Fact]
        public void TestNotSKNF2()
        {
            Composite expr = Parser.Parse("(a|b|(a|b))|(a|b)");
            LogicalFunctionsMinimizator l = new(expr);
            Assert.False(l.IsSKNF(expr));
        }

        [Fact]
        public void ConvertSKNFToBynary()
        {
            Composite expr = Parser.Parse("(a|b|c)&(a|!b|c)&(!a|!b|!c)");
            LogicalFunctionsMinimizator l = new(expr);
            var boolSKNF = new List<List<bool?>>
            {
                new List<bool ?> { true, true,true },
                new List<bool ?> { true,false, true },
                 new List<bool ?> { false,false, false }
            };
            Assert.Equal(boolSKNF, l.SKNF);
        }
    }
}
