using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GoFish
{
    public class Player
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="i">Players name (type PlayersNameEnum)</param>
        /// <param name="deck">Initial N cards to start playing </param>
        public Player(int i, Deck deck) 
        {
            name = (PlayersNameEnum)i;
            hand = deck;
        }

        private PlayersNameEnum name; 
        public PlayersNameEnum Name {
            get { return name; }
        }
        /// <summary>
        /// Players deck
        /// </summary>
        private Deck hand;
        public Deck Hand { get { return hand; } }

        private int score = 0;
        public int Score { get { return score; } }

        public override string ToString()
        {
            return Name.ToString(); 
        }
        /// <summary>
        /// Method to find a "books" in player "hand" and remove it
        /// </summary>
        public void FindABook()
        {
            var groupedHand =
                from card in hand
                group card by card.Value into valueGroup
                select valueGroup.Count() == 4 ? valueGroup : null ;
            foreach (var element in groupedHand)
            {
                    if (element != null)
                    {
                        score++;                        
                        for (int i = 0; i < 4; i++)
                        {
                            hand.RemoveCard(element.Key, (Suit)i);
                        }                        
                    }
            }           
        }
        /// <summary>
        /// Methom to check request "Do you have any value?" from another players. Finding any cards with requested value and remove it from "hand"
        /// </summary>
        /// <param name="value">Card value to search in players "hand"</param>
        /// <returns>a list of cards, that will be given to request author</returns>
        public List<Card> CheckRequest(Value value)
        {
            List<Card> result = new List<Card>();
            foreach (Card card in hand)
                if (card.Value == value) result.Add(card);
            foreach (Card card in result)
                hand.RemoveCard(card.Value, card.Suit);
            return result;
        }
        /// <summary>
        /// add a card to the "hand" and after that check "hand" for any completed books
        /// </summary>
        /// <param name="card">card, that will be added to players hand</param>
        public void TakeCard(Card card)
        {
            hand.Add(card);
            FindABook();
        }

    }
}
