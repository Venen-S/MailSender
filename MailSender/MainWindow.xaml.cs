using System;
using System.Collections.ObjectModel;
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
using MailSender.ViewModel;
using Microsoft.Win32;
using System.IO;

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
        }

        private void ExitMenuItem_OnClick(object sender, RoutedEventArgs e) //выход из проги
        {
            Close();
        }


        private void BtnSend_Click(object sender, RoutedEventArgs e) //планировщик
        {
            string strAttachFile = tbAttachFileWay.Text; //файл для отправки
            string strBody = BodyPost.Text;//тело письма
            string strSubject = SubjectPost.Text;//тема письма
            string strLogin = cbSenderSelect.Text;//логин
            string strPassword = cbSenderSelect.SelectedValue.ToString();//пароль
            string smtpServ = cbSmtpSelect.Text;//смтп сервер
            int sPort = int.Parse(((KeyValuePair<string, int>)cbSmtpSelect.SelectedItem).Value.ToString());//порт смпт сервера
            SchedulerClass sc = new SchedulerClass();
            TimeSpan tsSendTime = sc.GetSendTime(TimePicker.Text);
            if (tsSendTime==new TimeSpan())
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
            EmailSendServiceClass emailSender = new EmailSendServiceClass(strLogin, strPassword,
                strBody, strSubject, smtpServ, sPort, strAttachFile);
            var locator = (ViewModelLocator)FindResource("Locator");
            sc.SendEmails(emailSender, locator.Main.Emails);
            
        }

        private void BtnSendAtOnce_Click(object sender, RoutedEventArgs e) //отправить сейчас
        {
            string strAttachFile = tbAttachFileWay.Text; //файл для отправки
            string strBody = BodyPost.Text;//тело письма
            string strSubject = SubjectPost.Text;//тема письма
            string strLogin = cbSenderSelect.Text;//логин
            string strPassword = cbSenderSelect.SelectedValue.ToString();//пароль
            string smtpServ = cbSmtpSelect.Text;//смтп сервер
            int sPort = int.Parse(((KeyValuePair<string, int>)cbSmtpSelect.SelectedItem).Value.ToString());//порт смпт сервера
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
                strBody, strSubject, smtpServ, sPort, strAttachFile);
            var locator = (ViewModelLocator)FindResource("Locator");
            emailSender.SendMails(locator.Main.Emails);
        }

        private void BtnSendOne_Click(object sender, RoutedEventArgs e)
        {
            string strAttachFile = tbAttachFileWay.Text; //файл для отправки
            string strBody = BodyPost.Text;//тело письма
            string strSubject = SubjectPost.Text;//тема письма
            string strLogin = cbSenderSelect.Text;//логин
            string strPassword = cbSenderSelect.SelectedValue.ToString();//пароль
            string smtpServ = cbSmtpSelect.Text;//смтп сервер
            string strRecipient = saveEmail.RecipientEmailAddress.ToString();//Берем выделенного получателя для отправки
            int sPort = int.Parse(((KeyValuePair<string, int>)cbSmtpSelect.SelectedItem).Value.ToString());//порт смпт сервера
            if (string.IsNullOrEmpty(strLogin))
            {
                MessageBox.Show("Выберите отправителя");
                return;
            }
            if (string.IsNullOrEmpty(strPassword))
            {
                MessageBox.Show("Укажите пароль отправителя");
                return;
            }
            if (string.IsNullOrEmpty(strBody))
            {
                MessageBox.Show("Письмо не заполнено");
                return;
            }
            EmailSendServiceClassToOne emailSenderToOne = new EmailSendServiceClassToOne(strLogin, strPassword,
                strBody, strSubject, smtpServ, sPort, strAttachFile, strRecipient);
            emailSenderToOne.SendMail();
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

        private void FileAttach_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            FileInfo fileInfo = new FileInfo(ofd.FileName.ToString());
            if(fileInfo.Length> 105000000)
            {
                MessageBox.Show("Файл слишком большой. выберите файл не превышающий 100МБ");
                ofd = null;
                return;
            }
            else
            {
                tbAttachFileWay.Text = ofd.FileName.ToString();
            }
        }
    }
}
