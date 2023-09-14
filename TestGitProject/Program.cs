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
    class Letter
    {
        Letter()
        {
            Id = Convert.ToInt32(DateTime.Now.TimeOfDay.TotalSeconds.ToString().Split(",")[0]);
            IsNew = true;
            Size = 10;
        }
        public Letter(bool is_new)
        {
            Id = Convert.ToInt32(DateTime.Now.TimeOfDay.TotalSeconds.ToString().Split(",")[0]);
            IsNew = is_new;
        }
        public override string ToString()
        {
            string result_st = "Letter ";
            result_st += Id.ToString();
            result_st += " ";
            result_st += IsNew.ToString();
            return result_st;
        }
        public bool IsNew { get; set; }
        public int Size { get; set; }
        public int Id { get; }
    }
    class Program
    {

        static void Main(string[] args)
        {
            var test_letters = new Letter[5];
            test_letters[0] = new Letter(true);
            test_letters[1] = new Letter(false);
            test_letters[2] = new Letter(true);
            test_letters[3] = new Letter(true);
            test_letters[4] = new Letter(false);
            var result1 = GetNewLetterIds_ClassicWay(test_letters);
            var result2 = GetNewLetterIds_LinqWay(test_letters).ToArray();
            if (result1.Count() != result2.Count())
            {
                Console.WriteLine("Error");
            } else
            {
                for (int i = 0; i < result1.Count(); i += 1)
                    Console.WriteLine(result1[i] + " " + result2[i]);
            }
        }
        static public List<int> GetNewLetterIds_ClassicWay(Letter[] letters)
        {
            var res = new List<int>();
            for (int i = 0; i < letters.Length; i++)
            {
                if (letters[i].IsNew)
                    res.Add(letters[i].Id);
            }
            return res;
        }
        static public  IEnumerable<int> GetNewLetterIds_LinqWay(Letter[] letters)
        {
            return letters.Where(letter => letter.IsNew).Select(letter => letter.Id);
        }
    }
}
