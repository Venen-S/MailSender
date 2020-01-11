using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSender.Classes
{
    /// <summary>
    /// Класс отвечает за работу БД
    /// </summary>
    class DBClass
    {
        private RecipientDataContext emails = new RecipientDataContext();
        public IQueryable<Recipients> Emails
        {
            get
            {
                return from c in emails.Recipients select c;
            }
        }
    }
}
