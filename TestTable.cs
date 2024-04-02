using Lab3;
 namespace TestLogicalFunctionMinimizator
{
    public class TestTable
    {
        [Fact]
        public void MinimzSDNF()
        {
            Composite expr = Parser.Parse("(!a&b&c)|(a&!b&!c)|(a&!b&c)|(a&b&!c)|(a&b&c)");
            LogicalFunctionsMinimizator l = new(expr);
            Assert.Equal("a|(b&c)", l.KarnoMapMethodRez);
        }

        [Fact]
        public void MinimzSKNF()
        {
            Composite expr = Parser.Parse("(!a|b|c)&(a|!b|!c)&(!a|!b|!c)&(a|b|!c)&(a|b|c)");
            LogicalFunctionsMinimizator l = new(expr);
            Assert.Equal("(b|c)&(!b|!c)&(a|!c)&(a|b)", l.KarnoMapMethodRez);
        }

        [Fact]
        public void MinimzSKNF1()
        {
            Composite expr = Parser.Parse("(!a|b)&(a|!b)&(a|b)");
            LogicalFunctionsMinimizator l = new(expr);
            Assert.Equal("a&b", l.KarnoMapMethodRez);
        }

        [Fact]
        public void MinimzSKNF11()
        {
            Composite expr = Parser.Parse("(!a&b)|(a&!b)|(a&b)");
            LogicalFunctionsMinimizator l = new(expr);
            Assert.Equal("a|b", l.KarnoMapMethodRez);
        }

        [Fact]
        public void MinimzSDNF1()
        {
            Composite expr = Parser.Parse("(!a&b)|(a&!b)|(a&b)");
            LogicalFunctionsMinimizator l = new(expr);
            Assert.Equal("b|a", l.KarnoMapMethodRez);
        }


        [Fact]
        public void MinimzSKNF2()
        {
            Composite expr = Parser.Parse("(!a|b)&(a|!b)&(a|b)&(!a|!b)");
            LogicalFunctionsMinimizator l = new(expr);
            Assert.Equal("0", l.KarnoMapMethodRez);
        }

        [Fact]
        public void MinimzSDNF2()
        {
            Composite expr = Parser.Parse("(!a&b)|(a&!b)|(a&b)|(!a&!b)");
            LogicalFunctionsMinimizator l = new(expr);
            Assert.Equal("1", l.KarnoMapMethodRez);
        }


    }
}
