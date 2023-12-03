using System;
using System.Collections.Generic;
using System.Linq;

namespace TestGitProject
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] data = Console.ReadLine().Split();
            int n = Convert.ToInt32(data[0]), m = Convert.ToInt32(data[1]);
            int ans = 0;
            if (n < m)
                ans = n + (n + 1) * 2;
            if (n == m)
                ans = n + m * 2;
            if (n > m)
                ans = m * 2 + (m + 1);
            Console.WriteLine(ans);
        }
    }
}