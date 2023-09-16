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
            BusinessLogic bl = new BusinessLogic();

            var req1 = bl.GetUsersBySurname("Begging");
            foreach (var u in req1)
                Console.WriteLine(u);
            Console.WriteLine();

            var req2 = bl.GetUsersBySurname("NULL");
            foreach (var u in req2)
                Console.WriteLine(u);
            Console.WriteLine();

            Console.WriteLine(bl.GetUserByID(1));
            Console.WriteLine();

            var req3 = bl.GetUsersBySubstring("a");
            foreach (var u in req3)
                Console.WriteLine(u);
            Console.WriteLine();

            var req4 = bl.GetUsersBySubstring("Hell yeah");
            foreach (var u in req4)
                Console.WriteLine(u);
            Console.WriteLine();

            var req5 = bl.GetAllUniqueNames();
            foreach (var u in req5)
                Console.WriteLine(u);
            Console.WriteLine();

            var req6 = bl.GetAllAuthors();
            foreach (var u in req6)
                Console.WriteLine(u);
            Console.WriteLine();

            var req7 = bl.GetUsersDictionary();
            foreach (var kvl in req7)
                Console.WriteLine($"{kvl.Key} - {kvl.Value}");
            Console.WriteLine();

            var req8 = bl.GetMaxID();
            Console.WriteLine(req8);
            Console.WriteLine();

            var req9 = bl.GetOrderedUsers();
            foreach (var u in req9)
                Console.WriteLine(u);
            Console.WriteLine();

            var req10 = bl.GetDescendingOrderedUsers();
            foreach (var u in req10)
                Console.WriteLine(u);
            Console.WriteLine();

            var req11 = bl.GetReversedUsers();
            foreach (var u in req11)
                Console.WriteLine(u);
            Console.WriteLine();

            var req12 = bl.GetUsersPage(5, 1);
            foreach (var u in req12)
                Console.WriteLine(u);
            Console.WriteLine();
        }

    }

    class User
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
        public User(int id, String name, String surname)
        {
            this.ID = id; this.Name = name; this.Surname = surname;
        }
        public override string ToString() { return string.Format("ID={0}: {1} {2}", ID, Name, Surname); }
    }

    class Record
    {
        public User Author { get; set; }
        public String Message { get; set; }
        public Record(User author, String message)
        {
            this.Author = author; this.Message = message;
        }
    }

    class BusinessLogic
    {
        private List<User> users = new List<User>();
        private List<Record> records = new List<Record>();
        public BusinessLogic()
        {
            users.Add(new User(0, "Al", "Bal"));
            users.Add(new User(1, "Twen", "Folaks"));
            users.Add(new User(2, "Frodo", "Begging"));
            users.Add(new User(3, "Frodo", "Begging"));
            users.Add(new User(4, "Frodo", "Begging"));
            users.Add(new User(5, "Frodo", "Begging"));
            users.Add(new User(6, "Frodo", "Begging"));
            users.Add(new User(7, "Frodo", "Begging"));
            users.Add(new User(8, "Frodo", "Begging"));
            users.Add(new User(9, "Frodo", "Begging"));
            users.Add(new User(10, "Frodo", "Begging"));



            records.Add(new Record(users[0], "First message"));
            records.Add(new Record(users[1], "2nd message"));
            records.Add(new Record(users[1], "Third message"));
        }

        public List<User> GetUsersBySurname(String surname)
        {
            List<User> result = new List<User>();
            result = (from us in users where us.Surname == surname select us).ToList();
            return result;
        }

        public User GetUserByID(int id)
        {
            return (from us in users where us.ID == id select us).Single();
        }

        public List<User> GetUsersBySubstring(String substring)
        {
            return (from us in users where us.Name.Contains(substring) || us.Surname.Contains(substring) select us).ToList();
        }

        public List<String> GetAllUniqueNames()
        {
            return (from us in users select us.Name).Distinct().ToList();
        }

        public List<User> GetAllAuthors()
        {
            return (from rec in records select rec.Author).Distinct().ToList();
        }

        public Dictionary<int, User> GetUsersDictionary()
        {
            return users.ToDictionary(us1 => us1.ID, us2 => us2);
        }

        public int GetMaxID()
        {
            return (from us in users select us.ID).Max();
        }

        public List<User> GetOrderedUsers()
        {
            return (from us in users orderby us.ID select us).ToList();
        }

        public List<User> GetDescendingOrderedUsers()
        {
            return (from us in users orderby us.ID descending select us).ToList();
        }

        public List<User> GetReversedUsers()
        {
            return (from us in users select us).Reverse().ToList();
        }

        public List<User> GetUsersPage(int pageSize, int pageIndex)
        {
            List<User> result = new List<User>();
            result = users.Take(pageSize + pageIndex).ToList();
            result = users.Skip(pageIndex).ToList();
            return result;
        }
    }
}