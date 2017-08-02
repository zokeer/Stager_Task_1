using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DomainModel.Models;
using DomainModel.Repository;

namespace Task_1.DomainModel.Repository
{
    public class DBRepository : IRepository
    {
        private readonly List<Subnet> _subnets;
        private readonly string _connectionString;

        public DBRepository(string connection_string)
        {
            if (connection_string == null)
                throw new ArgumentNullException(nameof(connection_string), "Не может быть null.");

            _connectionString = connection_string;
            _subnets = GetDataFromPhysicalSource();
        }

        public List<Subnet> GetDataFromPhysicalSource()
        {
            var sql_expression = "SELECT * FROM Subnets";
            var subnets = new List<Subnet>();
            
            using (var sql_connection = new SqlConnection(_connectionString))
            {
                sql_connection.Open();
                var command = new SqlCommand(sql_expression, sql_connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var id = reader.GetValue(0).ToString();
                    var network = reader.GetValue(1).ToString();

                    subnets.Add(new Subnet(id, network));
                }
            }
            return subnets;
        }

        public void Create(string id, string raw_subnet)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id), "Не может быть null.");
            if (raw_subnet == null)
                throw new ArgumentNullException(nameof(raw_subnet), "Не может быть null.");

            _subnets.Add(new Subnet(id, raw_subnet));
        }

        public void Delete(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id), "Не может быть null.");

            throw new NotImplementedException();
        }

        public void Edit(string old_id, string new_id, string raw_subnet)
        {
            if (old_id == null)
                throw new ArgumentNullException(nameof(old_id), "Не может быть null.");
            if (new_id == null)
                throw new ArgumentNullException(nameof(new_id), "Не может быть null.");
            if (raw_subnet == null)
                throw new ArgumentNullException(nameof(raw_subnet), "Не может быть null.");

            throw new NotImplementedException();
        }

        public List<Subnet> Get()
        {
            return _subnets;
        }
    }
}