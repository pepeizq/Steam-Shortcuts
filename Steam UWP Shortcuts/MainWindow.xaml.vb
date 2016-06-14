Imports System.ComponentModel
Imports System.IO
Imports System.Text

Class MainWindow

    Dim WithEvents workerCarga As New BackgroundWorker
    Dim listaApps As List(Of Aplicacion)

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        tbCreditos.Text = "Version " + My.Application.Info.Version.Major.ToString + "." + My.Application.Info.Version.Minor.ToString + " - pepeizqapps.com"

        If Not File.Exists(My.Application.Info.DirectoryPath + "\Config.ini") Then
            File.WriteAllText(My.Application.Info.DirectoryPath + "\Config.ini", "[Options]" + Environment.NewLine + "Greenlight=False")
        End If

        If FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config.ini", "Options", "Greenlight") = "True" Then
            gridGreenlight.Visibility = Visibility.Collapsed
        End If

        workerCarga.WorkerReportsProgress = True
        workerCarga.RunWorkerAsync()

    End Sub

    Private Sub workerCarga_DoWork(sender As Object, e As DoWorkEventArgs) Handles workerCarga.DoWork

        listaApps = New List(Of Aplicacion)

        Dim unidades() As String = Directory.GetLogicalDrives

        For Each unidad As String In unidades
            listaApps = GenerarApps(listaApps, unidad)
        Next

    End Sub

    Private Sub workerCarga_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles workerCarga.RunWorkerCompleted

        If listaApps.Count > 0 Then
            listaApps.Sort(Function(x, y) x.Nombre.CompareTo(y.Nombre))

            Dim i As Integer = 0
            While i < listaApps.Count
                Dim grid As New Grid

                '----------------------------------------------------

                Dim col1 As New ColumnDefinition
                Dim col2 As New ColumnDefinition
                Dim col3 As New ColumnDefinition

                col1.Width = New GridLength(1, GridUnitType.Auto)
                col2.Width = New GridLength(1, GridUnitType.Auto)
                col3.Width = New GridLength(1, GridUnitType.Star)

                grid.ColumnDefinitions.Add(col1)
                grid.ColumnDefinitions.Add(col2)
                grid.ColumnDefinitions.Add(col3)

                grid.Margin = New Thickness(10, 5, 10, 5)

                '----------------------------------------------------

                Dim borde As New Border
                borde.Width = 50
                borde.Height = 50
                borde.Background = Brushes.DarkSlateBlue

                Dim imagen As New Image
                imagen.Width = 50
                imagen.Height = 50
                Dim bi3 As New BitmapImage
                bi3.BeginInit()
                bi3.UriSource = New Uri(listaApps(i).Imagen, UriKind.RelativeOrAbsolute)
                bi3.EndInit()
                imagen.Source = bi3

                borde.Child = imagen
                Grid.SetColumn(borde, 0)
                grid.Children.Add(borde)

                '----------------------------------------------------

                Dim textoBloque As New TextBlock
                textoBloque.Text = listaApps(i).Nombre
                textoBloque.VerticalAlignment = VerticalAlignment.Center
                textoBloque.Margin = New Thickness(10, 0, 0, 0)
                textoBloque.FontSize = 15

                Grid.SetColumn(textoBloque, 1)
                grid.Children.Add(textoBloque)

                '----------------------------------------------------

                Dim boton As New Button
                boton.Content = "Create"
                boton.Tag = listaApps(i)
                boton.HorizontalAlignment = HorizontalAlignment.Right
                boton.Padding = New Thickness(10, 0, 10, 0)
                AddHandler boton.Click, AddressOf buttonClick

                Grid.SetColumn(boton, 2)
                grid.Children.Add(boton)

                '----------------------------------------------------

                lvApps.Items.Add(grid)
                i += 1
            End While

            gridCarga.Visibility = Visibility.Collapsed
            gridLista.Visibility = Visibility.Visible
        Else
            gridCarga.Visibility = Visibility.Collapsed
            gridNoApps.Visibility = Visibility.Visible
        End If

    End Sub

    Private Function GenerarApps(listaApps As List(Of Aplicacion), unidad As String) As List(Of Aplicacion)

        Dim subCarpeta As String = Nothing

        If unidad = "C:\" Then
            subCarpeta = "Program Files\WindowsApps"
        Else
            subCarpeta = "WindowsApps"
        End If

        If Directory.Exists(unidad + subCarpeta) Then
            Try
                For Each carpeta As String In Directory.GetDirectories(unidad + subCarpeta)
                    Try
                        For Each fichero As String In Directory.GetFiles(carpeta)
                            If fichero.Contains("AppxManifest.xml") Then

                                Dim lineas As String = File.ReadAllText(fichero)

                                If lineas.Contains("<Application Id=") Then
                                    Dim temp, temp2, temp3 As String
                                    Dim int, int2, int3 As Integer

                                    int = lineas.IndexOf("<Application Id=")
                                    temp = lineas.Remove(0, int)

                                    int2 = temp.IndexOf(Chr(34))
                                    temp2 = temp.Remove(0, int2 + 1)

                                    int3 = temp2.IndexOf(Chr(34))
                                    temp3 = temp2.Remove(int3, temp2.Length - int3)

                                    Dim id As String = temp3

                                    If lineas.Contains("<Identity Name=") Then
                                        Dim temp4, temp5, temp6 As String
                                        Dim int4, int5, int6 As Integer

                                        int4 = lineas.IndexOf("<Identity Name=")
                                        temp4 = lineas.Remove(0, int4)

                                        int5 = temp4.IndexOf(Chr(34))
                                        temp5 = temp4.Remove(0, int5 + 1)

                                        int6 = temp5.IndexOf(Chr(34))
                                        temp6 = temp5.Remove(int6, temp5.Length - int6)

                                        If Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\AppData\Local\Packages") Then
                                            For Each carpeta_ As String In Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\AppData\Local\Packages")
                                                If carpeta_.Contains(temp6) Then
                                                    Dim int7 As Integer = carpeta_.LastIndexOf("\")
                                                    Dim package As String = carpeta_.Remove(0, int7 + 1)

                                                    Dim temp8, temp9, temp10 As String
                                                    Dim int8, int9, int10 As Integer

                                                    int8 = lineas.IndexOf("<DisplayName>")
                                                    temp8 = lineas.Remove(0, int8)

                                                    int9 = temp8.IndexOf(">")
                                                    temp9 = temp8.Remove(0, int9 + 1)

                                                    int10 = temp9.IndexOf("</DisplayName>")
                                                    temp10 = temp9.Remove(int10, temp9.Length - int10)

                                                    If temp10 = "ms-resource:AppStoreName" Then
                                                        temp10 = TituloMicrosoft.Buscar(lineas)
                                                    End If

                                                    If temp10 = "ms-resource:StoreTitle" Then
                                                        temp10 = TituloMicrosoft.Buscar(lineas)
                                                    End If

                                                    If temp10 = "ms-resource:ApplicationTitleWithBranding" Then
                                                        temp10 = TituloMicrosoft.Buscar(lineas)
                                                    End If

                                                    If temp10 = "ms-resource:///Resources/AppStoreName" Then
                                                        temp10 = TituloMicrosoft.Buscar(lineas)
                                                    End If

                                                    If temp10 = "ms-resource:/MSWifiResources/AppStoreName" Then
                                                        temp10 = TituloMicrosoft.Buscar(lineas)
                                                    End If

                                                    If temp10 = "ms-resource:IDS_MANIFEST_MUSIC_APP_NAME" Then
                                                        temp10 = TituloMicrosoft.Buscar(lineas)
                                                    End If

                                                    If temp10 = "ms-resource:IDS_MANIFEST_VIDEO_APP_NAME" Then
                                                        temp10 = TituloMicrosoft.Buscar(lineas)
                                                    End If

                                                    If Not temp10.Contains("ms-resource:") Then
                                                        Dim temp11, temp12, temp13 As String
                                                        Dim int11, int12, int13 As Integer

                                                        int11 = lineas.IndexOf("<Logo>")
                                                        temp11 = lineas.Remove(0, int11)

                                                        int12 = temp11.IndexOf(">")
                                                        temp12 = temp11.Remove(0, int12 + 1)

                                                        int13 = temp12.IndexOf("</Logo>")
                                                        temp13 = temp12.Remove(int13, temp12.Length - int13)

                                                        temp13 = carpeta + "\" + temp13

                                                        If Not File.Exists(temp13) Then
                                                            Dim int14 As Integer = temp13.LastIndexOf(".")
                                                            temp13 = temp13.Insert(int14, ".scale-100")
                                                        End If

                                                        If Not File.Exists(temp13) Then
                                                            temp13 = temp13.Replace(".scale-100", Nothing)
                                                            Dim int14 As Integer = temp13.LastIndexOf(".")
                                                            temp13 = temp13.Insert(int14, ".scale-125")
                                                        End If

                                                        If Not File.Exists(temp13) Then
                                                            temp13 = temp13.Replace(".scale-125", Nothing)
                                                            Dim int14 As Integer = temp13.LastIndexOf(".")
                                                            temp13 = temp13.Insert(int14, ".scale-150")
                                                        End If

                                                        If Not File.Exists(temp13) Then
                                                            temp13 = temp13.Replace(".scale-150", Nothing)
                                                            Dim int14 As Integer = temp13.LastIndexOf(".")
                                                            temp13 = temp13.Insert(int14, ".scale-200")
                                                        End If

                                                        If Not File.Exists(temp13) Then
                                                            temp13 = temp13.Replace(".scale-200", Nothing)
                                                            Dim int14 As Integer = temp13.LastIndexOf(".")
                                                            temp13 = temp13.Insert(int14, ".scale-400")
                                                        End If

                                                        Dim tituloBool As Boolean = False
                                                        Dim i As Integer = 0
                                                        While i < listaApps.Count
                                                            If listaApps(i).Nombre = temp10 Then
                                                                tituloBool = True
                                                            End If
                                                            i += 1
                                                        End While

                                                        If tituloBool = False Then
                                                            listaApps.Add(New Aplicacion(temp10, "shell:AppsFolder\" + package + "!" + id, temp13))
                                                        End If
                                                    End If
                                                End If
                                            Next
                                        End If
                                    End If
                                End If
                            End If
                        Next
                    Catch ex As Exception

                    End Try
                Next
            Catch ex As Exception
                workerCarga.ReportProgress(0, unidad + "Program Files\WindowsApps")
            End Try
        End If

        Return listaApps
    End Function

    Private Sub workerCarga_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles workerCarga.ProgressChanged

        If Not e.UserState.ToString = Nothing Then
            gridPermisos.Visibility = Visibility.Visible
            gridGreenlight.Visibility = Visibility.Collapsed

            If Not tbPermisos.Text = Nothing Then
                tbPermisos.Text = tbPermisos.Text + Environment.NewLine + e.UserState
            Else
                tbPermisos.Text = "Add read permissions to:" + Environment.NewLine + e.UserState
            End If
        End If

    End Sub

    Private Async Sub buttonClick(ByVal sender As Object, ByVal e As RoutedEventArgs)

        lvApps.IsEnabled = False

        Dim steamActivo As Boolean = False

        Try
            Process.GetProcessesByName("Steam")(0).Kill()
            steamActivo = True
        Catch ex As Exception
            steamActivo = False
        End Try

        If steamActivo = True Then
            Threading.Thread.Sleep(5000)
        End If

        Dim boton As Button = e.Source
        Dim app As Aplicacion = TryCast(boton.Tag, Aplicacion)

        Dim textoUsuarioID As String = Nothing
        Dim usuarioID As String = Nothing
        Dim rutaSteam As String = Nothing

        Try
            rutaSteam = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\Valve\Steam", "SteamPath", Nothing)
        Catch ex As Exception
            MsgBox("Steam not detected")
        End Try

        If Not rutaSteam = Nothing Then
            rutaSteam = rutaSteam.Replace("/", "\")

            textoUsuarioID = File.ReadAllText(rutaSteam + "\logs\connection_log.txt")

            If textoUsuarioID.Contains("SetSteamID") Then
                Dim temp, temp2, temp3 As String
                Dim int, int2, int3 As Integer

                int = textoUsuarioID.LastIndexOf("SetSteamID")
                temp = textoUsuarioID.Remove(0, int + 5)

                int2 = temp.IndexOf("[U:1:")
                temp2 = temp.Remove(0, int2 + 5)

                int3 = temp2.IndexOf("]")
                temp3 = temp2.Remove(int3, temp2.Length - int3)

                usuarioID = temp3

                If usuarioID.Length > 1 Then

                    Dim lineas As String = Nothing

                    If Not File.Exists(rutaSteam + "\userdata\" + usuarioID + "\config\shortcuts.vdf") Then
                        File.Create(rutaSteam + "\userdata\" + usuarioID + "\config\shortcuts.vdf").Close()
                    Else
                        Using sr As StreamReader = New StreamReader(rutaSteam + "\userdata\" + usuarioID + "\config\shortcuts.vdf")
                            lineas = lineas + Await sr.ReadLineAsync
                        End Using
                    End If

                    Dim numero As Integer

                    If Not lineas = Nothing Then
                        If lineas.Contains("appname") Then
                            Dim lineasTemp As String = lineas

                            Dim temp4 As String
                            Dim int4 As Integer

                            Dim i As Integer = 0
                            While i < 1000
                                If lineasTemp.Contains("appname") Then
                                    int4 = lineasTemp.IndexOf("appname")
                                    temp4 = lineasTemp.Remove(0, int4 + 2)
                                    lineasTemp = temp4
                                    numero += 1
                                End If
                                i += 1
                            End While
                        Else
                            numero = 0
                        End If
                    Else
                        numero = 0
                    End If

                    If Not lineas = Nothing Then
                        lineas = lineas.Remove(lineas.Length - 2, 2)
                    Else
                        lineas = Chr(0) + "shortcuts" + Chr(0)
                    End If

                    lineas = lineas + Chr(0) + numero.ToString + Chr(0) + Chr(1) + "appname" + Chr(0) + app.Nombre + Chr(0) + Chr(1) + "exe" + Chr(0) + Chr(34) + app.AccesoDirecto + Chr(34) + Chr(0) + Chr(1) + "StartDir" + Chr(0) + Chr(34) + "C:\Windows\" + Chr(34) + Chr(0) + Chr(1) + "icon" + Chr(0) + app.Imagen + Chr(0) + Chr(1) + "ShortcutPath" + Chr(0) + Chr(0) + Chr(2) + "IsHidden" + Chr(0) + Chr(0) + Chr(0) + Chr(0) + Chr(0) + Chr(2) + "AllowDesktopConfig" + Chr(0) + Chr(1) + Chr(0) + Chr(0) + Chr(0) + Chr(2) + "OpenVR" + Chr(0) + Chr(0) + Chr(0) + Chr(0) + Chr(0) + Chr(0) + "tags" + Chr(0) + Chr(8) + Chr(8)
                    lineas = lineas + Chr(8) + Chr(8)

                    Using sw As StreamWriter = New StreamWriter(rutaSteam + "\userdata\" + usuarioID + "\config\shortcuts.vdf", False, Encoding.ASCII)
                        Await sw.WriteAsync(lineas)
                    End Using

                    Process.Start(rutaSteam + "\steam.exe")
                Else
                    MsgBox("Steam User not found")
                End If
            Else
                MsgBox("Login a user in Steam")
            End If
        Else
            MsgBox("Steam not detected")
        End If

        lvApps.IsEnabled = True

    End Sub

    Private Sub buttonGreenlight_Click(sender As Object, e As RoutedEventArgs) Handles buttonGreenlight.Click

        Try
            Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=700966331")
            FicherosINI.Escribir(My.Application.Info.DirectoryPath + "\Config.ini", "Options", "Greenlight", "True")
            gridGreenlight.Visibility = Visibility.Collapsed
        Catch ex As Exception

        End Try

    End Sub

End Class
