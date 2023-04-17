using System;

class Program
{
    static double f(double x)
    {
        return 5 * x * x + 1;
    }

    static void Main(string[] args)
    {
        double T = 100.0;
        double wspolczynnikChlodzenia = 0.03;
        int liczbaIteracji = 10000;

        Random random = new Random();
        double x = random.NextDouble() * 10 - 5;

        double minimum = x;
        double wartoscWMinimum = f(x);

        for (int i = 0; i < liczbaIteracji; i++)
        {
            double noweX = x + random.NextDouble() * 2 - 1;

            double roznica = f(noweX) - f(x);

            if (roznica < 0 || Math.Exp(-roznica / T) > random.NextDouble())
            {
                x = noweX;
            }

            if (f(x) < wartoscWMinimum)
            {
                minimum = x;
                wartoscWMinimum = f(x);
            }

            T *= 1 - wspolczynnikChlodzenia;
        }

        Console.WriteLine("Minimum: {0}, Wartość w tym minimum = {1}", minimum, wartoscWMinimum);
    }
}