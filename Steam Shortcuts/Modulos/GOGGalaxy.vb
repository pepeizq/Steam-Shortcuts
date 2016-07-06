Imports System.ComponentModel
Imports System.IO

Module GOGGalaxy

    Public Function GenerarJuegos(lista As List(Of Aplicacion), bw As BackgroundWorker) As List(Of Aplicacion)

        Dim registroJuegos As Microsoft.Win32.RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\WOW6432Node\GOG.com\Games\")

        If Not registroJuegos Is Nothing Then
            For Each registro In registroJuegos.GetSubKeyNames
                Dim nombre As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\GOG.com\Games\" + registro, "GAMENAME", Nothing)
                Dim carpeta As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\GOG.com\Games\" + registro, "PATH", Nothing)

                Dim ejecutable As String = Nothing

                For Each fichero As String In Directory.GetFiles(carpeta)
                    If fichero.Contains("Launch " + nombre + ".lnk") Then
                        ejecutable = fichero
                    End If
                Next

                If ejecutable = Nothing Then
                    ejecutable = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\GOG.com\Games\" + registro, "LAUNCHCOMMAND", Nothing)
                End If

                Dim icono As String = Nothing

                For Each ficheroIcono As String In Directory.GetFiles(carpeta)
                    If ficheroIcono.Contains("goggame-") And ficheroIcono.Contains(".ico") Then
                        icono = ficheroIcono
                    End If
                Next

                Dim categoria As String = Nothing

                If Not FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config\Config.ini", "Category", "GOG") = Nothing Then
                    categoria = FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config\Config.ini", "Category", "GOG")
                End If

                Dim tituloBool As Boolean = False
                Dim i As Integer = 0
                While i < lista.Count
                    If lista(i).Nombre = nombre Then
                        tituloBool = True
                    End If
                    i += 1
                End While

                If tituloBool = False Then
                    lista.Add(New Aplicacion(nombre, ejecutable, Nothing, icono, Nothing, False, categoria))
                End If
            Next
        End If

        Return lista
    End Function

End Module
