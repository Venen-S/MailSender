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

namespace MailSender.View
{
    /// <summary>
    /// Логика взаимодействия для TabSwitcher.xaml
    /// </summary>
    public partial class TabSwitcher : UserControl
    {
        public event RoutedEventHandler Back;
        public event RoutedEventHandler Forward;

        public TabSwitcher() => InitializeComponent();

        private void BackvardButton_Click(object sender, RoutedEventArgs e) => Back?.Invoke(this, new RoutedEventArgs());
        private void ForvardButton_Click(object sender, RoutedEventArgs e) => Forward?.Invoke(this, new RoutedEventArgs());
    }
}
