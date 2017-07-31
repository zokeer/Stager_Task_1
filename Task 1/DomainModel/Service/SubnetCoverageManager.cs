using System;
using DomainModel.Models;
using System.Collections.Generic;
using System.Linq;

namespace DomainModel.Service
{
    /// <summary>
    /// Класс хранит в себе методы, для построения минимального покрытия.
    /// </summary>
    public static class SubnetCoverageManager
    {
        /// <summary>
        /// Метод, строящий минимальное покрытие группы подсетей.
        /// </summary>
        /// <param name="subnet_container">Группа подсетей.</param>
        /// <returns>
        /// Словарь минимального покрытия, устроенный по принципу:
        /// ключ - покрывающая подсеть
        /// значение - список подсетей, которые покрывает подсеть-ключ.
        /// </returns>
        public static Dictionary<Subnet, List<Subnet>> GetMinimalCoverage(List<Subnet> subnet_container)
        { 
            // Топологическая сортировка, теперь "наверху" списка будут большие подсети.
            subnet_container.Sort((s1, s2) => s2.CompareTo(s1));

            var coverage_dict = new Dictionary<Subnet, List<Subnet>>();
            var covered_subnets = new List<Subnet>();
            foreach (var subnet in subnet_container)
            {
                coverage_dict.Add(subnet, subnet_container.Where(s => subnet.IsCovering(s) && !covered_subnets.Contains(s)).ToList());
                covered_subnets.AddRange(coverage_dict[subnet]);
            }
            coverage_dict = RemoveEmptyCoverages(coverage_dict);

            return coverage_dict;
        }

        /// <summary>
        /// Метод убирает из словаря те записи, где список покрываемых подсетей пуст.
        /// </summary>
        /// <param name="coverage_dict">
        /// Словарь минимального покрытия, устроенный по принципу:
        /// ключ - покрывающая подсеть
        /// значение - список подсетей, которые покрывает подсеть-ключ.
        /// </param>
        /// <returns>Полученный словарь, без записей с пустым списком.</returns>
        private static Dictionary<Subnet, List<Subnet>> RemoveEmptyCoverages(Dictionary<Subnet, List<Subnet>> coverage_dict)
        {
            return coverage_dict
                .Where(s => s.Value.Count > 0)
                .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}