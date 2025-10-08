using System;
using System.IO;
using TitanicApp.Model;
using TitanicApp.View;

namespace TitanicApp.Controller
{
    public class TitanicController
    {
        private readonly TitanicModel _model;
        private readonly TitanicView _view;

        public TitanicController()
        {
            _model = new TitanicModel();
            _view = new TitanicView();
        }

        public void Run()
        {
            string relativePath = Path.Combine("Data", "titanic.csv");

            var passengers = _model.LoadFirstNPassengers(relativePath, 5);
            if (passengers.Count == 0)
            {
                _view.ShowMessage("No passengers loaded. Put titanic.csv into ProjectFolder/Data/");
                return;
            }

            _view.ShowMessage($"Loaded {passengers.Count} passengers (first sample).");

            var ages1D = _model.GetAges1D(passengers);
            _view.Show1DArray("Ages (1D)", ages1D);

            var idAge2D = _model.GetIdAge2D(passengers);
            _view.Show2DArray("ID and Age (2D rectangular)", idAge2D);

            var faresJagged = _model.GetFaresJaggedByClass(passengers);
            _view.ShowJaggedArray("Fares grouped by Pclass (Jagged)", faresJagged);

            var ageFareRect = _model.GetAgeFareRect(passengers);
            _view.ShowRectangularNumeric("Age-Fare matrix (rectangular)", ageFareRect);

            string searchName = passengers[1].Name;
            var foundLinear = _model.LinearSearchByName(passengers, searchName);
            _view.ShowSearchResult($"Linear search for '{searchName}'", foundLinear);

            int searchId = passengers[2].PassengerId; 
            var foundBinary = _model.BinarySearchById(passengers, searchId);
            _view.ShowSearchResult($"Binary search for ID {searchId}", foundBinary);

            var bubbleSortedByAge = _model.BubbleSortByAge(passengers);
            _view.ShowMessage("\nBubble sort result (by Age) — ascending:");
            _view.ShowPassengersShort(bubbleSortedByAge);

            var builtinSortedByFare = _model.SortByFareBuiltIn(passengers);
            _view.ShowMessage("\nBuilt-in sort result (by Fare) — ascending:");
            _view.ShowPassengersShort(builtinSortedByFare);

            var survByGender = _model.SurvivalRateByGender(passengers);
            _view.ShowSurvivalRates("Gender", survByGender);

            var survByClass = _model.SurvivalRateByClass(passengers);
            _view.ShowSurvivalRates("Class", survByClass);
        }
    }
}
