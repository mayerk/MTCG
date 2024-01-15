using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL {
    internal interface ICardManager {
        void CreateCard(Card card);
        void AquirePackage(User user, Package package);
        List<Card> GetAllUsersCards(string uid);
        Card? GetCardById(string id);
        List<Card> GetAllCardsByPId(string pid);

        void FillCardsInPackage(Package package);
    }
}
