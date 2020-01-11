using GalaSoft.MvvmLight;
using MailSender.Services;
using MailSender.Classes;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using System.Windows.Data;
using System.ComponentModel;

namespace MailSender.ViewModel
{
    
    public class MainViewModel : ViewModelBase
    {
        //логика чтения данных о получателях почты из таблицы и фильтр
        ObservableCollection<Recipients> _Recipients;
        public ObservableCollection<Recipients> Recipients
        {
            get { return _Recipients; }
            set
            {
                if (!Set(ref _Recipients, value)) return;
                _emailView = new CollectionViewSource { Source = value };
                _emailView.Filter+= OnEmailsCollectionViewSourceFilter;
                RaisePropertyChanged(nameof(EmailsView));
            }
        }

        private string filtName;
        public string FiltName
        {
            get => filtName;
            set
            {
                if (!Set(ref filtName, value)) return;
                EmailsView.Refresh();
            }
        }

        //логика фильтра имен
        private void OnEmailsCollectionViewSourceFilter(object sender, FilterEventArgs e)
        {
            if (!(e.Item is Recipients recipients) || string.IsNullOrWhiteSpace(filtName)) return;
            if (!recipients.Name.Contains(filtName))
                e.Accepted = false;
        }



        IDataAccessService _serviceProxy;
        //тут данные читаются из БД и помещаются в наблюдаемую коллекцию
        private void GetEmails() => Recipients = _serviceProxy.GetEmails();

        //Передача данных из View в ViewModel с записью в БД (см. класс DataAccessService)
        Recipients _RecipienstInfo;
        public Recipients RecipientsInfo
        {
            get { return _RecipienstInfo; }
            set
            {
                _RecipienstInfo = value;
                RaisePropertyChanged(nameof(RecipientsInfo));
            }
        }

        //Сохранение адреса
        void SaveEmail(Recipients recipients)
        {
            RecipientsInfo.Id = _serviceProxy.CreateEmail(recipients);
            if(RecipientsInfo.Id!=0)
            {
                Recipients.Add(RecipientsInfo);
                RaisePropertyChanged(nameof(RecipientsInfo));
            }
        }

        public void DeleteEmail(Recipients recipients)
        {
            RecipientsInfo.Id = _serviceProxy.DeleteEmail(recipients);
            if(RecipientsInfo.Id!=0)
            {
                Recipients.Remove(RecipientsInfo);
                RaisePropertyChanged(nameof(RecipientsInfo));
            }
        }

        public RelayCommand ReadAllCommand { get; set; }
        public RelayCommand<Recipients> SaveCommand { get; set; }
        public RelayCommand<Recipients> DeleteCommand { get; set; }

        public MainViewModel(IDataAccessService servProxy)
        {
            _serviceProxy = servProxy;
            Recipients = new ObservableCollection<Recipients>();
            RecipientsInfo = new Recipients();
            ReadAllCommand = new RelayCommand(GetEmails);
            SaveCommand = new RelayCommand<Recipients>(SaveEmail);
            DeleteCommand = new RelayCommand<Recipients>(DeleteEmail);
        }

        private CollectionViewSource _emailView;
        public ICollectionView EmailsView => _emailView?.View;
    }
}