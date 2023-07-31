using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish
{
    public class Deck : ObservableCollection<Card>
    {
        public Deck()
        {
            Reset();
        }
        private void Reset()
        {
            Clear();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 2; j < 15; j++)
                {
                    Add(new Card((Suit)i, (Value)j));
                }
            }
        }

        public Deck ShuffleCard()
        {
            List<Card> deck1 = new List<Card>(this);
            Clear();
            Random random = new Random();

            while (deck1.Count > 0)
            {
                int cardNumber = random.Next(deck1.Count);
                Add(deck1[cardNumber]);
                deck1.RemoveAt(cardNumber);
            }
            return this;
        }

        public void RemoveCard(Card cardThatWillBeRemoved)
        {

            if (this.Contains(cardThatWillBeRemoved))
                Remove(cardThatWillBeRemoved);
        }
        public void RemoveCard(Value value, Suit suit)
        {
            Card test = new Card(suit, value);
            bool findIt = false;
            foreach (Card card in this)
            {
                if ((card.Value == value) && (card.Suit == suit))
                {
                    test = card;
                    findIt = true;
                    break;
                }
            }
            if (findIt)
                Remove(test);
            else
                throw new Exception();
        }
        public bool ContainCard(Value value, Suit suit)
        {
            foreach (Card card in this)
            {
                if ((card.Value == value) && (card.Suit == suit))
                {
                    return true;
                }
            }
            return false;
        }
        public bool ContainValue(Value value)
        {
            foreach (Card card in this)
            {
                if (card.Value == value)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
