using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.BLL.Exceptions;
using MTCG.BLL.Managers;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.Models;

namespace MTCG.API.Routing.Users
{
    internal class RegisterCommand : IRouteCommand
    {
        private readonly IUserManager _userManager;
        private readonly Credentials _credentials;

        public RegisterCommand(IUserManager userManager, Credentials credentials)
        {
            _userManager = userManager;
            _credentials = credentials;
        }

        public HttpResponse Execute()
        {
            HttpResponse response;

            try
            {
                _userManager.RegisterUser(_credentials);
                response = new HttpResponse(StatusCode.Created);
            }
            catch (DuplicateUserException)
            {
                response = new HttpResponse(StatusCode.Conflict);
            }

            return response;
        }
    }
}
