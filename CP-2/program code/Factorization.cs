using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTA_cp2
{
    class Factorization
    {
        public List<double> CanRepOfNum(double p)
        {
            int count;
            double temp;
            List<double> n = new List<double>();
            var d = NumberSchedule(p);
            if (d.Count == 0)
                Console.WriteLine("Я не можу знайти канонiчний розклад числа:(");
            d.Sort();
            while (d.Count > 0) 
            {
                count = 0;
                temp = d.First();
                n.Add(temp);
                while(d.Contains(temp))
                {
                    count++;
                    d.RemoveAt(0);
                }
                n.Add(count);
            }
            return n;
        }

        public List<double> NumberSchedule(double p)
        {
            List<double> d = new List<double>();
            double k = 0;
            if (SoloveyStrassen(p))
            {
                d.Add(p); return d;
            }
            do
            {
                k = Trial(p);
                if (k != 1)
                {
                    p /= k;
                    d.Add(k);
                    if (SoloveyStrassen(p))
                    {
                        d.Add(p); return d;
                    }
                }
            } while (k != 0);
            do
            {
                k = Pollard(p);
                if (k != 1)
                {
                    p /= k;
                    d.Add(k);
                    if (SoloveyStrassen(p))
                    {
                        d.Add(p); return d;
                    }
                }
            } while ( k != 1);
            if (p != 1)
                d.Clear();
            return d;
        }

        private bool SoloveyStrassen(double p)
        {
            double sign;
            Random rnd = new Random();
            for (int k = 0; k < 200; k++)
            {
                double x = Math.Truncate(rnd.NextDouble() * (p - 2) + 2);
                if (GCD(p, x) > 1)
                    return false;
                if (p % 2 == 0)
                    return false;
                if (Power(x, p) == 1)
                    sign = 1;
                else
                    sign = -1;
                if (sign != Jacobi(x, p))
                    return false;
            }
            return true;
        }
        private double Power(double a, double p)
        {
            double n = (p - 1) / 2;
            double answer = 1;
            while (n != 0)
            {
                if (n % 2 == 1)
                    answer = (answer * a) % p;
                a = Math.Pow(a, 2) % p;
                n = Math.Truncate(n / 2);
            }
            return answer;
        }

        private double Jacobi(double a, double n)
        {
            if (a == 0 || a == 1)
                return a;
            double s, e = 0;
            while (a % 2 == 0)
            {
                a /= 2;
                e++;
            }
            if (e % 2 == 0 || n % 8 == 1 || n % 8 == 7)
                s = 1;
            else s = -1;
            if (n % 4 == 3 && a % 4 == 3)
                s *= -1;
            if (a == 1)
                return s;
            return s * Jacobi(n % a, a);
        }

        private double GCD(double a, double b)
        {
            if (a == 0)
                return 1;
            return b == 0 ? Math.Abs(a) : GCD(b, a % b);
        }

        private double Trial(double n)
        {
            List<double> prime = new List<double> { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47 };
            double p;
            for (int i = 0; i < prime.Count; i++)
            {
                p = n % prime[i];
                if (p == 0)
                    return prime[i];
            }
            return 1;
        }

         private double Func(double n, double p)
        {
            double k = Math.Pow(n, 2) + 1;
            if (k > p)
                k %= p;
            return k;
        }

        private double Pollard(double p)
        {
            List<double> X = new List<double> { };
            List<double> Y = new List<double> { };
            double x, y, c;
            double d = 1;
            x = 2;
            y = 2;
            X.Add(x);
            Y.Add(y);
            while (d == 1)
            {
                x = Func(X.Last(), p);
                X.Add(x);
                y = Func(Func(Y.Last(), p), p);
                Y.Add(y);
                c = x - y;
                d = GCD(c, p);
                if (x == y)
                    break;
            }
            return d;
        }
    }
}
