using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace IMDbApp.Model
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public double Rating { get; set; }
    }

    public class SinglyNode
    {
        public Movie Data { get; set; }
        public SinglyNode? Next { get; set; }
        public SinglyNode(Movie movie) => Data = movie;
    }

    public class DoublyNode
    {
        public Movie Data { get; set; }
        public DoublyNode? Next { get; set; }
        public DoublyNode? Prev { get; set; }
        public DoublyNode(Movie movie) => Data = movie;
    }

    public class CircularNode
    {
        public Movie Data { get; set; }
        public CircularNode? Next { get; set; }
        public CircularNode(Movie movie) => Data = movie;
    }

    public class MovieModel
    {
        private readonly string _datasetPath = @"C:\Users\nivethitha.devaraj\Documents\DSA\LinkedLists\Data\IMDB.csv";
        private readonly List<Movie> _movies = new();

        // CSV-safe splitter (handles quoted fields with commas)
        private string[] SplitCsvLine(string line)
        {
            var parts = new List<string>();
            if (string.IsNullOrEmpty(line)) return parts.ToArray();

            var sb = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                    continue;
                }

                if (c == ',' && !inQuotes)
                {
                    parts.Add(sb.ToString());
                    sb.Clear();
                }
                else
                {
                    sb.Append(c);
                }
            }

            parts.Add(sb.ToString());
            return parts.ToArray();
        }

        // Load movies from CSV (robust parsing + debug output)
        public void LoadMovies(int maxRows = 50)
        {
            if (!File.Exists(_datasetPath))
            {
                Console.WriteLine("Dataset not found: " + _datasetPath);
                return;
            }

            var lines = File.ReadAllLines(_datasetPath).Skip(1); // skip header
            int id = 1;

            foreach (var line in lines.Take(maxRows))
            {
                var cols = SplitCsvLine(line);

                // expect at least: Poster_Link, Series_Title, Released_Year, ..., IMDB_Rating at index 6
                if (cols.Length < 7)
                {
                    // print a short debug note for malformed lines
                    Console.WriteLine("Skipping malformed CSV row (not enough columns).");
                    continue;
                }

                // Trim whitespace
                for (int i = 0; i < cols.Length; i++) cols[i] = cols[i].Trim();

                // Parse fields safely
                if (!int.TryParse(cols[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out int year))
                {
                    // print debug note with title to inspect format
                    Console.WriteLine($"Skipping row: cannot parse Year for title '{cols[1]}'. Value='{cols[2]}'");
                    continue;
                }

                if (!double.TryParse(cols[6], NumberStyles.Any, CultureInfo.InvariantCulture, out double rating))
                {
                    Console.WriteLine($"Skipping row: cannot parse Rating for title '{cols[1]}'. Value='{cols[6]}'");
                    continue;
                }

                var movie = new Movie
                {
                    Id = id++,
                    Title = cols[1],
                    Year = year,
                    Rating = rating
                };

                _movies.Add(movie);
            }

            Console.WriteLine($"Loaded {_movies.Count} movie(s) from {_datasetPath}");
            if (_movies.Count > 0)
            {
                Console.WriteLine("Sample loaded movies:");
                foreach (var m in _movies.Take(Math.Min(5, _movies.Count)))
                    Console.WriteLine($"  {m.Title} ({m.Year}) - {m.Rating}");
            }
        }

        public IReadOnlyList<Movie> GetMovies() => _movies.AsReadOnly();

        public SinglyNode? BuildSinglyList()
        {
            if (_movies.Count == 0) return null;

            var head = new SinglyNode(_movies[0]);
            var current = head;
            for (int i = 1; i < _movies.Count; i++)
            {
                current.Next = new SinglyNode(_movies[i]);
                current = current.Next;
            }

            return head;
        }

        public DoublyNode? BuildDoublyList()
        {
            if (_movies.Count == 0) return null;

            var head = new DoublyNode(_movies[0]);
            var current = head;
            for (int i = 1; i < _movies.Count; i++)
            {
                var node = new DoublyNode(_movies[i]);
                current.Next = node;
                node.Prev = current;
                current = node;
            }
            return head;
        }

        public CircularNode? BuildCircularList()
        {
            if (_movies.Count == 0) return null;

            var head = new CircularNode(_movies[0]);
            var current = head;
            for (int i = 1; i < _movies.Count; i++)
            {
                current.Next = new CircularNode(_movies[i]);
                current = current.Next;
            }
            current.Next = head;
            return head;
        }

        public IEnumerable<Movie> TraverseSingly(SinglyNode? head)
        {
            var current = head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }

        public SinglyNode? ReverseSingly(SinglyNode? head)
        {
            SinglyNode? prev = null, current = head, next = null;
            while (current != null)
            {
                next = current.Next;
                current.Next = prev;
                prev = current;
                current = next;
            }
            return prev;
        }

        public IEnumerable<Movie> GetTopMovies(int startYear, int endYear, int topN)
        {
            var filtered = _movies
                .Where(m => m.Year >= startYear && m.Year <= endYear)
                .OrderByDescending(m => m.Rating)
                .Take(topN)
                .ToList();

            if (!filtered.Any())
                Console.WriteLine($" No movies found between {startYear} and {endYear}.");

            return filtered;
        }
    }
}
