using System;

namespace Task_1.ASMX
{
    [Serializable]
    public class SeriablizableSubnet
    {
        public string Id { get; set; }
        public string Address { get; set; }
        public string Mask { get; set; }
    }
}