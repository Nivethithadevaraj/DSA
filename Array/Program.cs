using TitanicApp.Controller;

namespace TitanicApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var controller = new TitanicController();
            controller.Run();
        }
    }
}
