using System;
using System.Collections.Generic;
using System.Linq;

namespace CoffeeMachine
{
    public class CoffeeMachine
    {
        #region Constants

        public const int MIN = 1;
        public const int MAX = 10;

        #endregion

        #region Properties

        public List<int> Coins { get; set; }
        public List<int> EnteredCoins { get; set; }
        public int Balance
        {
            get => EnteredCoins.Aggregate(0, (a, b) => a + b);
        }

        #endregion

        #region Constructors

        public CoffeeMachine()
        {
            this.Coins = new List<int> { 50, 100, 200, 500 };
            this.EnteredCoins = new List<int>();
        }

        #endregion

        #region Methods
        public void Start()
        {
            List<Coffees> coffees = Coffees.GetAllCoffees();
            string text = "Menu:";
            coffees.ForEach(item => text += string.Format("\n\r {0} - {1} - {2}", item.CoffeeNumber, item.CoffeeName, item.Price));
            Console.WriteLine(text);
            Console.WriteLine();

            this.ReadInput();
        }

        public void ReadInput()
        {
            Console.WriteLine("Insert Coins then press 1 to 10 to get Coffee.");
            this.PrintBalance();
            int coffeeNumber = 0;

            while (true)
            {
                try
                {
                    Int32.TryParse(Console.ReadLine(), out int value);

                    if (value == 0)
                    {
                        if (this.Balance != 0)
                        {
                            Console.WriteLine($"Please, take your {this.Balance} change.");
                        }
                        this.EnteredCoins.Clear();
                        this.ReadInput();
                        break;
                    }

                    if (this.Coins.Contains(value))
                    {
                        this.EnteredCoins.Add(value);
                        this.PrintBalance();
                    }
                    else
                    {
                        if (value >= MIN && value <= MAX)
                        {
                            coffeeNumber = value;
                            break;
                        }

                        throw new Exception();
                    }

                }
                catch
                {
                    Console.WriteLine("Invalid coin. Please enter 50, 100, 200 or 500 valued coins.");
                }
            }
            if (coffeeNumber != 0)
            {
                this.ChooseCoffee(coffeeNumber);
            }
        }

        #endregion

        #region Private Methods

        private void ChooseCoffee(int coffeeNumber)
        {
            try
            {
                Coffees coffee = Coffees.GetCoffeeByNumber(coffeeNumber);
                int balance = this.Balance;

                if (balance == 0)
                {
                    Console.WriteLine("Please enter coins to get coffee.");
                    this.ReadInput();
                    return;
                }
                else if (balance < coffee.Price)
                {
                    Console.WriteLine("Please enter enough money to get coffee or press 0 to get your money back.");
                    this.ReadInput();
                    return;
                }

                if (!Coffees.CheckStoreIsEnough(coffee, out string message))
                {
                    message += "\n\rPlease choose another coffee or press 0 to get your money back.";
                    Console.WriteLine(message);
                    this.ReadInput();
                }

                if (Coffees.UpdateStore(coffee))
                {
                    int change = this.Balance - coffee.Price;
                    this.EnteredCoins.Clear();

                    Console.WriteLine(string.Format("Your '{0}' is ready.", coffee.CoffeeName));
                    if (change > 0)
                    {
                        this.EnteredCoins.Add(change);
                        Console.WriteLine(string.Format("Please, take another coffee or press 0 to get your {0} change back.", change));
                    }

                    this.ReadInput();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private void PrintBalance()
        {
            Console.WriteLine($"Balance: {this.Balance}");
        }

        #endregion

    }
}