using Fractions;
using Radon.Reckon.Model.Equation;

namespace Radon.Reckon.Economy;

public static class MarketEquilibrium
{
    public static void MultiEquilibrium(
        Linear2DEq demand1,
        Linear2DEq supply1,
        Linear2DEq demand2,
        Linear2DEq supply2
    )
    {
        var (a0, a1, a2) = demand1;
        var (b0, b1, b2) = supply1;
        var (A0, A1, A2) = demand2;
        var (B0, B1, B2) = supply2;

        var c0 = a0 - b0;
        var c1 = a1 - b1;
        var c2 = a2 - b2;
        var C0 = A0 - B0;
        var C1 = A1 - B1;
        var C2 = A2 - B2;

        var p1 = new Fraction(c2 * C0 - c0 * C2, c1 * C2 - c2 * C1);
        var p2 = new Fraction(c0 * C1 - c1 * C0, c1 * C2 - c2 * C1);

        var q1 = new Fraction(a0) + new Fraction(a1) * p1 + new Fraction(a2) * p2;
        var q2 = new Fraction(A0) + new Fraction(A1) * p1 + new Fraction(A2) * p2;

        Console.WriteLine($"p1: {p1}");
        Console.WriteLine($"p2: {p2}");
        Console.WriteLine($"q1: {q1}");
        Console.WriteLine($"q2: {q2}");
    }
}
