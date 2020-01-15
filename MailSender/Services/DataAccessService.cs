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
        void DeleteEmail(int id);
        void EditEmail(Recipient recipients, int id);
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


        /// <summary>
        /// метод создания нового получателя
        /// </summary>
        /// <param name="recipients"></param>
        /// <returns></returns>
        public int CreateEmail(Recipient recipients)
        {
            context.Recipients.Add(recipients);
            context.SaveChanges();
            return recipients.Id;
        }

        /// <summary>
        /// метод удаления получателя
        /// </summary>
        /// <param name="id">id получателя которого надо грохнуть</param>
        public void DeleteEmail(int id)
        {
            Recipient delete = context.Recipients.Find(id);
            try
            {
                context.Recipients.Remove(delete);
            }
            catch(ArgumentNullException)
            {
                MessageBox.Show("Удаление возможно только по существующем Id в базе.\n");
                return;
            }
            context.SaveChanges();
        }

        /// <summary>
        /// метод редактирования получателя
        /// </summary>
        /// <param name="recipients">передаем данные получателя</param>
        /// <param name="id">номер id который потом удалим</param>
        /// <returns></returns>
        public void EditEmail(Recipient recipients, int id)
        {
            //тут редактируем получателя.
            //впринципе этот код рабочий, но что то мне подсказывает
            //что копирование кода не лучшая мысль, а так же возможно это
            //говнокод, поэтому это будет закоментино, а код который на
            //мой взгляд будет лучше будет ниже.

            //context.Recipients.Add(recipients);
            //context.SaveChanges();
            //Recipient delete = context.Recipients.Find(id);
            //context.Recipients.Remove(delete);
            //context.SaveChanges();
            //return recipients.Id;

            //а вот и норм код, велосипедов и костылей не придумывал,
            //просто вызвал методы которые и так работают
            //по сути тоже самое что и код выше в коментах, но
            //красивее на вид xD
            var email = recipients;
            CreateEmail(email);
            DeleteEmail(id);
        }
    }
}
