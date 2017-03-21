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
            if (path != null)
            {
                var sr = ZaveGlobalSettings.ZaveFile.StreamReaderFactory.createStreamReader(path);
                return sr;
            }
            return null;
        }

        public string OpenFileDialogService(string defaultPath)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Zave Document Format (*.zdf)|*.zdf";
            if (open.ShowDialog() == true)
            {
                return open.FileName;
            }

            return String.Empty;
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
            return String.Empty;
        }

        public static async Task CreateFileAsync(string filename)
        {
            await Task.Run(() =>
            {
                using (StreamWriter sw = ZaveGlobalSettings.ZaveFile.StreamWriterFactory.createStreamWriter(filename))
                {
                    try
                    {
                        sw.Write("[]");

                    }
                    catch (IOException ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        sw.Close();
                    }
                }
            });
        }

        public static void DeleteFile(string filename)
        {

            int maxAttempts = 20;
            int retryMilliseconds = 100;

            for (int attempts = 0; attempts <= maxAttempts; attempts++)
            {
                try
                {
                    File.Delete(filename);

                }
                catch (IOException iox)
                {
                    if (attempts == maxAttempts)
                    {
                        throw iox;
                    }
                    System.Threading.Thread.Sleep(retryMilliseconds);

                }


            }

        }
    }

    
}
