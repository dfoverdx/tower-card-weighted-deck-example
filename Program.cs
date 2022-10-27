using System;

namespace TowerCardsWeightedDeckExample
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 20; i++)
            {
                Cards.Purchase();
            }

            foreach (Card card in Cards.GameCards)
            {
                Console.WriteLine(card);
            }
        }
    }
}
