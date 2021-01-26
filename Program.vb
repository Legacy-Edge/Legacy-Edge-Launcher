Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Security.Principal
Imports System.Windows.Forms
Imports Microsoft.Win32

Module EntryPoint

    Public ReadOnly Property WaitDialog As New SetupDialog()

    <STAThread>
    Public Sub Main()

        Application.EnableVisualStyles()

        Dim SetupOrder As List(Of IEdgeRecoverySetup) = {New SideBySideExperience(), New _20H2Update()}.Select(Function(x) CType(x, IEdgeRecoverySetup)).ToList()

        Dim NecessarySetups As New List(Of IEdgeRecoverySetup)
        For Each setup As IEdgeRecoverySetup In SetupOrder 'Assembly.GetExecutingAssembly().DefinedTypes.Where(Function(x) GetType(IEdgeRecoverySetup).IsAssignableFrom(x))
            If setup.IsRecoveryNecessary Then
                NecessarySetups.Add(setup)
            End If
        Next

        If NecessarySetups.Count > 0 Then

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

            For Each setup As IEdgeRecoverySetup In NecessarySetups
                setup.Recover()
            Next

            Try
                WaitDialog.Invoke(Sub() WaitDialog.Hide())
            Catch : End Try

        End If

        Dim ActivationManager = New ApplicationActivationManager()
        If My.Application.CommandLineArgs.Count = 1 Then
            ActivationManager.ActivateApplication(My.Application.CommandLineArgs(0), Nothing, ActivateOptions.None, Nothing)
        Else
            ActivationManager.ActivateApplication("Microsoft.MicrosoftEdge_8wekyb3d8bbwe!MicrosoftEdge", Nothing, ActivateOptions.None, Nothing)
        End If
        Application.Exit()
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
End Module