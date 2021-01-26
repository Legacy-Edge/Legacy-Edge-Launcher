Imports System.Windows.Forms
Imports Microsoft.Win32


''' <summary>
''' https://docs.microsoft.com/en-us/deployedge/microsoft-edge-sysupdate-access-old-edge#side-by-side-experience-with-microsoft-edge-stable-channel-And-microsoft-edge-legacy
''' </summary>
Public Class SideBySideExperience
    Implements IEdgeRecoverySetup

    Public ReadOnly Property IsRecoveryNecessary As Boolean Implements IEdgeRecoverySetup.IsRecoveryNecessary
        Get
            Try
                Dim value As Integer = Registry.LocalMachine.OpenSubKey("SOFTWARE\Policies\Microsoft\EdgeUpdate").GetValue("Allowsxs")
                If value = Nothing Or Not value = 1 Then
                    Return True
                End If
            Catch ex As Exception
                Return True
            End Try
            Return False
        End Get
    End Property

    Public Sub Recover() Implements IEdgeRecoverySetup.Recover
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
    End Sub
End Class
