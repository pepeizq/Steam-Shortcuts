Imports System.ComponentModel
Imports System.IO

Class MainWindow

    Dim WithEvents workerCarga As New BackgroundWorker
    Dim listaAppsUWP As List(Of Aplicacion)

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        tbCreditos.Text = "Version " + My.Application.Info.Version.Major.ToString + "." + My.Application.Info.Version.Minor.ToString + " - pepeizqapps.com"

        If Not File.Exists(My.Application.Info.DirectoryPath + "\Config.ini") Then
            File.WriteAllText(My.Application.Info.DirectoryPath + "\Config.ini", "[Options]" + Environment.NewLine + "Greenlight=False")
        End If

        If FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config.ini", "Options", "Greenlight") = "True" Then
            buttonGreenlight.Visibility = Visibility.Collapsed
        End If

        workerCarga.WorkerReportsProgress = True
        workerCarga.RunWorkerAsync()

    End Sub

    Private Sub workerCarga_DoWork(sender As Object, e As DoWorkEventArgs) Handles workerCarga.DoWork

        listaAppsUWP = New List(Of Aplicacion)

        Dim unidades() As String = Directory.GetLogicalDrives

        For Each unidad As String In unidades
            listaAppsUWP = WindowsStore.GenerarApps(listaAppsUWP, unidad, workerCarga)
        Next

    End Sub

    Private Sub workerCarga_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles workerCarga.RunWorkerCompleted

        Dim boolWindows As Boolean = False

        If listaAppsUWP.Count > 0 Then
            Listado.Carga(listaAppsUWP, lvAppsUWP, False)
            boolWindows = True
        Else
            boolWindows = False
        End If

        gridCarga.Visibility = Visibility.Collapsed

        If boolWindows = True Then
            gridListas.Visibility = Visibility.Visible
        Else
            gridNoApps.Visibility = Visibility.Visible
        End If

    End Sub

    Private Sub workerCarga_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles workerCarga.ProgressChanged

        If Not e.UserState = Nothing Then
            gridPermisos.Visibility = Visibility.Visible

            If Not tbPermisos.Text = Nothing Then
                tbPermisos.Text = tbPermisos.Text + Environment.NewLine + e.UserState
            Else
                tbPermisos.Text = "Add read permissions to:" + Environment.NewLine + e.UserState
            End If
        End If

    End Sub

    Private Sub buttonGreenlight_Click(sender As Object, e As RoutedEventArgs) Handles buttonGreenlight.Click

        Try
            Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=700966331")
            FicherosINI.Escribir(My.Application.Info.DirectoryPath + "\Config.ini", "Options", "Greenlight", "True")
            buttonGreenlight.Visibility = Visibility.Collapsed
        Catch ex As Exception

        End Try

    End Sub

End Class
