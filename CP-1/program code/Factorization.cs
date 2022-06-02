using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NTA_cp1
{
    class Factorization
    {
        List<double> d = new List<double>();
        public Factorization(bool arg)
        {
            if (arg == true)
                Factorization_Term();
            else Factorization_File();
        }

        private void Factorization_Term()
        {
            Console.Write("Введiть число, канонiчний розклад якого потрiбно знайти: ");
            double p = Convert.ToDouble(Console.ReadLine());
            CanRepOfNum(p);
            if (d.Count == 0)
                Console.WriteLine("Я не можу знайти канонiчний розклад числа:(");
            else
            {
                Console.Write("Канонiчний розклад числа {0} = ", p);
                for (int i = 0; i < d.Count - 1; i++)
                    Console.Write(d[i] + " * ");
                Console.WriteLine(d.Last());
            }
        }

        private void Factorization_File()
        {
            var sr = File.OpenText("input.csv");
            var sw = File.CreateText("output.csv");
            double p;
            while(!sr.EndOfStream)
            {
                p = Convert.ToDouble(sr.ReadLine());
                CanRepOfNum(p);
                if (d.Count == 0)
                    sw.WriteLine("Я не можу знайти канонiчний розклад числа:(");
                else
                {
                    for (int i = 0; i < d.Count - 1; i++)
                        sw.Write(d[i] + " * ");
                    sw.WriteLine(d.Last());
                }
                d.Clear();
            }
            sw.Close();
            sr.Close();
        }

        private void CanRepOfNum(double p)
        {
            double k = 0;
            if (SoloveyStrassen(p))
            {
                d.Add(p); return;
            }
            List<double> prime = new List<double> { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47 };
            do
            {
                k = Trial(p, prime);
                if (k != 0)
                {
                    p /= k;
                    d.Add(k);
                    if (SoloveyStrassen(p))
                    {
                        d.Add(p); return;
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
                        d.Add(p); return;
                    }
                }
            } while ( k != 1);
            double add = 0;
            do
            {
                k = Pomerantz(p, add);
                if (k != 1)
                {
                    p /= k;
                    d.Add(k);
                    if (SoloveyStrassen(p))
                    {
                        d.Add(p); return;
                    }
                }
                else
                {
                    add += 50; k = 0;
                }
            } while (k != 1);
            if (p != 1)
                d.Clear();
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

        private double Trial(double n, List<double> prime)
        {
            int i = 0;
            if (prime[0] == -1)
                i = 1;
            double p;
            for (; i < prime.Count; i++)
            {
                p = n % prime[i];
                if (p == 0)
                    return prime[i];
            }
            return 0;
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

        private double Pomerantz(double p, double a)
        {
            double L = Math.Truncate(Math.Pow(Math.Pow(Math.E, Math.Sqrt(Math.Log(p) * Math.Log(Math.Log(p)))), 1 / Math.Sqrt(2))) + a;
            List<double> prime = new List<double> { -1, 2 };
            for (int i = 3; i <= L; i += 2)
            {
                if (SoloveyStrassen(i))
                    if (Jacobi(p, i) == 1)
                        prime.Add(i);
            }
            return Sieving(p, prime);
        }

        private double Sieving(double p, List<double> prime)
        {

            double a, b, m = Math.Truncate(Math.Sqrt(p));
            string str;
            int x = 0;
            int k = prime.Count;
            List<double> A = new List<double>();
            List<double> B = new List<double>();
            List<string> list = new List<string>();
            List<double> temp = new List<double>();
            List<List<double>> vectors = new List<List<double>>();
            while (A.Count <= k)
            {
                for (int i = 0; i < 2; i++, x = -x)
                {
                    a = x + m;
                    b = Math.Pow(a, 2) - p;
                    temp = Factor(b, prime);
                    if (temp.Count != 0)
                    {
                        vectors.Add(temp);
                        A.Add(a);
                        B.Add(b);
                    }
                    if (x == 0 || A.Count == k + 1)
                        break;
                }
                x++;
            }
            for (int i = 0; i < k + 1; i++)
            {
                str = "";
                for (int j = 0; j < k + 1; j++)
                    str += vectors[i][j] % 2;
                list.Add(str);
            }
            List<string> ans = Gauss(list);
            List<double> power = new List<double>(new double[k]);
            double X = 1, Y = 1;
            string vect;
            for (int j = 0; j < ans.Count; j++)
            {
                vect = ans[j];
                X = 1; Y = 1;
                for (int i = 0; i < k + 1; i++)
                {
                    if (vect[i] == '1')
                    {
                        X = (X * A[i]) % p;
                        for (int t = 0; t < k; t++)
                            power[t] += vectors[i][t];
                    }
                }
                Y = Horner(p, prime, power);
                if ((X + Y) % p != 0 && X != Y)
                    break;
            }
            return GCD((X + Y) % p, p);
        }

        private double Horner(double p, List<double> prime, List<double> power)
        {
            double a, Y = 1;
            string str;
            for (int i = 1; i < prime.Count; i++)
            {
                str = Convert.ToString((int)(power[i] / 2), 2);
                if (str != "0")
                {
                    a = prime[i];
                    for (int j = 1; j < str.Length; j++)
                    {
                        a = Math.Pow(a, 2) % p;
                        if (str[j] == '1')
                            a = (a * prime[i]) % p;
                    }
                    if (Y > Math.Abs(Y - p))
                        Y -= p;
                    if (a > Math.Abs(a - p))
                        a -= p;
                    Y = (Y * a) % p;
                    if (Y < 0)
                        Y += p;
                }
            }
            return Y;
        }

        private List<double> Factor(double p, List<double> prime)
        {
            double d;
            int k = prime.Count;
            List<double> answer = new List<double>(new double[k + 1]);
            if (p < 0) 
            {
                p = -p;
                answer[0] = 1;
            }
            while (p > 1) 
            {
                d = Trial(p, prime);
                if (d == 0)
                {
                    answer.Clear(); return answer;
                }
                p /= d;
                answer[prime.IndexOf(d)]++;
            }
            return answer;
        }

        private List<string> Gauss(List<string> A)
        {
            List<string> answer = new List<string>();
            int ind;
            string str;
            for (int i = 0; i < A.Count; i++)
            {
                ind = A[i].IndexOf('1', i);
                if (ind > i)
                    for (int j = i; j < A.Count; j++)
                    {
                        str = A[j];
                        if (str[i] != str[ind])
                            str = str.Substring(0, i) + str.Substring(ind, 1) + str.Substring(i + 1, ind - i - 1) + str.Substring(i, 1) + str.Substring(ind + 1);
                        A[j] = str;
                    }
                for (int j = i + 1; j < A.Count; j++)
                    if (A[i][j] == '1')
                        for (int l = i; l < A.Count; l++)
                            if (A[l][i] == '1')
                            {
                                if (A[l][j] == '0')
                                    A[l] = A[l].Substring(0, j) + "1" + A[l].Substring(j + 1);
                                else
                                    A[l] = A[l].Substring(0, j) + "0" + A[l].Substring(j + 1);
                            }
            }
            answer.Add("0");
            answer.Add("1");
            int sum = 0;
            str = "0";
            for (int i = A.Count - 2; i >= 0; i--)
            {
                str += "0";
                for (int r = answer.Count - 1; r >= 0; r--)
                {
                    sum = 0;
                    for (int j = A.Count - 1, l = str.Length - 2; j > i; j--, l--)
                        sum = (sum + (int)answer[r][l] * (int)A[j][i]) % 2;
                    if (A[i][i] == '1')
                    {
                        if (sum == 1)
                            answer[r] = "1" + answer[r];
                        else
                            answer[r] = "0" + answer[r];
                    }
                    else
                    {
                        if (sum == 1)
                            answer.RemoveAt(r);
                        else
                        {
                            answer.Add("1" + answer[r]);
                            answer[r] = "0" + answer[r];
                        }
                    }
                }
            }
            answer.Remove(str);
            return answer;
        }
    }
}