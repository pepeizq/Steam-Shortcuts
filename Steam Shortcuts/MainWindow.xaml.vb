Imports System.ComponentModel
Imports System.IO

Class MainWindow

    Dim WithEvents workerCarga As New BackgroundWorker
    Public listaUWP, listaGOG, listaUplay, listaOrigin As List(Of Aplicacion)

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        tbCreditos.Text = "Version " + My.Application.Info.Version.Major.ToString + "." + My.Application.Info.Version.Minor.ToString + " - pepeizqapps.com"

        If Not File.Exists(My.Application.Info.DirectoryPath + "\Config.ini") Then
            File.WriteAllText(My.Application.Info.DirectoryPath + "\Config.ini", "[Options]" + Environment.NewLine + "Category=True" + Environment.NewLine + "Greenlight=False")
        End If

        If FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config.ini", "Options", "Greenlight") = "True" Then
            botonGreenlight.Visibility = Visibility.Collapsed
        End If

        If FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config.ini", "Options", "Category") = "True" Then
            cbCategoriaSteam.IsChecked = True
        End If

        If Not Directory.Exists(My.Application.Info.DirectoryPath + "\Temp") Then
            Directory.CreateDirectory(My.Application.Info.DirectoryPath + "\Temp")
        End If

        workerCarga.WorkerReportsProgress = True
        workerCarga.RunWorkerAsync()

    End Sub

    Private Sub workerCarga_DoWork(sender As Object, e As DoWorkEventArgs) Handles workerCarga.DoWork

        listaGOG = New List(Of Aplicacion)

        workerCarga.ReportProgress(0, "Searching Games of GOG Galaxy")

        Try
            listaGOG = GOGGalaxy.GenerarJuegos(listaGOG, workerCarga)
        Catch ex As Exception

        End Try

        '-----------------------------------------

        listaOrigin = New List(Of Aplicacion)

        workerCarga.ReportProgress(0, "Searching Games of Origin")

        Try
            listaOrigin = Origin.GenerarJuegos(listaOrigin, workerCarga)
        Catch ex As Exception

        End Try

        '-----------------------------------------

        listaUplay = New List(Of Aplicacion)

        workerCarga.ReportProgress(0, "Searching Games of Uplay")

        Try
            listaUplay = Uplay.GenerarJuegos(listaUplay, workerCarga)
        Catch ex As Exception

        End Try

        '-----------------------------------------

        listaUWP = New List(Of Aplicacion)

        workerCarga.ReportProgress(0, "Searching Apps and Games of Windows Store")

        Try
            Dim unidades() As String = Directory.GetLogicalDrives
            For Each unidad As String In unidades
                listaUWP = WindowsStore.GenerarApps(listaUWP, unidad, workerCarga)
            Next
        Catch ex As Exception

        End Try

    End Sub

    Private Sub workerCarga_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles workerCarga.RunWorkerCompleted

        If listaGOG.Count > 0 Then
            Listado.Carga(listaGOG, True, tbControlMaestro, "GOG Galaxy", "pack://application:,,/Imagenes/goggalaxy.ico")
        End If

        If listaOrigin.Count > 0 Then
            Listado.Carga(listaOrigin, True, tbControlMaestro, "Origin", "pack://application:,,/Imagenes/origin.png")
        End If

        If listaUplay.Count > 0 Then
            Listado.Carga(listaUplay, True, tbControlMaestro, "Uplay", "pack://application:,,/Imagenes/uplay.png")
        End If

        If listaUWP.Count > 0 Then
            Listado.Carga(listaUWP, False, tbControlMaestro, "Windows Store", "pack://application:,,/Imagenes/windowsstore.png")
        End If

        gridCarga.Visibility = Visibility.Collapsed

        If tbControlMaestro.Items.Count > 0 Then
            gridListas.Visibility = Visibility.Visible
        Else
            gridNoApps.Visibility = Visibility.Visible
        End If

    End Sub

    Private Sub workerCarga_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles workerCarga.ProgressChanged

        If Not e.UserState = Nothing Then
            If Not e.UserState.ToString.Contains("/*ERROR*/") Then
                tbCarga.Text = e.UserState
            Else
                Dim temp As String = e.UserState

                If temp.Contains("/*ERRORUWP*/") Then
                    temp = temp.Replace("/*ERRORUWP*/", Nothing)

                    gridPermisos.Visibility = Visibility.Visible

                    If Not tbPermisos.Text = Nothing Then
                        tbPermisos.Text = tbPermisos.Text + Environment.NewLine + temp
                    Else
                        tbPermisos.Text = "Windows Store - Add read permissions to:" + Environment.NewLine + temp
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub botonCrearAccesos_Click(sender As Object, e As RoutedEventArgs) Handles botonCrearAccesos.Click

        Dim listaFinal As List(Of Aplicacion) = New List(Of Aplicacion)

        For Each app As Aplicacion In listaUWP
            If app.Añadir = True Then
                listaFinal.Add(app)
            End If
        Next

        For Each app As Aplicacion In listaGOG
            If app.Añadir = True Then
                listaFinal.Add(app)
            End If
        Next

        For Each app As Aplicacion In listaUplay
            If app.Añadir = True Then
                listaFinal.Add(app)
            End If
        Next

        For Each app As Aplicacion In listaOrigin
            If app.Añadir = True Then
                listaFinal.Add(app)
            End If
        Next

        Steam.CrearAccesos(listaFinal, cbCategoriaSteam, botonCrearAccesos)

        For Each tabitem As TabItem In tbControlMaestro.Items
            Dim lv As ListView = tabitem.Content

            For Each grid As Grid In lv.Items
                Dim cb As CheckBox = grid.Children.Item(2)
                cb.IsChecked = False
            Next
        Next

        botonCrearAccesos.IsEnabled = False

    End Sub

    Private Sub botonFAQ_Click(sender As Object, e As RoutedEventArgs) Handles botonFAQ.Click

        Try
            Process.Start("https://pepeizqapps.com/faq-steam-shortcuts/")
        Catch ex As Exception

        End Try

    End Sub

    Private Sub botonGreenlight_Click(sender As Object, e As RoutedEventArgs) Handles botonGreenlight.Click

        Try
            Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=700966331")
            FicherosINI.Escribir(My.Application.Info.DirectoryPath + "\Config.ini", "Options", "Greenlight", "True")
            botonGreenlight.Visibility = Visibility.Collapsed
        Catch ex As Exception

        End Try

    End Sub

    Private Sub cbCategoriaSteam_Checked(sender As Object, e As RoutedEventArgs) Handles cbCategoriaSteam.Checked

        FicherosINI.Escribir(My.Application.Info.DirectoryPath + "\Config.ini", "Options", "Category", "True")

    End Sub

    Private Sub cbCategoriaSteam_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbCategoriaSteam.Unchecked

        FicherosINI.Escribir(My.Application.Info.DirectoryPath + "\Config.ini", "Options", "Category", "False")

    End Sub
End Class
