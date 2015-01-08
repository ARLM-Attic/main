using SmartPrint.Model;
using SmartPrint.UI;

namespace SmartPrinter.UI.Commands
{
    public class PrintCommand : BaseCommand
    {
        public override void Execute(object parameter)
        {
            var pf = new PrintForm();
            
            var vm = new PrintFormVM();
            
            pf.DataContext = vm;
            
            pf.ShowDialog();

            pf.Topmost = true;

            // TODO: when dialog is closed delete PostScript file from Temp folder
        }
    }
}