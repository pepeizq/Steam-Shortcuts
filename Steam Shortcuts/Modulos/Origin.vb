﻿Imports System.ComponentModel
Imports System.IO

Module Origin

    Public Function GenerarJuegos(lista As List(Of Aplicacion), bw As BackgroundWorker) As List(Of Aplicacion)

        Dim registroJuegos As Microsoft.Win32.RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\")

        If Not registroJuegos Is Nothing Then
            For Each registro In registroJuegos.GetSubKeyNames
                If Directory.Exists("C:\ProgramData\Origin") Then
                    Dim localizacion As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + registro, "InstallLocation", Nothing)

                    If Not localizacion = Nothing Then
                        If localizacion.LastIndexOf("\") = (localizacion.Length - 1) Then
                            localizacion = localizacion.Remove(localizacion.Length - 1, 1)
                        End If

                        Dim int As Integer = localizacion.LastIndexOf("\")
                        Dim clave As String = localizacion.Remove(0, int + 1)

                        For Each carpeta As String In Directory.GetDirectories("C:\ProgramData\Origin\LocalContent")
                            If carpeta.Contains(clave) Then
                                For Each fichero As String In Directory.GetFiles(carpeta)
                                    If Not fichero.Contains("map.crc") Then

                                        Dim int2 As Integer = fichero.LastIndexOf("\")
                                        Dim temp2 As String = fichero.Remove(0, int2 + 1)

                                        Dim int3 As Integer = temp2.LastIndexOf(".")
                                        Dim ejecutable As String = "origin://launchgame/" + temp2.Remove(int3, temp2.Length - int3)

                                        If ejecutable.Contains("OFB-EAST") Then
                                            ejecutable = ejecutable.Replace("OFB-EAST", "OFB-EAST:")
                                        End If

                                        If ejecutable.Contains("DR") Then
                                            ejecutable = ejecutable.Replace("DR", "DR:")
                                        End If

                                        Dim titulo As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + registro, "DisplayName", Nothing)

                                        Dim tituloBool As Boolean = False
                                        Dim i As Integer = 0
                                        While i < lista.Count
                                            If lista(i).Nombre = titulo Then
                                                tituloBool = True
                                            End If
                                            i += 1
                                        End While

                                        If titulo = "Origin" Then
                                            tituloBool = True
                                        End If

                                        If tituloBool = False Then
                                            Dim iconoRuta As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + registro, "DisplayIcon", Nothing)

                                            If Not iconoRuta = Nothing Then
                                                iconoRuta = iconoRuta.Replace(Chr(34), Nothing)
                                                iconoRuta = iconoRuta.Replace(vbNullChar, Nothing)
                                            End If

                                            Dim icono As String = Iconos.Generar(iconoRuta, titulo)

                                            Dim categoria As String = Nothing

                                            If Not FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config\Config.ini", "Category", "Origin") = Nothing Then
                                                categoria = FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config\Config.ini", "Category", "Origin")
                                            End If

                                            If Not ejecutable = Nothing Then
                                                lista.Add(New Aplicacion(titulo, ejecutable, Nothing, icono, Nothing, False, categoria))
                                            End If
                                        End If
                                    End If
                                Next
                            End If
                        Next
                    End If
                End If
            Next
        End If

        Return lista
    End Function

End Module
