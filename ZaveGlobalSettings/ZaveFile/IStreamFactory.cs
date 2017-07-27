using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ZaveGlobalSettings.ZaveFile
{

    /// <summary>
    /// Methods for creating a StreamReader
    /// </summary>
    public class StreamReaderFactory
    {

        //public StreamReaderFactory(StreamReader sr)
        //{

        //}

        /// <summary>
        /// Creates a StreamReader, attempting multiple times to acquire access and timing out if unable to
        /// </summary>
        /// <param name="filepath">The full path of the file to open</param>
        /// <returns>A StreamReader object to use for writing to the filepath</returns>
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
                        throw new IOException("Unable to read from " + Path.GetFileName(filepath) + ". Ensure that the file is not locked or in use");

                    }

                    System.Threading.Thread.Sleep(DelayOnRetry);

                }



            }

            return null;




        }
    }

    /// <summary>
    /// Provides methods to create a StreamWriter
    /// </summary>
    public class StreamWriterFactory
    {

        //public StreamReaderFactory(StreamReader sr)
        //{

        //}
        /// <summary>
        /// Creates a StreamWriter, attempting multiple times to acquire access and timing out if unable to
        /// </summary>
        /// <param name="filepath">The full path of the file to open</param>
        /// <returns>A StreamWriter object to use for writing to the filepath</returns>
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
                        throw new IOException("Unable to write to " + Path.GetFileName(filepath) + ". Ensure that the file is not locked or in use");
                    }

                    System.Threading.Thread.Sleep(DelayOnRetry);
                }


            }

            return null;

        }
    }

    /// <summary>
    /// Generates custom GUIDs
    /// </summary>
    public static class GuidGenerator
    {
        private static Assembly _assembly = Assembly.GetExecutingAssembly();

        /// <summary>
        /// Returns the GUID of the Executing Assembly as a string
        /// </summary>
        /// <returns>The GUID of the executing assembly as a string</returns>
        public static string getExecutingAssemblyGuid()
        {
            var attribute = (GuidAttribute)_assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0];
            var id = attribute.Value;
            return (string)id;
        }

        public readonly static string UNSAVEDFILENAME = "UntitledDocument";
    }

}
