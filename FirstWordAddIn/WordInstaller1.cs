using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace FirstWordAddIn
{
    [RunInstaller(true)]
    public partial class WordInstaller1 : System.Configuration.Install.Installer
    {
        public WordInstaller1()
        {
            InitializeComponent();
            System.Diagnostics.Process.Start("FirstWordAddIn.vsto");
        }
    }
}
