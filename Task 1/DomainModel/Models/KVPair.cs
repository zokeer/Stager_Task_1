using System;

namespace DomainModel.Models
{
    /// <summary>
    /// Класс представляет собой объекты типа "ключ-значение", позволяет сериализацию.
    /// </summary>
    /// <typeparam name="TK">Класс ключа.</typeparam>
    /// <typeparam name="TV">Класс значение.</typeparam>
    [Serializable]
    public struct KVPair<TK, TV>
    {
        public TK Key { get; set; }
        public TV Value { get; set; }
    }
}