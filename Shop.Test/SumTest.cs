using MyCalculator;

namespace Shop.Test
{
    public class SumTest
    {
        [Fact]
        public void CalculatorSum()
        {
            int a = 5, b = 10;

            var calculator = new MyCalculator.Calculator { A = a, B = b };
            var Sum = calculator.Sum();
            var Subtract = calculator.Subtract();
            var Multiply = calculator.Multiply();
            var Square = calculator.Square();

            Assert.Equal(a + b, Sum);
            Assert.Equal(a - b, Subtract);
            Assert.Equal(a * b, Multiply);
            Assert.Equal(a * a, Square);
        }
    }
}
