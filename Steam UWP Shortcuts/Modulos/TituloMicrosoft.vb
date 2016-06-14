Module TituloMicrosoft

    Public Function Buscar(texto As String)

        Dim temp, temp2, temp3 As String
        Dim int, int2, int3 As Integer

        int = texto.IndexOf("<PublisherDisplayName>")
        temp = texto.Remove(0, int)

        int2 = temp.IndexOf(">")
        temp2 = temp.Remove(0, int2 + 1)

        int3 = temp2.IndexOf("</PublisherDisplayName>")
        temp3 = temp2.Remove(int3, temp2.Length - int3)

        If temp3 = "Microsoft Corporation" Then
            int = texto.IndexOf("<Identity Name=")
            temp = texto.Remove(0, int)

            int2 = temp.IndexOf(Chr(34))
            temp2 = temp.Remove(0, int2 + 1)

            int3 = temp2.IndexOf(Chr(34))
            temp3 = temp2.Remove(int3, temp2.Length - int3)

            temp3 = temp3.Replace("Microsoft.", Nothing)
            temp3 = temp3.Replace("Windows.", Nothing)
            temp3 = temp3.Replace("Windows", Nothing)
            temp3 = temp3.Replace("Zune", Nothing)
            temp3 = temp3.Replace("Bing", Nothing)
        End If

        If temp3 = "Microsoft Studios" Then
            int = texto.IndexOf("<Identity Name=")
            temp = texto.Remove(0, int)

            int2 = temp.IndexOf(Chr(34))
            temp2 = temp.Remove(0, int2 + 1)

            int3 = temp2.IndexOf(Chr(34))
            temp3 = temp2.Remove(int3, temp2.Length - int3)

            temp3 = temp3.Replace("Microsoft.", Nothing)

            If Not FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config.ini", "GamesMicrosoft", temp3) = Nothing Then
                temp3 = FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config.ini", "GamesMicrosoft", temp3)
            End If
        End If

        Return temp3
    End Function

End Module
