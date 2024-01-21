using MTCG.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models {
    public class CardSpecification {

        public CardType cardType;

        public ElementType elementType;

        public void Fill(string Identity) {
            int spellIndex = Identity.IndexOf("Spell", StringComparison.Ordinal);
            if(spellIndex == -1) {
                cardType = CardType.MONSTER;
            } else {
                cardType = CardType.SPELL;
            }
            
            if(Identity.Contains("Fire")) {
                elementType = ElementType.FIRE;
            } else if(Identity.Contains("Water")) {
                elementType = ElementType.WATER;
            } else {
                elementType = ElementType.NORMAL;
            }
        }
    }
}
