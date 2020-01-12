using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Common;
using System.Windows;

namespace MailSender.Services
{
    public interface IDataAccessService //определяет доступ к данным БД
    {
        ObservableCollection<Recipient> GetEmails();
        int CreateEmail(Recipient recipients);
        int DeleteEmail(Recipient recipients);
    }

    public class DataAccessService:IDataAccessService
    {
        RecipientModelContainer context;

        public DataAccessService()
        {
            context = new RecipientModelContainer();
        }

        public ObservableCollection<Recipient> GetEmails()
        {
            ObservableCollection<Recipient> Recipients = 
                new ObservableCollection<Recipient>();
            foreach(var item in context.Recipients)
            {
                Recipients.Add(item);
            }
            return Recipients;
        }

        public int CreateEmail(Recipient recipients)
        {
            context.Recipients.Add(recipients);
            context.SaveChanges();
            return recipients.Id;
        }

        public int DeleteEmail(Recipient recipients)
        {
            context.Recipients.Remove(recipients);
            context.SaveChanges();
            return recipients.Id;
        }
    }
}
