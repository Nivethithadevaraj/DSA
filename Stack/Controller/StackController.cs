using System;
using System.Linq;
using StackApp.Model;
using StackApp.View;

namespace StackApp.Controller
{
    public class StackController
    {
        private readonly StackModel model = new();
        private readonly StackView view = new();
        private System.Collections.Generic.List<Stock> allStocks = new();

        public void Run(string filePath)
        {
            allStocks = model.LoadStocks(filePath, 1000);
            if (allStocks.Count == 0)
            {
                view.ShowMessage($"No stocks loaded from: {filePath}");
                return;
            }

            view.ShowMessage($"Loaded {allStocks.Count} rows.");

            int pushCount = Math.Min(20, allStocks.Count);
            model.InitializeStack(allStocks.Take(pushCount).Reverse(), pushCount);

            view.DisplayStack(model.GetStackSnapshot());

            var undone = model.Undo();
            view.ShowUndo(undone);
            view.DisplayStack(model.GetStackSnapshot());

            var redone = model.Redo();
            view.ShowRedo(redone);
            view.DisplayStack(model.GetStackSnapshot());

            DateTime start = new DateTime(2010, 1, 1);
            DateTime end = new DateTime(2015, 12, 31);
            var (min, max) = model.GetMinMax(allStocks, start, end);
            view.ShowMinMax(min, max, start, end);
        }
    }
}
