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
        private string strAttachFile; //путь до файла
        #endregion

        
        //конструктор класса для всех
        public EmailSendServiceClass(string sLogin, string sPassword, string sBody, string sSubject,
            string sSmtp, int sPort, string sAttachFile)
        {
            strLogin = sLogin;
            strPassword = sPassword;
            strBody = sBody;
            strSubject = sSubject;
            iSmtpPort = sPort;
            strSmtp = sSmtp;
            strAttachFile = sAttachFile;
        }

        private (bool,string) SendMail(string mail, string name, ref bool flag, ref string except)
            //отправка email адресатам
            //передаем имя, мыло, флаг и строку для ошибки
        {
            
            using (MailMessage mm = new MailMessage(strLogin, mail))
            {
                (bool, string) cort=(false,string.Empty);
                mm.Subject = strSubject;
                mm.Body = strBody + "\nПисьмо от " + DateTime.Now;
                mm.IsBodyHtml = false;
                if(strAttachFile!="...")
                    mm.Attachments.Add(new Attachment(strAttachFile));
                SmtpClient sc = new SmtpClient(strSmtp, iSmtpPort);
                sc.EnableSsl = true;
                sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                sc.UseDefaultCredentials = false;
                sc.Credentials = new NetworkCredential(strLogin, strPassword);
                try
                {
                    sc.Send(mm);
                    flag = true; //если все норм флаг истина 
                    return cort; //возвращаем кортеж с флагом и пустой строкой
                }
                catch (Exception ex)
                {
                    flag = false; 
                    except = ex.ToString();//присваимваем строке для кортежа запись ошибки
                    return cort;//возвращаем флаг с ошибкой в кортеже в SendMails
                }
            }
        }

        public void SendMails(ObservableCollection<Common.Recipient> emails) //проходим по БД и вызываем SendMail
        {
            bool flag=true; //ставлю дефолт истина т.к. априори прога должна отработать без проблем
            string except = string.Empty;
            foreach (Common.Recipient email in emails)
            {
                Task.Factory.StartNew(() => SendMail(email.Address, email.Name, ref flag, ref except));
                //адрес и нэйм для отправки писем, 
                //флаг и строку для проверки как отработано
                //SendMail(email.Address, email.Name,ref flag, ref except); //мононопоточное выполнение
            }

            if (flag == true)//если все норм единожды выскочит окно что все гуд
            {
                View.SendEndWindow sendEnd = new View.SendEndWindow();
                sendEnd.ShowDialog();
            }
            else if (flag == false)//если не норм то обложит матами, но единожды xD
            {
                View.ErorrWindow erorrWindow = new View.ErorrWindow();
                erorrWindow.ShowDialog();
                MessageBox.Show(except.ToString());
            }
        }

        
    }
}
