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
            Dictionary<string, List<string>> id_name = new Dictionary<string, List<string>>();

            Dictionary<string, Movie> films = new Dictionary<string, Movie>();  // название: фильм
            Dictionary<string, List<Movie>> people = new Dictionary<string, List<Movie>>();  // имя учатсника: фильмы
            Dictionary<string, List<Movie>> tags_dict = new Dictionary<string, List<Movie>>();  // тэг: фильмы
            string dataset_path = @"C:\Универ\ml-latest\";

            string[] MovieCodes_IMDB = File.ReadAllLines(dataset_path + "MovieCodes_IMDB.tsv")[1..];
            // titleId (нужен), ordering, title(нужен), region(RU или US), language(RU или US), types, attributes, isOriginalTitle; делятся табами
            foreach (var line in MovieCodes_IMDB.AsParallel())
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
            Console.WriteLine("first films done");
            
            // добавление рейтинга во все фильмы
            string[] Ratings_IMDB = File.ReadAllLines(dataset_path + "Ratings_IMDB.tsv")[1..];
            foreach(var line in Ratings_IMDB.AsParallel())
            {
                string[] elements = line.Split('\t');
                string film_id = elements[0].Trim(), rating = elements[1].Trim();
                if (id_name.ContainsKey(film_id))
                {
                    foreach (var name in id_name[film_id])
                        films[name].rating = rating;
                }
            }
            Ratings_IMDB = null;
            Console.WriteLine("rating done");

            make_people(id_name);
            Console.WriteLine("make people done");
            
            Dictionary<string, List<string>> result_tags = make_tags(id_name);
            foreach (var film_name in result_tags.Keys.AsParallel())
            {
                films[film_name].tags = result_tags[film_name].ToHashSet<string>();
            }
            Console.WriteLine("make tags done");
            
            // наполнение второго словаря
            //  ...

            // наполнение третьего словаря, когда все классы фильмов заполнены
            foreach (var film_name in result_tags.Keys.AsParallel())
            {
                foreach (var tag in result_tags[film_name])
                {
                    if (!tags_dict.ContainsKey(tag))
                        tags_dict[tag] = new List<Movie>();
                    tags_dict[tag].Add(films[film_name]);
                }
            }

            // testing...
            //foreach (var key in films.Keys)
            //{
            //    var value = films[key];
            //    string name = value.name, rat = value.rating, id = value.id, director = value.give_director();
            //    Console.WriteLine($"{name} - {rat} - {id} - {director}");
            //}
        }

        static Dictionary<string, List<string>> make_tags(Dictionary<string, List<string>> films_id_name)
        {
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>(); // film name: [all tags]

            string dataset_path = @"C:\Универ\ml-latest\";
            Dictionary<string, int> relev_dict = new Dictionary<string, int>();
            Dictionary<string, string> tag_dict = new Dictionary<string, string>(); // tag_id: tag

            string[] TagScores_MovieLens = File.ReadAllLines(dataset_path + "TagScores_MovieLens.csv")[1..];
            foreach (var line in TagScores_MovieLens.AsParallel())
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
            TagScores_MovieLens = null;

            string[] TagCodes_MovieLens = File.ReadAllLines(dataset_path + "TagCodes_MovieLens.csv")[1..];
            foreach (var line in TagCodes_MovieLens.AsParallel())
            {
                string[] elements = line.Split(",");
                string tag_id = elements[0].Trim(), tag = elements[1].Trim();
                if (relev_dict[tag_id] < 50)
                    continue;
                if (tag != "" || tag != " ")
                    tag_dict[tag_id] = tag;
            }
            TagCodes_MovieLens = null;

            string[] links_IMDB_MovieLens = File.ReadAllLines(dataset_path + "links_IMDB_MovieLens.csv")[1..];
            foreach (var line in links_IMDB_MovieLens.AsParallel())
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
            links_IMDB_MovieLens = null;

            Dictionary<string, List<string>> last_result = new Dictionary<string, List<string>>(); // film name: [all tags]

            foreach (var key in result.Keys.AsParallel())
            {
                if (result[key].Count != 0)
                    last_result[key] = result[key];
            }
            return last_result;
        }

        static void make_people(Dictionary<string, List<string>> films_id_name)
        {
            Console.WriteLine("start make people");
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();  // имя человека: список фильмов
            string dataset_path = @"C:\Универ\ml-latest\";
            
            Dictionary<string, string> persons = new Dictionary<string, string>();  // id: name

            string[] ActorsDirectorsNames_IMDB = File.ReadAllLines(dataset_path + "ActorsDirectorsNames_IMDB.txt")[1..];
            Console.WriteLine("first reading done");
            
            foreach (var line in ActorsDirectorsNames_IMDB.AsParallel())
            {
                string[] elems = line.Split("\t");
                string id = elems[0].Trim(), name = elems[1].Trim();
                persons[id] = name;
            }
            Console.WriteLine("make people 1");
            ActorsDirectorsNames_IMDB = null;

            string[] ActorsDirectorsCodes_IMDB = File.ReadAllLines(dataset_path + "ActorsDirectorsCodes_IMDB.tsv")[1..];
            Console.WriteLine("second reading done");
            foreach (var line in ActorsDirectorsCodes_IMDB.AsParallel())
            {
                string[] elems = line.Split("\t");
                string mov_id = elems[0].Trim(), chel_id = elems[2].Trim();
                foreach (string mov_name in films_id_name[mov_id])
                {
                    if (!result.ContainsKey(persons[chel_id]))
                        result[persons[chel_id]] = new List<string>();
                    result[persons[chel_id]].Add(mov_name);
                }
            }
            ActorsDirectorsCodes_IMDB = null;
            Console.WriteLine("make people 2");

            
            foreach (var key in result.Keys)
            {
                Console.WriteLine($"{key} {result[key].ToString()}");
            }
            Console.WriteLine("make people 3");
        }
    }
}