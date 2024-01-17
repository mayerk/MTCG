using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL {
    internal class DatabaseTradeDao: ITradeDao {

        private readonly string _connectionString;

        private readonly string CreateTradeTableCommand = @"CREATE TABLE IF NOT EXISTS trades (id varchar not null, cid varchar not null, cardtype varchar, damage numeric(6,2), PRIMARY KEY(id));";
        private readonly string DeleteTableEntriesCommand = @"DELETE FROM trades;";
        private readonly string InsertTradingDealCommand = @"INSERT INTO trades (id, cid, cardtype, damage) VALUES (@id, @cid, @cardtype, @damage);";
        private readonly string GetAllTradesCommand = @"SELECT * from trades;";
        private readonly string GetTradeByIdCommand = @"SELECT * from trades WHERE id=@id;";
        private readonly string DeleteTradeByIdCommand = @"DELETE from trades WHERE id=@id;";

        public DatabaseTradeDao(string connectionString) { 
            _connectionString = connectionString;
            EnsureTables();
        }

        private void EnsureTables() {
            // TODO: handle exceptions
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(CreateTradeTableCommand, connection);
            cmd.ExecuteNonQuery();
            using var cmd2 = new NpgsqlCommand(DeleteTableEntriesCommand, connection);
            cmd2.ExecuteNonQuery();
        }

        public List<Trade> GetAllTrades() {
            List<Trade> trades = new List<Trade>();
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(GetAllTradesCommand, connection);
            using var reader = cmd.ExecuteReader();

            while(reader.Read()) {
                trades.Add(ReadTrade(reader));
            }
            return trades;
        }

        public Trade? GetTradeById(string id) {
            Trade trade = null;
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(GetTradeByIdCommand, connection);
            cmd.Parameters.AddWithValue("id", id);
            using var reader = cmd.ExecuteReader();
            if(reader.Read()) {
                trade = ReadTrade(reader);
            }
            return trade;
        }

        public bool CreateTradingDeal(Trade trade) {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            if(GetTradeById(trade.Id) != null) {
                return false;
            }

            using var cmd = new NpgsqlCommand(InsertTradingDealCommand, connection);
            cmd.Parameters.AddWithValue("id", trade.Id);
            cmd.Parameters.AddWithValue("cid", trade.CId);
            cmd.Parameters.AddWithValue("cardtype", trade.CardType);
            cmd.Parameters.AddWithValue("damage", trade.MinimumDamage);
            var affectedRows = cmd.ExecuteNonQuery();

            return affectedRows > 0;
        }

        public bool DeleteTradingDeal(string id) {
            if(GetTradeById(id) == null) {
                return false;
            }
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(DeleteTradeByIdCommand, connection);
            cmd.Parameters.AddWithValue("id", id);
            var affectedRows = cmd.ExecuteNonQuery();
            return affectedRows > 0;
        }

        private Trade ReadTrade(IDataRecord record) {
            var id = Convert.ToString(record["id"]);
            var cid = Convert.ToString(record["cid"]);
            var type = Convert.ToString(record["cardtype"]);
            var damage = Convert.ToInt32(record["damage"]);

            return new Trade(id, cid, type, damage);
        }
    }
}
