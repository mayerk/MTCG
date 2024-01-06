namespace MTCG.Models
{
    internal class User
    {
        public string id {  get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public UserData UserData { get; set; }
        public string Token => $"{Username}-mtcgToken";

        public User(string username, string password, UserData userdata)
        {
            id = Guid.NewGuid().ToString();
            Username = username;
            Password = password;
            UserData = userdata;
        }
    }
}
