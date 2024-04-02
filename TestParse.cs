using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab3;

namespace TestLogicalFunctionMinimizator
{
    public class TestParse
    {
        [Fact]
        public void T1()
        {
            IComponent parsedExpression = Parser.Parse("!(A|!B|!C)");
            string rpnRez = Parser.ConvertToRPN(parsedExpression);
            Assert.Equal("AB!C!|!", rpnRez); 
        }

        [Fact]
        public void T2()
        {
            IComponent parsedExpression = Parser.Parse("A&B|C&D");
            string rpnRez = Parser.ConvertToRPN(parsedExpression);
            Assert.Equal("AB&CD&|", rpnRez);
        }

        [Fact]
        public void TT2()
        {
            IComponent parsedExpression = Parser.Parse("A|B&C|D");
            string rpnRez = Parser.ConvertToRPN(parsedExpression);
            Assert.Equal("ABC&D|", rpnRez);
        }

        [Fact]
        public void SDNF()
        {
            IComponent parsedExpression = Parser.Parse("(A&B&C&D)|(!A&!B&!C&!D)");
            string rpnRez = Parser.ConvertToRPN(parsedExpression);
            Assert.Equal("ABCD&A!B!C!D!&|", rpnRez);
        }


        [Fact]
        public void SKNF()
        {
            IComponent parsedExpression = Parser.Parse("(A|B|C)&(!A|!B|!C)");
            string rpnRez = Parser.ConvertToRPN(parsedExpression);
            Assert.Equal("ABC|A!B!C!|&", rpnRez);
        }


        [Fact]
        public void T5()
        {
            IComponent parsedExpression = Parser.Parse("!(!A|(!B&(!C|!D)))");
            string rpnRez = Parser.ConvertToRPN(parsedExpression);
            Assert.Equal("A!B!C!D!|&|!", rpnRez);
        }

        [Fact]
        public void T6()
        {
            IComponent parsedExpression = Parser.Parse("A&B|C");
            string rpnRez = Parser.ConvertToRPN(parsedExpression);
            Assert.Equal("AB&C|", rpnRez);
        }

        [Fact]
        public void T7()
        {
            IComponent parsedExpression = Parser.Parse("A|B&C");
            string rpnRez = Parser.ConvertToRPN(parsedExpression);
            Assert.Equal("ABC&|", rpnRez);
        }

        [Fact]
        public void T8()
        {
            IComponent parsedExpression = Parser.Parse("!(A&B|C)");
            string rpnRez = Parser.ConvertToRPN(parsedExpression);
            Assert.Equal("AB&C|!", rpnRez);
        }

        [Fact]
        public void T9()
        {
            IComponent parsedExpression = Parser.Parse("(!b&c)");
            string rpnRez = Parser.ConvertToRPN(parsedExpression);
            Assert.Equal("b!c&", rpnRez);
        }

        [Fact]
        public void T10()
        {
            IComponent parsedExpression = Parser.Parse("(!b|c)");// "(!a&b&!c)|(a&!b&!c)|(!a&!b&c)|(a&b&!c)|(a&b&c)"
            string rpnRez = Parser.ConvertToRPN(parsedExpression);
            Assert.Equal("b!c|", rpnRez);
        }


        [Fact]
        public void TestForInvalidInputStrings()
        {
            Assert.Throws<InvalidOperationException>(() => Parser.Parse("A&"));
        }

        [Fact]
        public void TestForInvalidInputStrings1()
        {
            Assert.Throws<InvalidOperationException>(() => Parser.Parse("A||B"));
        }

        [Fact]
        public void TestForInvalidInputStrings2()
        {
            Assert.Throws<InvalidOperationException>(() => Parser.Parse("!|A"));
        }

        [Fact]
        public void TestForInvalidInputStrings3()
        {
            Assert.Throws<InvalidOperationException>(() => Parser.Parse("A&("));
        }

        [Fact]
        public void TestForInvalidInputStrings4()
        {
            Assert.Throws<InvalidOperationException>(() => Parser.Parse("A&(B|C"));
        }

    }
}
