using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL {
    internal interface IDeckManager {
        List<Deck> GetDeckByUId(string uid);
        void ConfigureDeck(List<Card> cards);
    }
}
