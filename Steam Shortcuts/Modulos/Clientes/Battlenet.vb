Imports System.ComponentModel

Module Battlenet

    Public Function GenerarJuegos(lista As List(Of Aplicacion), bw As BackgroundWorker) As List(Of Aplicacion)

        Dim registroJuegos As Microsoft.Win32.RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\")

        If registroJuegos Is Nothing Then
            registroJuegos = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\")
        End If

        If Not registroJuegos Is Nothing Then
            Log.Actualizar("Battle.net", "001 " + registroJuegos.GetSubKeyNames.Count.ToString)
            For Each registro In registroJuegos.GetSubKeyNames
                Dim compañia As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + registro.ToString, "Publisher", Nothing)

                If compañia = Nothing Then
                    compañia = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + registro.ToString, "Publisher", Nothing)
                End If

                Dim compañiaBool As Boolean = False

                If Not compañia = Nothing Then
                    If compañia.Contains("Blizzard") Then
                        compañiaBool = True
                    End If
                End If

                If compañiaBool = True Then
                    Log.Actualizar("Battle.net", "002 Possible Matches")
                    Dim titulo As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + registro.ToString, "DisplayName", Nothing)

                    If titulo = Nothing Then
                        titulo = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + registro.ToString, "DisplayName", Nothing)
                    End If

                    If Not titulo = Nothing Then
                        If Not titulo = "Battle.net" Then
                            Dim iconoRuta As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + registro.ToString, "DisplayIcon", Nothing)

                            If iconoRuta = Nothing Then
                                iconoRuta = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + registro.ToString, "DisplayIcon", Nothing)
                            End If

                            If Not iconoRuta = Nothing Then
                                iconoRuta = iconoRuta.Replace(Chr(34), Nothing)
                                iconoRuta = iconoRuta.Replace(vbNullChar, Nothing)
                            End If

                            Dim icono As String = Iconos.Generar(iconoRuta, titulo)

                            Log.Actualizar("Battle.net", "003 " + titulo)

                            Dim ejecutable As String = Nothing

                            If Not FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config\Battlenet.ini", "Games", titulo) = Nothing Then
                                ejecutable = "battlenet://" + FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config\Battlenet.ini", "Games", titulo)
                            End If

                            Log.Actualizar("Battle.net", "004 " + ejecutable)

                            Dim tituloBool As Boolean = False
                            Dim i As Integer = 0
                            While i < lista.Count
                                If lista(i).Nombre = titulo Then
                                    tituloBool = True
                                End If
                                i += 1
                            End While

                            Dim categoria As String = Nothing

                            If Not FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config\Config.ini", "Category", "Battlenet") = Nothing Then
                                categoria = FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config\Config.ini", "Category", "Battlenet")
                            End If

                            If tituloBool = False Then
                                If Not ejecutable = Nothing Then
                                    lista.Add(New Aplicacion(titulo, ejecutable, Nothing, icono, Nothing, False, categoria))
                                End If
                            End If
                        End If
                    End If
                End If
            Next
        End If

        Return lista
    End Function

End Module
