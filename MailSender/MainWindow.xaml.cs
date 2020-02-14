using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
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

        private void BtnSend_Click(object sender, RoutedEventArgs e) // запланировать для всех
        {
            SchedulerClass sc = new SchedulerClass();
            TimeSpan tsSendTime = sc.GetSendTime(TimePicker.Text);
            if (string.IsNullOrEmpty(cbSenderSelect.Text))
            {
                MessageBox.Show("Выберите отправителя");
                return;
            }
            if (string.IsNullOrEmpty(cbSmtpSelect.Text))
            {
                MessageBox.Show("Укажите Smtp сервер");
                return;
            }
            if (string.IsNullOrEmpty(BodyPost.Text))
            {
                MessageBox.Show("Письмо не заполнено");
                return;
            }
            if (tsSendTime == new TimeSpan())
            {
                MessageBox.Show("Некорректный формат даты");
                return;
            }
            DateTime dtSendDateTime = (cldSchedulDateTimes.SelectedDate
                ?? DateTime.Today).Add(tsSendTime);
            if (dtSendDateTime < DateTime.Now)
            {
                MessageBox.Show("Дата и время не могут быть раньше, чем настоящее время");
                return;
            }
            var _field = new Fields()
            {
                AttachFile = tbAttachFileWay.Text,
                Body = BodyPost.Text,
                Subject = SubjectPost.Text,
                Login = cbSenderSelect.Text,
                Password = cbSenderSelect.SelectedValue.ToString(),
                Smtp = cbSmtpSelect.Text,
                SmtpPort = int.Parse(((KeyValuePair<string, int>)cbSmtpSelect.SelectedItem).Value.ToString())
            };
            EmailSendServiceClass emailSender = new EmailSendServiceClass(_field);
            var locator = (ViewModelLocator)FindResource("Locator");
            sc.SendEmails(emailSender, locator.Main.Emails);
        }

        private void BtnSendAtOnce_Click(object sender, RoutedEventArgs e) //отправить сейчас всем
        {
            if (string.IsNullOrEmpty(cbSenderSelect.Text))
            {
                MessageBox.Show("Выберите отправителя");
                return;
            }
            if (string.IsNullOrEmpty(cbSmtpSelect.Text))
            {
                MessageBox.Show("Укажите Smtp сервер");
                return;
            }
            if (string.IsNullOrEmpty(BodyPost.Text))
            {
                MessageBox.Show("Письмо не заполнено");
                return;
            }
            var _field = new Fields()
            {
                AttachFile = tbAttachFileWay.Text,
                Body = BodyPost.Text,
                Subject = SubjectPost.Text,
                Login = cbSenderSelect.Text,
                Password = cbSenderSelect.SelectedValue.ToString(),
                Smtp = cbSmtpSelect.Text,
                SmtpPort = int.Parse(((KeyValuePair<string, int>)cbSmtpSelect.SelectedItem).Value.ToString())
            };
            EmailSendServiceClass emailSender = new EmailSendServiceClass(_field);
            var locator = (ViewModelLocator)FindResource("Locator");
            emailSender.SendMails(locator.Main.Emails);
        }

        private void BtnSendOne_Click(object sender, RoutedEventArgs e) //отправить сейчас одному
        {
            if (string.IsNullOrEmpty(cbSenderSelect.Text))
            {
                MessageBox.Show("Выберите отправителя");
                return;
            }
            if (string.IsNullOrEmpty(cbSmtpSelect.Text))
            {
                MessageBox.Show("Укажите Smtp сервер");
                return;
            }
            if (string.IsNullOrEmpty(BodyPost.Text))
            {
                MessageBox.Show("Письмо не заполнено");
                return;
            }
            if(string.IsNullOrEmpty(saveEmail.RecipientEmailAddress.ToString()))
            {
                MessageBox.Show("Получатель не выбран");
                return;
            }
            var _field = new Fields()
            {
                AttachFile = tbAttachFileWay.Text,
                Body = BodyPost.Text,
                Subject = SubjectPost.Text,
                Login = cbSenderSelect.Text,
                Password = cbSenderSelect.SelectedValue.ToString(),
                Smtp = cbSmtpSelect.Text,
                Recipient = saveEmail.RecipientEmailAddress.ToString(),
                SmtpPort = int.Parse(((KeyValuePair<string, int>)cbSmtpSelect.SelectedItem).Value.ToString())
            };
            EmailSendServiceClassToOne emailSenderToOne = new EmailSendServiceClassToOne(_field);
            emailSenderToOne.SendMails();
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

        private void FileAttach_MouseDoubleClick(object sender, MouseButtonEventArgs e)//Прикрепить файл к рассылке
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

        private void btnClearThePath_Click(object sender, RoutedEventArgs e)//Очистить путь к файлу
        {
            tbAttachFileWay.Text = ". . .";
        }
    }
}
