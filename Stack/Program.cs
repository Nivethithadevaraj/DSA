using StackApp.Controller;

namespace StackApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = @"C:\Users\nivethitha.devaraj\Documents\DSA\Stack\Data\stocks.csv";
            var controller = new StackController();
            controller.Run(filePath);
        }
    }
}
