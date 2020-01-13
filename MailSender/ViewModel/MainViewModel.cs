using GalaSoft.MvvmLight;
using MailSender.Services;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using System.Windows.Data;
using System.ComponentModel;
using Common;

namespace MailSender.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        ObservableCollection<Recipient> _Emails;
        public ObservableCollection<Recipient> Emails
        {
            get { return _Emails; }
            set
            {
                if (!Set(ref _Emails, value)) return;
                _emailView = new CollectionViewSource { Source = value };
                _emailView.Filter += OnEmailsCollectionViewSourceFilter;
                RaisePropertyChanged(nameof(EmailsView));
            }
        }

        private void OnEmailsCollectionViewSourceFilter(object sender, FilterEventArgs e)
        {
            if (!(e.Item is Recipient email) || string.IsNullOrWhiteSpace(filtName)) return;
            if (!email.Name.Contains(filtName))
                e.Accepted = false;
        }

        IDataAccessService _serviceProxy;
        private void GetEmails() => Emails = _serviceProxy.GetEmails();

        public RelayCommand ReadAllCommand { get; set; }

        public MainViewModel(IDataAccessService servProxy)
        {
            _serviceProxy = servProxy;
            Emails = new ObservableCollection<Recipient>();
            EmailInfo = new Recipient();

            ReadAllCommand = new RelayCommand(GetEmails);
            SaveCommand = new RelayCommand<Recipient>(SaveEmail);
            DeleteCommand = new RelayCommand<Recipient>(DeleteEmail);
        }

        Recipient _EmailInfo;
        public Recipient EmailInfo
        {
            get { return _EmailInfo; }
            set
            {
                _EmailInfo = value;
                RaisePropertyChanged(nameof(EmailInfo));
            }
        }

        public void SaveEmail(Recipient email)
        {
            EmailInfo.Id = _serviceProxy.CreateEmail(email);
            if (EmailInfo.Id != 0)
            {
                Emails.Add(EmailInfo);
                RaisePropertyChanged(nameof(EmailInfo));
            }
        }

        public void DeleteEmail(Recipient email)
        {
            if (EmailInfo.Id != 0)
            {
                _serviceProxy.DeleteEmail(EmailInfo.Id);
                Emails.Remove(EmailInfo);
                RaisePropertyChanged(nameof(EmailsView));
            }
        }


        public RelayCommand<Recipient> SaveCommand { get; set; }
        public RelayCommand<Recipient> DeleteCommand { get; set; }

        private string filtName;
        public string FilterName
        {
            get => filtName;
            set
            {
                if (!Set(ref filtName, value)) return;
                EmailsView.Refresh();
            }
        }
        private CollectionViewSource _emailView;
        public ICollectionView EmailsView => _emailView?.View;
    }
}