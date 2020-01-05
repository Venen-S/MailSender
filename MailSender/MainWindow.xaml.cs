using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Mail;
using MailSender.Classes;

namespace MailSender
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            cbSenderSelect.ItemsSource = VariablesClass.Senders;
            cbSenderSelect.DisplayMemberPath = "Key";
            cbSenderSelect.SelectedValuePath = "Value";
            cbSmtpSelect.ItemsSource = VariablesSmtp.Smtpserv;
            cbSmtpSelect.DisplayMemberPath = "Key";
            cbSmtpSelect.SelectedValue = "Value";
            DBClass db = new DBClass();
            dgEmails.ItemsSource = db.Emails;
        }

        private void ExitMenuItem_OnClick(object sender, RoutedEventArgs e) //выход из проги
        {
            Close();
        }

        private void BtnClock_Click(object sender, RoutedEventArgs e) //переход в планировщик
        {
            tbConrol.SelectedItem = tbPlanner;
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e) //планировщик
        {
            SchedulerClass sc = new SchedulerClass();
            TimeSpan tsSendTime = sc.GetSendTime(tbTimePicker.Text);
            if(tsSendTime==new TimeSpan())
            {
                MessageBox.Show("Некорректный формат даты");
                return;
            }
            DateTime dtSendDateTime = (cldSchedulDateTimes.SelectedDate 
                ?? DateTime.Today).Add(tsSendTime);
            if(dtSendDateTime<DateTime.Now)
            {
                MessageBox.Show("Дата и время не могут быть раньше, чем настоящее время");
                return;
            }
            EmailSendServiceClass emailSender = new EmailSendServiceClass(cbSenderSelect.Text,
                cbSenderSelect.SelectedValue.ToString(), BodyPost.Text, SubjectPost.Text, cbSmtpSelect.Text,
                int.Parse(((KeyValuePair<string, int>)cbSenderSelect.SelectedItem).Value.ToString()));
            sc.SendEmails(dtSendDateTime, emailSender, (IQueryable<Recipients>)dgEmails.ItemsSource);
        }

        private void BtnSendAtOnce_Click(object sender, RoutedEventArgs e) //отправить сейчас
        {
            string strBody = BodyPost.Text;
            string strSubject = SubjectPost.Text;
            string strLogin = cbSenderSelect.Text;
            string strPassword = cbSenderSelect.SelectedValue.ToString();
            string smtpServ = cbSmtpSelect.Text;
            int sPort = int.Parse(((KeyValuePair<string, int>)cbSmtpSelect.SelectedItem).Value.ToString());
            if(string.IsNullOrEmpty(strLogin))
            {
                MessageBox.Show("Выберите отправителя");
                return;
            }
            if(string.IsNullOrEmpty(strPassword))
            {
                MessageBox.Show("Укажите пароль отправителя");
                return;
            }
            if(string.IsNullOrEmpty(strBody))
            {
                MessageBox.Show("Письмо не заполнено");
                return;
            }
            EmailSendServiceClass emailSender=new EmailSendServiceClass(strLogin, strPassword,
                strBody, strSubject, smtpServ, sPort);
            emailSender.SendMails((IQueryable<Recipients>)dgEmails.ItemsSource);
        }

        private void TabSwitcher_Back(object sender, RoutedEventArgs e)
        {
            if (tbConrol.SelectedIndex == 0) return;
            tbConrol.SelectedIndex--;
        }

        private void TabSwitcher_Forward(object sender, RoutedEventArgs e)
        {
            if (tbConrol.SelectedIndex == tbConrol.Items.Count - 1) return;
            tbConrol.SelectedIndex++;
        }
    }
}
