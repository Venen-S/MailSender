using System;
using System.Net;
using System.Net.Mail;
using System.Windows;

namespace MailSender.Classes
{
    public class EmailSendServiceClassToOne
    {
        private string strLogin;    // email с которого будет рассылка
        private string strPassword; // пароль к email
        private string strSmtp;     // smtp сервер
        private int iSmtpPort;      // порт smtp сервера
        private string strBody;      // текст письма
        private string strSubject;   // тема письма
        private string strAttachFile; //путь до файла
        private string strRecipient; //получатель

        //конструктор класса
        public EmailSendServiceClassToOne(string sLogin, string sPassword, string sBody, string sSubject,
            string sSmtp, int sPort, string sAttachFile, string strRecipient)
        {
            strLogin = sLogin;
            strPassword = sPassword;
            strBody = sBody;
            strSubject = sSubject;
            iSmtpPort = sPort;
            strSmtp = sSmtp;
            strAttachFile = sAttachFile;
            this.strRecipient = strRecipient;
        }

        /// <summary>
        /// отправка одному
        /// </summary>
        public void SendMail()
        {
            using (MailMessage mm = new MailMessage(strLogin, strRecipient))
            {
                mm.Subject = strSubject;
                mm.Body = strBody + "\nПисьмо от " + DateTime.Now;
                mm.IsBodyHtml = false;
                if (strAttachFile != "...")
                    mm.Attachments.Add(new Attachment(strAttachFile));
                SmtpClient sc = new SmtpClient(strSmtp, iSmtpPort);
                sc.EnableSsl = true;
                sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                sc.UseDefaultCredentials = false;
                sc.Credentials = new NetworkCredential(strLogin, strPassword);
                try
                {
                    sc.Send(mm);
                    View.SendEndWindow sendEnd = new View.SendEndWindow();
                    sendEnd.ShowDialog();
                }
                catch (Exception ex)
                {
                    View.ErorrWindow erorrWindow = new View.ErorrWindow();
                    erorrWindow.NotSend.Text = "Ошибка при отправке письма \n"+ex.ToString();
                    erorrWindow.ShowDialog();
                }
            }
        }
    }
}