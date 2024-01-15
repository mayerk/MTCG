using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models {
    internal class Package {

        public string Id;
        public string PId;
        public Card[] Cards = new Card[5];
        public string tmpCId = "";

        public Package(Card[] cards) {
            Id = Guid.NewGuid().ToString();
            PId = Guid.NewGuid().ToString();
            Cards = cards;
        }

        public Package(string id, string pid, string cid) {
            Id = id;
            PId = pid;
            tmpCId = cid;
        }
    }
}
