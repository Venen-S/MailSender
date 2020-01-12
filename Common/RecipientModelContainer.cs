using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Common
{
    public partial class RecipientModelContainer:DbContext
    {
        public RecipientModelContainer() : base("MailSenderDB") { }
        public virtual DbSet<Recipient> Recipients { get; set; }
    }
}
