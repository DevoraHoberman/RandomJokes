using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RandomJokes.Data;
using RandomJokes.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RandomJokes.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JokesController : ControllerBase
    {
        private string _connectionString;
       

        public JokesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");          
        }

        [Route("getjoke")]
        [HttpGet]
        public JokesWithLikesDislikes GetJoke()
        {
            using var client = new HttpClient();
            var json = client.GetStringAsync("https://jokesapi.lit-projects.com/jokes/programming/random").Result;
            Joke joke = JsonConvert.DeserializeObject<List<Joke>>(json).FirstOrDefault();           
            var repo = new JokeRepo(_connectionString);
            var newJoke = repo.AddJoke(joke);
            var jwld = new JokesWithLikesDislikes
            {
                Id = newJoke.Id,
                Punchline = newJoke.Punchline,
                Type = newJoke.Type,
                Setup = newJoke.Setup,
                Likes = repo.GetLikes(joke.Id),
                Dislikes = repo.GetDislikes(joke.Id)
            };
            return jwld;
        }

        [Route("getjokes")]
        [HttpGet]
        public List<JokesWithLikesDislikes> ViewAll()
        {
            var repo = new JokeRepo(_connectionString);
            var jokes = repo.GetJokes();
            return jokes.Select(joke => new JokesWithLikesDislikes
            {
                Id = joke.Id,
                Punchline = joke.Punchline,
                Type = joke.Type,
                Setup = joke.Setup,
                Likes = repo.GetLikes(joke.Id),
                Dislikes = repo.GetDislikes(joke.Id)
            }).ToList();
        }

        [Route("getlikes")]
        [HttpGet]
        public int GetLikes(int id)
        {
            var repo = new JokeRepo(_connectionString);
            return repo.GetLikes(id);
        }

        [Route("getdislikes")]
        [HttpGet]
        public int GetDislikes(int id)
        {
            var repo = new JokeRepo(_connectionString);
            return repo.GetDislikes(id);
        }

        [Route("likejoke")]
        [HttpPost]
        public void LikeJoke(int id)
        {
            var jokeRepo = new JokeRepo(_connectionString);
            var userRepo = new UserRepo(_connectionString);
            string email = User.FindFirst("user")?.Value;
            var user = userRepo.GetByEmail(email);
            var userlikedJokes = new UserLikedJokes
            {
                JokeId = id,
                Liked = true,
                UserId = user.Id,
                Date = DateTime.Now
            };
            jokeRepo.LikeorDislike(userlikedJokes);
        }

        [Route("dislikejoke")]
        [HttpPost]
        public void DislikeJoke(int id)
        {
            var jokeRepo = new JokeRepo(_connectionString);
            var userRepo = new UserRepo(_connectionString);
            string email = User.FindFirst("user")?.Value;
            var user = userRepo.GetByEmail(email);
            var userlikedJokes = new UserLikedJokes
            {
                JokeId = id,
                Liked = false,
                UserId = user.Id,
                Date = DateTime.Now
            };
            jokeRepo.LikeorDislike(userlikedJokes);
        }

        [Route("getlikeordislike")]
        [HttpGet]
        public UserLikedJokes GetLikeorDislike(int id)
        {
            var jokeRepo = new JokeRepo(_connectionString);
            var userRepo = new UserRepo(_connectionString);
            string email = User.FindFirst("user")?.Value;
            var user = userRepo.GetByEmail(email);
            return jokeRepo.GetLikeorDislike(id, user.Id);
        }
    }
}
