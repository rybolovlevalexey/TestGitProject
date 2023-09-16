using System;
using System.Net.Http;
using System.Net;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace TestGitProject
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<int> numbers = Enumerable.Range(0, 10000000);
            List<int> result = (from num in numbers.AsParallel() where 
                                Enumerable.Range(1, num / 2 + 1).Where(d => num % d == 0).Sum() == num select num).ToList();
            foreach (var elem in result)
                Console.WriteLine(elem);
        }

    }
}