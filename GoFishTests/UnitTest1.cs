namespace GoFishTests
{
    using GoFish;
    using System.Windows.Markup;

    [TestClass]
    public class GoFishTests
    {
        
        [TestMethod]
        public void ConstructorTest()
        {
            GameController controller = new GameController(5);
            var player1 = controller.Players[0];
            var player5 = controller.Players[4];

            Assert.AreEqual("You", player1.Name.ToString());
            Assert.AreEqual(5, player5.Hand.Count);
            Assert.AreEqual(27, controller.Stock.Count());
        }
        
        [TestMethod]
        public void PlayerFindCardTest()
        {
            Deck deck = new Deck();
            deck.Remove(deck[0]);
            Player player = new Player(0, deck);
            player.FindABook();

            Deck deck2 = new Deck();
            deck2.Clear();
            Player player2 = new Player(1, deck2);
            player2.FindABook();

            Assert.AreEqual (3, player.Hand.Count);
            Assert.AreEqual (Value.Two, player.Hand[2].Value);
            Assert.AreEqual (12, player.Score);
            Assert.AreEqual (0, player2.Hand.Count);
            Assert.AreEqual (0, player2.Score);
        }
        
        [TestMethod]
        public void PlayersTurnLogicTest()
        {
            GameController controller = new GameController(2);
            controller.Stock.Clear();

            for (int i = 0; i < 3; i++)
            {
                Card card = new Card((Suit)i, Value.Jack);
                controller.Stock.Add(card);
            }
            Deck deck1 = new Deck();
            deck1.Clear();
            for(int i = 0; i < 3; i++)
            {
                Card card = new Card((Suit)i, Value.Ace);
                deck1.Add(card);
            }
            controller.Players[0] = new Player(0, deck1);
            Deck deck2 = new Deck();
            deck2.Clear();
            deck2.Add(new Card(Suit.Diamonds, Value.Ace));
            deck2.Add(new Card(Suit.Diamonds, Value.Jack));
            controller.Players[1] = new Player(1, deck2);

            controller.PlayersTurnLogic(controller.Players[1], 0);

            Assert.IsFalse(controller.GameOver);
            Assert.AreEqual(1, controller.Players[0].Score);
            Assert.AreEqual(controller.Players[0].Hand[0].Value, Value.Jack);
            Assert.AreEqual(controller.Players[0].Hand.Count, 1);
        }
        
        [TestMethod]
        public void PickACardFromAStockTest()
        {
            GameController controller = new GameController(2);
            controller.Stock.Clear();

            Card card = new Card((Suit)0, Value.Jack);
            controller.Stock.Add(card);

            Deck deck1 = new Deck();
            deck1.Clear();
            controller.Players[0] = new Player(0, deck1);
            controller.PickACardFromAStock(controller.Players[0]);

            Assert.AreEqual(controller.Players[0].Hand.First().Value, Value.Jack);
            Assert.AreEqual(controller.Stock.Count, 0);
            Assert.AreEqual(controller.Players[0].Hand.Count, 1);
        }
        [TestMethod]
        public void AITurnLogicTest()
        {
            GameController controller = new GameController(2);
            controller.Stock.Clear();            
            controller.Stock.Add(new Card(Suit.Spades, Value.King));
            controller.Stock.Add(new Card(Suit.Hearts, Value.King));
            controller.Stock.Add(new Card(Suit.Clubs, Value.King));
            controller.Stock.Add(new Card(Suit.Diamonds, Value.Ace));


            Deck deck1 = new Deck();
            deck1.Clear();
            for (int i = 0; i < 3; i++)
            {
                Card card = new Card((Suit)i, Value.Ace);
                deck1.Add(card);
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 2; j < 13; j++)
                {
                    deck1.Add(new Card((Suit)i, (Value)j));
                }
            }
            controller.Players[0] = new Player(0, deck1);

            Deck deck2 = new Deck();
            deck2.Clear();
            deck2.Add(new Card(Suit.Diamonds, Value.King));
            controller.Players[1] = new Player(1, deck2);



            controller.PlayersTurnLogic(controller.Players[1], 0);

            Assert.IsTrue(controller.GameOver);
            Assert.AreEqual(controller.Players[0].Score, 11);
            Assert.AreEqual(controller.Players[1].Score, 2);
        }

    }
}