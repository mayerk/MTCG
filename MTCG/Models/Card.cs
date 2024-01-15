using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MTCG.Models {
    internal class Card {
        public string Id { get; set; }
        public string Name { get; set; } = "";
        public double Damage { get; } = 0;
        [JsonIgnore]
        public string UId { get; set; } = "";
        [JsonIgnore]
        public CardSpecification CardSpecification = new();

        [JsonConstructor]
        public Card(string id, string name, double damage) { 
            Id = id;
            Name = name;
            Damage = damage;
            CardSpecification.Fill(name);
        }

        public Card(string id, string name, double damage, string uid) {
            Id = id;
            Name = name;
            Damage = damage;
            UId = uid;
            CardSpecification.Fill(name);
        }

        public Card(string id) {
            Id = id;
        }
    }
}
