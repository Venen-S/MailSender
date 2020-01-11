using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Windows;
using System.Threading;
using System.Collections.Generic;

namespace MailSender.Classes
{
    public class EmailSendServiceClass //класс непосредственно занимается отправкой писем
    {
        #region variables
        private string strLogin;    // email с которого будет рассылка
        private string strPassword; // пароль к email
        private string strSmtp;     // smtp сервер
        private int iSmtpPort;      // порт smtp сервера
        private string strBody;      // текст письма
        private string strSubject;   // тема письма
        #endregion

        //конструктор класса
        public EmailSendServiceClass(string sLogin, string sPassword, string sBody, string sSubject,
            string sSmtp, int sPort)
        {
            strLogin = sLogin;
            strPassword = sPassword;
            strBody = sBody;
            strSubject = sSubject;
            iSmtpPort = sPort;
            strSmtp = sSmtp;
        }

        private void SendMail(string mail, string name)//отправка email конкретном адресатам
        {
            using (MailMessage mm = new MailMessage(strLogin, mail))
            {
                mm.Subject = strSubject;
                mm.Body = strBody + "/nПисьмо от " + DateTime.Now;
                mm.IsBodyHtml = false;
                SmtpClient sc = new SmtpClient(strSmtp, iSmtpPort);
                sc.EnableSsl = true;
                sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                sc.UseDefaultCredentials = false;
                sc.Credentials = new NetworkCredential(strLogin, strPassword);
                try
                {
                    sc.Send(mm);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Невозможно отправить письмо " + ex.ToString());
                }
            }
        }

        private async Task SendMailAsync(string mail, string name) // асинхронный метод отправки писем
        {
            using (MailMessage mm = new MailMessage(strLogin, mail))
            {
                mm.Subject = strSubject;
                mm.Body = strBody + "\nПисьмо от " + DateTime.Now;
                mm.IsBodyHtml = false;
                SmtpClient sc = new SmtpClient(strSmtp, iSmtpPort);
                sc.EnableSsl = true;
                sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                sc.UseDefaultCredentials = false;
                sc.Credentials = new NetworkCredential(strLogin, strPassword);
                try
                {
                    await sc.SendMailAsync(mm).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Невозможно отправить письмо " + ex.ToString());
                }
            }
        }

        public void SendMails(ObservableCollection<Recipients> emails) //проходим по БД и вызываем SendMail
        {
            foreach (Recipients email in emails)
            {
                SendMail(email.Address, email.Name);
            }
        }

        //далее идут асинхронные и паралельные методы SendMails
        public void SendMailsParallel(ObservableCollection<Recipients> emails)
        {
            foreach (Recipients email in emails)
            {
                ThreadPool.QueueUserWorkItem(_ => SendMails(emails));
            }
        }

        public async Task SendParallelAsync(ObservableCollection<Recipients> emails)
        {
            var send_task = new List<Task>();
            foreach (Recipients email in emails)
            {
                send_task.Add(SendMailAsync(email.Address, email.Name));
            }
            await Task.WhenAll(send_task).ConfigureAwait(false);
        }

        public async Task SendMailsAsync(ObservableCollection<Recipients> emails)
        {
            foreach (Recipients email in emails)
            {
                await SendMailAsync(email.Address, email.Name).ConfigureAwait(false);
            }
        }
    }
}
