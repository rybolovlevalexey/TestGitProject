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
            int t = Convert.ToInt32(Console.ReadLine());
            Dictionary<int, Tuple<int, int>> lines = new Dictionary<int, Tuple<int, int>>();
            Dictionary<int, Tuple<int, int>> columns = new Dictionary<int, Tuple<int, int>>();

            for (int iter = 0; iter < t; iter += 1)
            {
                data = Console.ReadLine().Split();
                int color = Convert.ToInt32(data[2]);
                lines[Convert.ToInt32(data[0]) - 1] = Tuple.Create(color, iter);
                columns[Convert.ToInt32(data[1]) - 1] = Tuple.Create(color, iter);
            }

            for (int i = 0; i < n; i += 1)  // номер строки
            {
                for (int j = 0; j < m; j += 1)  // номер столбца
                {
                    if (!lines.ContainsKey(i) && !columns.ContainsKey(j))
                        Console.Write("0 ");
                    if (lines.ContainsKey(i) && columns.ContainsKey(j))
                    {
                        Tuple<int, int> lin = lines[i];
                        Tuple<int, int> col = columns[j];
                        if (lin.Item2 > col.Item2)
                            Console.Write($"{lin.Item1} ");
                        else
                            Console.Write($"{col.Item1} ");
                    }
                    if (lines.ContainsKey(i) && !columns.ContainsKey(j))
                    {
                        Console.Write($"{lines[i].Item1} ");
                    }
                    if (!lines.ContainsKey(i) && columns.ContainsKey(j))
                    {
                        Console.Write($"{columns[j].Item1} ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}