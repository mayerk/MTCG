using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models {
    public class Coupon {
        public string Id { get; set; }
        public int Coins { get; set; }

        public Coupon(string id, int coins) {
            Id = id;
            Coins = coins;
        }
    }
}
