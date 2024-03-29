﻿using MTCG.BLL.Exceptions;
using MTCG.BLL.Managers;
using MTCG.Enums;
using MTCG.HttpServer.Response;
using MTCG.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.API.Routing
{
    public class ProcessBattleCommand: AuthenticatedRouteCommand {

        private readonly IUserManager _userManager;
        private readonly ICardManager _cardManager;
        private readonly IDeckManager _deckManager;
        private readonly IGameManager _gameManager;

        private GameResult? _gameResult;

        private static readonly ConcurrentQueue<User> lobby = new();
        private static readonly ConcurrentDictionary<string, GameResult> _results = new();

        private static readonly object _lockObject = new();

        public ProcessBattleCommand(IUserManager userManager, ICardManager cardManager, IDeckManager deckManager, IGameManager gameManager, User identity): base(identity) {
            _userManager = userManager;
            _cardManager = cardManager;
            _deckManager = deckManager;
            _gameManager = gameManager;
            FillDeck(identity);
            lobby.Enqueue(identity);
        }

        public override HttpResponse Execute() {
            while(true) {
                lock(_lockObject) {
                    try {
                        if(lobby.Count >= 2) {
                            lobby.TryDequeue(out var player1);
                            lobby.TryDequeue(out var player2);
                            if(player1 != null && player2 != null) {
                                _gameResult = _gameManager.Start(player1, player2);
                                _results.TryAdd((player1.Id == Identity.Id) ? player2.Id : player1.Id, _gameResult);

                                HttpResponse response = new HttpResponse(StatusCode.Ok, _gameResult.Log);
                                return response;
                            }
                        }
                    } catch(Exception) {

                    }
                }
                if(_results.TryGetValue(Identity.Id, out var result)) {
                    _results.TryRemove(Identity.Id, out result);
                    if(result?.Looser != null && result?.Winner != null) {
                        List<Deck> decks = _deckManager.GetDeckByUId(result.Looser.Id);
                        List<Card> cards = new List<Card>();
                        foreach(var deck in decks) {
                            Card? card = _cardManager.GetCardById(deck.CId);
                            if(card == null) {
                                throw new CardNotFoundException();
                            }
                            cards.Add(card);
                        }
                        _deckManager.DeleteDeckByUId(result.Looser.Id);
                        foreach(var card in cards) {
                            card.UId = result.Winner.Id;
                            _cardManager.UpdateCardUId(card);
                        }

                        result.Winner.Elo += 3;
                        result.Winner.Wins += 1;
                        result.Looser.Elo = (result.Looser.Elo - 5 <= 0) ? 0 : result.Looser.Elo - 5;
                        result.Looser.Losses += 1;

                        _userManager.UpdateUser(result.Winner);
                        _userManager.UpdateUser(result.Looser);
                    }
                    HttpResponse response = new HttpResponse(StatusCode.Ok, result?.Log);
                    return response;
                }
                Thread.Sleep(100);
            }
        }

        private void FillDeck(User user) {
            List<Deck> decks = _deckManager.GetDeckByUId(user.Id);
            List<Card> cards = new List<Card>();
            foreach (Deck deck in decks) {
                var card = _cardManager.GetCardById(deck.CId);
                if (card == null) {
                    throw new CardNotFoundException();
                }
                user.Deck.Add(card);
            }
        }
    }
}
