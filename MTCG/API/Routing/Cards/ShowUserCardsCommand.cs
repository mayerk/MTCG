﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.BLL.Managers;
using MTCG.HttpServer.Response;
using MTCG.Models;
using Newtonsoft.Json;

namespace MTCG.API.Routing.Cards
{
    public class ShowUserCardsCommand: AuthenticatedRouteCommand {

        private readonly ICardManager _cardManager;
        public ShowUserCardsCommand(ICardManager cardManager, User identity): base(identity) {
            _cardManager = cardManager;
        }

        public override HttpResponse Execute() {
            HttpResponse response;
            List<Card> cards = _cardManager.GetAllUsersCards(Identity.Id);
            if(!cards.Any()) {
                response = new HttpResponse(StatusCode.NoContent);
                return response;
            }
            response = new HttpResponse(StatusCode.Ok, JsonConvert.SerializeObject(cards));
            return response;
        }
    }
}
