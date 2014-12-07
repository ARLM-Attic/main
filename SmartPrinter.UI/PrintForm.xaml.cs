using System.Windows.Input;

namespace SmartPrint.UI
{
    public partial class PrintForm
    {
        public PrintForm()
        {
            InitializeComponent();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
