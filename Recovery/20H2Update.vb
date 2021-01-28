Imports Microsoft.Win32

''' <summary>
''' https://techcommunity.microsoft.com/t5/discussions/microsoft-edge-legacy/m-p/1624481/highlight/true#M34656
''' </summary>
Public Class _20H2Update
    Implements IEdgeRecoverySetup

    Public ReadOnly Property IsRecoveryNecessary As Boolean Implements IEdgeRecoverySetup.IsRecoveryNecessary
        Get
            Try
                If Not Registry.LocalMachine.OpenSubKey("SOFTWARE\WOW6432Node\Microsoft\EdgeUpdate\ClientState\{56EB18F8-B008-4CBD-B6D2-8C97FE7E9062}").GetValue("BrowserReplacement") = Nothing Then
                    Return True
                End If
            Catch : End Try
            Return False
        End Get
    End Property

    Public Sub Recover() Implements IEdgeRecoverySetup.Recover
        Dim EdgeRegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\WOW6432Node\Microsoft\EdgeUpdate\ClientState\{56EB18F8-B008-4CBD-B6D2-8C97FE7E9062}", True)
        EdgeRegistryKey.DeleteValue("BrowserReplacement")
    End Sub
End Class
