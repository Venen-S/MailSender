using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSender.Classes
{
    /// <summary>
    /// Пользовательская валидация данных в поле Email
    /// </summary>
    public partial class Recipients : INotifyPropertyChanging, INotifyPropertyChanged, IDataErrorInfo
    {
        public string Error
        {
            get
            {
                return "";
            }
        }
        public string this[string propertyName]
        {
            get
            {
                if (propertyName == "Address")
                {
                    if (_Address != null && _Address.Length < 4) return "Необходимо указать правильный e-mail!";
                }
                return "";
            }
        }
    }
}
