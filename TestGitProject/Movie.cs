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

        public HashSet<string> actors = new HashSet<string>();
        public HashSet<string> directors = new HashSet<string>();

        public Movie(string name, string id) {
            this.name = name;
            this.id = id;
        }
    }
}
