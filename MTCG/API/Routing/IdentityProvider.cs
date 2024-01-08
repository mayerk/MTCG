using MTCG.BLL;
using MTCG.HttpServer.Request;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MTCG.API.Routing
{
    internal class IdentityProvider
    {
        private readonly IUserManager _userManager;

        public IdentityProvider(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public User? GetIdentityForRequest(HttpRequest request)
        {
            User? currentUser = null;

            if (request.Header.TryGetValue("Authorization", out var authToken))
            {
                const string prefix = "Bearer ";
                if (authToken.StartsWith(prefix))
                {
                    try
                    {
                        currentUser = _userManager.GetUserByAuthToken(authToken.Substring(prefix.Length));
                    }
                    catch { }
                }
            }

            return currentUser;
        }

        public User ValidateIdentity(HttpRequest request, string path) {
            User? currentUser = GetIdentityForRequest(request);

            int index = path.LastIndexOf('/');
            string username = path.Substring(index + 1);
            if(currentUser != null && username != currentUser.Username) {
                currentUser = null;
            }
            return currentUser;
        }
    }
}
