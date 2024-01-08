using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models {
    internal class Card {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Damage { get; }
        public string uId { get; set; } = "";
        public string pId { get; set; } = "";

        public CardSpecification CardSpecification = new();

        public Card(string id, string name, double damage) { 
            Id = id;
            Name = name;
            Damage = damage;
            CardSpecification.Fill(name);
        }
    }
}
