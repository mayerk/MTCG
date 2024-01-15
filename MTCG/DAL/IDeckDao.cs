using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL {
    internal interface IDeckDao {
        List<Deck> GetDeckByUId(string uid);
        bool InsertDeck(List<Card> cards);
    }
}
