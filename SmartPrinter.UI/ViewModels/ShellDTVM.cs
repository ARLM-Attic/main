using SmartPrint.Model.Repository;
using SmartPrint.Model.ViewModels;

namespace SmartPrinter.UI.ViewModels
{
    public class ShellDTVM : ShellVM
    {
        public ShellDTVM() : base(new DesignTimeRepository())
        {
            Initialize();
        }
    }
}