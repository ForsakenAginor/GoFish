using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish
{
    public class Card : IComparable<Card>
    {
        public Suit Suit { get; private set; }
        public Value Value {get; private set;}
        public Card(Suit suit, Value value)
        {
            this.Suit = suit;
            this.Value = value;
        }
        public override string ToString()
        {
            return ($"A {Value} of {Suit}");
        }

        public void ShowCard()
        {
            Console.WriteLine($"A {Value} of {Suit}");
        }

        public int CompareTo(Card other)
        {
            return new CardComparerByValue().Compare( this, other );
        }
    }
}
