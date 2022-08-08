using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ShortDev.Spartan.Launcher.Helpers
{
    internal static class ApplicationActivationManager
    {
        static readonly Guid CLSID_ApplicationActivationManager = new("45BA127D-10A8-46EA-8AB7-56EA9078943C");
        public static Process ActivateApplication(string appUserModelId, string? arguments)
        {
            Type? type = Type.GetTypeFromCLSID(CLSID_ApplicationActivationManager);
            if (type == null)
                throw new NotSupportedException();

            IApplicationActivationManager? activationManager = (IApplicationActivationManager?)Activator.CreateInstance(type);
            if (activationManager == null)
                throw new NotSupportedException();

            Marshal.ThrowExceptionForHR(
                activationManager.ActivateApplication(appUserModelId, arguments, ActivateOptions.None, out var pId)
            );
            return Process.GetProcessById(pId);
        }
    }

    enum ActivateOptions
    {
        None = 0x0
    }

    // https://docs.microsoft.com/en-us/windows/win32/api/shobjidl_core/nn-shobjidl_core-iapplicationactivationmanager

    [ComImport]
    [Guid("2e941141-7f97-4756-ba1d-9decde894a3d")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IApplicationActivationManager
    {
        [PreserveSig]
        int ActivateApplication([In] string appUserModelId, [In] string? arguments, [In] ActivateOptions options, out int processId);
    }
}