using System;
using System.Collections.Generic;
using System.Text;

namespace TestGitProject
{
    class Person
    {
        public string name;
        public string person_id;
        public List<string> movies_id = new List<string>();

        public List<string> actor_movis_id = new List<string>();
        public List<string> director_movies_id = new List<string>();

        public Person(string name, string person_id)
        {
            this.name = name;
            this.person_id = person_id;
        }
    }
}
