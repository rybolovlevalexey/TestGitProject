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
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;

namespace TestGitProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, Movie> films = new Dictionary<string, Movie>();  // название: фильм
            Dictionary<string, Movie> people = new Dictionary<string, Movie>();  // имя учатсника: фильм
            Dictionary<string, Movie> tags_dict = new Dictionary<string, Movie>();  // тэг: фильм
            string dataset_path = @"C:\Универ\ml-latest";

            string[] MovieCodes_IMDB = File.ReadAllLines(dataset_path + "MovieCodes_IMDB.tsv")[1..];
            // titleId (нужен), ordering, title(нужен), region(RU или US), language(RU или US), types, attributes, isOriginalTitle; делятся табами
            foreach (var line in MovieCodes_IMDB)
            {
                string[] elements = line.Split('\t');
                string film_id = elements[0], title = elements[2], region = elements[3], language = elements[4];
                if (region == "RU" || region == "US" || language == "RU" || language == "US")
                {
                    if (!films.ContainsKey(title))
                    {
                        films[title] = new Movie()
                    }
                }
            }

            string[] ActorsDirectorsNames_IMDB = File.ReadAllLines(dataset_path + "ActorsDirectorsNames_IMDB.txt"); 
            // 
            string[] Ratings_IMDB = File.ReadAllLines(dataset_path + "Ratings_IMDB.tsv"); // 
            string[] links_IMDB_MovieLens = File.ReadAllLines(dataset_path + "links_IMDB_MovieLens.csv"); // 
            string[] TagCodes_MovieLens = File.ReadAllLines(dataset_path + "TagCodes_MovieLens.csv"); // 
            string[] TagScores_MovieLens = File.ReadAllLines(dataset_path + "TagScores_MovieLens.csv"); // 
        }
    }
}