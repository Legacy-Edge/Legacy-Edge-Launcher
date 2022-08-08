using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ShortDev.Spartan.Launcher.Helpers
{
    internal static class CertificatHelper
    {
        public static void RemoveCertificat(string filePath)
        {
            using (var fileStream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite))
            using (var safeHandle = fileStream.SafeFileHandle)
            {
                IntPtr hFile = safeHandle.DangerousGetHandle();

                int index = 0;
                while (/* Imagehlp.dll */ImageRemoveCertificate(hFile, index))
                    index++;
            }
        }


        [DllImport("Imagehlp.dll", SetLastError = true)]
        static extern bool ImageRemoveCertificate(IntPtr FileHandle, int Index);
    }
}
