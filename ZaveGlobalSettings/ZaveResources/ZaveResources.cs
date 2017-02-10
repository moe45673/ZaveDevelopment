using System;
using System.Runtime.InteropServices;
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
            //using (MemoryStream ms = new MemoryStream(Properties.Resources.marker_cursor2))
            //{
            //    ZaveCursor = new Cursor(ms);
            //}

            var cross_i = Properties.Resources.marker_cursor2;
            ZaveCursor = cross_i.CursorFromArray(0);
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
            Stop();
        }

        private const int OCR_IBEAM = 32513;
        private const uint UNS_OCR_IBEAM = OCR_IBEAM;
        private IntPtr PTR_OCR_IBEAM = new IntPtr(OCR_IBEAM);





        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool SetSystemCursor(IntPtr hCursor, uint id);


        


    }

    public static class Extensions
    {
        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        struct IconHeader
        {
            [FieldOffset(0)]
            public short reserved;
            [FieldOffset(2)]
            public short type;
            [FieldOffset(4)]
            public short count;
        }

        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        struct IconInfo
        {
            [FieldOffset(0)]
            public byte width;
            [FieldOffset(1)]
            public byte height;
            [FieldOffset(2)]
            public byte colors;
            [FieldOffset(3)]
            public byte reserved;
            [FieldOffset(4)]
            public short planes; // ICO file
            [FieldOffset(6)]
            public short bpp; // ICO file
            [FieldOffset(4)]
            public short hotspot_x; // CUR file
            [FieldOffset(6)]
            public short hotspot_y; // CUR file
            [FieldOffset(8)]
            public int size;
            [FieldOffset(12)]
            public int offset;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr CreateIconFromResource(IntPtr pbIconBits, int dwResSize, bool fIcon, int dwVer);

        public static Cursor CursorFromArray(this byte[] data, int imageIndex)
        {
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);

            IconHeader iHeader = (IconHeader)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(IconHeader));

            if (imageIndex > iHeader.count - 1)
            {
                handle.Free();
                throw new ArgumentOutOfRangeException("imageIndex");
            }

            IntPtr iconInfoPtr = handle.AddrOfPinnedObject() + Marshal.SizeOf(typeof(IconHeader)) + imageIndex * Marshal.SizeOf(typeof(IconInfo));
            IconInfo iInfo = (IconInfo)Marshal.PtrToStructure(iconInfoPtr, typeof(IconInfo));

            handle.Free();

            IntPtr iconImage = Marshal.AllocHGlobal(iInfo.size + 4);
            Marshal.WriteInt16(iconImage + 0, iInfo.hotspot_x);
            Marshal.WriteInt16(iconImage + 2, iInfo.hotspot_y);
            Marshal.Copy(data, iInfo.offset, iconImage + 4, iInfo.size);

            IntPtr hCursor = CreateIconFromResource(iconImage, iInfo.size + 4, false, 0x30000);
            Marshal.FreeHGlobal(iconImage);
            return new Cursor(hCursor);
        }



    }
}
