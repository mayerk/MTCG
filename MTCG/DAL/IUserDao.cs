using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;

namespace MTCG.DAL
{
    internal interface IUserDao
    {
        User? GetUserByAuthToken(string authToken);
        User? GetUserByCredentials(string username, string password);
        bool InsertUser(User user);
        User? GetUserByUsername(string username);
        bool UpdateUser(User user);
        bool UpdateUserCoins(User user);
        List<Card> GetDeckByAuthToken(string token);
        List<User> GetScoreboard();
    }
}
