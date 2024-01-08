using MTCG.BLL;
using MTCG.HttpServer.Response;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.API.Routing.Packages
{
    internal class AquirePackageCommand : AuthenticatedRouteCommand
    {
        private readonly IPackageManager _packageManager;
        private readonly ICardManager _cardManager;
        public AquirePackageCommand(IPackageManager packageManager, ICardManager cardManager, User identity) : base(identity)
        {
            _packageManager = packageManager;
            _cardManager = cardManager;
        }
        public override HttpResponse Execute()
        {
            HttpResponse response;

            try {
                Package? package = _packageManager.GetFirstPackage();
                _cardManager.AquirePackage(Identity, package.Id);
                _packageManager.DeletePackage(package.Id);
                response = new HttpResponse(StatusCode.Ok);
            } catch(NoPackageAvailableException e) {
                response = new HttpResponse(StatusCode.NotFound);
            } catch(NotEnoughCoinsException e) {
                response = new HttpResponse(StatusCode.Forbidden);
            }

            return response;
        }
    }
}
