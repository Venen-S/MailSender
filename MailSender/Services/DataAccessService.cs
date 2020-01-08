using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using MailSender.Classes;

namespace MailSender.Services
{
    public interface IDataAccessService //определяет доступ к данным БД
    {
        ObservableCollection<Recipients> GetEmails();
        int CreateEmail(Recipients recipients);
    }

    public class DataAccessService:IDataAccessService
    {
        RecipientDataContext context;

        public DataAccessService()
        {
            context = new RecipientDataContext();
        }

        public ObservableCollection<Recipients> GetEmails()
        {
            ObservableCollection<Recipients> recipients = new ObservableCollection<Recipients>();
            foreach(var item in context.Recipients)
            {
                recipients.Add(item);
            }
            return recipients;
        }

        public int CreateEmail(Recipients recipients)
        {
            context.Recipients.InsertOnSubmit(recipients);
            context.SubmitChanges();
            return recipients.Id;
        }
    }
}
