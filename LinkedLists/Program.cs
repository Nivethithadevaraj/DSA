using IMDbApp.Controller;

namespace IMDbApp
{
    class Program
    {
        static void Main(string[] args)
        {
            MovieController controller = new MovieController();
            controller.Run();
        }
    }
}
