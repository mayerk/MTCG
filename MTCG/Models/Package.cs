using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models {
    internal class Package {

        public string Id;
        public Card[] cards = new Card[5];

        public Package(Card[] cards) {
            Id = Guid.NewGuid().ToString();
            this.cards = cards;
            foreach (Card card in cards) {
                card.PId = Id;
            }
        }
    }
}
