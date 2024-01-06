using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL
{
    internal class InMemoryUserDao : IUserDao
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

            User? tmp = _users.SingleOrDefault(u => u.Username == user.Username);
            if (tmp != null) {
                tmp.UserData = user.UserData;
                updated = true;
            }
            return updated;
        }
    }
}
