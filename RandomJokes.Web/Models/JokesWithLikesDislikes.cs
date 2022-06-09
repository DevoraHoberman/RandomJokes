using RandomJokes.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RandomJokes.Web.Models
{
    public class JokesWithLikesDislikes : Joke
    {
        public int Likes { get; set; }
        public int Dislikes { get; set; }
    }
}
