using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RandomJokes.Data
{
    public class Joke
    {
        public int Id { get; set; }        
        public string Setup { get; set; }
        public string Punchline { get; set; }        
        public string Type { get; set; }    
    }
}
