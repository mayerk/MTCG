using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MTCG.Models {
    internal class Card {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Damage { get; }
        [JsonIgnore]
        public string UId { get; set; } = "";
        [JsonIgnore]
        public string PId { get; set; } = "";
        [JsonIgnore]
        public CardSpecification CardSpecification = new();

        public Card(string id, string name, double damage) { 
            Id = id;
            Name = name;
            Damage = damage;
            CardSpecification.Fill(name);
        }
    }
}
