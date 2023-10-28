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
            Console.WriteLine("словарь с людьми наполнен");

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
            Console.WriteLine("словарь с тэгами наполнен");

            for (int i = 0; i < 25; i += 1)
            {
                var cur = films[films.Keys.ToArray()[i]];
                Console.WriteLine($"{cur.name} {cur.rating} {cur.id} {cur.directors}");
                if (cur.tags != null)
                {
                    foreach (var el in cur.tags)
                        Console.Write($"{el} ");
                }
                Console.WriteLine();
                if (cur.actors != null)
                {
                    foreach (var el in cur.actors)
                        Console.Write($"{el} ");
                }
                Console.WriteLine("--------------");
            }

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
                            if (result.tags != null)
                                Console.WriteLine($"располагает следующими тэгами: {result.tags.ToString()}");
                            Console.WriteLine($"и актёрами: {result.actors.ToString()}");
                            Console.WriteLine($"режиссёры - {result.directors}");
                        }
                        break;
                    case "b":
                        string person_name = Console.ReadLine();
                        if (!people.ContainsKey(person_name))
                        {
                            Console.WriteLine("Указанный человек не найден");
                        } else
                        {
                            Console.WriteLine($"Человек с именем {person_name} участвовал в следующих проектах:");
                            foreach (var cur_film in people[person_name])
                                Console.WriteLine(cur_film.name);
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
                            foreach (var cur_film in tags_dict[tag_name])
                                Console.WriteLine(cur_film.name);
                        }
                        break;
                }
            }
        }

        static Dictionary<string, List<string>> make_tags(Dictionary<string, List<string>> films_id_name)
        {
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>(); // film name: [all tags]

            string dataset_path = @"C:\Универ\ml-latest\";
            Dictionary<string, int> relev_dict = new Dictionary<string, int>(); // film_id-tag_id
            Dictionary<string, string> tag_dict = new Dictionary<string, string>(); // tag_id: tag

            using (StreamReader reader = new StreamReader(dataset_path + "TagScores_MovieLens.csv"))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] elements = line.Split(",");
                    string tagid = elements[1].Trim(), rel = elements[2].Trim(), film_id = elements[0].Trim(), filmid = "tt";
                    for (int i = 0; i < (7 - film_id.Length); i += 1)
                        filmid += "0";
                    filmid += film_id;
                    int relevants;
                    if (rel.Length < 4)
                        rel += "0";
                    relevants = Convert.ToInt32(rel.Substring(2, 2));
                    relev_dict[filmid + "-" + tagid] = relevants;
                }
                
            }
            Console.WriteLine("Make tags done 1/3.");

            using (StreamReader reader = new StreamReader(dataset_path + "TagCodes_MovieLens.csv"))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] elements = line.Split(",");
                    string tag_id = elements[0].Trim(), tag = elements[1].Trim();
                    tag_dict[tag_id] = tag;
                }
            }
            Console.WriteLine("Make tags done 2/3.");
            foreach (var elem in tags_dict.Keys.Take<string>(10))
                Console.WriteLine(elem);


            using (StreamReader reader = new StreamReader(dataset_path + "links_IMDB_MovieLens.csv"))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
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
            }
            Console.WriteLine("Make tags done 3/3.");

            Dictionary<string, List<string>> last_result = new Dictionary<string, List<string>>(); // film name: [all tags]
            foreach (var key in result.Keys.AsParallel())
            {
                if (result[key].Count != 0)
                    last_result[key] = result[key];
            }
            return last_result;
        }
        static Dictionary<string, Person> make_people(Dictionary<string, List<string>> films_id_name)
        {
            Console.WriteLine("start make people");

            //Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();  // имя человека: список фильмов
            string dataset_path = @"C:\Универ\ml-latest\";

            Dictionary<string, Person> persons = new Dictionary<string, Person>();  // id: name

            using (StreamReader reader = new StreamReader(dataset_path + "ActorsDirectorsNames_IMDB.txt"))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] elems = line.Split("\t");
                    string id = elems[0].Trim(), name = elems[1].Trim();
                    persons[id] = new Person(name, id);
                }
            }
            Console.WriteLine("make people 1 done");

            using (StreamReader reader = new StreamReader(dataset_path + "ActorsDirectorsCodes_IMDB.tsv"))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] elems = line.Split("\t");
                    string mov_id = elems[0].Trim(), chel_id = elems[2].Trim(), categ = elems[3].Trim();
                    
                    if (!(categ == "director" || categ == "actor" || categ == "actress") || !persons.ContainsKey(chel_id) || 
                        !films_id_name.ContainsKey(mov_id))
                        continue;
                    if (categ == "director")
                    {
                        foreach (string mov_name in films_id_name[mov_id])
                        {
                            persons[chel_id].director_movies_id.Add(mov_name);
                        }
                    } else
                    {
                        foreach (string mov_name in films_id_name[mov_id])
                        {
                            persons[chel_id].actor_movis_id.Add(mov_name);
                        }
                    }

                    foreach (string mov_name in films_id_name[mov_id])
                    {
                        persons[chel_id].movies_id.Add(mov_name);
                    }
                }
            }
            Console.WriteLine("make people 2 done");

            return persons;
        }
    }
}