using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL {
    public class DatabaseCouponDao: ICouponDao {

        private readonly string _connectionString;

        private readonly string CreateCardTableCommand = @"CREATE TABLE IF NOT EXISTS coupons(id varchar NOT NULL, coins integer not null, PRIMARY KEY(id));";
        private readonly string DeleteTableEntriesCommand = @"DELETE from coupons";
        private readonly string GetCouponByIdCommand = @"SELECT * from coupons WHERE id=@id;";
        private readonly string InsertCouponCommand = @"INSERT INTO coupons (id, coins) VALUES (@id, @coins);";
        private readonly string DeleteCouponByIdCommand = @"DELETE from coupons WHERE id=@id;";

        public DatabaseCouponDao(string connectionString) {
            _connectionString = connectionString;
            EnsureTables();
        }

        private void EnsureTables() {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(CreateCardTableCommand, connection);
            cmd.ExecuteNonQuery();
            using var cmd1 = new NpgsqlCommand(DeleteTableEntriesCommand, connection);
            cmd1.ExecuteNonQuery();
        }

        public Coupon? GetCouponById(string id) {
            Coupon? coupon = null;
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(GetCouponByIdCommand, connection);
            cmd.Parameters.AddWithValue("id", id);
            using var reader = cmd.ExecuteReader();

            if(reader.Read()) {
                coupon = ReadCoupon(reader);
            }
            return coupon;
        }

        public bool InsertCoupon(Coupon coupon) {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            if(GetCouponById(coupon.Id) != null) {
                return false;
            }

            using var cmd = new NpgsqlCommand(InsertCouponCommand, connection);
            cmd.Parameters.AddWithValue("id", coupon.Id);
            cmd.Parameters.AddWithValue("coins", coupon.Coins);
            var affectedRows = cmd.ExecuteNonQuery();

            return affectedRows > 0;
        }

        public bool DeleteCouponById(string id) {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(DeleteCouponByIdCommand, connection);
            cmd.Parameters.AddWithValue("id", id);
            var affectedRows = cmd.ExecuteNonQuery();
            return affectedRows > 0;
        }

        private Coupon ReadCoupon(IDataRecord record) {
            var id = Convert.ToString(record["id"]);
            var coins = Convert.ToInt32(record["coins"]);
            return new(id, coins);
        }
    }
}
