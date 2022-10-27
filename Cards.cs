using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace TowerCardsWeightedDeckExample
{
    public static class Cards
    {
        private static readonly Random s_random = new Random();

        public static readonly ReadOnlyCollection<Card> GameCards = new ReadOnlyCollection<Card>(new[] {
            new Card(
                name: "Damage",
                rarity: Rarity.Common,
                getDescription: value => $"Increase tower damage by x{value}",
                rankValues: new[] { 1.5f, 2.0f, 2.4f, 2.8f, 3.2f, 3.6f, 4.0f } // hmm, should rank 1 really be 1.5 and not 1.6?
            ),
            // ...
            new Card
            (
                name: "Critical Coin",
                rarity: Rarity.Rare,
                getDescription: value => $"If a basic enemy dies from a critical shot it has a chance to drop coins of {(int)(value * 100)}%",
                rankValues: new[] { 0.15f, 0.18f, 0.21f, 0.24f, 0.27f, 0.30f, 0.33f }
            ),
            // ...
            new Card
            (
                name: "Energy Shield",
                rarity: Rarity.Epic,
                getDescription: value => $"Shield that ignores a single attack, replenishes after {value} min",
                rankValues: new[] { 20f, 18f, 16f, 14f, 12f, 10f, 8f },
                locked: true
            )
        });

        public static Card Purchase()
        {
            int totalWeight = GameCards.Aggregate(0, (w, c) => w + c.PurchaseWeight);

            if (totalWeight == 0)
            {
                throw new Exception("You've already purchased all your cards, n00b.");
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            var debugStr = $"Purchasing a card from a deck with a weight of {totalWeight}";
            Console.WriteLine(debugStr);
            Console.WriteLine(new string('-', debugStr.Length));

            foreach (Card c in GameCards)
            {
                Console.WriteLine(
                    $"{c.Name} has been purchased {c.Purchased} times and has a current weight of {c.PurchaseWeight} ({((float)c.PurchaseWeight / totalWeight * 100).ToString("#.00")}%)"
                );
            }

            int weightedIndex = s_random.Next(totalWeight);
            Card theChosenOne = null;

            foreach (Card card in GameCards)
            {
                weightedIndex -= card.PurchaseWeight;
                if (weightedIndex < 0)
                {
                    theChosenOne = card;
                    break;
                }
            }

            Console.WriteLine($"Picked {theChosenOne.Name}");
            Console.WriteLine();
            Console.ResetColor();

            if (theChosenOne is null)
            {
                throw new Exception("Really should have picked a card.  Check your loop logic, dx/dt!  Srsly.  🤦‍♀️");
            }

            theChosenOne.Purchased++;
            return theChosenOne;
        }
    }
}