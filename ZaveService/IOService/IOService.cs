using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using Microsoft.Win32;


namespace ZaveService.IOService
{
    public interface IIOService
    {
        string OpenFileDialogService(string defaultPath);
        string SaveFileDialogService(string defaultPath);
        
        StreamReader OpenFileService(string path);
        StreamWriter SaveFileService(string path);
    }

    public class IOService : IIOService
    {
        public StreamReader OpenFileService(string path)
        {
            var sr = ZaveGlobalSettings.ZaveFile.StreamReaderFactory.createStreamReader(path);
            return sr;
        }

        public string OpenFileDialogService(string defaultPath)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Zave Document Format (*.zdf)|*.zdf";
            if (open.ShowDialog() == true)
            {
                return open.FileName;
            }

            return null;
        }

        public StreamWriter SaveFileService(string path)
        {
            StreamWriter sw = ZaveGlobalSettings.ZaveFile.StreamWriterFactory.createStreamWriter(path);
            return sw;
        }

        public string SaveFileDialogService(string defaultPath)
        {
            var save = new SaveFileDialog();
            
            save.Filter = "Zave Document Format (*.zdf)|*.zdf";
            if (save.ShowDialog() == true)
            {
                return save.FileName;
                
            }
            return null;
        }
    }
}
