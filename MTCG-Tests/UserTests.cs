using MTCG.BLL.Managers;
using MTCG.DAL;
using MTCG.Models;

namespace MTCG_Tests {
    public class UserTests {

        private static readonly string connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=mtcg";

        private IUserManager _userManager;
        private IUserDao _userDao = new DatabaseUserDao(connectionString);

        [SetUp]
        public void Setup() {
            _userManager = new UserManager(_userDao);
        }

        [Test][Order(0)]
        public void Test_RegisterUser() {
            Credentials credentials = new("test", "test");
            _userManager.RegisterUser(credentials);
            User? user = _userManager.GetUserByUsername(credentials.Username);
            if(user == null) {
                Assert.Fail("User 'test' could not be retreived.");
                return;
            }
            Assert.That(user.Username, Is.EqualTo(credentials.Username));
        }

        [Test][Order(1)]
        public void Test_UserLogin() {
            Credentials credentials = new("test", "test");
            User? user = _userManager.LoginUser(credentials);
            if(user == null) {
                Assert.Fail("User 'test' could not be retreived.");
                return;
            }
            Assert.That(user.Username, Is.EqualTo(credentials.Username));
        }

        [Test][Order(2)]
        public void Test_UpdateUser() {
            User? user = _userManager.GetUserByUsername("test");
            if(user == null) {
                Assert.Fail("User 'test' could not be retreived.");
                return;
            }
            UserData userData = new("MTCGGamer", "I'm the best at this game!", ";-)");
            user.UserData = userData;
            _userManager.UpdateUser(user);

            user = _userManager.GetUserByUsername("test");
            Assert.That(user.UserData.Displayname, Is.EqualTo(userData.Displayname));
        }

        [Test][Order(3)]
        public void Test_GetUserByUsername() {
            User? user = _userManager.GetUserByUsername("test");
            if (user == null) {
                Assert.Fail("User 'test' could not be retreived.");
                return;
            }
            Assert.That(user.Username, Is.EqualTo("test"));
        }

        [Test][Order(4)]
        public void Test_UpdateUserCoins() {
            User? user = _userManager.GetUserByUsername("test");
            if (user == null) {
                Assert.Fail("User 'test' could not be retreived.");
                return;
            }
            user.Coins = 15;
            _userManager.UpdateUser(user);
            user = _userManager.GetUserByUsername("test");
            Assert.That(user.Coins, Is.EqualTo(15));
        }

        [Test][Order(5)]
        public void Test_GetScoreboard() {
            List<User> users = _userManager.GetScoreboard();
            if(!users.Any()) {
                Assert.Fail("No users found.");
            }
            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test][Order(6)]
        public void Test_DeleteUser() {
            bool deleted = _userManager.DeleteUserByUsername("test");
            Assert.IsTrue(deleted);
        }
    }
}