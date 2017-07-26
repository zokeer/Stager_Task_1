using DomainModel.Models;
using System.Collections.Generic;

namespace DomainModel.Repository
{
    /// <summary>
    /// Интерфейс репозитория, представляет требования к созданию новых репозиториев, описывая их поведение.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Получает все записи. Преобразует их в экземпляры класса Subnet, складывает в контейнер.
        /// </summary>
        /// <returns>Контейнер экземпляров класса Subnet</returns>
        List<Subnet> GetDataFromPhysicalSource();

        /// <summary>
        /// Добавляет новую подсеть.
        /// </summary>
        /// <param name="id">ID новой подсети.</param>
        /// <param name="raw_subnet">Подсеть в строковом формате.</param>
        void Create(string id, string raw_subnet);

        /// <summary>
        /// Удаляет подсеть.
        /// </summary>
        /// <param name="id">ID сети, которую нужно удалить.</param>
        void Delete(string id);

        /// <summary>
        /// Более быстрый метод, нежели GetDataFromPhysicalSource. 
        /// Работает только с виртуальным хранилищем.
        /// </summary>
        /// <returns>Контейнер экземпляров класса Subnet</returns>
        List<Subnet> Get();
    }
}
