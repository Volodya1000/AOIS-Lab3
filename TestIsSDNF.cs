using Lab3;
namespace TestLogicalFunctionMinimizator
{
    public class IsSDNFTests
    {
        [Fact]
        public void TestIsSDNF()
        {
            Composite expr = Parser.Parse("(a&b&c)|(a&!b&c)|(!a&!b&!c)");
            LogicalFunctionsMinimizator l = new(expr);
            Assert.True(l.IsSDNF(expr));
        }

        [Fact]
        public void TestNotSDNF()
        {
            Composite expr = Parser.Parse("(a&b)|(a&b&c)");
            LogicalFunctionsMinimizator l = new(expr);
            Assert.False(l.IsSDNF(expr));
        }

        [Fact]
        public void TestNotSDNF11()
        {
            Composite expr = Parser.Parse("(a&b|(a&b))|(a&b)");
            LogicalFunctionsMinimizator l = new(expr);
            Assert.False(l.IsSDNF(expr));
        }

        [Fact]
        public void TestNotSDNF1()
        {
            Composite expr = Parser.Parse("(a&b)|(a&b)");
            LogicalFunctionsMinimizator l = new(expr);
            Assert.False(l.IsSDNF(expr));
        }

        [Fact]
        public void TestNotSDNF2()
        {
            Composite expr = Parser.Parse("(a&b&(a&b))&(a&b)");
            LogicalFunctionsMinimizator l = new(expr);
            Assert.False(l.IsSDNF(expr));
        }

        [Fact]
        public void ConvertSDNFToBynary()
        {
            Composite expr = Parser.Parse("(a&b&c)|(a&!b&c)|(!a&!b&!c)");
            LogicalFunctionsMinimizator l = new(expr);
            var boolSDNF = new List<List<bool?>>
            {
                new List<bool?> { true, true,true },
                new List<bool?> { true,false, true },
                 new List<bool?> { false,false, false }
            };
            Assert.Equal(boolSDNF, l.SDNF);
        }

      
    }
}
