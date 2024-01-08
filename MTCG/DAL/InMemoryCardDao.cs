using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL {
    internal class InMemoryCardDao: ICardDao {

        private readonly List<Card> _cards = new();

        public bool CreateCard(Card card) {
            bool inserted = false;

            if (GetCardById(card.Id) == null) {
                _cards.Add(card);
                inserted = true;
            }
            return inserted;
        }

        public Card? GetCardById(string id) {
            return _cards.SingleOrDefault(u => u.Id == id);
        }

        public List<Card> GetCardsByPId(string pId) {
            return _cards.FindAll(u => u.pId == pId);
        }

        public List<Card> GetAllCardsByUId(string uid) {
            return _cards.FindAll(u => u.uId == uid);
        }
    }
}
