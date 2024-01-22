using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;

namespace MTCG.DAL {
    public interface IDeckDao {
        List<Deck> GetDeckByUId(string uid);
        bool InsertDeck(List<Card> cards);
        Deck? GetDeckByCId(string cid);
        bool DeleteDeckByUId(string uid);
    }
}
