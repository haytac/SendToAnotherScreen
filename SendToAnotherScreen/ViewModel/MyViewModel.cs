using SendToAnotherScreen.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Input;

namespace SendToAnotherScreen.ViewModel
{
    sealed class MyViewModel : INotifyPropertyChanged
    {
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        private ObservableCollection<Process> _processes;
        public ObservableCollection<Process> Processes
        {
            get { return _processes; }
            set
            {
                _processes = value;
                OnPropertyChange("Processes");
            }
        }
        public ICommand SendFirstCommand { get; set; }
        public ICommand SendSecondCommand { get; set; }
        public MyViewModel()
        {
            ListProcesses();
            InitTimer();
            SendFirstCommand = new RelayCommand(SendFirstScreen, param => true);
            SendSecondCommand = new RelayCommand(SendSecondScreen, param => true);
        }

        public void ListProcesses()
        {
            Processes = new ObservableCollection<Process>(Process.GetProcesses().Where(w => !String.IsNullOrEmpty(w.MainWindowTitle)));
        }

        public void SendFirstScreen(object obj)
        {
            SendToScreen((Process)obj, 0);
        }
        public void SendSecondScreen(object obj)
        {
            SendToScreen((Process)obj, 1);
        }
        public void SendToScreen(Process process, int scrInd)
        {
            var m = FindWindow(process.ProcessName, process.MainWindowTitle);
            IntPtr handle = process.MainWindowHandle;
            Rectangle screen = Screen.AllScreens[scrInd].WorkingArea;
            SetWindowPos(handle, 0, screen.Left, screen.Top, screen.Width, screen.Height, 0);
        }

        #region Timer Stuffs
        private Timer timer1;
        public void InitTimer()
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 2000; // in miliseconds
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ListProcesses();
        }

        #endregion

        #region Mvvm sutffs
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

    }
}
