using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Management;
using Dapper;
using DomainModel.Models;
using Microsoft.Ajax.Utilities;

namespace DomainModel.Repository
{
    /// <summary>
    /// Репозиторий для работы с базой данных. Реализует интерфейс IRepository. 
    /// </summary>
    public class DBRepository : IRepository
    {
        /// <summary>
        /// Контейнер Подсетей.
        /// </summary>
        private List<Subnet> _subnets;
        /// <summary>
        /// Строка соединения с базой данных.
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// Конструктор сохраняет себе строку подключения.
        /// При отсутствии таблицы Subnets в базе данных создаст её.
        /// </summary>
        /// <param name="connection_string">Строка подключения к базе данных.</param>
        public DBRepository(string connection_string)
        {
            if (connection_string.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(connection_string), @"Аргумент должен был представлять строку
                                                                            подключения к базе данных, но был получен null.");

            _connectionString = connection_string;
            const string sqlExpression = @"if not exists (select * from sysobjects where name='Subnets' and xtype='U')
                                        create table Subnets (
                                            id nvarchar(255) not null,
                                            network nvarchar(32) not null
                                        )";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(sqlExpression, connection))
                    command.ExecuteNonQuery();
            }
             _subnets = GetDataFromPhysicalSource();
        }

        /// <summary>
        /// Отправляет запрос к базе данных. Запрос создаёт новую запись в таблицу Subnets.
        /// Добавляет в свой private-контейнер данную подсеть.
        /// </summary>
        /// <param name="id">Идентификатор новой подсети.</param>
        /// <param name="raw_subnet">Маскированный адрес новой подсети.</param>
        public void Create(string id, string raw_subnet)
        {
            if (id.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(id), "Идентификатор новой подсети не может быть null.");
            if (raw_subnet.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(raw_subnet), @"Аргумент должен быть маскированным
                                                                    адресом подсети, но был получен null.");

            var sql_expression = $"INSERT INTO Subnets (id, network) VALUES (N'{id}', N'{raw_subnet}')";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sql_expression, connection);
                var rows_affected = command.ExecuteNonQuery();
                if (rows_affected == 0)
                    throw new SqlExecutionException("Возникла ошибка при добавлении новых данных.");
            }

            _subnets.Add(new Subnet(id, raw_subnet));
        }

        /// <summary>
        /// Отправляет запрос к базе данных. Запрос удаляет запись из таблицы Subnets.
        /// Удаляет из своего private-контейнера данную подсеть.
        /// </summary>
        /// <param name="id">Идентификатор подсети, которую надо удалить.</param>
        public void Delete(string id)
        {
            if (id.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(id), @"Идентификатор удаляемой подсети не может быть null.
                                                              Выберите уже существующий идентификатор.");

            var sql_expression = $"DELETE FROM Subnets WHERE id = N'{id}'";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Query(sql_expression);
            }
            _subnets = _subnets.Where(subnet => subnet.Id != id).ToList();
        }

        /// <summary>
        /// Отправляет запрос к базе данных. Запрос изменяет запись в таблице Subnets.
        /// Изменяет в своём private-контейнере данную подсеть.
        /// </summary>
        /// <param name="old_id">Идентификатор подсети, которую нужно изменить.</param>
        /// <param name="new_id">Новый идентифкатор подсети</param>
        /// <param name="raw_subnet">Новый маскированный адрес подсети.</param>
        public void Edit(string old_id, string new_id, string raw_subnet)
        {
            if (old_id.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(old_id), @"Идентификатор подсети, которую нужно изменить
                                                                не может быть null.
                                                                Выберите существующий идентификатор подсети.");
            if (new_id.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(new_id), @"Новый идентификатор подсети не может быть null.
                                                                 Это либо новый уникальный идентификатор,
                                                                 либо тот, который изменяется.");
            if (raw_subnet.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(raw_subnet), @"Аргумент должен быть маскированным
                                                                    адресом подсети, но был получен null.");

            var sql_expression = $"UPDATE Subnets SET id = N'{new_id}', network = N'{raw_subnet}' WHERE id = N'{old_id}'";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Query(sql_expression);
            }
            _subnets = _subnets.Where(subnet => subnet.Id != old_id).ToList();
            _subnets.Add(new Subnet(new_id, raw_subnet));
        }

        /// <summary>
        /// Возвращает список подсетей из базы данных.
        /// </summary>
        /// <returns>Список экземпляров класса Subnet.</returns>
        public List<Subnet> Get()
        {
            _subnets = GetDataFromPhysicalSource();
            return _subnets;
        }

        /// <summary>
        /// Обращается к базе данных и запрашивает всё содержимое таблицы Subnets.
        /// </summary>
        /// <returns>Список экземпляров класса Subnet.</returns>
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