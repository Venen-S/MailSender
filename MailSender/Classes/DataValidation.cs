using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MailSender.Classes
{
    /// <summary>
    /// Класс для проверки что ID получателя больше 0
    /// </summary>
    public class DataValidation:ValidationRule
    {
        public override ValidationResult Validate(object value,
            System.Globalization.CultureInfo cultureInfo)
        {
            int IntValue = 0;
            ValidationResult result = null;
            try
            {
                IntValue = Convert.ToInt32(value);
            }
            catch (Exception)
            {
                return new ValidationResult(false, "Введите число");
            }
            if (IntValue < 0) return new ValidationResult(false, "Введите число больше 0");
            return new ValidationResult(true, null);
        }
    }
}
