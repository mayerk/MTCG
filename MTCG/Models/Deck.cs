using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models {
    internal class Deck {
        public string Id { get; set; }
        public string UId { get; set; }
        public string CId { get; set; }

        public Deck(string id, string uId, string cId) {
            Id = id;
            UId = uId;
            CId = cId;
        }
    }
}
