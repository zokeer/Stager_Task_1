using System.Text;

namespace DomainModel.Models
{
    /// <summary>
    /// Класс хранит логирование процесса создания подсети.
    /// Его поля показывают какое поле соотвествует требованиям.
    /// </summary>
    public class ValidationLog
    {
        /// <summary>
        /// Поле Subnet, над которым была произведена операция.
        /// </summary>
        public SubnetField Field { get; set; }
        /// <summary>
        /// Информация относительно Field, полученная в ходе операции.
        /// </summary>
        public LogInfo LogInfo { get; set; }

        public ValidationLog(SubnetField field, LogInfo log_info)
        {
            Field = field;
            LogInfo = log_info;
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            switch (Field)
            {
                case SubnetField.Id:
                    result.Append("Идентификатор");
                    break;
                case SubnetField.Address:
                    result.Append("Адрес");
                    break;
                case SubnetField.Mask:
                    result.Append("Маска");
                    break;
                case SubnetField.Everything:
                    result.Append("Всё");
                    break;
            }
            switch (LogInfo) {
                case LogInfo.NotUnique:
                    result.Append(" не уникален.");
                    break;
                case LogInfo.NotExists:
                    result.Append(" не существует.");
                    break;
                case LogInfo.Invalid:
                    result.Append(" содержит ошибку.");
                    break;
                case LogInfo.NoErrors:
                    result.Append(" в порядке.");
                    break;
            }
            return result.ToString();
        }
    }
}