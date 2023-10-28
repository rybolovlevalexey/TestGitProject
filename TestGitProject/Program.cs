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
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;

namespace TestGitProject
{
    class Program
    {
        static Dictionary<string, Movie> films = new Dictionary<string, Movie>();  // название: фильм
        static Dictionary<string, List<Movie>> people = new Dictionary<string, List<Movie>>();  // имя учатсника: фильмы
        static Dictionary<string, List<Movie>> tags_dict = new Dictionary<string, List<Movie>>();  // тэг: фильмы

        static void Main(string[] args)
        {
            Dictionary<string, List<string>> id_name = new Dictionary<string, List<string>>();  // id фильма: название фильма

            string dataset_path = @"C:\Универ\ml-latest\";

            // наполнение словарей films и id_name
            string[] MovieCodes_IMDB = File.ReadAllLines(dataset_path + "MovieCodes_IMDB.tsv")[1..];
            foreach (var line in MovieCodes_IMDB.AsParallel())
            {
                string[] elements = line.Split('\t');
                string film_id = elements[0].Trim(), title = elements[2].Trim(), region = elements[3].Trim(), language = elements[4].Trim();
                if (region == "RU" || region == "US" || language == "RU" || language == "US")
                {
                    if (!films.ContainsKey(title))
                    {
                        films[title] = new Movie(title, film_id);
                    }
                    if (!id_name.ContainsKey(film_id))
                        id_name[film_id] = new List<string>();
                    id_name[film_id].Add(title);
                }
            }
            MovieCodes_IMDB = null;
            Console.WriteLine("The initial review of the films is done.");


            // добавление рейтинга во все фильмы
            string[] Ratings_IMDB = File.ReadAllLines(dataset_path + "Ratings_IMDB.tsv")[1..];
            foreach (var line in Ratings_IMDB.AsParallel())
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
            Console.WriteLine("Rating done.");

            Dictionary<string, List<string>> result_tags = make_tags(id_name);
            foreach (var film_name in result_tags.Keys.AsParallel())
            {
                films[film_name].tags = result_tags[film_name].ToHashSet<string>();
            }
            Console.WriteLine("Make tags done.");

            // наполнение фильмов их актёрами и режиссёрами
            Dictionary<string, Person> result_people = make_people(id_name);
            foreach (var per in result_people.Values)
            {
                foreach (var movie_id in per.actor_movis_id)
                {
                    if (!id_name.ContainsKey(movie_id))
                        continue;
                    foreach (var movie_name in id_name[movie_id])
                        films[movie_name].actors.Add(per.name);
                }
                foreach (var movie_id in per.director_movies_id)
                {
                    if (!id_name.ContainsKey(movie_id))
                        continue;
                    foreach (var movie_name in id_name[movie_id])
                        films[movie_name].directors.Add(per.name);
                }
            }
            Console.WriteLine("Make people done");

            // наполнение второго словаря
            foreach (var per in result_people.Values)
            {
                if (!people.ContainsKey(per.name))
                    people[per.name] = new List<Movie>();
                foreach (var mov_id in per.movies_id)
                {
                    if (!id_name.ContainsKey(mov_id))
                        continue;
                    foreach (var mov_name in id_name[mov_id])
                        people[per.name].Add(films[mov_name]);
                }
            }
            Console.WriteLine("Dictionary with persons is ready.");

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
            Console.WriteLine("Dictionary with tags is ready.");


            while (true)
            {
                Console.WriteLine("a - фильмы, b - люди, c - тэги");
                string mode = Console.ReadLine();
                switch (mode)
                {
                    case "a":
                        string movie_name = Console.ReadLine();
                        if (!films.ContainsKey(movie_name))
                            Console.WriteLine("Указанный фильм не найден");
                        else
                        {
                            var result = films[movie_name];
                            Console.WriteLine($"Фильм {result.name} с рейтингом {result.rating}");
                            Console.WriteLine($"располагает следующими тэгами: {print_iter(result.tags)}");
                            Console.WriteLine($"и актёрами: {print_iter(result.actors)}");
                            Console.WriteLine($"режиссёры - {print_iter(result.directors)}");
                        }
                        break;
                    case "b":
                        string person_name = Console.ReadLine();
                        if (!people.ContainsKey(person_name))
                        {
                            Console.WriteLine("Указанный человек не найден");
                        } else
                        {
                            int num = 1;
                            Console.WriteLine($"Человек с именем {person_name} участвовал в следующих проектах:");
                            foreach (var cur_film in people[person_name])
                            {
                                Console.Write($"{num}) {cur_film.name}  ");
                                num += 1;
                            }
                            Console.WriteLine();
                        }
                        break;
                    case "c":
                        string tag_name = Console.ReadLine();
                        if (!tags_dict.ContainsKey(tag_name))
                        {
                            Console.WriteLine("Указанный тэг не найден");
                        }
                        else
                        {
                            Console.WriteLine($"Тэг {tag_name} присутствует в следующих фильмах:");
                            int num = 1;
                            foreach (var cur_film in tags_dict[tag_name])
                            {
                                Console.Write($"{num}) " + cur_film.name + "  ");
                                num += 1;
                            }
                            Console.WriteLine();
                        }
                        break;
                }
            }
        }

