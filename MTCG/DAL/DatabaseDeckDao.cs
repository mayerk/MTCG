using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;
using Npgsql;

namespace MTCG.DAL {
    internal class DatabaseDeckDao: IDeckDao {

        private readonly string _connectionString;

        private readonly string CreateDeckTableCommand = @"CREATE TABLE IF NOT EXISTS decks(id varchar NOT NULL, uid varchar NOT NULL, cid varchar, PRIMARY KEY(id));";
        private readonly string DeleteTableEntriesCommand = @"DELETE from decks;";
        private readonly string GetDeckByUIdCommand = @"SELECT * from decks WHERE uid=@uid;";
        private readonly string GetDeckByCIdCommand = @"SELECT * from decks WHERE cid=@cid;";
        private readonly string InsertDeckCommand = @"INSERT INTO decks (id, uid, cid) VALUES (@id, @uid, @cid);";


        public DatabaseDeckDao(string connectionString) {
            _connectionString = connectionString;
            EnsureTables();
        }

        private void EnsureTables() {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(CreateDeckTableCommand, connection);
            cmd.ExecuteNonQuery();
            using var cmd2 = new NpgsqlCommand(DeleteTableEntriesCommand, connection);
            cmd2.ExecuteNonQuery();
        }

        public List<Deck> GetDeckByUId(string uid) {
            List<Deck> deck = new List<Deck>();

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(GetDeckByUIdCommand, connection);
            cmd.Parameters.AddWithValue("uid", uid);
            using var reader = cmd.ExecuteReader();

            while (reader.Read()) {
                deck.Add(ReadDeck(reader));
            }
            return deck;
        }

        public bool InsertDeck(List<Card> cards) {
            bool inserted = true;

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            foreach (Card card in cards) {
                using var cmd = new NpgsqlCommand(InsertDeckCommand, connection);
                cmd.Parameters.AddWithValue("id", Guid.NewGuid().ToString());
                cmd.Parameters.AddWithValue("uid", card.UId);
                cmd.Parameters.AddWithValue("cid", card.Id);
                var affectedRows = cmd.ExecuteNonQuery();
                inserted = affectedRows > 0;
            }
            return inserted;
        }

        public Deck? GetDeckByCId(string cid) {
            Deck? deck = null;
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(GetDeckByCIdCommand, connection);
            cmd.Parameters.AddWithValue("cid", cid);
            using var reader = cmd.ExecuteReader();
            if(reader.Read()) {
                deck = ReadDeck(reader);
            }
            return deck;
        }

        private Deck ReadDeck(IDataRecord record) {
            var id = Convert.ToString(record["id"]);
            var uid = Convert.ToString(record["uid"]);
            var cid = Convert.ToString(record["cid"]);
            return new Deck(id, uid, cid);
        }
    }
}
