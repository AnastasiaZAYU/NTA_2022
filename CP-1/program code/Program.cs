using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTA_cp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Чи бажаєте Ви продовжити роботу в термiналi?\n 1 - якщо так\n 0 - якщо числа зчитати з файлу");
            Factorization p;
            if (Console.ReadLine() == "1")
                p = new Factorization(true);
            else p = new Factorization(false);

            Console.ReadKey();
        }
    }
}
