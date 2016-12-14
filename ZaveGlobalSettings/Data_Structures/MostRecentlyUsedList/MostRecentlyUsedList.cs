using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Westwind.Utilities;

namespace ZaveGlobalSettings.Data_Structures.MostRecentlyUsedList
{
    public static class MostRecentlyUsedList
    {
        /// <summary>
        /// Adds Recently Used Document to the MRU list in Windows.
        /// Item is added to the global MRU list as well as to the
        /// application specific shortcut that is associated with
        /// the application and shows up in the task bar icon MRU list.
        /// </summary>
        /// <param name="path">Full path of the file</param>
        public static void AddToRecentlyUsedDocs(string path)
        {
            SHAddToRecentDocs(ShellAddToRecentDocsFlags.Path, path);
        }


        private enum ShellAddToRecentDocsFlags
        {
            Pidl = 0x001,
            Path = 0x002,
        }

        [DllImport("shell32.dll", CharSet = CharSet.Ansi)]
        private static extern void
            SHAddToRecentDocs(ShellAddToRecentDocsFlags flag, string path);



        public static List<string> GetMostRecentDocs(string fileSpec)
        {
            var recentFiles = new List<string>();

            var path = Environment.GetFolderPath(Environment.SpecialFolder.Recent);

            var di = new DirectoryInfo(path);
            var files = di.GetFiles(fileSpec + ".lnk")
                .OrderByDescending(fi => fi.LastWriteTimeUtc)
                .ToList();
            if (files.Count < 1)
                return recentFiles;

            dynamic script = ReflectionUtils.CreateComInstance("Wscript.Shell");

            foreach (var file in files)
            {
                dynamic sc = script.CreateShortcut(file.FullName);
                recentFiles.Add(sc.TargetPath);
                Marshal.FinalReleaseComObject(sc);
            }
            Marshal.FinalReleaseComObject(script);

            return recentFiles;
        }
    }
    
}
