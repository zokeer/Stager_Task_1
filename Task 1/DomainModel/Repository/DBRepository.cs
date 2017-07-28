using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DomainModel.Models;
using DomainModel.Repository;

namespace Task_1.DomainModel.Repository
{
    public class DBRepository : IRepository
    {
        private List<Subnet> _subnets;
        private readonly string _connectionString;

        public DBRepository(string connection_string)
        {
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
                    string id = reader.GetValue(0).ToString();
                    string network = reader.GetValue(1).ToString();

                    subnets.Add(new Subnet(id, network));
                }
            }
            return subnets;
        }

        public void Create(string id, string raw_subnet)
        {
            _subnets.Add(new Subnet(id, raw_subnet));


        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public void Edit(string old_id, string new_id, string raw_subnet)
        {
            throw new NotImplementedException();
        }

        public List<Subnet> Get()
        {
            return _subnets;
        }
    }
}