﻿using System;
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
            Dictionary<string, List<string>> id_name = new Dictionary<string, List<string>>();

            Dictionary<string, Movie> films = new Dictionary<string, Movie>();  // название: фильм
            Dictionary<string, Movie> people = new Dictionary<string, Movie>();  // имя учатсника: фильм
            Dictionary<string, Movie> tags_dict = new Dictionary<string, Movie>();  // тэг: фильм
            string dataset_path = @"C:\Универ\ml-latest\";

            string[] MovieCodes_IMDB = File.ReadAllLines(dataset_path + "MovieCodes_IMDB.tsv")[1..];
            // titleId (нужен), ordering, title(нужен), region(RU или US), language(RU или US), types, attributes, isOriginalTitle; делятся табами
            foreach (var line in MovieCodes_IMDB)
            {
                string[] elements = line.Split('\t');
                string film_id = elements[0].Trim(), title = elements[2].Trim(), region = elements[3].Trim(), language = elements[4].Trim();
                if (region == "RU" || region == "US" || language == "RU" || language == "US")
                {
                    if (!films.ContainsKey(title))
                    {
                        films[title] = new Movie(title, film_id);
                        //Console.WriteLine($"{title} {film_id}");
                    }
                    if (!id_name.ContainsKey(film_id))
                        id_name[film_id] = new List<string>();
                    id_name[film_id].Add(title);
                }
            }
            string[] Ratings_IMDB = File.ReadAllLines(dataset_path + "Ratings_IMDB.tsv")[1..];
            foreach(var line in Ratings_IMDB)
            {
                string[] elements = line.Split('\t');
                string film_id = elements[0].Trim(), rating = elements[1].Trim();
                if (id_name.ContainsKey(film_id))
                {
                    foreach (var name in id_name[film_id])
                        films[name].rating = rating;
                }
            }

            make_tags(id_name);
        }

        static void make_tags(Dictionary<string, List<string>> films_id_name)
        {
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>(); // film name: [all tags]

            string dataset_path = @"C:\Универ\ml-latest\";
            string[] links_IMDB_MovieLens = File.ReadAllLines(dataset_path + "links_IMDB_MovieLens.csv")[1..];
            string[] TagCodes_MovieLens = File.ReadAllLines(dataset_path + "TagCodes_MovieLens.csv")[1..];
            string[] TagScores_MovieLens = File.ReadAllLines(dataset_path + "TagScores_MovieLens.csv")[1..];
            Dictionary<string, int> relev_dict = new Dictionary<string, int>();
            Dictionary<string, string> tag_dict = new Dictionary<string, string>(); // tag_id: tag

            foreach (var line in TagScores_MovieLens)
            {
                string[] elements = line.Split(",");
                string tagid = elements[1].Trim(), rel = elements[2].Trim();
                int relevants;
                if (rel.Length > 3)
                    relevants = Convert.ToInt32(rel[2] + rel[3]);
                else
                    relevants = Convert.ToInt32(rel[2] + "0");
                relev_dict[tagid] = relevants;
            }
            foreach (var line in TagCodes_MovieLens)
            {
                string[] elements = line.Split(",");
                string tag_id = elements[0].Trim(), tag = elements[1].Trim();
                if (relev_dict[tag_id] < 50)
                    continue;
                tag_dict[tag_id] = tag;
            }
            foreach (var line in links_IMDB_MovieLens)
            {
                string[] elements = line.Split(",");
                string movie_id = elements[0].Trim(), tag_id = elements[2].Trim();
                string zeros = "";
                for (int i = 0; i < (7 - movie_id.Length); i += 1)
                    zeros += "0";
                movie_id = "tt" + zeros + movie_id;
                if (films_id_name.ContainsKey(movie_id))
                {
                    foreach (var movie_name in films_id_name[movie_id])
                    {
                        if (!result.ContainsKey(movie_name))
                        {
                            result[movie_name] = new List<string>();
                        }
                        if (tag_dict.ContainsKey(tag_id))
                            result[movie_name].Add(tag_dict[tag_id]);
                    }
                }
            }
            int cnt = 0;
            foreach (var key in result.Keys)
            {
                foreach (var value in result[key])
                    Console.Write(value);
                Console.WriteLine();
                if (cnt == 10)
                    break;
                cnt += 1;
            }
        }
    }
}