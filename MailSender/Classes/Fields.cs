using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSender.Classes
{
    public class Fields
    {
        public string Login { get; set; }    
        public string Password { get; set; }
        public string Smtp { get; set; }
        public int SmtpPort { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string AttachFile { get; set; }
        public string Recipient { get; set; }
    }
}
