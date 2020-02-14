using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Windows;
using System.Collections.ObjectModel;
using System.Threading;
using MailSender.View;

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
        public (bool, string) SendMail(ref bool flag, ref string except)
        {
            using (MailMessage mm = new MailMessage(strLogin, strRecipient))
            {
                (bool, string) cort = (false, string.Empty);
                mm.Subject = strSubject;
                mm.Body = strBody + "\nПисьмо от " + DateTime.Now;
                mm.IsBodyHtml = false;
                if (strAttachFile != ". . .")
                    mm.Attachments.Add(new Attachment(strAttachFile));
                SmtpClient sc = new SmtpClient(strSmtp, iSmtpPort);
                sc.EnableSsl = true;
                sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                sc.UseDefaultCredentials = false;
                sc.Credentials = new NetworkCredential(strLogin, strPassword);
                try
                {
                    sc.Send(mm);
                    flag = true;
                    return cort;
                }
                catch (Exception ex)
                {
                    flag = false;
                    except = ex.ToString();//присваимваем строке для кортежа запись ошибки
                    return cort;//возвращаем флаг с ошибкой в кортеже в SendMails
                }
            }
        }

        public void SendMails()
            //в этом методе иной мотив чем отправить для всех.
            //если логику ту что в if else запихнуть в блок try catch метода SendMail то,
            //будет вылетать исключение на блок catch,
            //поэтому дабы устранить эту проблему сделал так.
        {
            bool flag = true; 
            string except = string.Empty;
            Task.Factory.StartNew(() => SendMail(ref flag, ref except));
            if (flag == true)//если все норм выскочит окно что все гуд
            {
                SendEndWindow sendEnd = new SendEndWindow();
                sendEnd.ShowDialog();
            }
            else if (flag == false)//если не норм то обложит матами
            {
                ErorrWindow erorrWindow = new ErorrWindow();
                erorrWindow.NotSend.Text = "Ошибка при отправке письма \n" + except.ToString();
                erorrWindow.ShowDialog();
            }
        }
    }
}