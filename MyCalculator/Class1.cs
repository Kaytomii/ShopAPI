namespace MyCalculator;

public class Calculator
{
    public int A { get; set; }
    public int B { get; set; }

    public int Sum()
    {
        return A + B;
    }
    public int Subtract()
    {
        return A - B;
    }
    public int Multiply()
    {
        return A * B;
    }
    public int Square()
    {
        return A * A;
    }
}
