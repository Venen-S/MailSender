using GalaSoft.MvvmLight;
using MailSender.Services;
using MailSender.Classes;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;

namespace MailSender.ViewModel
{
    
    public class MainViewModel : ViewModelBase
    {
        //логика чтения данных о получателях почты из таблицы
        ObservableCollection<Recipients> _Recipients;
        public ObservableCollection<Recipients> Recipients
        {
            get { return _Recipients; }
            set
            {
                _Recipients = value;
                RaisePropertyChanged(nameof(Recipients));
            }
        }

        IDataAccessService _serviceProxy;
        //тут данные читаются из БД и помещаются в наблюдаемую коллекцию
        void GetEmails()
        {
            Recipients.Clear();
            foreach (var item in _serviceProxy.GetEmails())
            {
                Recipients.Add(item);
            }
        }

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

        public RelayCommand ReadAllCommand { get; set; }
        public RelayCommand<Recipients> SaveCommand { get; set; }

        public MainViewModel(IDataAccessService servProxy)
        {
            _serviceProxy = servProxy;
            Recipients = new ObservableCollection<Recipients>();
            ReadAllCommand = new RelayCommand(GetEmails);
            SaveCommand = new RelayCommand<Recipients>(SaveEmail);
        }
    }
}