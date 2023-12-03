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
            string line = Console.ReadLine();

            if (m > 0)
            {
                List<int> answer = new List<int>();
                answer.Add(Func(n, m, line, 'R'));
                answer.Add(Func(n, m, line, 'G'));
                answer.Add(Func(n, m, line, 'B'));
                Console.WriteLine(answer.Max());
            } else
            {
                int cur_len = 0, answer = -1, i = 0;
                while (i < n - 1)
                {
                    cur_len += 1;
                    if (line[i] != line[i + 1])
                    {
                        if (cur_len > answer)
                            answer = cur_len;
                        cur_len = 0;
                    }
                    i += 1;
                }
                if (line[n - 2] == line[n - 1])
                {
                    cur_len += 1;
                }
                if (cur_len > answer)
                    answer = cur_len;
                Console.WriteLine(answer);
            }
        }

        static int Func(int n, int m, string line, char letter)
        {
            List<int> changes = new List<int>();
            int i = 0, start = 0, answer = -1, cur_len = -1;
            while (i < n)
            {
                if (line[i] != letter)
                {
                    if (changes.Count == m)
                    {
                        cur_len = i - start;
                        if (cur_len > answer)
                            answer = cur_len;
                        start = changes[0] + 1;
                        changes.RemoveAt(0);
                    }
                    changes.Add(i);
                }
                i += 1;
            }
            cur_len = n - start;
            if (cur_len > answer)
                answer = cur_len;
            return answer;
        }
    }
}