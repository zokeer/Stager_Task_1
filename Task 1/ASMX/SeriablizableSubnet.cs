using System;

namespace Task_1.ASMX
{
    /// <summary>
    /// Класс, представляющий экземпляры класса Subnet в сериализованном виде.
    /// </summary>
    [Serializable]
    public class SeriablizableSubnet
    {
        public string Id { get; set; }
        public string Address { get; set; }
        public string Mask { get; set; }
    }
}