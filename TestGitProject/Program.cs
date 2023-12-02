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
            int n = Convert.ToInt32(data[0]), k = Convert.ToInt32(data[1]);
            
            int answer = 0, cur_sum = 0, zero_cnt = 0, index = 0;
            Dictionary<int, List<int>> prefics = new Dictionary<int, List<int>>(); // кол-во нулей: сумма

            foreach (var elem in Console.ReadLine().Split())
            {
                int number = Convert.ToInt32(elem);
                if (number == 0)
                {
                    zero_cnt += 1;
                    prefics[zero_cnt] = new List<int>();
                }
                cur_sum += number;
                if (cur_sum <= k && zero_cnt < 2)
                    answer += (index + 1);
                else
                {
                    int key1 = zero_cnt - 1, key2 = zero_cnt;
                    if (prefics.ContainsKey(key1))
                    {
                        foreach (var pre_sum in prefics[key1])
                        {
                            if (cur_sum - pre_sum <= k)
                                answer += 1;
                        }
                    }
                    if (prefics.ContainsKey(key2)) 
                    { 
                        foreach (var pre_sum in prefics[key2])
                        {
                            if (cur_sum - pre_sum <= k)
                                answer += 1;
                        }
                    }
                }

                if (!prefics.ContainsKey(zero_cnt))
                    prefics[zero_cnt] = new List<int>();
                prefics[zero_cnt].Add(cur_sum);
                index += 1;
            }
            Console.Write(answer);
        }
    }
}