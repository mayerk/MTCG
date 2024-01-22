using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.BLL.Exceptions;
using MTCG.BLL.Managers;
using MTCG.HttpServer.Response;
using MTCG.Models;

namespace MTCG.API.Routing.Packages
{
    public class AquirePackageCommand : AuthenticatedRouteCommand
    {
        private readonly IPackageManager _packageManager;
        private readonly ICardManager _cardManager;
        private readonly IUserManager _userManager;
        public AquirePackageCommand(IPackageManager packageManager, ICardManager cardManager, IUserManager userManager, User identity) : base(identity)
        {
            _packageManager = packageManager;
            _cardManager = cardManager;
            _userManager = userManager;
        }
        public override HttpResponse Execute()
        {
            HttpResponse response;

            try {
                Package? package = _packageManager.GetFirstPackage();
                _cardManager.FillCardsInPackage(package);
                _cardManager.AquirePackage(Identity, package);
                _userManager.UpdateUser(Identity);
                _packageManager.DeletePackage(package.PId);
                response = new HttpResponse(StatusCode.Ok);
            } catch(NoPackageAvailableException) {
                response = new HttpResponse(StatusCode.NotFound);
                return response;
            } catch(NotEnoughCoinsException) {
                response = new HttpResponse(StatusCode.Forbidden);
                return response;
            }

            return response;
        }
    }
}
