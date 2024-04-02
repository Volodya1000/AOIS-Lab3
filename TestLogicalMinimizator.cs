using Lab3;
namespace TestLogicalFunctionMinimizator
{
    public class TestLogicalFunctionMinimizator
    {
        [Fact]
        public void ConvertBynaryToLetters() 
        {
            Composite expr = Parser.Parse("(a|b|c)&(a|!b|c)&(!a|!b|!c)");
            LogicalFunctionsMinimizator l = new(expr);
            string s = l.GenerateString(l.SKNF,false);
            Assert.Equal("(a|b|c)&(a|!b|c)&(!a|!b|!c)", s);
        }

        [Fact]
        public void ChangeSDNF()
        {
            Composite expr = Parser.Parse("(!a&b&c)|(a&!b&!c)|(a&!b&c)|(a&b&!c)|(a&b&c)");
            LogicalFunctionsMinimizator l = new(expr);
            List<List<bool?>> Min= l.CalculationMethod(l.SDNF);
            string s = l.GenerateString(Min,true);
            Assert.Equal("(b&c)|a", s);
        }

        [Fact]
        public void ChangeSDNF1()
        {
            Composite expr = Parser.Parse("(!a&b&!c)|(a&!b&!c)|(!a&!b&c)|(a&b&!c)|(a&b&c)");
            LogicalFunctionsMinimizator l = new(expr);
            List<List<bool?>> Min = l.CalculationMethod(l.SDNF);
            string s = l.GenerateString(Min,true);
            Assert.Equal("(!a&!b&c)|(b&!c)|(a&!c)|(a&b)", s);
        }
       

        [Fact]
        public void ChangeSKNF2()
        {
            Composite expr = Parser.Parse("(!a|b|c|d)&(!a|b|!c|d)");
            LogicalFunctionsMinimizator l = new(expr);
            string s = l.CalculationMethodRez;
            Assert.Equal("(!a|b|d)", s);
        }

        [Fact]
        public void ImlicantSKNF()
        {
            Composite expr = Parser.Parse("(!a&!b&c)|(!a&b&!c)|(!a&b&c)|(a&b&!c)");
            LogicalFunctionsMinimizator l = new(expr);
            List<List<bool?>> Min = l.CalculationMethod(l.SDNF);
            Min = l.DeleteImlications(Min);
            string s = l.GenerateString(Min, false);
           Assert.Equal("(!a|c)&(b|!c)", s);
        }

        [Fact]
        public void ImlicantSKNF1()
        {
            Composite expr = Parser.Parse("(!a&!b&c)|(!a&b&!c)|(!a&b&c)|(a&b&!c)");
            LogicalFunctionsMinimizator l = new(expr);
            string s = l.CalculationTableMethodRez;
            Assert.Equal("(!a&c)|(b&!c)", s);
        }


    }
}