using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace GoFish
{
    public class GameController
    {
        /// <summary>
        /// Constructor, that will make a game with 2-5 players
        /// </summary>
        /// <param name="NumberOfPlayers">amount of players</param>
        public GameController(int NumberOfPlayers)
        {
            if ((NumberOfPlayers > 5) && (NumberOfPlayers < 2))
                throw new Exception();
            Deck stock = new Deck();
            stock.ShuffleCard();

            List<Player> players = new List<Player>();

            for (int i = 0; i < NumberOfPlayers; i++)
            {
                Deck deck = new Deck();
                deck.Clear();
                for(int j = 0; j < INITIALHANDSIZE; j++)
                {
                    Card card = stock.First();
                    deck.Add(card);
                    stock.RemoveCard(card);
                }    
                players.Add(new Player(i, deck));

                GameOver = false;
                Stock = stock;
                Players = players;  
            }
        }
        /// <summary>
        /// starting "hand" size of a players
        /// </summary>
        const int INITIALHANDSIZE = 5;
        /// <summary>
        /// game status, if true - game is end
        /// </summary>
        public bool GameOver;
        /// <summary>
        /// list of players
        /// </summary>
        public List<Player> Players;
        /// <summary>
        /// Stock of a cards
        /// </summary>
        public Deck Stock;
        /// <summary>
        /// Random class using to AI Player logic
        /// </summary>
        public static Random random = new Random();
        /// <summary>
        /// list of players scores, using to calculate total amount of "books" and check gameover status
        /// </summary>
        public List<int> Scores = new List<int>();
        /// <summary>
        /// list of players scores, using to show in the screen
        /// </summary>
        public List<string> ScoresString = new List<string>();
        /// <summary>
        /// log of the game, using to show it in screen
        /// </summary>
        private string gameLog = "";
        public string GameLog { get { return gameLog; } }
        /// <summary>
        /// AI turn's logic.
        /// </summary>
        /// <param name="quest"> Player, that asked for any cards</param>
        /// <param name="answer">Player, that will respond</param>
        /// <param name="currentPlayerNumber">"quest" number in "Players" list</param>
        public void AITurnLogic(Player quest, Player answer, int currentPlayerNumber)
        {
            Card card;
            List<Card> swap = new List<Card>();

            int handSize = quest.Hand.Count;
            
            if ((handSize == 0) && (Stock.Count != 0))  // take a card from a stock, if your hand is empty
            {
                PickACardFromAStock(quest);
                handSize = quest.Hand.Count;
            }

            int seed = random.Next(0, handSize);
            if (handSize != 0)
                gameLog += $"{quest} ask {answer} if he has a {quest.Hand[seed].Value} \n";


            if (handSize != 0)
            {
                card = quest.Hand[seed];
                swap = answer.CheckRequest(card.Value);
                gameLog += $"{answer} has {swap.Count} {quest.Hand[seed].Value}\n";
            }
            if ((swap.Count == 0) && (Stock.Count != 0)) //Go fish
            {
                gameLog += $"{quest} must draw  from the stock\n";
                if (Stock.First().Value == quest.Hand[seed].Value)
                {
                    PickACardFromAStock(quest);
                    RefreshScores();
                    GameOverCheck();
                    NextTurn(currentPlayerNumber - 1); // Current AI player turn continues
                }
                else
                {
                    PickACardFromAStock(quest);
                    RefreshScores();
                    GameOverCheck();
                    NextTurn(currentPlayerNumber);
                }
            }
            else if (swap.Count > 0)
            {
                foreach (Card s in swap)
                    quest.TakeCard(s);
                
                RefreshScores();
                GameOverCheck();
                NextTurn(currentPlayerNumber-1); // Current AI player turn continues
            }
            else   // If stock is empty and current player don't have any cards in his hand
                NextTurn(currentPlayerNumber);
        }
        /// <summary>
        /// Method, that start turn to the next player
        /// </summary>
        /// <param name="currentPlayerNumber">number of a player in "Players" list who moved previous</param>
        public void NextTurn(int currentPlayerNumber)
        {
            if (GameOver) { return; }
            currentPlayerNumber++;
            if (currentPlayerNumber != Players.Count)
            {
                gameLog += $"It's {Players[currentPlayerNumber]}'s turn\n";
                AITurnLogic(Players[currentPlayerNumber], ChoseAPlayer(Players[currentPlayerNumber]), currentPlayerNumber);
            }
            else
            {
                gameLog += $"It's yours turn\n";
                PlayersCardsAmountReport();
                PlayersTurnEmptyHandCheck();
            }
        }
        /// <summary>
        /// Method that will show the to the player amount of cards each AI players
        /// </summary>
        private void PlayersCardsAmountReport()
        {
            for (int i = 1; i < Players.Count; i++)
            {
                gameLog += $"{Players[i]} has {Players[i].Hand.Count} cards\n";
            }
        }
        /// <summary>
        /// The players turn logic. Player must chose any AI opponent to ask (answer) and a card in his "hand" (ask)
        /// </summary>
        /// <param name="answer">AI player, that will be asked for any card.value</param>
        /// <param name="ask">number of a card in players "hand" (hand[ask]) to ask</param>
        public void PlayersTurnLogic(Player answer, int ask)
        {
            if (GameOver) { return; }
            gameLog += $"{Players[0]} ask {answer} if he has a {Players[0].Hand[ask].Value} \n";
            Card card;
            List<Card> swap = new List<Card>();

            int handSize = Players[0].Hand.Count;
            
            if (handSize != 0)
            {
                card = Players[0].Hand[ask];
                swap = answer.CheckRequest(card.Value);
                gameLog += $"{answer} has {swap.Count} {Players[0].Hand[ask].Value}\n";
            }
            if ((swap.Count == 0) && (Stock.Count != 0)) //Go fish
            {
                gameLog += $"{Players[0]} must draw  from the stock\n";
                gameLog += $"{Players[0]} pick up {Stock.First()}\n";
                if (Stock.First().Value == Players[0].Hand[ask].Value)
                {
                    PickACardFromAStock(Players[0]);
                    RefreshScores();
                    GameOverCheck();
                    PlayersCardsAmountReport();
                    //Players turn continues with new request
                }
                else
                {
                    PickACardFromAStock(Players[0]);
                    RefreshScores();
                    GameOverCheck();
                    NextTurn(0); // 1st AI playes turn
                } 
            }
            else if (swap.Count > 0) 
            {
                foreach (Card s in swap)
                    Players[0].TakeCard(s);

                RefreshScores();
                GameOverCheck();
                PlayersTurnEmptyHandCheck();
                PlayersCardsAmountReport();
                //Players turn continues with new request
                if ((Players[0].Hand.Count == 0)&&(Stock.Count == 0)) //if you are out of the game
                    NextTurn(0);
            }
            else
                NextTurn(0); // 1st AI player turn
        }
        /// <summary>
        /// Check the players hand, if it's empty - pick up card from a stock
        /// </summary>
        public void PlayersTurnEmptyHandCheck()
        {
            if ((Players[0].Hand.Count == 0) && (Stock.Count != 0)) // take a card from a stock, if your hand is empty            
                PickACardFromAStock(Players[0]);
        }
        /// <summary>
        /// AI logic to chose another player for asking
        /// </summary>
        /// <param name="player">player, that will asked</param>
        /// <returns>player, who will answering</returns>
        private Player ChoseAPlayer(Player player)
        {

            int seed = random.Next(Players.Count);
            while ((Players[seed] == player) || (Players[seed].Hand.Count == 0))
                seed = random.Next(Players.Count);
            return Players[seed];
        }
        /// <summary>
        /// update score lists after each actions
        /// </summary>
        private void RefreshScores()
        {
            Scores.Clear();
            ScoresString.Clear();
            foreach(Player player in Players)
            {
                Scores.Add(player.Score);
                ScoresString.Add($"{player} {player.Score}");
            }
        }
        /// <summary>
        /// GameOver check method
        /// </summary>
        private void GameOverCheck()
        {
            if (Scores.Sum() == 13)
            {
                gameLog += $"All 13 books collected";
                GameOver = true;
            }
        }
        /// <summary>
        /// draw a card from a stock
        /// </summary>
        /// <param name="player">player, that draw a card</param>
        /// <exception cref="Exception">method called when stock is empty</exception>
        public void PickACardFromAStock(Player player)
        {
            if (Stock.Count > 0)
            {
                Card card = Stock.First();
                Stock.Remove(card);
                player.TakeCard(card);
            }
            else
                throw new Exception();
        }
    }
}
