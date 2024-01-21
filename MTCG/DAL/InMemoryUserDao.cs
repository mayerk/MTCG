using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;

namespace MTCG.DAL
{
    public class InMemoryUserDao : IUserDao
    {
        private readonly List<User> _users = new();

        public User? GetUserByAuthToken(string authToken)
        {
            return _users.SingleOrDefault(u => u.Token == authToken);
        }

        public User? GetUserByCredentials(string username, string password)
        {
            return _users.SingleOrDefault(u => u.Username == username && u.Password == password);
        }

        public bool InsertUser(User user)
        {
            bool inserted = false;

            if (GetUserByUsername(user.Username) == null)
            {
                _users.Add(user);
                inserted = true;
            }

            return inserted;
        }

        public User? GetUserByUsername(string username)
        {
            return _users.SingleOrDefault(u => u.Username == username);
        }

        public bool UpdateUser(User user) {
            bool updated = false;

            User? tmp = GetUserByUsername(user.Username);
            if (tmp != null) {
                tmp.UserData = user.UserData;
                updated = true;
            }
            return updated;
        }
        public List<Card> GetDeckByAuthToken(string token) {
            List<Card> cards = new List<Card>();
            User? user = GetUserByAuthToken(token);
            if(user != null) {
                cards = user.Deck;
            }
            return cards;
        }

        public bool UpdateUserCoins(User user) {
            throw new NotImplementedException();
        }

        public List<User> GetScoreboard() {
            throw new NotImplementedException();
        }

        public bool DeleteUserByUsername(string username) {
            throw new NotImplementedException();
        }
    }
}
