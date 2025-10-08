using System;
using IMDbApp.Model;
using IMDbApp.View;

namespace IMDbApp.Controller
{
    public class MovieController
    {
        private readonly MovieModel model;
        private readonly MovieView view;

        public MovieController()
        {
            model = new MovieModel();
            view = new MovieView();
        }

        public void Run()
        {
            string filePath = @"C:\Users\nivethitha.devaraj\Documents\DSA\LinkedLists\Data\IMDB.csv";
            var movies = model.LoadMovies(filePath, 15);

            foreach (var m in movies) { model.InsertSingly(m); model.InsertDoubly(m); model.InsertCircular(m); }

            view.PrintMovies("Singly Traversal", model.TraverseSingly());
            model.UpdateSingly(movies[0].Title, 9.9);
            view.PrintMovies("After Update", model.TraverseSingly());
            model.DeleteSingly(movies[1].Title);
            view.PrintMovies("After Delete", model.TraverseSingly());
            model.ReverseSingly();
            view.PrintMovies("After Reverse", model.TraverseSingly());

            view.PrintMovies("Doubly Forward", model.TraverseDoublyForward());
            view.PrintMovies("Doubly Reverse", model.TraverseDoublyReverse());
            view.PrintMovies("Circular (sample)", model.TraverseCircular(8));

            view.PrintMovies("Top Rated (2000-2010)", model.FindTopRated(2000, 2010));

            var sortedModel = new MovieModel();
            foreach (var m in movies) sortedModel.InsertSorted(m);
            view.PrintMovies("Sorted by Rating", sortedModel.TraverseSingly());
        }
    }
}
