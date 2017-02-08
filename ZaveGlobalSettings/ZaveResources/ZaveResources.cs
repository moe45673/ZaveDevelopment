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

    public interface IZaveLowLevelHook
    {
        void Init();
        void Start();
        void Stop();
        void Dispose();

    }

    public class ZaveCursorHook : IZaveLowLevelHook

    {


        //private static IntPtr ptr_IBeam = IntPtr.Zero;


        private IntPtr programCursor = IntPtr.Zero;
        private IntPtr systemCursor = IntPtr.Zero;
        private Cursor ZaveCursor = null;

        private Cursor DefaultCursor = null;
        //private static GCHandle ptr_IBeamConst = GCHandle.Alloc(PTR_OCR_IBEAM.ToInt32(), GCHandleType.Pinned);


        public void Init()
        {

            //programCursor = Marshal.AllocHGlobal(resourceCursor.marker_cursor2.Length);
            //Marshal.Copy(resourceCursor.marker_cursor2, 0, programCursor, resourceCursor.marker_cursor2.Length);
            using (MemoryStream ms = new MemoryStream(Properties.Resources.marker_cursor2))
            {
                ZaveCursor = new Cursor(ms);
            }
            DefaultCursor = new Cursor(Cursors.IBeam.CopyHandle());



        }

        public void Start()
        {
            programCursor = ZaveCursor.CopyHandle();
            SetSystemCursor(programCursor, UNS_OCR_IBEAM);

        }


        public void Stop()
        {
            systemCursor = DefaultCursor.CopyHandle();
            SetSystemCursor(systemCursor, UNS_OCR_IBEAM);


        }

        public void Dispose()
        {

        }

        private const int OCR_IBEAM = 32513;
        private const uint UNS_OCR_IBEAM = OCR_IBEAM;
        private IntPtr PTR_OCR_IBEAM = new IntPtr(OCR_IBEAM);





        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool SetSystemCursor(IntPtr hCursor, uint id);




    }
}
