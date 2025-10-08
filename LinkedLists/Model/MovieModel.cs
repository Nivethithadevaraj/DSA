using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace IMDbApp.Model
{
    public class Movie
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public double Rating { get; set; }
    }


    public class MovieModel
    {
        // -------------------------------
        // Singly Linked List
        // -------------------------------
        private class SinglyNode
        {
            public Movie Data;
            public SinglyNode Next;
            public SinglyNode(Movie data) { Data = data; }
        }
        private SinglyNode headSingly;

        public void InsertSingly(Movie movie)
        {
            var node = new SinglyNode(movie);
            if (headSingly == null) { headSingly = node; return; }
            var current = headSingly;
            while (current.Next != null) current = current.Next;
            current.Next = node;
        }

        public List<Movie> TraverseSingly()
        {
            var list = new List<Movie>();
            var current = headSingly;
            while (current != null) { list.Add(current.Data); current = current.Next; }
            return list;
        }

        public bool UpdateSingly(string title, double newRating)
        {
            var current = headSingly;
            while (current != null)
            {
                if (string.Equals(current.Data.Title, title, StringComparison.OrdinalIgnoreCase))
                { current.Data.Rating = newRating; return true; }
                current = current.Next;
            }
            return false;
        }

        public bool DeleteSingly(string title)
        {
            if (headSingly == null) return false;
            if (headSingly.Data.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
            { headSingly = headSingly.Next; return true; }
            var current = headSingly;
            while (current.Next != null)
            {
                if (current.Next.Data.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                { current.Next = current.Next.Next; return true; }
                current = current.Next;
            }
            return false;
        }

        public void ReverseSingly()
        {
            SinglyNode prev = null, current = headSingly, next = null;
            while (current != null)
            {
                next = current.Next;
                current.Next = prev;
                prev = current;
                current = next;
            }
            headSingly = prev;
        }

        public void InsertSorted(Movie movie)
        {
            var node = new SinglyNode(movie);
            if (headSingly == null || movie.Rating > headSingly.Data.Rating)
            { node.Next = headSingly; headSingly = node; return; }
            var current = headSingly;
            while (current.Next != null && current.Next.Data.Rating >= movie.Rating) current = current.Next;
            node.Next = current.Next;
            current.Next = node;
        }

        public List<Movie> FindTopRated(int startYear, int endYear)
        {
            var result = new List<Movie>();
            var current = headSingly;
            while (current != null)
            {
                if (current.Data.Year >= startYear && current.Data.Year <= endYear)
                    result.Add(current.Data);
                current = current.Next;
            }
            result.Sort((a, b) => b.Rating.CompareTo(a.Rating));
            return result.Count > 5 ? result.GetRange(0, 5) : result;
        }

        // -------------------------------
        // Doubly Linked List
        // -------------------------------
        private class DoublyNode
        {
            public Movie Data;
            public DoublyNode Next;
            public DoublyNode Prev;
            public DoublyNode(Movie data) { Data = data; }
        }
        private DoublyNode headDoubly;
        private DoublyNode tailDoubly;

        public void InsertDoubly(Movie movie)
        {
            var node = new DoublyNode(movie);
            if (headDoubly == null) { headDoubly = tailDoubly = node; return; }
            tailDoubly.Next = node; node.Prev = tailDoubly; tailDoubly = node;
        }

        public List<Movie> TraverseDoublyForward()
        {
            var list = new List<Movie>();
            var current = headDoubly;
            while (current != null) { list.Add(current.Data); current = current.Next; }
            return list;
        }

        public List<Movie> TraverseDoublyReverse()
        {
            var list = new List<Movie>();
            var current = tailDoubly;
            while (current != null) { list.Add(current.Data); current = current.Prev; }
            return list;
        }

        // -------------------------------
        // Circular Linked List
        // -------------------------------
        private class CircularNode
        {
            public Movie Data;
            public CircularNode Next;
            public CircularNode(Movie data) { Data = data; }
        }
        private CircularNode headCircular;
        private CircularNode tailCircular;

        public void InsertCircular(Movie movie)
        {
            var node = new CircularNode(movie);
            if (headCircular == null) { headCircular = tailCircular = node; node.Next = node; return; }
            node.Next = headCircular; tailCircular.Next = node; tailCircular = node;
        }

        public List<Movie> TraverseCircular(int limit = 10)
        {
            var list = new List<Movie>();
            if (headCircular == null) return list;
            var current = headCircular; int count = 0;
            do { list.Add(current.Data); current = current.Next; count++; }
            while (current != headCircular && count < limit);
            return list;
        }

        // -------------------------------
        // Load movies from CSV
        // -------------------------------

public List<Movie> LoadMovies(string filePath, int maxRows = 20)
    {
        var list = new List<Movie>();

        using (TextFieldParser parser = new TextFieldParser(filePath))
        {
            parser.SetDelimiters(new string[] { "," });
            parser.HasFieldsEnclosedInQuotes = true;

            // skip header
            parser.ReadLine();

            int count = 0;
            while (!parser.EndOfData && count < maxRows)
            {
                var parts = parser.ReadFields();
                if (parts.Length < 10) continue;

                string title = parts[1].Trim(); // Series_Title
                int year = int.TryParse(parts[2].Trim(), out var y) ? y : 0;
                string genre = parts[5].Trim();
                double rating = double.TryParse(parts[6].Trim(), out var r) ? r : 0.0;
                string director = parts[9].Trim();

                list.Add(new Movie
                {
                    Title = title,
                    Year = year,
                    Genre = genre,
                    Rating = rating,
                    Director = director
                });

                count++;
            }
        }

        return list;
    }


}
}
