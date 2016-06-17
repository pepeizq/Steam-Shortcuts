Imports System.ComponentModel
Imports System.IO

Module GOGGalaxy

    Public Function GenerarJuegos(listaApps As List(Of Aplicacion), carpeta As String, bw As BackgroundWorker) As List(Of Aplicacion)

        If Directory.Exists(carpeta) Then

            Dim carpetaFinal As String = Nothing

            If carpeta.Contains("C:\") Then
                carpetaFinal = carpeta + "\Games"
            Else
                carpetaFinal = carpeta
            End If

            If Directory.Exists(carpetaFinal) Then
                For Each carpetaJuego As String In Directory.GetDirectories(carpetaFinal)
                    For Each fichero As String In Directory.GetFiles(carpetaJuego)
                        If fichero.Contains("goggame-") And fichero.Contains(".info") Then

                            Dim lineas As String = File.ReadAllText(fichero)

                            If lineas.Contains(Chr(34) + "name" + Chr(34)) Then
                                Dim temp, temp2, temp3 As String
                                Dim int, int2, int3 As Integer

                                int = lineas.IndexOf(Chr(34) + "name" + Chr(34))
                                temp = lineas.Remove(0, int + 6)

                                int2 = temp.IndexOf(Chr(34))
                                temp2 = temp.Remove(0, int2 + 1)

                                int3 = temp2.IndexOf(Chr(34))
                                temp3 = temp2.Remove(int3, temp2.Length - int3)


                            End If
                        End If
                    Next
                Next
            End If
        End If

        Return listaApps
    End Function

End Module
