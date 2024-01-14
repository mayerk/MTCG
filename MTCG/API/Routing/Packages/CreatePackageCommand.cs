using MTCG.BLL;
using MTCG.HttpServer.Response;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.API.Routing.Packages
{
    internal class CreatePackageCommand : AuthenticatedRouteCommand
    {
        private readonly IPackageManager _packageManager;
        private readonly ICardManager _cardManager;

        private readonly Card[] _cards = new Card[5];

        public CreatePackageCommand(IPackageManager packageManager, ICardManager cardManager, Card[] cards, User identity) : base(identity)
        {
            _packageManager = packageManager;
            _cardManager = cardManager;
            _cards = cards;
        }
        public override HttpResponse Execute()
        {
            HttpResponse response;

            if (Identity.Token != "admin-mtcgToken")
            {
                response = new HttpResponse(StatusCode.Forbidden);
                return response;
            }
            bool inserted = true;

            foreach (Card card in _cards)
            {
                try
                {
                    _cardManager.CreateCard(card);
                }
                catch (DuplicateCardException e)
                {
                    inserted = false;
                }
            }
            if (inserted)
            {
                _packageManager.CreatePackage(_cards);
                response = new HttpResponse(StatusCode.Created);
            }
            else
            {
                response = new HttpResponse(StatusCode.Conflict);
            }

            return response;
        }
    }
}
