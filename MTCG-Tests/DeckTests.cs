using MTCG.BLL.Managers;
using MTCG.DAL;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Tests {
    public class DeckTests {

        private static readonly string connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=mtcg";

        private IDeckDao _deckDao = new DatabaseDeckDao(connectionString);
        private IDeckManager _deckManager;

        private readonly string _uId = Guid.NewGuid().ToString();

        [SetUp]
        public void Setup() {
            _deckManager = new DeckManager(_deckDao);
        }

        [Test][Order(0)]
        public void Test_InsertDeck() {
            List<Card> cards = new List<Card>();
            cards.Add(new("845f0dc7-37d0-426e-994e-43fc3ac83c08", "WaterGoblin", 10.0, _uId));
            cards.Add(new("99f8f8dc-e25e-4a95-aa2c-782823f36e2a", "FireSpell", 15.0, _uId));
            cards.Add(new("e85e3976-7c86-4d06-9a80-641c2019a79f", "Dragon", 22.0, _uId));
            cards.Add(new("171f6076-4eb5-4a7d-b3f2-2d650cc3d237", "RegularSpell", 11.0, _uId));

            _deckManager.ConfigureDeck(cards);
            Deck? deck = _deckManager.GetDeckByCId("845f0dc7-37d0-426e-994e-43fc3ac83c08");
            if(deck == null) {
                Assert.Fail("Could not retreive deck with CardId '845f0dc7-37d0-426e-994e-43fc3ac83c08'.");
                return;
            }
            Assert.That(deck.CId, Is.EqualTo("845f0dc7-37d0-426e-994e-43fc3ac83c08"));
        }

        [Test][Order(1)]
        public void Test_GetDeckByUId() {
            List<Deck> deck = _deckManager.GetDeckByUId(_uId);
            if(!deck.Any()) {
                Assert.Fail("No deck configured for UId '" + _uId + "'.");
            } 
            Assert.That(deck.Count, Is.EqualTo(4));
        }

        [Test]
        [Order(2)]
        public void Test_GetDeckByCId() {
            Deck? deck = _deckManager.GetDeckByCId("845f0dc7-37d0-426e-994e-43fc3ac83c08");
            if(deck == null) {
                Assert.Fail("Could not retreive deck with CId '845f0dc7-37d0-426e-994e-43fc3ac83c08'.");
            }
            Assert.That(deck.CId, Is.EqualTo("845f0dc7-37d0-426e-994e-43fc3ac83c08"));
        }
    }
}
