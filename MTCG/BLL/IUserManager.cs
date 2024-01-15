using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL
{
    internal interface IUserManager
    {
        User LoginUser(Credentials credentials);
        void RegisterUser(Credentials credentials);
        User GetUserByAuthToken(string authToken);
        User GetUserByUsername(string username);
        void UpdateUser(User user);
        void UpdateUserCoins(User user);
        List<Card> GetDeckByAuthToken(string token);
        List<User> GetScoreboard();
    }
}
