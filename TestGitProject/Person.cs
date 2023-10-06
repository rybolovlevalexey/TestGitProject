using System;
using System.Collections.Generic;
using System.Text;

namespace TestGitProject
{
    class Person
    {
        public string name;
        public List<string> movies_id;

        public Person(string name, List<string> ids)
        {
            this.name = name;
            movies_id = ids;
        }
    }
}
