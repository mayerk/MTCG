using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.BLL.Exceptions;
using MTCG.DAL;
using MTCG.Models;

namespace MTCG.BLL.Managers
{
    public class DeckManager : IDeckManager
    {
        private readonly IDeckDao _deckDao;

        public DeckManager(IDeckDao deckDao)
        {
            _deckDao = deckDao;
        }

        public List<Deck> GetDeckByUId(string uid)
        {
            return _deckDao.GetDeckByUId(uid);
        }

        public void ConfigureDeck(List<Card> cards)
        {
            _deckDao.InsertDeck(cards);
        }

        public Deck? GetDeckByCId(string cid)
        {
            return _deckDao.GetDeckByCId(cid);
        }

        public void DeleteDeckByUId(string uid) {
            if(_deckDao.DeleteDeckByUId(uid) == false) {
                throw new DeckNotFoundException();
            }
        }
    }
}
