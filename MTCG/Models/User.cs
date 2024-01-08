namespace MTCG.Models
{
    internal class User
    {
        public string Id {  get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public int Coins { get; set; } = 20;
        public UserData UserData { get; set; }
        public List<Card> Deck = new List<Card>();
        public string Token => $"{Username}-mtcgToken";
        public int Elo { get; set; } = 0;
        public int Wins { get; set; } = 0;
        public int Losses { get; set; } = 0;

        public User(string username, string password, UserData userdata)
        {
            Id = Guid.NewGuid().ToString();
            Username = username;
            Password = password;
            UserData = userdata;
        }
    }
}
