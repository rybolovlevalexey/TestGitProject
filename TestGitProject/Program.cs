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
            // наполнение обеих коллекций тестовыми данными
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
        {// внимательно проверить
            List<String> result = new List<string>();
            result = (from us in users where result.Contains(us.Name) select us.Name).ToList();
            return result;
        }

        public List<User> GetAllAuthors()
        {

            //Напишите реализацию

        }

        public Dictionary<int, User> GetUsersDictionary()
        {

            //Напишите реализацию

        }

        public int GetMaxID()
        {

            //Напишите реализацию

        }

        public List<User> GetOrderedUsers()
        {

            //Напишите реализацию

        }

        public List<User> GetDescendingOrderedUsers()
        {

            //Напишите реализацию

        }

        public List<User> GetReversedUsers()
        {

            //Напишите реализацию

        }

        public List<User> GetUsersPage(int pageSize, int pageIndex)
        {

            //Напишите реализацию

        }
    }
}