using System;
using System.Collections.Generic;
using System.Text;

namespace TestGitProject
{
    class Movie
    {
        public string name;
        public string rating;
        public string id;
        public HashSet<string> tags = new HashSet<string>();

        private HashSet<string> actors = new HashSet<string>();
        private string director;

        public Movie(string name, string id, HashSet<string> actors=null, string director=null, HashSet<string> tags=null, string rating=null) {
            this.name = name;
            this.actors = actors;
            this.director = director;
            this.tags = tags;
            this.rating = rating;
            this.id = id;
        }

        public void add_actor(string name_actor)
        {
            this.actors.Add(name_actor);
        }
        public HashSet<string> give_actors() { return this.actors; }
        public void change_director(string director_name) { this.director = director_name; }
        public string give_director() { return this.director; }
    }
}
