using MTCG.DAL;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL {
    internal class CardManager : ICardManager {

        private readonly ICardDao _cardDao;

        public CardManager(ICardDao cardDao) {
            _cardDao = cardDao;
        }

        public void CreateCard(Card card) {
            if(_cardDao.CreateCard(card) == false) {
                throw new DuplicateCardException();
            }
        }

        public void AquirePackage(User user, string pId) {
            if(user.Coins < 5) {
                throw new NotEnoughCoinsException();
            }
            List<Card> cards = _cardDao.GetCardsByPId(pId);
            user.Coins -= 5;
            foreach(Card card in cards) {
                card.uId = user.Id;
                card.pId = "";
            }
        }

        public List<Card> GetAllCards(string uid) {
            return _cardDao.GetAll(uid);
        }
    }
}
