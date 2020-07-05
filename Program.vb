Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Security.Principal
Imports System.Windows.Forms
Imports Microsoft.Win32

Module EntryPoint

    <STAThread>
    Public Sub Main()
        ' https://docs.microsoft.com/en-us/deployedge/microsoft-edge-sysupdate-access-old-edge#side-by-side-experience-with-microsoft-edge-stable-channel-and-microsoft-edge-legacy
        Try
            Dim value As Integer = Registry.LocalMachine.OpenSubKey("SOFTWARE\Policies\Microsoft\EdgeUpdate").GetValue("Allowsxs")
            If Not value = 1 Then
                SetRegKey()
            End If
        Catch ex As NullReferenceException
            SetRegKey()
        End Try

        ' https://docs.microsoft.com/en-us/windows/win32/api/shobjidl_core/nn-shobjidl_core-iapplicationactivationmanager
        Dim ActivationManager = New ApplicationActivationManager()
        ActivationManager.ActivateApplication("Microsoft.MicrosoftEdge_8wekyb3d8bbwe!MicrosoftEdge", Nothing, ActivateOptions.None, Nothing)
    End Sub

    Public Sub SetRegKey()
        If Not IsAdministrator() Then
            Dim p As New ProcessStartInfo()
            p.FileName = Application.ExecutablePath
            p.Verb = "runas"
            Process.Start(p)
            Environment.Exit(0)
        End If
        Registry.LocalMachine.CreateSubKey("SOFTWARE\Policies\Microsoft\EdgeUpdate", True).SetValue("Allowsxs", 1)
    End Sub

    Public Function IsAdministrator() As Boolean
        Try
            Dim user = WindowsIdentity.GetCurrent()
            Dim principal = New WindowsPrincipal(user)
            Return principal.IsInRole(WindowsBuiltInRole.Administrator)
        Catch ex As UnauthorizedAccessException
            Return False
        End Try
    End Function

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
End Module