using MTCG.BLL.Managers;
using MTCG.DAL;
using MTCG.Models;

namespace MTCG_Tests {
    public class CardTests {

        private static readonly string connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=mtcg";

        private ICardDao _cardDao = new DatabaseCardDao(connectionString);
        private ICardManager _cardManager;

        private readonly string _cardId = Guid.NewGuid().ToString();

        [SetUp]
        public void Setup() {
            _cardManager = new CardManager(_cardDao);
        }

        [Test][Order(0)]
        public void Test_InsertCard() {
            Card card = new(_cardId, "WaterGoblin", 10.0);
            _cardManager.CreateCard(card);
            Card? tmp = _cardManager.GetCardById(card.Id);
            if(tmp == null) {
                Assert.Fail("Could not retreive Card with Id " + _cardId);
                return;
            }
            Assert.That(tmp.Name, Is.EqualTo(card.Name));
        }

        [Test][Order(1)]
        public void Test_UpdateCardUId() {
            Card? card = _cardManager.GetCardById(_cardId);
            if(card == null) {
                Assert.Fail("Could not retreive Card with Id " + _cardId);
                return;
            }
            string UId = Guid.NewGuid().ToString();
            card.UId = UId;
            _cardManager.UpdateCardUId(card);
            card = _cardManager.GetCardById(_cardId);
            Assert.That(card.UId, Is.EqualTo(UId));
        }

        [Test][Order(2)]
        public void Test_GetAllUsersCards() {
            Card? card = _cardManager.GetCardById(_cardId);
            if (card == null) {
                Assert.Fail("Could not retreive Card with Id " + _cardId);
                return;
            }
            List<Card> cards = _cardManager.GetAllUsersCards(card.UId);
            Assert.That(cards.Count, Is.EqualTo(1));
        }

        [Test][Order(3)]
        public void Test_DeleteCard() {
            bool deleted = _cardManager.DeleteCardById(_cardId);
            Assert.IsTrue(deleted);
        }
    }
}
