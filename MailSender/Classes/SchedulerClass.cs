using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MailSender.Classes
{
    /// <summary>
    /// Класс-планировщик, который создает расписание, следит за его выполнением и
    /// напоминает о событиях
    /// Также помогает автоматизировать рассылку писем в соответствии с расписанием
    /// </summary>
    class SchedulerClass
    {
        DispatcherTimer timer = new DispatcherTimer(); //таймер
        EmailSendServiceClass emailSender;             //экземпл. кл. отвечающего за отправку
        DateTime dtSend;                               //дата и время отправки
        ObservableCollection<Recipients> emails;       //коллекция адресов

        /// <summary>
        /// Метод, который превращает строку из текстбокса tbTimePicker в TimeSpan
        /// </summary>
        /// <param name="strSendTime"></param>
        /// <returns></returns>
        public TimeSpan GetSendTime(string strSendTime)
        {
            TimeSpan tsSendTime = new TimeSpan();
            try
            {
                tsSendTime = TimeSpan.Parse(strSendTime);
            }
            catch { }
            return tsSendTime;
        }

        /// <summary>
        //// Непосредственно отправка писем
        /// </summary>
        /// <param name="dtSend"></param>
        /// <param name="emailSender"></param>
        /// <param name="emails"></param>
        public void SendEmails(DateTime dtSend,EmailSendServiceClass emailSender,
            ObservableCollection<Recipients> emails)
        {
            this.emailSender = emailSender;
            this.dtSend = dtSend;
            this.emails = emails;
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if(dtSend.ToShortTimeString()==DateTime.Now.ToShortTimeString())
            {
                emailSender.SendMails(emails);
                timer.Stop();
                MessageBox.Show("Письма отправлены");
            }
        }
    }
}
