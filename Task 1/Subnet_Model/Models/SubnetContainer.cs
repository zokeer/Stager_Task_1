using System.Collections.Generic;

namespace Task_1.Models
{
    /// <summary>
    /// Класс-обёртка, модель для работы с данными.
    /// </summary>
    public class SubnetContainer
    {
        /// <summary>
        /// Контейнер подсетей. С ним взаимодействует сервис.
        /// </summary>
        public List<Subnet> Subnets { get; set; }
    }
}