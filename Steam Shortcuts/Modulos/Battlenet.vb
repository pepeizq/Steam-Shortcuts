Imports System.ComponentModel

Module Battlenet

    Public Function GenerarJuegos(lista As List(Of Aplicacion), bw As BackgroundWorker) As List(Of Aplicacion)

        Dim cliente As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Blizzard Entertainment\Battle.net\Capabilities\", "ApplicationIcon", Nothing)

        If Not cliente = Nothing Then
            Dim registroJuegos As Microsoft.Win32.RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\")

            If Not registroJuegos Is Nothing Then
                For Each registro In registroJuegos.GetSubKeyNames
                    Dim compañia As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + registro.ToString, "Publisher", Nothing)
                    Dim compañiaBool As Boolean = False

                    If Not compañia = Nothing Then
                        If compañia.Contains("Blizzard") Then
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

                        Dim ejecutable As String = Nothing

                        If Not FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config\Battlenet.ini", "Games", titulo) = Nothing Then
                            ejecutable = "battlenet://" + FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config\Battlenet.ini", "Games", titulo)
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

                        If Not FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config\Config.ini", "Category", "Battlenet") = Nothing Then
                            categoria = FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config\Config.ini", "Category", "Battlenet")
                        End If

                        If tituloBool = False Then
                            If Not ejecutable = Nothing Then
                                lista.Add(New Aplicacion(titulo, ejecutable, Nothing, icono, Nothing, False, categoria))
                            End If
                        End If
                    End If
                Next
            End If
        End If

        Return lista
    End Function

End Module
