using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ZaveGlobalSettings.ZaveFile
{
    public class StreamReaderFactory
    {

        //public StreamReaderFactory(StreamReader sr)
        //{

        //}

        public static StreamReader createStreamReader(string filepath)
        {
            int NumberOfRetries = 20;
            int DelayOnRetry = 100;
            StreamReader sr;

            for (int i = 1; i <= NumberOfRetries; i++)
            {
                try
                {
                    sr = new StreamReader(filepath);
                    return sr;
                }
                catch (IOException ex)
                {
                    if (i == NumberOfRetries)
                    {
                        throw new IOException("Entry not saved!");
                       
                    }

                    System.Threading.Thread.Sleep(DelayOnRetry);

                }
               
                
            }

            return null;

            
             
            
        }
    }

    public class StreamWriterFactory
    {

        //public StreamReaderFactory(StreamReader sr)
        //{

        //}

        public static StreamWriter createStreamWriter(string filepath)
        {
            int NumberOfRetries = 20;
            int DelayOnRetry = 100;

            for (int i = 1; i <= NumberOfRetries; i++)
            {
                try
                {
                    StreamWriter sw = new StreamWriter(filepath);
                    return sw;
                }
                catch (IOException ex)
                {
                    if (i == NumberOfRetries)
                    {
                        throw new IOException("Entry not written!");
                    }

                    System.Threading.Thread.Sleep(DelayOnRetry);
                }
                
                
            }

            return null;
            
        }
    }
}
