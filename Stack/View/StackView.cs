using System;
using System.Collections.Generic;
using StackApp.Model;

namespace StackApp.View
{
    public class StackView
    {
        public void ShowMessage(string text)
        {
            Console.WriteLine(text);
        }

        public void DisplayStack(List<Stock> stackSnapshot)
        {
            Console.WriteLine("\nCurrent Stack (Top ? Bottom):");
            if (stackSnapshot == null || stackSnapshot.Count == 0)
            {
                Console.WriteLine("  [Empty]");
                return;
            }
            foreach (var s in stackSnapshot)
                Console.WriteLine($"  {s.Date:yyyy-MM-dd} -> {s.Price}");
        }

        public void ShowUndo(Stock? s)
        {
            Console.WriteLine(s == null ? "Undo: nothing to undo" : $"Undo: removed {s}");
        }

        public void ShowRedo(Stock? s)
        {
            Console.WriteLine(s == null ? "Redo: nothing to redo" : $"Redo: restored {s}");
        }

        public void ShowMinMax(double min, double max, DateTime start, DateTime end)
        {
            Console.WriteLine($"\nMin/Max from {start:yyyy-MM-dd} to {end:yyyy-MM-dd}: Min = {min}, Max = {max}");
        }
    }
}
