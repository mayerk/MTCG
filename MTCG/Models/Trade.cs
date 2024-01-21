using Newtonsoft.Json;

namespace MTCG.Models {
    public class Trade {
        public string Id { get; set; }
        public string CId { get; set; }
        public string CardType { get; set; }
        public int MinimumDamage { get; set; }

        [JsonConstructor]
        public Trade(string id, string cardToTrade, string type, int minimumDamage) {
            Id = id;
            CId = cardToTrade;
            CardType = type;
            MinimumDamage = minimumDamage;
        }

        public Trade(string cardToTrade, string type, int minimumDamage) {
            Id = Guid.NewGuid().ToString();
            CId = cardToTrade;
            CardType = type;
            MinimumDamage = minimumDamage;
        }
    }
}
