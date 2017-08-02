using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Management;
using Dapper;
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
            if (connection_string == null)
                throw new ArgumentNullException(nameof(connection_string), "Не может быть null.");

            _connectionString = connection_string;
            _subnets = GetDataFromPhysicalSource();
        }

        public void Create(string id, string raw_subnet)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id), "Не может быть null.");
            if (raw_subnet == null)
                throw new ArgumentNullException(nameof(raw_subnet), "Не может быть null.");

            var sql_expression = $"INSERT INTO Subnets (id, network) VALUES ({id}, {raw_subnet})";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sql_expression, connection);
                var rows_affected = command.ExecuteNonQuery();
                if (rows_affected == 0)
                    throw new SqlExecutionException("Не удалось добавить данные.");
            }

            _subnets.Add(new Subnet(id, raw_subnet));
        }

        public void Delete(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id), "Не может быть null.");

            var sql_expression = $"DELETE FROM Subnets WHERE id = {id}";
            using (var connection = new SqlConnection(_connectionString))
            {
                var rows_affected = connection.Query(sql_expression);
                if (rows_affected.AsList().Count == 0)
                    throw new SqlExecutionException("Не удалось удалить данные.");
            }
        }

        public void Edit(string old_id, string new_id, string raw_subnet)
        {
            if (old_id == null)
                throw new ArgumentNullException(nameof(old_id), "Не может быть null.");
            if (new_id == null)
                throw new ArgumentNullException(nameof(new_id), "Не может быть null.");
            if (raw_subnet == null)
                throw new ArgumentNullException(nameof(raw_subnet), "Не может быть null.");

            var sql_expression = $"UPDATE Subnets SET id = {new_id}, network = {raw_subnet} WHERE id='{old_id}'";
            using (var connection = new SqlConnection(_connectionString))
            {
                var rows_affected = connection.Query(sql_expression);
                if (rows_affected.AsList().Count == 0)
                    throw new SqlExecutionException("Не удалось изменить данные.");
            }
            _subnets = _subnets.Where(subnet => subnet.Id != old_id).ToList();
            _subnets.Add(new Subnet(new_id, raw_subnet));
        }

        public List<Subnet> Get()
        {
            _subnets = GetDataFromPhysicalSource();
            return _subnets;
        }

        private List<Subnet> GetDataFromPhysicalSource()
        {
            const string sqlExpression = "SELECT * FROM Subnets";
            var subnets = new List<Subnet>();

            using (var sql_connection = new SqlConnection(_connectionString))
            {
                sql_connection.Open();
                var command = new SqlCommand(sqlExpression, sql_connection);
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
    }
}