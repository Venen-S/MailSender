using GalaSoft.MvvmLight;
using MailSender.Services;
using MailSender.Classes;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;

namespace MailSender.ViewModel
{
    
    public class MainViewModel : ViewModelBase
    {
        //логика чтени€ данных о получател€х почты из таблицы
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
        //тут данные читаютс€ из Ѕƒ и помещаютс€ в наблюдаемую коллекцию
        void GetEmails()
        {
            Recipients.Clear();
            foreach (var item in _serviceProxy.GetEmails())
            {
                Recipients.Add(item);
            }
        }

        public RelayCommand ReadAllCommand { get; set; }

        public MainViewModel(IDataAccessService servProxy)
        {
            _serviceProxy = servProxy;
            Recipients = new ObservableCollection<Recipients>();
            ReadAllCommand = new RelayCommand(GetEmails);
        }
    }
}