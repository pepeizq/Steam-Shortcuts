Imports System.ComponentModel
Imports System.IO

Class MainWindow

    Dim WithEvents workerCarga As New BackgroundWorker
    Public listaUWP, listaGOG, listaUplay, listaOrigin, listaBattlenet As List(Of Aplicacion)

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        Try
            tbCargaCreditos.Text = "Version " + My.Application.Info.Version.Major.ToString + "." + My.Application.Info.Version.Minor.ToString + " - pepeizqapps.com"
        Catch ex As Exception
            tbCargaCreditos.Text = "pepeizqapps.com"
        End Try

        Try
            If Not File.Exists(My.Application.Info.DirectoryPath + "\Config\Config.ini") Then
                File.WriteAllText(My.Application.Info.DirectoryPath + "\Config\Config.ini", "[Options]" + Environment.NewLine + "[Category]" + Environment.NewLine + "[Games]" + Environment.NewLine + "[Apps]")
            End If
        Catch ex As Exception

        End Try

        Try
            If FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config\Config.ini", "Options", "Category") = "True" Then
                cbCategoriaSteam.IsChecked = True
            End If
        Catch ex As Exception
            cbCategoriaSteam.IsChecked = True
        End Try

        Try
            If Not Directory.Exists(My.Application.Info.DirectoryPath + "\Temp") Then
                Directory.CreateDirectory(My.Application.Info.DirectoryPath + "\Temp")
            End If
        Catch ex As Exception

        End Try

        Try
            If Not File.Exists(My.Application.Info.DirectoryPath + "\Config\Log.txt") Then
                File.Create(My.Application.Info.DirectoryPath + "\Config\Log.txt")
            Else
                File.WriteAllText(My.Application.Info.DirectoryPath + "\Config\Log.txt", " ")
            End If
        Catch ex As Exception

        End Try

        Log.Actualizar(Nothing, "Application Loaded" + Environment.NewLine)

        workerCarga.WorkerReportsProgress = True
        workerCarga.RunWorkerAsync()

    End Sub

    Private Sub workerCarga_DoWork(sender As Object, e As DoWorkEventArgs) Handles workerCarga.DoWork

        listaBattlenet = New List(Of Aplicacion)

        Log.Actualizar(Nothing, "Searching Games of Battle.net")
        workerCarga.ReportProgress(0, "/*BATTLENET*/Searching Games")

        Dim booleanErrorBattlenet As Boolean = False

        Try
            listaBattlenet = Battlenet.GenerarJuegos(listaBattlenet, workerCarga)
        Catch ex As Exception
            booleanErrorBattlenet = True
            workerCarga.ReportProgress(0, "/*BATTLENET*/Error Please send the log")
        End Try

        Log.Actualizar(Nothing, "Battle.net List: " + listaBattlenet.Count.ToString + Environment.NewLine)

        If Not listaBattlenet.Count = 0 Then
            workerCarga.ReportProgress(0, "/*BATTLENET*/" + listaBattlenet.Count.ToString + " Games Detected")
        Else
            If booleanErrorBattlenet = False Then
                workerCarga.ReportProgress(0, "/*BATTLENET*/Games Not Detected")
            End If
        End If

        '-----------------------------------------

        listaGOG = New List(Of Aplicacion)

        Log.Actualizar(Nothing, "Searching Games of GOG Galaxy")
        workerCarga.ReportProgress(0, "/*GOGGALAXY*/Searching Games")

        Dim booleanErrorGOG As Boolean = False

        Try
            listaGOG = GOGGalaxy.GenerarJuegos(listaGOG, workerCarga)
        Catch ex As Exception
            booleanErrorGOG = True
        workerCarga.ReportProgress(0, "/*GOGGALAXY*/Error")
        End Try

        Log.Actualizar(Nothing, "GOG Galaxy List: " + listaGOG.Count.ToString + Environment.NewLine)

        If Not listaGOG.Count = 0 Then
            workerCarga.ReportProgress(0, "/*GOGGALAXY*/" + listaGOG.Count.ToString + " Games Detected")
        Else
            If booleanErrorGOG = False Then
                workerCarga.ReportProgress(0, "/*GOGGALAXY*/Games Not Detected")
            End If
        End If

        '-----------------------------------------

        listaOrigin = New List(Of Aplicacion)

        Log.Actualizar(Nothing, "Searching Games of Origin")
        workerCarga.ReportProgress(0, "/*ORIGIN*/Searching Games")

        Dim booleanErrorOrigin As Boolean = False

        Try
            listaOrigin = Origin.GenerarJuegos(listaOrigin, workerCarga)
        Catch ex As Exception
            booleanErrorOrigin = True
            workerCarga.ReportProgress(0, "/*ORIGIN*/Error")
        End Try

        Log.Actualizar(Nothing, "Origin List: " + listaOrigin.Count.ToString + Environment.NewLine)

        If Not listaOrigin.Count = 0 Then
            workerCarga.ReportProgress(0, "/*ORIGIN*/" + listaOrigin.Count.ToString + " Games Detected")
        Else
            If booleanErrorOrigin = False Then
                workerCarga.ReportProgress(0, "/*ORIGIN*/Games Not Detected")
            End If
        End If

        '-----------------------------------------

        listaUplay = New List(Of Aplicacion)

        Log.Actualizar(Nothing, "Searching Games of Uplay")
        workerCarga.ReportProgress(0, "/*UPLAY*/Searching Games")

        Dim booleanErrorUplay As Boolean = False

        Try
            listaUplay = Uplay.GenerarJuegos(listaUplay, workerCarga)
        Catch ex As Exception
            booleanErrorUplay = True
            workerCarga.ReportProgress(0, "/*UPLAY*/Error")
        End Try

        Log.Actualizar(Nothing, "Uplay List: " + listaUplay.Count.ToString + Environment.NewLine)

        If Not listaUplay.Count = 0 Then
            workerCarga.ReportProgress(0, "/*UPLAY*/" + listaUplay.Count.ToString + " Games Detected")
        Else
            If booleanErrorUplay = False Then
                workerCarga.ReportProgress(0, "/*UPLAY*/Games Not Detected")
            End If
        End If

        '-----------------------------------------

        listaUWP = New List(Of Aplicacion)

        Log.Actualizar(Nothing, "Searching Apps and Games of Windows Store")
        workerCarga.ReportProgress(0, "/*WINDOWSSTORE*/Searching Apps and Games")

        Dim booleanErrorWindowsStore As Boolean = False

        Try
            Dim unidades() As String = Directory.GetLogicalDrives
            For Each unidad As String In unidades
                listaUWP = WindowsStore.GenerarApps(listaUWP, unidad, workerCarga)
            Next
        Catch ex As Exception
            booleanErrorWindowsStore = True
            workerCarga.ReportProgress(0, "/*WINDOWSSTORE*/Error")
        End Try

        Log.Actualizar(Nothing, "Windows Store List: " + listaUWP.Count.ToString + Environment.NewLine)

        If Not listaUWP.Count = 0 Then
            workerCarga.ReportProgress(0, "/*WINDOWSSTORE*/" + listaUWP.Count.ToString + " Apps and Games Detected")
        Else
            If booleanErrorWindowsStore = False Then
                workerCarga.ReportProgress(0, "/*WINDOWSSTORE*/Apps or Games Not Detected")
            End If
        End If

    End Sub

    Private Sub workerCarga_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles workerCarga.RunWorkerCompleted

        Try
            If listaBattlenet.Count > 0 Then
                Listado.Carga(listaBattlenet, True, tbControlMaestro, "Battle.net", "pack://application:,,/Imagenes/battlenet.png")
            End If
        Catch ex As Exception

        End Try

        Try
            If listaGOG.Count > 0 Then
                Listado.Carga(listaGOG, True, tbControlMaestro, "GOG Galaxy", "pack://application:,,/Imagenes/goggalaxy.ico")
            End If
        Catch ex As Exception

        End Try

        Try
            If listaOrigin.Count > 0 Then
                Listado.Carga(listaOrigin, True, tbControlMaestro, "Origin", "pack://application:,,/Imagenes/origin.png")
            End If
        Catch ex As Exception

        End Try

        Try
            If listaUplay.Count > 0 Then
                Listado.Carga(listaUplay, True, tbControlMaestro, "Uplay", "pack://application:,,/Imagenes/uplay.png")
            End If
        Catch ex As Exception

        End Try

        Try
            If listaUWP.Count > 0 Then
                Listado.Carga(listaUWP, False, tbControlMaestro, "Windows Store", "pack://application:,,/Imagenes/windowsstore.png")
            End If
        Catch ex As Exception

        End Try

        gridCarga.Visibility = Visibility.Collapsed
        gridListas.Visibility = Visibility.Visible

        Main.Width = 710
        Main.Height = 750

        Dim workArea As Rect = SystemParameters.WorkArea
        Main.Left = (workArea.Width - Main.Width) / 2 + workArea.Left
        Main.Top = (workArea.Height - Main.Height) / 2 + workArea.Top

        Dim clientesError As String = Nothing
        Dim booleanError As Boolean = False

        If tbCargaBattlenet.Text.Contains("Error") Then
            booleanError = True

            If Not clientesError = Nothing Then
                clientesError = clientesError + ", Battle.net"
            Else
                clientesError = clientesError + "Battle.net"
            End If
        ElseIf tbCargaGogGalaxy.Text.Contains("Error") Then
            booleanError = True

            If Not clientesError = Nothing Then
                clientesError = clientesError + ", GOG Galaxy"
            Else
                clientesError = clientesError + "GOG Galaxy"
            End If
        ElseIf tbCargaOrigin.Text.Contains("Error") Then
            booleanError = True

            If Not clientesError = Nothing Then
                clientesError = clientesError + ", Origin"
            Else
                clientesError = clientesError + "Origin"
            End If
        ElseIf tbCargaUplay.Text.Contains("Error") Then
            booleanError = True

            If Not clientesError = Nothing Then
                clientesError = clientesError + ", Uplay"
            Else
                clientesError = clientesError + "Uplay"
            End If
        ElseIf tbCargaWindowsStore.Text.Contains("Error") Then
            booleanError = True

            If Not clientesError = Nothing Then
                clientesError = clientesError + ", Windows Store"
            Else
                clientesError = clientesError + "Windows Store"
            End If
        End If

        If booleanError = True Then
            gridListasError.Visibility = Visibility.Visible
            tbListasError.Text = "Please, use the option Report a Bug. Errors detected in: " + clientesError
        End If

    End Sub

    Private Sub workerCarga_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles workerCarga.ProgressChanged

        If Not e.UserState = Nothing Then
            If e.UserState.ToString.Contains("/*BATTLENET*/") Then
                Dim temp As String = e.UserState.ToString
                temp = temp.Replace("/*BATTLENET*/", Nothing)

                tbCargaBattlenet.Text = temp

                If temp = "Searching Games" Then
                    prCargaBattlenet.Visibility = Visibility.Visible
                Else
                    prCargaBattlenet.Visibility = Visibility.Collapsed
                End If

            ElseIf e.UserState.ToString.Contains("/*GOGGALAXY*/") Then
                Dim temp As String = e.UserState.ToString
                temp = temp.Replace("/*GOGGALAXY*/", Nothing)

                tbCargaGogGalaxy.Text = temp

                If temp = "Searching Games" Then
                    prCargaGogGalaxy.Visibility = Visibility.Visible
                Else
                    prCargaGogGalaxy.Visibility = Visibility.Collapsed
                End If

            ElseIf e.UserState.ToString.Contains("/*ORIGIN*/") Then
                Dim temp As String = e.UserState.ToString
                temp = temp.Replace("/*ORIGIN*/", Nothing)

                tbCargaOrigin.Text = temp

                If temp = "Searching Games" Then
                    prCargaOrigin.Visibility = Visibility.Visible
                Else
                    prCargaOrigin.Visibility = Visibility.Collapsed
                End If

            ElseIf e.UserState.ToString.Contains("/*UPLAY*/") Then
                Dim temp As String = e.UserState.ToString
                temp = temp.Replace("/*UPLAY*/", Nothing)

                tbCargaUplay.Text = temp

                If temp = "Searching Games" Then
                    prCargaUplay.Visibility = Visibility.Visible
                Else
                    prCargaUplay.Visibility = Visibility.Collapsed
                End If

            ElseIf e.UserState.ToString.Contains("/*WINDOWSSTORE*/") Then
                Dim temp As String = e.UserState.ToString
                temp = temp.Replace("/*WINDOWSSTORE*/", Nothing)

                tbCargaWindowsStore.Text = temp

                If temp = "Searching Apps and Games" Then
                    prCargaWindowsStore.Visibility = Visibility.Visible
                Else
                    prCargaWindowsStore.Visibility = Visibility.Collapsed
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

        For Each app As Aplicacion In listaBattlenet
            If app.Añadir = True Then
                listaFinal.Add(app)
            End If
        Next

        Steam.CrearAccesos(listaFinal, cbCategoriaSteam, botonCrearAccesos)

        For Each tabitem As TabItem In tbControlMaestro.Items
            Dim lv As ListView = tabitem.Content

            For Each grid As Grid In lv.Items
                Dim cb As CheckBox = grid.Children.Item(0)
                cb.IsChecked = False
            Next
        Next

        botonCrearAccesos.IsEnabled = False

    End Sub

    Private Sub cbCategoriaSteam_Checked(sender As Object, e As RoutedEventArgs) Handles cbCategoriaSteam.Checked

        FicherosINI.Escribir(My.Application.Info.DirectoryPath + "\Config\Config.ini", "Options", "Category", "True")

    End Sub

    Private Sub cbCategoriaSteam_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbCategoriaSteam.Unchecked

        FicherosINI.Escribir(My.Application.Info.DirectoryPath + "\Config\Config.ini", "Options", "Category", "False")

    End Sub

    Private Sub menuItemReportBug_Click(sender As Object, e As RoutedEventArgs) Handles menuItemReportBug.Click

        gridWebBrowser.Visibility = Visibility.Visible
        gridListas.Visibility = Visibility.Collapsed

        wb.Navigate("https://pepeizqapps.com/contact/")

    End Sub

    Private Sub wb_LoadCompleted(sender As Object, e As NavigationEventArgs) Handles wb.LoadCompleted

        If e.Uri.ToString = "https://pepeizqapps.com/contact/" Then
            buttonReportBugEnviar.IsEnabled = True
        End If

    End Sub

    Private Sub buttonReportBugEnviar_Click(sender As Object, e As RoutedEventArgs) Handles buttonReportBugEnviar.Click

        buttonReportBugEnviar.IsEnabled = False

        Try
            Dim documento As mshtml.IHTMLDocument2 = TryCast(wb.Document, mshtml.IHTMLDocument2)

            Dim nombre As String = tbReportBugNombre.Text.Trim

            If nombre = Nothing Then
                nombre = "Sin Nombre"
            End If

            Dim nombreElemento As mshtml.IHTMLElement = CType(documento.all.item("g2-name"), mshtml.IHTMLElement)
            nombreElemento.innerText = nombre

            Dim correo As String = tbReportBugCorreo.Text.Trim

            If correo = Nothing Then
                correo = "nocorreo@pepeizqapps.com"
            End If

            Dim correoElemento As mshtml.IHTMLElement = CType(documento.all.item("g2-email"), mshtml.IHTMLElement)
            correoElemento.innerText = correo

            Dim log As String = Nothing

            For Each linea As String In File.ReadAllLines(My.Application.Info.DirectoryPath + "\Config\Log.txt")
                If Not linea = Nothing Then
                    Dim cliente As String = cbReportBugClientes.SelectedItem.ToString

                    cliente = cliente.Replace("System.Windows.Controls.ComboBoxItem:", Nothing)
                    cliente = cliente.Trim

                    If linea.Contains(cliente) Then
                        log = log + linea + Environment.NewLine
                    End If
                End If
            Next

            Dim textoMensaje As String = tbReportBugMensaje.Text.Trim

            textoMensaje = textoMensaje + Environment.NewLine + Environment.NewLine + "LOG------------------------------" + Environment.NewLine + log

            Dim mensaje As mshtml.IHTMLElement = CType(documento.all.item("contact-form-comment-g2-message"), mshtml.IHTMLElement)
            mensaje.innerText = textoMensaje

            For Each elemento As mshtml.IHTMLElement In documento.all
                If elemento.className = "pushbutton-wide" Then
                    elemento.click()
                End If
            Next

            MsgBox("Report sent")
        Catch ex As Exception

        End Try

        buttonReportBugEnviar.IsEnabled = True

    End Sub

    Private Sub buttonReportBugVolver_Click(sender As Object, e As RoutedEventArgs) Handles buttonReportBugVolver.Click

        gridWebBrowser.Visibility = Visibility.Collapsed
        gridListas.Visibility = Visibility.Visible

    End Sub

    Private Sub menuItemFAQ_Click(sender As Object, e As RoutedEventArgs) Handles menuItemFAQ.Click

        Try
            Process.Start("https://pepeizqapps.com/faq-steam-shortcuts/")
        Catch ex As Exception

        End Try

    End Sub

    Private Sub menuItemWeb_Click(sender As Object, e As RoutedEventArgs) Handles menuItemWeb.Click

        Try
            Process.Start("https://pepeizqapps.com/")
        Catch ex As Exception

        End Try

    End Sub


End Class
