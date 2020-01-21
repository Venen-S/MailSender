using Common;
using System;
using System.Collections.Generic;
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
    public class SchedulerClass
    {

        DispatcherTimer timer = new DispatcherTimer(); //таймер
        EmailSendServiceClass emailSender;             //экземпл. кл. отвечающего за отправку
        DateTime dtSend;                               //дата и время отправки
        ObservableCollection<Common.Recipient> emails;       //коллекция адресов

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
            dtSend = DateTime.Parse(tsSendTime.ToString());
            return tsSendTime;
        }

        /// <summary>
        /// Непосредственно отправка писем
        /// </summary>
        public void SendEmails(EmailSendServiceClass emailSender,
            ObservableCollection<Common.Recipient> emails)
        {
            this.emailSender = emailSender;
            this.emails = emails;
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        //private void Timer_Tick(object sender, EventArgs e)
        //{
        //    if (dicDates.Count == 0)
        //    {
        //        timer.Stop();
        //        MessageBox.Show("Письма отправлены");
        //    }
        //    else if (dicDates.Keys.First<DateTime>().ToShortTimeString() == DateTime.Now.ToShortTimeString())
        //    {
        //        emailSender.strBody = dicDates[dicDates.Keys.First<DateTime>()];
        //        emailSender.strSubject = $"Рассылка от {dicDates.Keys.First<DateTime>().ToShortTimeString()}";
        //        emailSender.SendMails(emails);
        //        dicDates.Remove(dicDates.Keys.First<DateTime>());
        //    }
        //}


        private void Timer_Tick(object sender, EventArgs e)
        {
            if (dtSend.ToString() == DateTime.Now.ToString())
            {
                emailSender.SendMails(emails);
                timer.Stop();
                MessageBox.Show("Письма отправлены");
            }
        }


        //тупо для unit теста, черт ногу сломит, какая та магия
        //Dictionary<DateTime, string> dicDates = new Dictionary<DateTime, string>();
        //public Dictionary<DateTime,string> DatesEmailTexts
        //{
        //    get { return dicDates; }
        //    set
        //    {
        //        dicDates = value;
        //        dicDates = dicDates.OrderBy(pair => pair.Key).ToDictionary(pair =>
        //            pair.Key, pair => pair.Value);
        //    }
        //}
    }
}
