using System.Configuration;

namespace CoffeeMachine
{
    class Program
    {
        public static readonly string ConnectionString;

        static Program ()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        }

        static void Main(string[] args)
        {
            CoffeeMachine coffeeMachine = new CoffeeMachine();
            coffeeMachine.Start();
        }
    }
}
