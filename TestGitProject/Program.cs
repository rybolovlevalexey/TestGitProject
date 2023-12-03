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
            int sum_left = 0, sum_right = Convert.ToInt32(data[n - 1]);
            int index = 0;  // индекс, с которого начинается правая часть
            bool flag = false;
            for (int i = 0; i < n - 1; i += 1)
                sum_left += Convert.ToInt32(data[i]);
            index = n - 1;

            if (n > 2)
            {
                while (index > 0)
                {
                    if (sum_left == sum_right)
                    {
                        flag = true;
                        break;
                    }
                    index -= 1;
                    int num = Convert.ToInt32(data[index]);
                    sum_left -= num;
                    sum_right += num;
                }
                if (!flag)
                    Console.WriteLine(-1);
                else
                {
                    for (int i = 0; i < n; i += 1)
                    {
                        Console.Write(data[i]);
                        if (i + 1 == index)
                            Console.Write("=");
                        else
                        {
                            if (i + 1 != n)
                                Console.Write("+");
                        }
                    }
                }
            } else
            {
                if (data[0] == data[1])
                    Console.WriteLine($"{data[0]}={data[0]}");
                else
                    Console.WriteLine(-1);
            }
        }
    }
}