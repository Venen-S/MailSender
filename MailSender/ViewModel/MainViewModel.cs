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

        Recipient _EmailInfo;
        readonly IDataAccessService _serviceProxy;
        ObservableCollection<Recipient> _Emails;
        private CollectionViewSource _emailView;

        public RelayCommand ReadAllCommand { get; set; }
        public RelayCommand<Recipient> SaveCommand { get; set; }
        public RelayCommand<Recipient> DeleteCommand { get; set; }
        public RelayCommand<Recipient> EditCommand { get; set; }

        public MainViewModel(IDataAccessService servProxy)
        {
            _serviceProxy = servProxy;
            Emails = new ObservableCollection<Recipient>();
            EmailInfo = new Recipient();

            ReadAllCommand = new RelayCommand(GetEmails);
            SaveCommand = new RelayCommand<Recipient>(SaveEmail);
            DeleteCommand = new RelayCommand<Recipient>(DeleteEmail);
            EditCommand = new RelayCommand<Recipient>(EditEmail);
        }

        //эта ерунда для поиска по фильтру
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

        public Recipient EmailInfo
        {
            get { return _EmailInfo; }
            set
            {
                _EmailInfo = value;
                RaisePropertyChanged(nameof(EmailInfo));
            }
        }

        //коллекция получателей + фичи для фильта имен
        public ObservableCollection<Recipient> Emails
        {
            get => _Emails;
            set
            {
                if (!Set(ref _Emails, value)) return;
                _emailView = new CollectionViewSource { Source = value };
                _emailView.Filter += OnEmailsCollectionViewSourceFilter;
                RaisePropertyChanged(nameof(EmailsView));
            }
        }

        //метод фильтра имен
        private void OnEmailsCollectionViewSourceFilter(object sender, FilterEventArgs e)
        {
            if (!(e.Item is Recipient email) || string.IsNullOrWhiteSpace(filtName)) return;
            if (!email.Name.Contains(filtName))
                e.Accepted = false;
        }

        private void GetEmails() => Emails = _serviceProxy.GetEmails();

        //метод сохранения получателя
        public void SaveEmail(Recipient email)
        {

            EmailInfo.Id = _serviceProxy.CreateEmail(email);
            //проверка от "дурака" чтобы не было попыток создать получателя с id 0
            if (EmailInfo.Id != 0)
            {
                GetEmails();
                RaisePropertyChanged(nameof(EmailInfo));
            }

        }

        //метод удаления получателя
        public void DeleteEmail(Recipient email)
        {
            if (EmailInfo.Id != 0)
            {
                _serviceProxy.DeleteEmail(EmailInfo.Id);
                GetEmails();
                RaisePropertyChanged(nameof(EmailsView));
            }
        }

        //метод редактирования получателя
        public void EditEmail(Recipient email)
        {
            if (EmailInfo.Id!=0)
            {
                _serviceProxy.EditEmail(email, EmailInfo.Id);
                GetEmails();
                RaisePropertyChanged(nameof(EmailsView));
            }
        }

        //не спрашивай меня что это, я сам не знаю, пусть будет магией, но
        //это крайне важная вещь для EmailsView, даже не знаю что будет если удалить
        //поэтому просто не трогай, пусть будет, не мешает же
        public ICollectionView EmailsView => _emailView?.View;
    }
}