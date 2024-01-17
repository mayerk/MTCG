using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.BLL.Exceptions;
using MTCG.DAL;
using MTCG.Models;

namespace MTCG.BLL.Managers
{
    internal class UserManager : IUserManager
    {
        private readonly IUserDao _userDao;

        public UserManager(IUserDao userDao)
        {
            _userDao = userDao;
        }

        public User GetUserByAuthToken(string authToken)
        {
            return _userDao.GetUserByAuthToken(authToken) ?? throw new UserNotFoundException();
        }

        public User LoginUser(Credentials credentials)
        {
            return _userDao.GetUserByCredentials(credentials.Username, credentials.Password) ?? throw new UserNotFoundException();
        }

        public void RegisterUser(Credentials credentials)
        {
            UserData userData = new(credentials.Username, "", "");
            var user = new User(credentials.Username, credentials.Password, userData);
            if (_userDao.InsertUser(user) == false)
            {
                throw new DuplicateUserException();
            }
        }

        public User GetUserByUsername(string username)
        {
            return _userDao.GetUserByUsername(username) ?? throw new UserNotFoundException();
        }

        public void UpdateUser(User user)
        {
            if (!_userDao.UpdateUser(user))
            {
                throw new UserNotFoundException();
            }
        }

        public void UpdateUserCoins(User user)
        {
            if (!_userDao.UpdateUserCoins(user))
            {
                throw new UserNotFoundException();
            }
        }
        public List<Card> GetDeckByAuthToken(string token)
        {
            return _userDao.GetDeckByAuthToken(token);
        }

        public List<User> GetScoreboard()
        {
            return _userDao.GetScoreboard();
        }
    }
}
