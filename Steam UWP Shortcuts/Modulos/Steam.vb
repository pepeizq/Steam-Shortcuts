Imports System.IO
Imports System.Text

Module Steam

    Public Async Sub CrearAccesos(listaApps As List(Of Aplicacion), cb As CheckBox, boton As Button)

        boton.IsEnabled = False

        Dim listaAñadir As List(Of Aplicacion) = New List(Of Aplicacion)

        For Each app As Aplicacion In listaApps
            If app.Añadir = True Then
                listaAñadir.Add(app)
            End If
        Next

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

                    For Each app As Aplicacion In listaAñadir
                        Dim categoria As String = Nothing

                        If cb.IsChecked = True Then
                            categoria = Chr(1) + "0" + Chr(0) + app.Categoria + Chr(0)
                        End If

                        lineas = lineas + Chr(0) + numero.ToString + Chr(0) + Chr(1) + "appname" + Chr(0) + app.Nombre + Chr(0) + Chr(1) + "exe" + Chr(0) + Chr(34) + "C:\Windows\explorer.exe" + Chr(34) + " " + app.AccesoDirecto + Chr(0) + Chr(1) + "StartDir" + Chr(0) + Chr(34) + "C:\Windows\" + Chr(34) + Chr(0) + Chr(1) + "icon" + Chr(0) + app.Imagen + Chr(0) + Chr(1) + "ShortcutPath" + Chr(0) + Chr(0) + Chr(2) + "IsHidden" + Chr(0) + Chr(0) + Chr(0) + Chr(0) + Chr(0) + Chr(2) + "AllowDesktopConfig" + Chr(0) + Chr(1) + Chr(0) + Chr(0) + Chr(0) + Chr(2) + "OpenVR" + Chr(0) + Chr(0) + Chr(0) + Chr(0) + Chr(0) + Chr(0) + "tags" + Chr(0) + categoria + Chr(8) + Chr(8)

                        numero += 1
                    Next

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

        boton.IsEnabled = True

    End Sub

End Module
