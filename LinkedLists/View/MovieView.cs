using System;
using System.Collections.Generic;
using IMDbApp.Model;

namespace IMDbApp.View
{
    public class MovieView
    {
        public void ShowMovies(IEnumerable<Movie> movies)
        {
            foreach (var movie in movies.Take(50))
            {
                Console.WriteLine($" {movie.Title} ({movie.Year}) - Rating: {movie.Rating}");
            }


        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
