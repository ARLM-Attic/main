using System.Windows;

namespace SmartPrinter.UI
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = ((App)Application.Current).Shell;
        }
    }
}