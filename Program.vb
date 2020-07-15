Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Security.Principal
Imports System.Windows.Forms
Imports Microsoft.Win32

Module EntryPoint

    <STAThread>
    Public Sub Main()

        Application.EnableVisualStyles()

        ' https://docs.microsoft.com/en-us/deployedge/microsoft-edge-sysupdate-access-old-edge#side-by-side-experience-with-microsoft-edge-stable-channel-and-microsoft-edge-legacy
        Try
            Dim value As Integer = Registry.LocalMachine.OpenSubKey("SOFTWARE\Policies\Microsoft\EdgeUpdate").GetValue("Allowsxs")
            If Not value = 1 Then
                Setup()
            End If
        Catch ex As NullReferenceException
            Setup()
        End Try

        ' https://docs.microsoft.com/en-us/windows/win32/api/shobjidl_core/nn-shobjidl_core-iapplicationactivationmanager
        Dim ActivationManager = New ApplicationActivationManager()
        If My.Application.CommandLineArgs.Count = 1 Then
            ActivationManager.ActivateApplication(My.Application.CommandLineArgs(0), Nothing, ActivateOptions.None, Nothing)
        Else
            ActivationManager.ActivateApplication("Microsoft.MicrosoftEdge_8wekyb3d8bbwe!MicrosoftEdge", Nothing, ActivateOptions.None, Nothing)
        End If
        Application.Exit()
    End Sub

    Sub Setup()
        If Not IsAdministrator() Then
            Dim p As New ProcessStartInfo()
            p.FileName = Application.ExecutablePath
            p.Verb = "runas"
            Process.Start(p)
            Environment.Exit(0)
        End If

        Dim WaitDialog As New SetupDialog()
        Dim Thread As New Threading.Thread(Sub()
                                               Application.Run(WaitDialog)
                                           End Sub)
        Thread.IsBackground = True
        Thread.Start()

        Threading.Thread.Sleep(1000)

        ' https://docs.microsoft.com/en-us/deployedge/microsoft-edge-sysupdate-access-old-edge#side-by-side-experience-with-microsoft-edge-stable-channel-and-microsoft-edge-legacy
        Registry.LocalMachine.CreateSubKey("SOFTWARE\Policies\Microsoft\EdgeUpdate", True).SetValue("Allowsxs", 1)

        Dim EdgeRegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Microsoft Edge", False)
        If EdgeRegistryKey Is Nothing Then
            EdgeRegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Microsoft Edge", False)
        End If
        If Not EdgeRegistryKey Is Nothing Then
            Dim ModifyPath As String = EdgeRegistryKey.GetValue("ModifyPath")
            Dim StartInfos As New ProcessStartInfo() With {
                .FileName = "cmd.exe",
                .WindowStyle = ProcessWindowStyle.Hidden,
                .Arguments = $"/C call {ModifyPath}"
            }
            Dim SetupProcess = Process.Start(StartInfos)

            While Not SetupProcess.HasExited : End While
        Else
            Try
                WaitDialog.Invoke(Sub() MessageBox.Show(WaitDialog, "Please meanualy start the installer of the new edge for changes to take affect!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation))
            Catch ex As Exception
                MessageBox.Show("Please meanualy start the installer of the new edge for changes to take affect!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
        End If
        Try
            WaitDialog.Invoke(Sub() WaitDialog.Hide())
        Catch : End Try
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

#Region "Application Start via COM"
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
#End Region
End Module