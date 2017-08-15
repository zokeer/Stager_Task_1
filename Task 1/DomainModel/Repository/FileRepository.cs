using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using DomainModel.Models;

namespace DomainModel.Repository
{
    /// <summary>
    /// Репозиторий работы с файлом, реализует интерфейс IRepository.
    /// </summary>
    public class FileRepository : IRepository
    {
        /// <summary>
        /// Контейнер Подсетей.
        /// </summary>
        private List<Subnet> _subnets;
        /// <summary>
        /// Путь до расположения файла с данными.
        /// </summary>
        private readonly string _repositoryPath;

        /// <summary>
        /// Конструктор инициализирует путь до файла с данными.
        /// </summary>
        /// <param name="repository_path">Путь до файла с данными.</param>
        public FileRepository(string repository_path)
        {
            if (repository_path == null)
                throw new ArgumentNullException(nameof(repository_path), "Не может быть null.");

            _repositoryPath = repository_path;
            _subnets = GetDataFromPhysicalSource();
        }

        /// <summary>
        /// Записывает в файл и в контейнер новую подсеть в формате Json.
        /// </summary>
        /// <param name="id">ID новой подсети.</param>
        /// <param name="raw_subnet">Подсеть в строковом формате.</param>
        public void Create(string id, string raw_subnet)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id), "Не может быть null.");
            if (raw_subnet == null)
                throw new ArgumentNullException(nameof(raw_subnet), "Не может быть null.");
            _subnets.Add(new Subnet(id, raw_subnet));

            try
            {
                File.WriteAllText(_repositoryPath,
                    JsonConvert.SerializeObject(
                        _subnets.Select(subnet => $"{subnet.Id},{subnet.Address}/{subnet.Mask}"))
                );
            }
            catch (Newtonsoft.Json.JsonSerializationException)
            {
                throw new JsonSerializationException(@"Данные, которые вы пытаетесь записать неверны.
                                                       Требуется соотвествие списку экземпляров класса Subnet.");
            }

        }

        /// <summary>
        /// Удаляет из файла подсеть.
        /// </summary>
        /// <param name="id">ID сети, которую нужно удалить.</param>
        public void Delete(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id), "Не может быть null.");
            _subnets = GetDataFromPhysicalSource();

            _subnets.Remove(_subnets.Find(subnet => subnet.Id == id));

            try
            {
                File.WriteAllText(_repositoryPath,
                    JsonConvert.SerializeObject(
                        _subnets.Select(subnet => $"{subnet.Id},{subnet.Address}/{subnet.Mask}"))
                );
            }
            catch (Newtonsoft.Json.JsonSerializationException)
            {
                throw new JsonSerializationException(@"Данные, которые вы пытаетесь записать неверны.
                                                       Требуется соотвествие списку экземпляров класса Subnet.");
            }
        }

        /// <summary>
        /// Метод выполняет последовательный вызов Delete и Create для изменения подсети.
        /// </summary>
        /// <param name="old_id">ID той сети, чьи параметры нужно изменить.</param>
        /// <param name="new_id">ID новой подсети.</param>
        /// <param name="raw_subnet">Строковое представление подсети.</param>
        public void Edit(string old_id, string new_id, string raw_subnet)
        {
            if (old_id == null)
                throw new ArgumentNullException(nameof(old_id), "Не может быть null.");
            if (new_id == null)
                throw new ArgumentNullException(nameof(new_id), "Не может быть null.");
            if (raw_subnet == null)
                throw new ArgumentNullException(nameof(raw_subnet), "Не может быть null.");

            Delete(old_id);
            Create(new_id, raw_subnet);
        }

        /// <summary>
        /// Более быстрый метод, нежели GetDataFromPhysicalSource. 
        /// Работает только с внутренним полем Subnets.
        /// </summary>
        /// <returns>Контейнер экземпляров класса Subnet</returns>
        public List<Subnet> Get()
        {
            _subnets = GetDataFromPhysicalSource();
            return _subnets;
        }

        /// <summary>
        /// Получает все записи из файла. Преобразует их в экземпляры класса Subnet, складывает в контейнер.
        /// Используется для вынуждения синхронизации виртуального контейнера с действительным файлом.
        /// </summary>
        /// <returns>Контейнер экземпляров класса Subnet.</returns>
        private List<Subnet> GetDataFromPhysicalSource()
        {
            var data = File.ReadAllText(_repositoryPath);
            List<Subnet> subnets;

            try
            {
                subnets = JsonConvert.DeserializeObject<IEnumerable<string>>(data)
                    .Select(raw_data => new Subnet(raw_data.Split(',')[0], raw_data.Split(',')[1])).ToList();
            }
            catch (Newtonsoft.Json.JsonReaderException)
            {
                throw new JsonReaderException(
                    "Файл-репозиторий поставляет неверные данные. Они должны быть в формате JSON.");
            }

            return subnets;
        }
    }
}