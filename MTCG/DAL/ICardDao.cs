using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;

namespace MTCG.DAL {
    public interface ICardDao {

        bool InsertCard(Card card);
        Card? GetCardById(string id);
        List<Card> GetAllCardsByUId(string uid);
        bool UpdateCardUId(Card card);
        bool DeleteCardById(string id);
    }
}
