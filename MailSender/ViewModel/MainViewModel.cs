using GalaSoft.MvvmLight;
using MailSender.Services;
using MailSender.Classes;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;

namespace MailSender.ViewModel
{
    
    public class MainViewModel : ViewModelBase
    {
        //������ ������ ������ � ����������� ����� �� �������
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
        //��� ������ �������� �� �� � ���������� � ����������� ���������
        void GetEmails()
        {
            Recipients.Clear();
            foreach (var item in _serviceProxy.GetEmails())
            {
                Recipients.Add(item);
            }
        }

        //�������� ������ �� View � ViewModel � ������� � �� (��. ����� DataAccessService)
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

        //���������� ������
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