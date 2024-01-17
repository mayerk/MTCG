using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.BLL.Managers;
using MTCG.HttpServer.Request;
using MTCG.Models;

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
                        int index = authToken.Substring(prefix.Length).IndexOf("-", StringComparison.Ordinal);
                        string username = authToken.Substring(prefix.Length).Substring(0, index);
                        currentUser = _userManager.GetUserByUsername(username);
                    }
                    catch { }
                }
            }

            return currentUser;
        }

        public User? ValidateIdentity(HttpRequest request, string path) {
            User? user = GetIdentityForRequest(request);
            if (user != null) {
                string queueUsername = GetParameterFromRequest(path);
                if(user.Username ==  queueUsername || user.Username == "admin") {
                    //user = new(queueUsername);
                    user = _userManager.GetUserByUsername(queueUsername);
                } else {
                    user = null;
                }
            }
            return user;
        }

        private string GetParameterFromRequest(string path) {
            int index = path.LastIndexOf("/");
            return path.Substring(index + 1);
        }
    }
}
