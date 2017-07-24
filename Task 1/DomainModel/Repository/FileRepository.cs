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
        private List<Subnet> Subnets;
        /// <summary>
        /// Путь до расположения файла с данными.
        /// </summary>
        private readonly string _repository_path;

        /// <summary>
        /// Конструктор инициализирует путь до файла с данными.
        /// </summary>
        /// <param name="repository_path">Путь до файла с данными.</param>
        public FileRepository(string repository_path)
        {
            _repository_path = repository_path;
            Subnets = GetDataFromPhysicalSource();
        }

        /// <summary>
        /// Записывает в файл и в контейнер новую подсеть в формате Json.
        /// </summary>
        /// <param name="id">ID новой подсети.</param>
        /// <param name="raw_subnet">Подсеть в строковом формате.</param>
        public void Create(string id, string raw_subnet)
        {
            var subnets = GetDataFromPhysicalSource();
            subnets.Add(new Subnet(id, raw_subnet));
            File.WriteAllText(_repository_path,
                JsonConvert.SerializeObject(
                    subnets.Select(subnet => $"{subnet.Id},{subnet.Network.Network}/{subnet.Network.Cidr}")
                    )
                );
            Subnets.Add(new Subnet(id, raw_subnet));

        }

        /// <summary>
        /// Удаляет из файла подсеть.
        /// </summary>
        /// <param name="id">ID сети, которую нужно удалить.</param>
        public void Delete(string id)
        {
            var subnets = GetDataFromPhysicalSource();
            subnets.Remove(subnets.Find(subnet => subnet.Id == id));
            File.WriteAllText(_repository_path,
                JsonConvert.SerializeObject(
                    subnets.Select(subnet => $"{subnet.Id},{subnet.Network.Network}/{subnet.Network.Cidr}"))
                    );
            Subnets = Subnets.Where(subnet => subnet.Id != id).ToList();
        }

        /// <summary>
        /// Получает все записи из файла. Преобразует их в экземпляры класса Subnet, складывает в контейнер.
        /// </summary>
        /// <returns>Контейнер экземпляров класса Subnet.</returns>
        public List<Subnet> GetDataFromPhysicalSource()
        {
            var data = File.ReadAllText(_repository_path);
            return JsonConvert.DeserializeObject<IEnumerable<string>>(data)
                .Select(raw_data => new Subnet(raw_data.Split(',')[0], raw_data.Split(',')[1])).ToList();
        }

        /// <summary>
        /// Более быстрый метод, нежели GetDataFromPhysicalSource. 
        /// Работает только с внутренним полем Subnets.
        /// </summary>
        /// <returns>Контейнер экземпляров класса Subnet</returns>
        public List<Subnet> Get()
        {
            return Subnets;
        }
    }
}