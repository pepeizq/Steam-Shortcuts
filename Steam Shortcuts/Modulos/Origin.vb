Imports System.ComponentModel

Module Origin

    Public Function GenerarJuegos(lista As List(Of Aplicacion), bw As BackgroundWorker) As List(Of Aplicacion)

        Dim cliente As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Origin\", "ClientPath", Nothing)

        If Not cliente = Nothing Then
            Dim registroJuegos As Microsoft.Win32.RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\")

            If Not registroJuegos Is Nothing Then
                For Each registro In registroJuegos.GetSubKeyNames
                    Dim compañia As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + registro.ToString, "Publisher", Nothing)
                    Dim compañiaBool As Boolean = False

                    If Not compañia = Nothing Then
                        If compañia.Contains("Electronic Arts") Then
                            compañiaBool = True
                        End If

                        If compañia.Contains("PopCap Games") Then
                            compañiaBool = True
                        End If
                    End If

                    If compañiaBool = True Then
                        Dim titulo As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + registro.ToString, "DisplayName", Nothing)

                        Dim iconoRuta As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + registro.ToString, "DisplayIcon", Nothing)

                        If Not iconoRuta = Nothing Then
                            iconoRuta = iconoRuta.Replace(Chr(34), Nothing)
                            iconoRuta = iconoRuta.Replace(vbNullChar, Nothing)
                        End If

                        Dim icono As String = Iconos.Generar(iconoRuta, titulo)

                        Dim registroIDs As Microsoft.Win32.RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\WOW6432Node\Origin Games\")
                        Dim ejecutable As String = Nothing
                        Dim listaIDs As New List(Of Integer)

                        If Not registroIDs Is Nothing Then
                            For Each registroID In registroIDs.GetSubKeyNames
                                Dim tituloID As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Origin Games\" + registroID, "DisplayName", Nothing)

                                If tituloID.Contains(titulo) Then
                                    listaIDs.Add(registroID)
                                End If
                            Next
                        End If

                        If listaIDs.Count = 0 Then
                            If Not registroIDs Is Nothing Then
                                For Each registroID In registroIDs.GetSubKeyNames
                                    Dim tituloID As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Origin Games\" + registroID, "DisplayName", Nothing)

                                    If Not titulo = Nothing Then
                                        titulo = titulo.Replace("™", Nothing)
                                        titulo = titulo.Replace("®", Nothing)
                                        titulo = titulo.Replace("'", Nothing)
                                    End If

                                    If Not tituloID = Nothing Then
                                        tituloID = tituloID.Replace("™", Nothing)
                                        tituloID = tituloID.Replace("®", Nothing)
                                        tituloID = tituloID.Replace("'", Nothing)
                                    End If

                                    If tituloID = titulo Then
                                        listaIDs.Add(registroID)
                                    End If
                                Next
                            End If
                        End If

                        If listaIDs.Count > 0 Then
                            listaIDs.Sort(Function(x, y) x.CompareTo(y))

                            ejecutable = "origin://launchgame/" + listaIDs(0).ToString
                        End If

                        Dim tituloBool As Boolean = False
                        Dim i As Integer = 0
                        While i < lista.Count
                            If lista(i).Nombre = titulo Then
                                tituloBool = True
                            End If
                            i += 1
                        End While

                        Dim categoria As String = Nothing

                        If Not FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config.ini", "Category", "Origin") = Nothing Then
                            categoria = FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config.ini", "Category", "Origin")
                        End If

                        If tituloBool = False Then
                            If Not ejecutable = Nothing Then
                                lista.Add(New Aplicacion(titulo, cliente, ejecutable, icono, Nothing, False, categoria))
                            End If
                        End If
                    End If
                Next
            End If
        End If

        Return lista
    End Function

End Module
