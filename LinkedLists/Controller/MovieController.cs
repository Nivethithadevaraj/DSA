using System.Linq;
using IMDbApp.Model;
using IMDbApp.View;

namespace IMDbApp.Controller
{
    public class MovieController
    {
        private readonly MovieModel _model;
        private readonly MovieView _view;

        public MovieController()
        {
            _model = new MovieModel();
            _view = new MovieView();
        }

        public void Run()
        {
            _model.LoadMovies(50); // load first 50 rows for demo

            var all = _model.GetMovies();
            _view.ShowMessage($"Total movies loaded: {all.Count}");

            if (all.Count == 0)
            {
                _view.ShowMessage("No movies loaded; check dataset path and CSV format.");
                return;
            }

            var singly = _model.BuildSinglyList();
            if (singly == null)
            {
                _view.ShowMessage("Singly linked list is null.");
            }
            else
            {
                var movies = _model.TraverseSingly(singly);
                _view.ShowMessage("\nAll Movies (from singly linked list):");
                _view.ShowMovies(movies.Take(50));
            }

            _view.ShowMessage("\nTop Rated Movies (2000-2010):");
            var top = _model.GetTopMovies(2000, 2010, 5);
            _view.ShowMovies(top);
        }

    }
}
