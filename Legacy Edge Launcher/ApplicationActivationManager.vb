' https://docs.microsoft.com/en-us/windows/win32/api/shobjidl_core/nn-shobjidl_core-iapplicationactivationmanager

Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices

Public Enum ActivateOptions
    None = &H0
End Enum
<ComImport, Guid("2e941141-7f97-4756-ba1d-9decde894a3d"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
Interface IApplicationActivationManager
    Function ActivateApplication(<[In]> ByVal appUserModelId As String, <[In]> ByVal arguments As String, <[In]> ByVal options As ActivateOptions, <Out> ByRef processId As UInt32) As IntPtr
End Interface
<ComImport, Guid("45BA127D-10A8-46EA-8AB7-56EA9078943C")>
Class ApplicationActivationManager
    Implements IApplicationActivationManager
    <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
    Public Function ActivateApplication(<[In]> ByVal appUserModelId As String, <[In]> ByVal arguments As String, <[In]> ByVal options As ActivateOptions, <Out> ByRef processId As UInt32) As IntPtr Implements IApplicationActivationManager.ActivateApplication : End Function
End Class