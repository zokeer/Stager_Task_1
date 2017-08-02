using System.Collections.Generic;
using DomainModel.Models;

namespace DomainModel.Repository
{
    /// <summary>
    /// Интерфейс репозитория, представляет требования к созданию новых репозиториев, описывая их поведение.
    /// </summary>
    public interface IRepository
    {
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
        /// Изменяет сеть по old_id на сеть (new_id, raw_subnet).
        /// </summary>
        /// <param name="old_id">ID той сети, чьи параметры нужно изменить.</param>
        /// <param name="new_id">ID новой подсети.</param>
        /// <param name="raw_subnet">Строковое представление подсети.</param>
        void Edit(string old_id, string new_id, string raw_subnet);

        /// <summary>
        /// Более быстрый метод, нежели GetDataFromPhysicalSource. 
        /// Работает только с виртуальным хранилищем.
        /// </summary>
        /// <returns>Контейнер экземпляров класса Subnet</returns>
        List<Subnet> Get();
    }
}
