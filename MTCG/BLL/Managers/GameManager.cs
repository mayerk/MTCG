using MTCG.BLL.Exceptions;
using MTCG.Enums;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL.Managers {
    public class GameManager: IGameManager {

        public GameManager() {}

        public GameResult Start(User player1, User player2) {
            int rounds = 0;
            string log = "";
            List<Card> defeatedCards = new List<Card>();
            Random random = new Random();

            while (player1.Deck.Any() && player2.Deck.Any() && ++rounds <= 100) {
                Card card1 = player1.Deck[random.Next(player1.Deck.Count)];
                Card card2 = player2.Deck[random.Next(player2.Deck.Count)];
                double damageCard1 = card1.Damage;
                double damageCard2 = card2.Damage;

                log += player1.Username + ": " + card1.Name + "(" + damageCard1 + ") vs " + player2.Username + ": " + card2.Name + "(" + damageCard2 + ") => ";

                if (card1.CardSpecification.CardType == CardType.MONSTER && card2.CardSpecification.CardType == CardType.MONSTER) {
                    ConvertSpecials(card1, card2);
                    if (card1.Damage < card2.Damage) {
                        player1.Deck.Remove(card1);
                        player2.Deck.Add(card1);
                        log += card2.Name + " wins\n";
                    } else if (card2.Damage < card1.Damage) {
                        player2.Deck.Remove(card2);
                        player1.Deck.Add(card2);
                        log += card1.Name + " wins\n";
                    } else {
                        log += "Draw (no action)\n";
                    }
                } else {
                    CalculateEffectiveness(card1, card2);
                    ConvertSpecials(card1, card2);
                    if (card1.Damage < card2.Damage) {
                        player1.Deck.Remove(card1);
                        player2.Deck.Add(card1);
                        log += card2.Name + " wins\n";
                    } else if(card2.Damage < card1.Damage) {
                        player2.Deck.Remove(card2);
                        player1.Deck.Add(card2);
                        log += card1.Name + " wins\n";
                    } else {
                        log += "Draw (no action)\n";
                    }
                }
                card1.Damage = damageCard1;
                card2.Damage = damageCard2;
            }
            User? winner = (player1.Deck.Any() && player2.Deck.Any()) ? null : (player1.Deck.Any()) ? player1 : player2;
            return new(winner, log);
        }

        private void CalculateEffectiveness(Card card1, Card card2) {
            var effectiveness = GetEffectiveness(card1, card2);
            card1.Damage = (effectiveness == Effectiveness.EFFECTIVE) ? card1.Damage * 2 : (effectiveness == Effectiveness.NOTEFFECTIVE) ? card1.Damage / 2 : card1.Damage;
            effectiveness = GetEffectiveness(card2, card1);
            card2.Damage = (effectiveness == Effectiveness.EFFECTIVE) ? card2.Damage * 2 : (effectiveness == Effectiveness.NOTEFFECTIVE) ? card2.Damage / 2 : card2.Damage;
        }

        private Effectiveness GetEffectiveness(Card attacker, Card defender) {
            return attacker.CardSpecification.ElementType switch {
                ElementType.WATER when defender.CardSpecification.ElementType == ElementType.FIRE => Effectiveness.EFFECTIVE,
                ElementType.FIRE when defender.CardSpecification.ElementType == ElementType.NORMAL => Effectiveness.EFFECTIVE,
                ElementType.NORMAL when defender.CardSpecification.ElementType == ElementType.WATER => Effectiveness.EFFECTIVE,

                ElementType.WATER when defender.CardSpecification.ElementType == ElementType.NORMAL => Effectiveness.NOTEFFECTIVE,
                ElementType.FIRE when defender.CardSpecification.ElementType == ElementType.WATER => Effectiveness.NOTEFFECTIVE,
                ElementType.NORMAL when defender.CardSpecification.ElementType == ElementType.FIRE => Effectiveness.NOTEFFECTIVE,

                ElementType.WATER when defender.CardSpecification.ElementType == ElementType.WATER => Effectiveness.NORMAL,
                ElementType.FIRE when defender.CardSpecification.ElementType == ElementType.FIRE => Effectiveness.NORMAL,
                ElementType.NORMAL when defender.CardSpecification.ElementType == ElementType.NORMAL => Effectiveness.NORMAL,

                _ => throw new NotImplementedException()
            };
        }

        private void ConvertSpecials(Card attacker, Card defender) {
            switch(attacker.CardSpecification.MonsterType) {
                case MonsterType.GOBLIN when defender.CardSpecification.MonsterType == MonsterType.DRAGON: attacker.Damage = 0; break;
                case MonsterType.WIZZARD when defender.CardSpecification.MonsterType == MonsterType.ORK: defender.Damage = 0; break;
                case MonsterType.KNIGHT when defender.CardSpecification.CardType == CardType.SPELL && defender.CardSpecification.ElementType == ElementType.WATER: attacker.Damage = 0; break;
                case MonsterType.KRAKEN when defender.CardSpecification.CardType == CardType.SPELL: defender.Damage = 0; break;
                case MonsterType.FIREELVE when defender.CardSpecification.MonsterType == MonsterType.DRAGON: defender.Damage = 0; break;
            }

            switch (defender.CardSpecification.MonsterType) {
                case MonsterType.GOBLIN when attacker.CardSpecification.MonsterType == MonsterType.DRAGON: defender.Damage = 0; break;
                case MonsterType.WIZZARD when attacker.CardSpecification.MonsterType == MonsterType.ORK: attacker.Damage = 0; break;
                case MonsterType.KNIGHT when attacker.CardSpecification.CardType == CardType.SPELL && attacker.CardSpecification.ElementType == ElementType.WATER: defender.Damage = 0; break;
                case MonsterType.KRAKEN when attacker.CardSpecification.CardType == CardType.SPELL: attacker.Damage = 0; break;
                case MonsterType.FIREELVE when attacker.CardSpecification.MonsterType == MonsterType.DRAGON: attacker.Damage = 0; break;
            }
        }
    }
}
