using System;
using System.Collections.Generic;
using System.Linq;

namespace TestGitProject
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine());
            List<string> data = Console.ReadLine().Split().ToList<string>();
            int? minim = null, maxim = null;
            foreach (var elem in  data)
            {
                if (minim == null)
                {
                    minim = Convert.ToInt32(elem);
                    maxim = Convert.ToInt32(elem);
                }
                else
                {
                    int number = Convert.ToInt32(elem);
                    if (number < minim)
                    {
                        minim = number;
                    }
                    if (number > maxim)
                    {
                        maxim = number;
                    }
                }
            }

            int i = n - 1, j = 0;
            while (i > 0)
            {
                if (Convert.ToInt32(data[i]) == maxim)
                    break;
                i -= 1;
            }
            while (j < n)
            {
                if (Convert.ToInt32(data[j]) == minim)
                    break;
                j += 1;
            }
            Console.WriteLine($"{i + 1} {j + 1}");
        }
    }
}