        static string print_iter(IEnumerable<string> iterable)
        {
            int i = 0;
            string st = "";
            foreach (var elem in iterable)
            {
                st += $"{i + 1}) {elem}  ";
                i += 1;
            }
            return st;
        }
        static Dictionary<string, List<string>> make_tags(Dictionary<string, List<string>> films_id_name)
        {
            Console.WriteLine("Start make tags.");
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>(); // film name: [all tags]
            Dictionary<string, string> MovLens_IMDB = new Dictionary<string, string>();
            string dataset_path = @"C:\Универ\ml-latest\";
            Dictionary<string, string> tag_dict = new Dictionary<string, string>(); // tag_id: tag


            using (StreamReader reader = new StreamReader(dataset_path + "links_IMDB_MovieLens.csv"))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string[] elements = reader.ReadLine().Split(",");
                    MovLens_IMDB[elements[0]] = "tt" + elements[1];
                }
            }
            Console.WriteLine("Make tags done 1/3.");


            using (StreamReader reader = new StreamReader(dataset_path + "TagCodes_MovieLens.csv"))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string[] elements = reader.ReadLine().Split(",");
                    tag_dict[elements[0].Trim()] = elements[1].Trim();
                }
            }
            Console.WriteLine("Make tags done 2/3.");

            using (StreamReader reader = new StreamReader(dataset_path + "TagScores_MovieLens.csv"))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string[] elements = reader.ReadLine().Split(",");
                    string tagid = elements[1].Trim(), rel = elements[2].Trim(), MovLens_id = elements[0].Trim();
                    string IMDB_id = MovLens_IMDB[MovLens_id];
                    int relevants;
                    if (rel.Length < 4)
                        rel += "0";
                    relevants = Convert.ToInt32(rel.Substring(2, 2));
                    if (relevants > 50)
                    {
                        if (!films_id_name.ContainsKey(IMDB_id))
                            continue;
                        foreach (var film_name in films_id_name[IMDB_id])
                        {
                            if (!result.ContainsKey(film_name))
                                result[film_name] = new List<string>();
                            result[film_name].Add(tag_dict[tagid]);
                        }
                        
                    }
                }
            }
            Console.WriteLine("Make tags done 3/3.");

            return result;
        }
        static Dictionary<string, Person> make_people(Dictionary<string, List<string>> films_id_name)
        {
            Console.WriteLine("Start make people.");
            string dataset_path = @"C:\Универ\ml-latest\";
            Dictionary<string, Person> persons = new Dictionary<string, Person>();  // id: Person

            using (StreamReader reader = new StreamReader(dataset_path + "ActorsDirectorsNames_IMDB.txt"))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string[] elems = reader.ReadLine().Split("\t");
                    string per_id = elems[0].Trim(), per_name = elems[1].Trim();
                    string[] professions = elems[4].Trim().Split(","), target_profs = new string[3] { "director", "actor", "actress" };
                    bool flag = false;
                    foreach(var prof in professions)
                    {
                        if (target_profs.Contains(prof))
                            flag = true;
                    }
                    if (flag)
                        persons[per_id] = new Person(per_name, per_id);
                }
            }
            Console.WriteLine("Make people 1/2 done.");


            using (StreamReader reader = new StreamReader(dataset_path + "ActorsDirectorsCodes_IMDB.tsv"))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string[] elems = reader.ReadLine().Split("\t");
                    string mov_id = elems[0].Trim(), chel_id = elems[2].Trim(), categ = elems[3].Trim();
                    if (!persons.ContainsKey(chel_id) || !films_id_name.ContainsKey(mov_id))
                        continue;
                    if (categ == "director")
                    {
                        persons[chel_id].director_movies_id.Add(mov_id);
                    } else
                    {
                        persons[chel_id].actor_movis_id.Add(mov_id);
                    }
                    persons[chel_id].movies_id.Add(mov_id);
                }
            }
            Console.WriteLine("Make people 2/2 done.");

            return persons;
        }
    }
}