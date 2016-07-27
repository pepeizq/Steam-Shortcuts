Imports System.ComponentModel
Imports System.IO

Module Uplay

    Public Function GenerarJuegos(lista As List(Of Aplicacion), bw As BackgroundWorker) As List(Of Aplicacion)

        Dim registroJuegos As Microsoft.Win32.RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\WOW6432Node\Ubisoft\Launcher\Installs")

        If registroJuegos Is Nothing Then
            registroJuegos = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Ubisoft\Launcher\Installs")
        End If

        If Not registroJuegos Is Nothing Then
            Log.Actualizar("Uplay", "001 " + registroJuegos.GetSubKeyNames.Count.ToString)
            For Each registro In registroJuegos.GetSubKeyNames
                Dim ejecutable As String = "uplay://launch/" + registro.ToString + "/0"

                Log.Actualizar("Uplay", "002 " + ejecutable)

                Dim identificador As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Ubisoft\Launcher\Installs\" + registro.ToString, "InstallDir", Nothing)

                If identificador = Nothing Then
                    identificador = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Ubisoft\Launcher\Installs\" + registro.ToString, "InstallDir", Nothing)
                End If

                Log.Actualizar("Uplay", "003 " + identificador)

                If Not identificador = Nothing Then
                    If identificador.LastIndexOf("/") = identificador.Length - 1 Then
                        identificador = identificador.Remove(identificador.Length - 1, 1)
                    End If

                    If identificador.Contains("/") Then
                        Dim int As Integer
                        int = identificador.LastIndexOf("/")
                        identificador = identificador.Remove(0, int + 1)
                    End If

                    Dim carpeta As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Ubisoft\Launcher", "InstallDir", Nothing)

                    If carpeta = Nothing Then
                        carpeta = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Ubisoft\Launcher", "InstallDir", Nothing)
                    End If

                    If Not carpeta = Nothing Then
                        carpeta = carpeta + "cache\configuration"

                        Log.Actualizar("Uplay", "004 " + carpeta)

                        For Each fichero As String In Directory.GetFiles(carpeta)
                            Dim lineas As String = File.ReadAllText(fichero)

                            If Not lineas = Nothing Then
                                If lineas.Contains("game_identifier: " + identificador) Then
                                    Dim temp2, temp3, temp4, temp5 As String
                                    Dim int2, int3, int4, int5 As Integer

                                    int2 = lineas.IndexOf("game_identifier: " + identificador)
                                    temp2 = lineas.Remove(int2, lineas.Length - int2)

                                    int3 = temp2.LastIndexOf("icon_image")
                                    temp3 = temp2.Remove(0, int3)

                                    int4 = temp3.IndexOf(":")
                                    temp4 = temp3.Remove(0, int4 + 1)

                                    int5 = temp4.IndexOf(Chr(10))
                                    temp5 = temp4.Remove(int5, temp4.Length - int5)

                                    Dim icono As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Ubisoft\Launcher", "InstallDir", Nothing) + "\data\games\" + temp5.Trim

                                    If icono = Nothing Then
                                        icono = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Ubisoft\Launcher", "InstallDir", Nothing) + "\data\games\" + temp5.Trim
                                    End If

                                    If Not icono = Nothing Then
                                        If icono.Contains("\\") Then
                                            icono = icono.Replace("\\", "\")
                                        End If
                                    End If

                                    Log.Actualizar("Uplay", "005 " + icono)

                                    Dim categoria As String = Nothing

                                    If Not FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config\Config.ini", "Category", "Uplay") = Nothing Then
                                        categoria = FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config\Config.ini", "Category", "Uplay")
                                    End If

                                    Dim tituloBool As Boolean = False
                                    Dim i As Integer = 0
                                    While i < lista.Count
                                        If lista(i).Nombre = identificador Then
                                            tituloBool = True
                                        End If
                                        i += 1
                                    End While

                                    If tituloBool = False Then
                                        lista.Add(New Aplicacion(identificador, ejecutable, Nothing, icono, Nothing, False, categoria))
                                    End If
                                End If
                            End If
                        Next
                    End If
                End If
            Next
        End If

        Return lista
    End Function

End Module
