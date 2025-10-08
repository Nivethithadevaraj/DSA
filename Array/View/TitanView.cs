using System;
using System.Collections.Generic;
using TitanicApp.Model;

namespace TitanicApp.View
{
    public class TitanicView
    {
        public void ShowMessage(string msg)
        {
            Console.WriteLine(msg);
        }

        public void Show1DArray(string title, double?[] arr)
        {
            Console.WriteLine($"\n{title}:");
            for (int i = 0; i < arr.Length; i++)
            {
                Console.WriteLine($"[{i}] = {(arr[i].HasValue ? arr[i].Value.ToString("F2") : "N/A")}");
            }
        }

        public void Show2DArray(string title, object[,] mat)
        {
            Console.WriteLine($"\n{title}:");
            int rows = mat.GetLength(0), cols = mat.GetLength(1);
            for (int r = 0; r < rows; r++)
            {
                Console.Write($"Row {r}: ");
                for (int c = 0; c < cols; c++)
                    Console.Write(mat[r, c] + (c == cols - 1 ? "" : " | "));
                Console.WriteLine();
            }
        }

        public void ShowJaggedArray(string title, double[][] jagged)
        {
            Console.WriteLine($"\n{title}:");
            for (int i = 0; i < jagged.Length; i++)
            {
                Console.Write($"Group {i}: ");
                Console.WriteLine(string.Join(", ", jagged[i].Select(x => x.ToString("F2"))));
            }
        }

        public void ShowRectangularNumeric(string title, double[,] mat)
        {
            Console.WriteLine($"\n{title}:");
            int rows = mat.GetLength(0);
            for (int r = 0; r < rows; r++)
            {
                Console.WriteLine($"Row {r}: Age={mat[r, 0]:F2}, Fare={mat[r, 1]:F2}");
            }
        }

        public void ShowSearchResult(string title, Passenger? p)
        {
            Console.WriteLine($"\n{title}:");
            if (p == null) Console.WriteLine("Not found.");
            else ShowPassenger(p);
        }

        public void ShowPassenger(Passenger p)
        {
            Console.WriteLine($"{p.PassengerId} | {p.Name} | Sex: {p.Sex} | Age: {(p.Age.HasValue ? p.Age.Value.ToString("F1") : "N/A")} | Fare: {p.Fare:F2} | Survived: {p.Survived} | Class: {p.Pclass}");
        }

        public void ShowPassengersShort(IEnumerable<Passenger> list)
        {
            foreach (var p in list)
                Console.WriteLine($"{p.PassengerId} - {p.Name} - Age: {(p.Age.HasValue ? p.Age.Value.ToString("F1") : "N/A")} - Fare: {p.Fare:F2} - Survived: {p.Survived}");
        }

        public void ShowSurvivalRates(string category, Dictionary<string, double> rates)
        {
            Console.WriteLine($"\nSurvival rates by {category}:");
            foreach (var kv in rates)
            {
                Console.WriteLine($"{kv.Key}: {kv.Value:F2}%");
            }
        }
    }
}
