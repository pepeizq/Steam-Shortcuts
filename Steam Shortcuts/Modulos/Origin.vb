Imports System.ComponentModel

Module Origin

    Public Function GenerarJuegos(lista As List(Of Aplicacion), bw As BackgroundWorker) As List(Of Aplicacion)

        Dim cliente As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Origin\", "ClientPath", Nothing)

        If Not cliente = Nothing Then
            Dim registroJuegos As Microsoft.Win32.RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\WOW6432Node\Origin Games\")

            If Not registroJuegos Is Nothing Then
                For Each registro In registroJuegos.GetSubKeyNames
                    Dim titulo As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Origin Games\" + registro.ToString, "DisplayName", Nothing)

                    Dim argumentos As String = "origin://launchgame/" + registro.ToString

                    Dim categoria As String = Nothing

                    If Not FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config.ini", "Category", "Origin") = Nothing Then
                        categoria = FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config.ini", "Category", "Origin")
                    End If

                    Dim tituloBool As Boolean = False
                    Dim i As Integer = 0
                    While i < lista.Count
                        If lista(i).Nombre = titulo Then
                            Dim limpiarArgumentos As String = lista(i).Argumentos.Replace("origin://launchgame/", Nothing)

                            Try
                                If Convert.ToInt32(limpiarArgumentos) > Convert.ToInt32(registro) Then
                                    lista.RemoveAt(i)
                                    tituloBool = False
                                Else
                                    tituloBool = True
                                End If
                            Catch ex As Exception
                                tituloBool = True
                            End Try
                        End If
                        i += 1
                    End While

                    Dim registroUninstall As Microsoft.Win32.RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\")

                    If Not registroUninstall Is Nothing Then
                        For Each registroUni In registroUninstall.GetSubKeyNames
                            Dim tituloUninstall As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + registroUni.ToString, "DisplayName", Nothing)

                            If Not tituloUninstall = Nothing Then
                                If Not titulo = Nothing Then
                                    titulo = titulo.Replace("™", Nothing)
                                    titulo = titulo.Replace("®", Nothing)
                                End If

                                If Not tituloUninstall = Nothing Then
                                    tituloUninstall = tituloUninstall.Replace("™", Nothing)
                                    tituloUninstall = tituloUninstall.Replace("®", Nothing)
                                End If

                                If titulo.Contains(tituloUninstall) Then
                                    Dim exe As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + registroUni.ToString, "DisplayIcon", Nothing)
                                    Dim icono As String = Iconos.Generar(exe, titulo)

                                    i = 0
                                    While i < lista.Count
                                        If lista(i).Nombre = titulo Then
                                            tituloBool = True
                                        End If
                                        i += 1
                                    End While

                                    If tituloBool = False Then
                                        lista.Add(New Aplicacion(titulo, cliente, argumentos, icono, Nothing, False, categoria))
                                    End If
                                End If
                            End If
                        Next
                    End If
                Next
            End If
        End If

        Return lista
    End Function

End Module
