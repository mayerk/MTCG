using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;

namespace MTCG.BLL.Managers
{
    internal interface IDeckManager
    {
        List<Deck> GetDeckByUId(string uid);
        void ConfigureDeck(List<Card> cards);

        Deck GetDeckByCId(string cid);
    }
}
