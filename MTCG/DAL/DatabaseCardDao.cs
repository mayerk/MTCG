﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.BLL;
using MTCG.Models;
using Npgsql;
using System.Data;
using MTCG.BLL.Exceptions;

namespace MTCG.DAL {
    public class DatabaseCardDao : ICardDao {

        private readonly string _connectionString;

        private readonly string CreateCardTableCommand = @"CREATE TABLE IF NOT EXISTS cards(id varchar NOT NULL, name varchar NOT NULL, damage numeric(6,2), uid varchar default '', PRIMARY KEY(id));";
        private readonly string DeleteTableEntriesCommand = @"DELETE from cards";
        private readonly string CreateCardCommand = @"INSERT INTO cards(id, name, damage) VALUES (@id, @name, @damage);";
        private readonly string GetCardByIdCommand = @"SELECT * from cards WHERE id=@id;";
        private readonly string UpdateCardUIdCommand = @"UPDATE cards SET uid=@uid WHERE id=@id;";
        private readonly string GetAllUsersCardsCommand = @"SELECT * from cards WHERE uid=@uid;";
        private readonly string DeleteCardByIdCommand = @"DELETE from cards WHERE id=@id;";

        public DatabaseCardDao(string connectionString) {
            _connectionString = connectionString;
            EnsureTables();
        }
        private void EnsureTables() {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(CreateCardTableCommand, connection);
            cmd.ExecuteNonQuery();
            using var cmd2 = new NpgsqlCommand(DeleteTableEntriesCommand, connection);
            cmd2.ExecuteNonQuery();
        }
        public bool InsertCard(Card card) {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            if(GetCardById(card.Id) != null) {
                throw new DuplicateCardException();
            }

            using var cmd = new NpgsqlCommand(CreateCardCommand, connection);
            cmd.Parameters.AddWithValue("id", card.Id);
            cmd.Parameters.AddWithValue("name", card.Name);
            cmd.Parameters.AddWithValue("damage", card.Damage);
            var affectedRows = cmd.ExecuteNonQuery();

            return affectedRows > 0;
        }

        public List<Card> GetAllCardsByUId(string uid) {
            List<Card> cards = new List<Card>();

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(GetAllUsersCardsCommand, connection);
            cmd.Parameters.AddWithValue("uid", uid);
            using var reader = cmd.ExecuteReader();

            while(reader.Read()) {
                cards.Add(ReadCard(reader));
            }
            return cards;
        }

        public Card? GetCardById(string id) {
            Card? card = null;

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(GetCardByIdCommand, connection);
            cmd.Parameters.AddWithValue("id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read()) {
                card = ReadCard(reader);
            }
            return card;
        }

        public bool UpdateCardUId(Card card) {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(UpdateCardUIdCommand, connection);
            cmd.Parameters.AddWithValue("uid", card.UId);
            cmd.Parameters.AddWithValue ("id", card.Id);
            var affectedRows = cmd.ExecuteNonQuery();

            return affectedRows > 0;
        }

        public bool DeleteCardById(string id) {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(DeleteCardByIdCommand, connection);
            cmd.Parameters.AddWithValue("id", id);
            var affectedRows = cmd.ExecuteNonQuery();
            return affectedRows > 0;
        }

        private Card ReadCard(IDataRecord record) {
            var id = Convert.ToString(record["id"]);
            var name = Convert.ToString(record["name"]);
            var damage = Convert.ToDouble(record["damage"]);
            var uid = Convert.ToString(record["uid"]);
            return new Card(id, name, damage, uid);
        }
    }
}
