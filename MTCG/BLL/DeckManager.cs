using MTCG.DAL;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL {
    internal class DeckManager: IDeckManager {
        private readonly IDeckDao _deckDao;

        public DeckManager(IDeckDao deckDao) {
            _deckDao = deckDao;
        }

        public List<Deck> GetDeckByUId(string uid) {
            return _deckDao.GetDeckByUId(uid);
        }

        public void ConfigureDeck(List<Card> cards) {
            _deckDao.InsertDeck(cards);
        }

        public Deck GetDeckByCId(string cid) {
            return _deckDao.GetDeckByCId(cid);
        }
    }
}
