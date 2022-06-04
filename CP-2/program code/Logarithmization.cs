using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTA_cp2
{
    class Logarithmization
    {
        public Logarithmization(double a, double b, double n)
        {
            var f = new Factorization();
            var rep = f.CanRepOfNum(n - 1);
            var table = Table(rep, a, n);
            List<double> x = new List<double>();
            for (int i = 0; i < table.Count; i++)
                x.Add(Chain(a, b, n, rep[2 * i], rep[2 * i + 1], table[i]));
            for (int i = 0; i < rep.Count; i++)
            {
                rep[i] = Horner(rep[i], rep[i + 1], n);
                rep.RemoveAt(i + 1);
            }
            double ans = Comparison(x, rep, n - 1);
            Console.WriteLine(ans);
        }

        private double Comparison(List<double> x, List<double> m, double M)
        {
            double mi, ni, answer = 0;
            if (x.Count == 1)
                return x[0];
            for (int i = 0; i < m.Count; i++)
            {
                mi = M / m[i];
                ni = Inverse(m[i], mi);
                answer = (answer + x[i] * mi * ni % M) % M;
            }
            return answer;
        }

        private double Chain(double a, double b, double n, double p, double l, List<double> r)
        {
            a = Inverse(n, a);
            double temp, answer;
            answer = r.IndexOf(Horner(b, (n - 1) / p, n));
            for (int i = 1; i < l; i++)
            {
                temp = Horner(a, answer, n);
                temp = (temp * b) % n;
                temp = Horner(temp, (n - 1) / Math.Pow(p, i + 1), n);
                answer += (r.IndexOf(temp) * Math.Pow(p, i));
            }
            return answer % n;
        }

        private double Inverse(double n, double a)
        {
            double q, u, u0 = 0, u1 = 1, m = n;
            while (a != 1)
            {
                q = Math.Truncate(n / a);
                u = u0 - u1 * q;
                q = n % a;
                n = a;
                a = q;
                u0 = u1;
                u1 = u % m;
            }
            return (u1 + m) % m;
        }

        private List<List<double>> Table(List<double> rep, double a, double n)
        {
            List<List<double>> r = new List<List<double>>();
            for (int i = 0; i < rep.Count; i += 2) 
                r.Add(Ri(a, rep[i], n));
            return r;
        }

        private List<double> Ri(double a, double p, double n)
        {
            List<double> r = new List<double>();
            for (int j = 0; j < p; j++)
                r.Add(Horner(a, (n - 1) * j / p, n));
            return r;
        }

        private double Horner(double a, double pow, double n)
        {
            string str;
            double answer;
            if (pow == 0)
                return 1;
            str = Convert.ToString((int)pow, 2);
            answer = a;
            for (int i = 1; i < str.Length; i++)
            {
                answer = Math.Pow(answer, 2) % n;
                if (str[i] == '1')
                    answer = (answer * a) % n;
            }
            return answer;
        }
    }
}
