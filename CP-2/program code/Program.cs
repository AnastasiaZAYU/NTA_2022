using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTA_cp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введiть генератор групи: ");
            var a = int.Parse(Console.ReadLine());
            Console.Write("Введiть елемент групи: ");
            var b = int.Parse(Console.ReadLine());
            Console.Write("Введiть порядок групи: ");
            var n = int.Parse(Console.ReadLine());
            var p = new Logarithmization(a, b, n);

            Console.ReadKey();
        }
    }
}
