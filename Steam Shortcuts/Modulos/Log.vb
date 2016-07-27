Imports System.IO

Module Log

    Public Sub Actualizar(categoria As String, mensaje As String)

        Try
            If Not categoria = Nothing Then
                categoria = categoria + ": "
            End If

            If Not File.ReadAllText(My.Application.Info.DirectoryPath + "\Config\Log.txt").Contains(categoria + mensaje) Then
                File.AppendAllText(My.Application.Info.DirectoryPath + "\Config\Log.txt", Environment.NewLine + "[" + Now.TimeOfDay.ToString + "] - " + categoria + mensaje)
            End If

        Catch ex As Exception

        End Try

    End Sub

End Module
