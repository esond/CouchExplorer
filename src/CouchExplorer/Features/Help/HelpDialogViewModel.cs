using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CouchExplorer.Features.Help
{
    public class HelpDialogViewModel : ViewModelBase
    {
        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }
}
