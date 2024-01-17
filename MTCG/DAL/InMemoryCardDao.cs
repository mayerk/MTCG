using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;

namespace MTCG.DAL {
    internal class InMemoryCardDao: ICardDao {

        private readonly List<Card> _cards = new();

        public bool InsertCard(Card card) {
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

        public List<Card> GetAllCardsByUId(string uid) {
            return _cards.FindAll(u => u.UId == uid);
        }

        public bool UpdateCardUId(Card card) {
            throw new NotImplementedException();
        }
    }
}
