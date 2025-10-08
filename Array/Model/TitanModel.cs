using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace TitanicApp.Model
{
	public class Passenger
	{
		public int PassengerId { get; set; }
		public int Survived { get; set; }
		public int Pclass { get; set; }
		public string Name { get; set; } = "";
		public string Sex { get; set; } = "";
		public double? Age { get; set; }
		public int SibSp { get; set; }
		public int Parch { get; set; }
		public string Ticket { get; set; } = "";
		public double Fare { get; set; }
		public string Cabin { get; set; } = "";
		public string Embarked { get; set; } = "";
	}

	public class TitanicModel
	{
		private string[] SplitCsvLine(string line)
		{
			var parts = new List<string>();
			if (string.IsNullOrEmpty(line)) return parts.ToArray();

			var sb = new StringBuilder();
			bool inQuotes = false;
			for (int i = 0; i < line.Length; i++)
			{
				char c = line[i];
				if (c == '\"')
				{
					inQuotes = !inQuotes;
					continue;
				}
				if (c == ',' && !inQuotes)
				{
					parts.Add(sb.ToString());
					sb.Clear();
				}
				else sb.Append(c);
			}
			parts.Add(sb.ToString());
			return parts.ToArray();
		}

		// Loads only first N passengers
		public List<Passenger> LoadFirstNPassengers(string relativePath, int n)
		{
			string fullPath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

			if (!File.Exists(fullPath))
			{
				Console.WriteLine($"File not found: {fullPath}");
				return new List<Passenger>();
			}

			var lines = File.ReadAllLines(fullPath);
			var list = new List<Passenger>();

			foreach (var line in lines.Skip(1).Take(n))
			{
				var cols = SplitCsvLine(line);
				while (cols.Length < 12)
				{
					Array.Resize(ref cols, 12);
					for (int i = 0; i < cols.Length; i++) if (cols[i] == null) cols[i] = "";
				}

				try
				{
					var p = new Passenger
					{
						PassengerId = int.TryParse(cols[0], out var id) ? id : 0,
						Survived = int.TryParse(cols[1], out var s) ? s : 0,
						Pclass = int.TryParse(cols[2], out var pc) ? pc : 0,
						Name = cols[3].Trim(),
						Sex = cols[4].Trim(),
						Age = double.TryParse(cols[5], NumberStyles.Any, CultureInfo.InvariantCulture, out var a) ? a : (double?)null,
						SibSp = int.TryParse(cols[6], out var ss) ? ss : 0,
						Parch = int.TryParse(cols[7], out var pa) ? pa : 0,
						Ticket = cols[8].Trim(),
						Fare = double.TryParse(cols[9], NumberStyles.Any, CultureInfo.InvariantCulture, out var f) ? f : 0,
						Cabin = cols[10].Trim(),
						Embarked = cols[11].Trim()
					};
					list.Add(p);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"?? Skipped row due to parse error: {ex.Message}");
				}
			}

			return list;
		}

		// 1D: ages
		public double?[] GetAges1D(List<Passenger> sample)
		{
			return sample.Select(p => p.Age).ToArray();
		}

		// 2D (rectangular)
		public object[,] GetIdAge2D(List<Passenger> sample)
		{
			int r = sample.Count;
			var mat = new object[r, 2];
			for (int i = 0; i < r; i++)
			{
				mat[i, 0] = sample[i].PassengerId;
				mat[i, 1] = sample[i].Age.HasValue ? (object)sample[i].Age.Value : "N/A";
			}
			return mat;
		}

		// Jagged:
		public double[][] GetFaresJaggedByClass(List<Passenger> sample)
		{
			var groups = sample.GroupBy(p => p.Pclass).OrderBy(g => g.Key).ToList();
			var jagged = new double[groups.Count][];
			for (int i = 0; i < groups.Count; i++)
			{
				jagged[i] = groups[i].Select(p => p.Fare).ToArray();
			}
			return jagged;
		}

		// Rectangular numeric matrix: Age, Fare
		public double[,] GetAgeFareRect(List<Passenger> sample)
		{
			int r = sample.Count;
			var mat = new double[r, 2];
			for (int i = 0; i < r; i++)
			{
				mat[i, 0] = sample[i].Age ?? 0;
				mat[i, 1] = sample[i].Fare;
			}
			return mat;
		}

		// Linear search by name
		public Passenger? LinearSearchByName(List<Passenger> sample, string name)
		{
			return sample.FirstOrDefault(p => !string.IsNullOrEmpty(p.Name) &&
				p.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
		}

		// Binary search by PassengerId
		public Passenger? BinarySearchById(List<Passenger> sample, int id)
		{
			var sorted = sample.OrderBy(p => p.PassengerId).ToList();
			int left = 0, right = sorted.Count - 1;
			while (left <= right)
			{
				int mid = (left + right) / 2;
				if (sorted[mid].PassengerId == id) return sorted[mid];
				if (sorted[mid].PassengerId < id) left = mid + 1;
				else right = mid - 1;
			}
			return null;
		}

		// Manual bubble sort by Age
		public List<Passenger> BubbleSortByAge(List<Passenger> sample)
		{
			var arr = sample.ToList();
			int n = arr.Count;
			for (int i = 0; i < n - 1; i++)
			{
				for (int j = 0; j < n - i - 1; j++)
				{
					double aj = arr[j].Age ?? double.MaxValue;
					double aj1 = arr[j + 1].Age ?? double.MaxValue;
					if (aj > aj1)
					{
						var tmp = arr[j];
						arr[j] = arr[j + 1];
						arr[j + 1] = tmp;
					}
				}
			}
			return arr;
		}

		// Built-in sort by Fare
		public List<Passenger> SortByFareBuiltIn(List<Passenger> sample)
		{
			return sample.OrderBy(p => p.Fare).ToList();
		}

		// Survival rate by gender (percent)
		public Dictionary<string, double> SurvivalRateByGender(List<Passenger> sample)
		{
			return sample
				.Where(p => !string.IsNullOrEmpty(p.Sex))
				.GroupBy(p => p.Sex)
				.ToDictionary(g => g.Key, g => g.Average(p => p.Survived) * 100);
		}

		// Survival rate by class (percent)
		public Dictionary<string, double> SurvivalRateByClass(List<Passenger> sample)
		{
			return sample
				.GroupBy(p => p.Pclass)
				.ToDictionary(g => g.Key.ToString(), g => g.Average(p => p.Survived) * 100);
		}
	}
}
