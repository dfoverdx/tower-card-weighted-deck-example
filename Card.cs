using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace TowerCardsWeightedDeckExample
{
    public enum Rarity
    {
        Common = 80,
        Rare = 17,
        Epic = 3
    }

    public class Card
    {
        private static readonly ReadOnlyCollection<int> RankCosts = new ReadOnlyCollection<int>(new [] { 1, 2, 5, 8, 12, 20, 32 });
        private static readonly int TotalPurchasesPerCard = RankCosts.Aggregate((x, y) => x + y); // 80

        public Card(
            string name,
            Rarity rarity,
            Func<float, string> getDescription,
            float[] rankValues,
            int purchased = 0,
            bool equipped = false,
            bool locked = false
        )
        {
            Name = name;
            Rarity = rarity;
            GetDescription = getDescription;
            RankValues = new ReadOnlyCollection<float>(rankValues);
            Purchased = purchased;
            Equipped = equipped;
            Locked = locked;
        }

        public string Name { get; }
        public Rarity Rarity { get; }
        public int Purchased { get; set; } = 0;
        private Func<float, string> GetDescription { get; }
        public ReadOnlyCollection<float> RankValues { get; }
        public bool Equipped { get; set; }
        public bool Locked { get; } = false;

        public string Description => GetDescription(this.Value);

        public float Value => Rank == 0 ? 0 : RankValues[Rank - 1];

        public int Rank
        {
            get
            {
                int rank = 0;
                int purchased = Purchased;
                foreach (int cost in RankCosts)
                {
                    if (cost > purchased)
                    {
                        return rank;
                    }

                    purchased -= cost;
                    rank++;
                }

                return rank;
            }
        }

        public int PurchaseWeight => (TotalPurchasesPerCard - Purchased) * (int)this.Rarity;

        public override string ToString() {
            return @$"{Name}
{new string('=', Name.Length)}
Purchased: {Purchased} / {TotalPurchasesPerCard}
Rank: {Rank}
Value: {Value}
{Description}
";
        }
    }
}
