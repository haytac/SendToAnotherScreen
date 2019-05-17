using SendToAnotherScreen.ViewModel;
using WinForms = System.Windows.Forms;
using System.Windows;
using System.Windows.Controls;

namespace SendToAnotherScreen
{
    public partial class MainWindow : Window
    {
        private readonly MyViewModel _viewModel;
        private WinForms.NotifyIcon notifier = new WinForms.NotifyIcon();


        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MyViewModel();
            DataContext = _viewModel;
            
        }
        
    }
}
