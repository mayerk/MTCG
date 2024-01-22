using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using MTCG.Models;
using System.Data;
using MTCG.BLL;
using MTCG.BLL.Exceptions;

namespace MTCG.DAL
{
    public class DatabaseUserDao: IUserDao
    {
        private const string DeleteTableEntriesCommand = @"DELETE from users";
        private const string CreateUserTableCommand = @"CREATE TABLE IF NOT EXISTS users (id varchar NOT NULL, name varchar NOT NULL, password varchar, displayname varchar NOT NULL, coins integer default 20, bio varchar, image varchar, elo integer default 100, wins integer default 0, losses integer default 0, PRIMARY KEY (id));";
        private const string SelectAllUsersCommand = @"SELECT username, password FROM users";
        private const string SelectUserByUsernameCommand = @"SELECT * from users WHERE name=@username;";
        private const string SelectUserByCredentialsCommand = @"SELECT * from users WHERE name=@username AND password=@password";
        private const string InsertUserCommand = @"INSERT INTO users(id, name, password, displayname, bio, image) VALUES (@id, @name, @pwd, @displayname, @bio, @image)";
        private const string UpdateUserCommand = @"UPDATE users SET displayname=@displayname, bio=@bio, image=@image, elo=@elo, wins=@wins, losses=@losses WHERE name=@name";
        private const string UpdateUserCoinsCommand = @"UPDATE users SET coins=@coins WHERE name=@name;";
        private const string GetScoreboardCommand = @"SELECT * from users ORDER BY elo desc;";
        private const string DeleteUserByUsernameCommand = @"DELETE from users WHERE name=@name;";

        private readonly string _connectionString;


        public DatabaseUserDao(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }

        private void EnsureTables() {
            // TODO: handle exceptions
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(CreateUserTableCommand, connection);
            cmd.ExecuteNonQuery();
            using var cmd2 = new NpgsqlCommand(DeleteTableEntriesCommand, connection);
            cmd2.ExecuteNonQuery();
        }

        public List<Card> GetDeckByAuthToken(string token) {
            throw new NotImplementedException();
        }

        public User? GetUserByAuthToken(string authToken)
        {
            return GetAllUsers().SingleOrDefault(u => u.Token == authToken);
        }

        public User? GetUserByCredentials(string username, string password)
        {
            // TODO: handle exceptions
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            User? user = null;

            using var cmd = new NpgsqlCommand(SelectUserByCredentialsCommand, connection);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);

            // take the first row, if any
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                user = ReadUser(reader);
            }

            return user;
        }

        public User? GetUserByUsername(string username) {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            User? user = null;

            using var cmd = new NpgsqlCommand(SelectUserByUsernameCommand, connection);
            cmd.Parameters.AddWithValue("username", username);
            using var reader = cmd.ExecuteReader();
            if (reader.Read()) {
                user = ReadUser(reader);
            }
            return user;
        }

        public bool InsertUser(User user)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            // TODO: handle exceptions

            User? tmp = GetUserByUsername(user.Username);
            if(tmp != null) {
                throw new DuplicateUserException();
            }

            using var cmd = new NpgsqlCommand(InsertUserCommand, connection);
            cmd.Parameters.AddWithValue("id", user.Id);
            cmd.Parameters.AddWithValue("name", user.Username);
            cmd.Parameters.AddWithValue("pwd", user.Password);
            cmd.Parameters.AddWithValue("displayname", user.Username);
            cmd.Parameters.AddWithValue("bio", user.UserData.Bio);
            cmd.Parameters.AddWithValue("image", user.UserData.Image);
            var affectedRows = cmd.ExecuteNonQuery();

            return affectedRows > 0;
        }

        public bool UpdateUser(User user) {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            User? tmp = GetUserByUsername(user.Username);

            if(tmp == null) {
                return false;
            }

            using var cmd = new NpgsqlCommand(UpdateUserCommand, connection);
            cmd.Parameters.AddWithValue("displayname", user.UserData.Displayname);
            cmd.Parameters.AddWithValue("bio", user.UserData.Bio);
            cmd.Parameters.AddWithValue("image", user.UserData.Image);
            cmd.Parameters.AddWithValue("elo", user.Elo);
            cmd.Parameters.AddWithValue("wins", user.Wins);
            cmd.Parameters.AddWithValue("losses", user.Losses);
            cmd.Parameters.AddWithValue("name", user.Username);
            var affectedRows = cmd.ExecuteNonQuery();

            return affectedRows > 0;
        }

        public bool UpdateUserCoins(User user) {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            User? tmp = GetUserByUsername(user.Username);

            if(tmp == null) {
                return false;
            }
            using var cmd = new NpgsqlCommand(UpdateUserCoinsCommand, connection);
            cmd.Parameters.AddWithValue("coins", user.Coins);
            cmd.Parameters.AddWithValue("name", user.Username);
            var affectedRows = cmd.ExecuteNonQuery();

            return affectedRows > 0;
        }

        private IEnumerable<User> GetAllUsers() 
        {
            // TODO: handle exceptions
            var users = new List<User>();

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(SelectAllUsersCommand, connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                users.Add(ReadUser(reader));
            }

            return users;
        }

        public bool DeleteUserByUsername(string username) {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(DeleteUserByUsernameCommand, connection);
            cmd.Parameters.AddWithValue("name", username);
            var affectedRows = cmd.ExecuteNonQuery();
            return affectedRows > 0;
        }

        public List<User> GetScoreboard() {
            List<User> users = new List<User>();
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(GetScoreboardCommand, connection);
            using var reader = cmd.ExecuteReader();

            while(reader.Read()) {
                users.Add(ReadUser(reader));
            }
            return users;
        }

        private User ReadUser(IDataRecord record)
        {
            var id = Convert.ToString(record["id"])!;
            var username = Convert.ToString(record["name"])!;
            var password = Convert.ToString(record["password"])!;
            var displayname = Convert.ToString(record["displayname"])!;
            var coins = Convert.ToInt16(record["coins"])!;
            var bio = Convert.ToString(record["bio"])!;
            var image = Convert.ToString(record["image"])!;
            var elo = Convert.ToInt16(record["elo"])!;
            var wins = Convert.ToInt16(record["wins"])!;
            var losses = Convert.ToInt16(record["losses"])!;
            UserData userData = new(displayname, bio, image);

            return new User(id, username, password, coins, userData, elo, wins, losses);
        }
    }
}
