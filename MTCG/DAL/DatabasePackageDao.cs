using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL {
    internal class DatabasePackageDao : IPackageDao {

        private readonly string _connectionString;
           
        private readonly string CreatePackageTableCommand = @"CREATE TABLE IF NOT EXISTS packages(id varchar not null, pid varchar not null, cid varchar, number SERIAL, PRIMARY KEY(id));";
        private readonly string DeleteTableEntriesCommand = @"DELETE from packages";
        private readonly string InsertPackageCommand = @"INSERT INTO packages (id, pid, cid) VALUES (@id, @pid, @cid);";
        private readonly string GetPackageByIdCommand = @"SELECT * from packages WHERE id=@id;";
        private readonly string GetFirstPackageCommand = @"SELECT * from packages WHERE number = (SELECT min(number) from packages);";
        private readonly string GetPackagesByPIdCommand = @"SELECT * from packages WHERE pid=@pid;";
        private readonly string DeletePackageByPIdCommand = @"DELETE from packages WHERE pid=@pid;";

        public DatabasePackageDao(string connectionString) {
            _connectionString = connectionString;
            EnsureTables();
        }
        private void EnsureTables() {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(CreatePackageTableCommand, connection);
            cmd.ExecuteNonQuery();
            using var cmd2 = new NpgsqlCommand(DeleteTableEntriesCommand, connection);
            cmd2.ExecuteNonQuery();
        }
        public bool InsertPackage(Package package) {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            bool inserted = true;

            if(GetPackageById(package.Id) != null) {
                throw new DuplicatePackageException();
            }

            foreach(var card in package.Cards) {
                using var cmd = new NpgsqlCommand(InsertPackageCommand, connection);
                cmd.Parameters.AddWithValue("id", Guid.NewGuid().ToString());
                cmd.Parameters.AddWithValue("pid", package.PId);
                cmd.Parameters.AddWithValue("cid", card.Id);
                var affectedRows = cmd.ExecuteNonQuery();
                inserted = affectedRows > 0;
            }
            return inserted;
        }

        public bool DeletePackage(string pid) {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(DeletePackageByPIdCommand, connection);
            cmd.Parameters.AddWithValue("pid", pid);
            var affectedRows = cmd.ExecuteNonQuery();
            return affectedRows > 0;
        }

        public Package? GetFirstPackage() {
            Package? package = null;
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(GetFirstPackageCommand, connection);
            using var reader = cmd.ExecuteReader();
            if(reader.Read()) {
                package = ReadPackage(reader);
            }
            return package;
        }

        public Package? GetPackageById(string id) {
            Package? package = null;
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(GetPackageByIdCommand, connection);
            cmd.Parameters.AddWithValue("id", id);
            using var reader = cmd.ExecuteReader();
            if(reader.Read()) {
                package = ReadPackage(reader);
            }
            return package;
        }

        public List<Package> GetPackagesByPId(string pid) {
            List<Package> packages = new List<Package>();
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(GetPackagesByPIdCommand, connection);
            cmd.Parameters.AddWithValue("pid", pid);
            using var reader = cmd.ExecuteReader();
            
            while(reader.Read()) {
                packages.Add(ReadPackage(reader));
            }
            return packages;
        }

        private Package ReadPackage(IDataRecord record) {
            var id = Convert.ToString(record["id"]);
            var pid = Convert.ToString(record["pid"]);
            var cid = Convert.ToString(record["cid"]);
            return new Package(id, pid, cid);
        }
    }
}
