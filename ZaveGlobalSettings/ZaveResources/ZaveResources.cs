using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZaveGlobalSettings.ZaveResources
{
    class ZaveResources
    {
    }
    public static class CursorHook

    {


        private static IntPtr ptr_IBeam = IntPtr.Zero;


        private static IntPtr programCursor = IntPtr.Zero;
        private static IntPtr systemCursor = IntPtr.Zero;
        private static Cursor ZaveCursor = null;

        private static Cursor DefaultCursor = null;
        //private static GCHandle ptr_IBeamConst = GCHandle.Alloc(PTR_OCR_IBEAM.ToInt32(), GCHandleType.Pinned);


        public static void Init()
        {

            //programCursor = Marshal.AllocHGlobal(resourceCursor.marker_cursor2.Length);
            //Marshal.Copy(resourceCursor.marker_cursor2, 0, programCursor, resourceCursor.marker_cursor2.Length);
            using (MemoryStream ms = new MemoryStream(Properties.Resources.marker_cursor2))
            {
                ZaveCursor = new Cursor(ms);
            }
            DefaultCursor = new Cursor(Cursors.IBeam.CopyHandle());



        }

        public static void Start()
        {
            programCursor = ZaveCursor.CopyHandle();
            SetSystemCursor(programCursor, UNS_OCR_IBEAM);

        }


        public static void Stop()
        {
            systemCursor = DefaultCursor.CopyHandle();
            SetSystemCursor(systemCursor, UNS_OCR_IBEAM);


        }

        public static void Dispose()
        {

        }

        private const int OCR_IBEAM = 32513;
        private const uint UNS_OCR_IBEAM = OCR_IBEAM;
        private static IntPtr PTR_OCR_IBEAM = new IntPtr(OCR_IBEAM);





        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool SetSystemCursor(IntPtr hCursor, uint id);




    }
}
