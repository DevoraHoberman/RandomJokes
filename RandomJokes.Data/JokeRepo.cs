using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomJokes.Data
{
    public class JokeRepo
    {
        private readonly string _connectionString;

        public JokeRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Joke> GetJokes()
        {
            using var context = new RandomJokesDataContext(_connectionString);
            return context.Jokes.ToList();
        }
        public int GetLikes(int id)
        {
            using var context = new RandomJokesDataContext(_connectionString);
            return context.UserLikedJokes.Where(q => q.JokeId == id && q.Liked == true).Count();
        }
        public int GetDislikes(int id)
        {
            using var context = new RandomJokesDataContext(_connectionString);
            return context.UserLikedJokes.Where(q => q.JokeId == id && q.Liked == false).Count();
        }

        public bool JokeAlreadyExists(Joke joke)
        {
            using var context = new RandomJokesDataContext(_connectionString);
            return context.Jokes.Any(a => a.Setup == joke.Setup) && !context.Jokes.Any(a => a.Punchline == joke.Punchline); ;
        }

        public Joke AddJoke(Joke joke)
        {
            using var context = new RandomJokesDataContext(_connectionString);
            joke.Id = 0;                      
            if(!JokeAlreadyExists(joke))
            {
                context.Jokes.Add(joke);
                context.SaveChanges();
            }
            else
            {
                joke = context.Jokes.Where(a => a.Setup == joke.Setup && a.Punchline == joke.Punchline).FirstOrDefault();

            }
            return joke;
        }
        public void LikeorDislike(UserLikedJokes userLikedJokes)
        {
            using var context = new RandomJokesDataContext(_connectionString);            
            if(!context.UserLikedJokes.Any(a => a.JokeId == userLikedJokes.JokeId && a.UserId == userLikedJokes.UserId))
            {               
                context.UserLikedJokes.Add(userLikedJokes);
                context.SaveChanges();                
            }
            else
            {
                context.UserLikedJokes.Update(userLikedJokes);
                context.SaveChanges();
            }            
        }
        public UserLikedJokes GetLikeorDislike(int id, int userId)
        {
            using var context = new RandomJokesDataContext(_connectionString);
            return context.UserLikedJokes.FirstOrDefault(a => a.UserId == userId && a.JokeId == id);
        }
    }
}
