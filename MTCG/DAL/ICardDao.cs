using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL {
    internal interface ICardDao {

        bool CreateCard(Card card);
        Card? GetCardById(string id);
        List<Card> GetCardsByPId(string pId);
        List<Card> GetAll(string uid);
    }
}
