using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

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
                    result.Append("ID");
                    break;
                case SubnetField.Address:
                    result.Append("Address");
                    break;
                case SubnetField.Mask:
                    result.Append("Mask");
                    break;
                case SubnetField.Everything:
                    result.Append("Everything");
                    break;
            }
            switch (LogInfo) {
                case LogInfo.NotUnique:
                    result.Append(" must be unique.");
                    break;
                case LogInfo.NotExists:
                    result.Append(" does not exist.");
                    break;
                case LogInfo.Invalid:
                    result.Append(" is invalid.");
                    break;
                case LogInfo.NoErrors:
                    result.Append(" is fine.");
                    break;
            }
            return result.ToString();
        }
    }
}