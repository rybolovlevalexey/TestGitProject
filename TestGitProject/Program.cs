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
            string[] data = Console.ReadLine().Split();

            if (n > 2)
            {
                // индекс, до которого идёт префикс: префиксная сумма
                Dictionary<int, int> dict = new Dictionary<int, int>();
                int[] numbers = new int[n];
                int summa = 0;
                for (int i = 0; i < n - 1; i += 1)
                {
                    int num = Convert.ToInt32(data[i]);
                    summa += num;
                    numbers[i] = num;
                    dict[i] = summa;
                }
                numbers[n - 1] = Convert.ToInt32(data[n - 1]);
                
                int ans = -1;
                summa = numbers[n - 1];
                for (int i = n - 2; i >= 0; i -= 1)
                {
                    if (summa == dict[i])
                    {
                        ans = i;
                        break;
                    }
                    summa += numbers[i];
                }
                
                if (ans == -1)
                    Console.WriteLine(ans);
                else
                {
                    for (int i = 0; i < n; i += 1)
                    {
                        if (i == ans)
                            Console.Write($"{data[i]}=");
                        else
                        {
                            if (i == n - 1)
                                Console.Write(data[i]);
                            else
                                Console.Write($"{data[i]}+");
                        }
                    }
                }
            } 
            else
            {
                if (data[0] == data[1])
                    Console.WriteLine($"{data[0]}={data[0]}");
                else
                    Console.WriteLine(-1);
            }
        }
    }
}