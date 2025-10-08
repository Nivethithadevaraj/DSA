using System;
using System.Collections.Generic;
using IMDbApp.Model;

namespace IMDbApp.View
{
    public class MovieView
    {
        public void PrintMovies(string header, List<Movie> movies)
        {
            Console.WriteLine($"\n{header}:");
            foreach (var m in movies)
                Console.WriteLine($"{m.Title} ({m.Year}) [{m.Genre}] Dir: {m.Director} -> {m.Rating}");
        }


        public void PrintMessage(string message) => Console.WriteLine(message);
    }
}
