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

        private HashSet<string> actors = new HashSet<string>();
        private string director;
        private HashSet<string> tags = new HashSet<string>();

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
        public void add_tags(string tag_name) { this.tags.Add(tag_name); }
        public void change_director(string director_name) { this.director = director_name; }
    }
}
