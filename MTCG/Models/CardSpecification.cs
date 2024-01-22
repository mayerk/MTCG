using MTCG.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models {
    public class CardSpecification {

        public CardType CardType;

        public ElementType ElementType;

        public MonsterType MonsterType = MonsterType.NONE;

        public void Fill(string Identity) {
            int spellIndex = Identity.IndexOf("Spell", StringComparison.Ordinal);
            if(spellIndex == -1) {
                CardType = CardType.MONSTER;
            } else {
                CardType = CardType.SPELL;
            }
            
            if(Identity.Contains("Fire")) {
                ElementType = ElementType.FIRE;
            } else if(Identity.Contains("Water")) {
                ElementType = ElementType.WATER;
            } else {
                ElementType = ElementType.NORMAL;
            }

            if(CardType == CardType.MONSTER) {
                MonsterType = GetMonsterType(Identity);
            }
        }

        private MonsterType GetMonsterType(string cardStr) {
            MonsterType type;
            string str = GetMonsterTypeAsString(cardStr);
            if(str == "") {
                throw new Exception();
            }
            switch(str) {
                case "Goblin": type = MonsterType.GOBLIN; break;
                case "Dragon": type = MonsterType.DRAGON; ElementType = ElementType.FIRE; break;
                case "Wizzard": type = MonsterType.WIZZARD; break;
                case "Ork": type = MonsterType.ORK; break;
                case "Knight": type = MonsterType.KNIGHT; break;
                case "Kraken": type = MonsterType.KRAKEN; ElementType = ElementType.WATER; break;
                case "FireElve": type = MonsterType.FIREELVE; ElementType = ElementType.FIRE; break;
                case "Troll": type = MonsterType.TROLL; break;
                default: type = MonsterType.NONE; break;
            }
            return type;
        }

        private string GetMonsterTypeAsString(string cardStr) {
            int index = Array.FindLastIndex<char>(cardStr.ToCharArray(), Char.IsUpper);
            if(index == -1) {
                return "";
            }
            return cardStr.Substring(index);
        }
    }
}
