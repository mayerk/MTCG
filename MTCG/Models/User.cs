namespace MTCG.Models
{
    internal class User
    {
        public string Id {  get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public int Coins { get; set; }
        public UserData UserData { get; set; }
        public string Token => $"{Username}-mtcgToken";

        public User(string username, string password, UserData userdata)
        {
            Id = Guid.NewGuid().ToString();
            Username = username;
            Password = password;
            Coins = 20;
            UserData = userdata;
        }
    }
}
