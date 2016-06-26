Imports System.ComponentModel
Imports System.IO

Module Uplay

    Public Function GenerarJuegos(lista As List(Of Aplicacion), bw As BackgroundWorker) As List(Of Aplicacion)

        Dim registroMaestro As Microsoft.Win32.RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\WOW6432Node\Ubisoft\Launcher\Installs")

        If Not registroMaestro Is Nothing Then
            For Each registro In registroMaestro.GetSubKeyNames
                Dim ejecutable As String = "uplay://launch/" + registro.ToString + "/0"

                Dim identificador As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Ubisoft\Launcher\Installs\" + registro.ToString, "InstallDir", Nothing)

                If Not identificador = Nothing Then
                    Dim int As Integer
                    identificador = identificador.Remove(identificador.Length - 1, 1)

                    int = identificador.LastIndexOf("/")
                    identificador = identificador.Remove(0, int + 1)
                End If

                Dim carpeta As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Ubisoft\Launcher", "InstallDir", Nothing)

                If Not carpeta = Nothing Then
                    carpeta = carpeta + "cache\configuration"

                    For Each fichero As String In Directory.GetFiles(carpeta)
                        Dim lineas As String = File.ReadAllText(fichero)

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

                            Dim categoria As String = Nothing

                            If Not FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config.ini", "Category", "Uplay") = Nothing Then
                                categoria = FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config.ini", "Category", "Uplay")
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
                    Next
                End If
            Next
        End If

        Return lista
    End Function

End Module
