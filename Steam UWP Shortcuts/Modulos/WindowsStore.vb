Imports System.ComponentModel
Imports System.IO

Module WindowsStore

    Public Function GenerarApps(listaApps As List(Of Aplicacion), unidad As String, bw As BackgroundWorker) As List(Of Aplicacion)

        Dim carpetaFinal As String = Nothing

        If unidad = "C:\" Then
            carpetaFinal = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\WindowsApps"
            carpetaFinal = carpetaFinal.Replace(" (x86)", Nothing)
        Else
            carpetaFinal = unidad + "WindowsApps"
        End If

        If Directory.Exists(carpetaFinal) Then
            Try
                Dim i As Integer = 0

                bw.ReportProgress(0, unidad)

                For Each carpeta As String In Directory.GetDirectories(carpetaFinal)
                    Try
                        i += 1
                        bw.ReportProgress(CInt((100 / (Directory.GetDirectories(carpetaFinal).Count)) * i))

                        For Each fichero As String In Directory.GetFiles(carpeta)
                            If fichero.Contains("AppxManifest.xml") Then

                                Dim lineas As String = File.ReadAllText(fichero)

                                If lineas.Contains("<Application Id=") Then
                                    Dim temp, temp2, temp3 As String
                                    Dim int, int2, int3 As Integer

                                    int = lineas.IndexOf("<Application Id=")
                                    temp = lineas.Remove(0, int)

                                    int2 = temp.IndexOf(Chr(34))
                                    temp2 = temp.Remove(0, int2 + 1)

                                    int3 = temp2.IndexOf(Chr(34))
                                    temp3 = temp2.Remove(int3, temp2.Length - int3)

                                    Dim id As String = temp3

                                    If lineas.Contains("<Identity Name=") Then
                                        Dim temp4, temp5, temp6 As String
                                        Dim int4, int5, int6 As Integer

                                        int4 = lineas.IndexOf("<Identity Name=")
                                        temp4 = lineas.Remove(0, int4)

                                        int5 = temp4.IndexOf(Chr(34))
                                        temp5 = temp4.Remove(0, int5 + 1)

                                        int6 = temp5.IndexOf(Chr(34))
                                        temp6 = temp5.Remove(int6, temp5.Length - int6)

                                        Dim identidad As String = temp6

                                        listaApps = TerminarCargarApps(listaApps, Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\AppData\Local\Packages", carpeta, identidad, lineas, id)

                                        If Directory.Exists(unidad + "WpSystem") Then
                                            For Each carpeta_ As String In Directory.GetDirectories(unidad + "WpSystem")
                                                listaApps = TerminarCargarApps(listaApps, carpeta_ + "\AppData\Local\Packages", carpeta, identidad, lineas, id)
                                            Next
                                        End If
                                    End If
                                End If
                            End If
                        Next
                    Catch ex As Exception

                    End Try
                Next
            Catch ex As Exception
                bw.ReportProgress(0, "/*ERROR*/" + carpetaFinal)
            End Try
        End If

        Return listaApps
    End Function

    Private Function TerminarCargarApps(listaApps As List(Of Aplicacion), carpetaLocal As String, carpetaFicheros As String, identidad As String, lineas As String, id As String) As List(Of Aplicacion)

        If Directory.Exists(carpetaLocal) Then
            For Each carpeta As String In Directory.GetDirectories(carpetaLocal)
                If carpeta.Contains(identidad) Then
                    Dim int7 As Integer = carpeta.LastIndexOf("\")
                    Dim paquete As String = carpeta.Remove(0, int7 + 1)

                    Dim temp8, temp9, temp10 As String
                    Dim int8, int9, int10 As Integer

                    int8 = lineas.IndexOf("<DisplayName>")
                    temp8 = lineas.Remove(0, int8)

                    int9 = temp8.IndexOf(">")
                    temp9 = temp8.Remove(0, int9 + 1)

                    int10 = temp9.IndexOf("</DisplayName>")
                    temp10 = temp9.Remove(int10, temp9.Length - int10)

                    If temp10.Contains("ms-resource:") Then
                        temp10 = BuscarTitulo(lineas)
                    End If

                    Dim nombre As String = temp10.Trim

                    If Not nombre.Contains("ms-resource:") Then
                        Dim imagen As String = SacarImagen(lineas, carpetaFicheros)

                        Dim colorFondo As String

                        If lineas.Contains("BackgroundColor=" + Chr(34)) Then
                            Dim temp14, temp15, temp16 As String
                            Dim int14, int15, int16 As Integer

                            int14 = lineas.IndexOf("BackgroundColor=" + Chr(34))
                            temp14 = lineas.Remove(0, int14)

                            int15 = temp14.IndexOf(Chr(34))
                            temp15 = temp14.Remove(0, int15 + 1)

                            int16 = temp15.IndexOf(Chr(34))
                            temp16 = temp15.Remove(int16, temp15.Length - int16)

                            temp16 = temp16.Trim

                            If temp16 = "transparent" Then
                                temp16 = Nothing
                            End If

                            colorFondo = temp16
                        Else
                            colorFondo = Nothing
                        End If

                        Dim categoria As String = Nothing

                        If Not FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config.ini", "Options", "CategoryUWP") = Nothing Then
                            categoria = FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config.ini", "Options", "CategoryUWP")
                        End If

                        Dim tituloBool As Boolean = False
                        Dim i As Integer = 0
                        While i < listaApps.Count
                            If listaApps(i).Nombre = temp10 Then
                                tituloBool = True
                            End If
                            i += 1
                        End While

                        If tituloBool = False Then
                            listaApps.Add(New Aplicacion(nombre, "shell:AppsFolder\" + paquete + "!" + id, imagen, colorFondo, False, categoria))
                        End If
                    End If
                End If
            Next
        End If

        Return listaApps
    End Function

    Private Function BuscarTitulo(texto As String)

        Dim temp, temp2, temp3 As String
        Dim int, int2, int3 As Integer

        int = texto.IndexOf("<Identity Name=")
        temp = texto.Remove(0, int)

        int2 = temp.IndexOf(Chr(34))
        temp2 = temp.Remove(0, int2 + 1)

        int3 = temp2.IndexOf(Chr(34))
        temp3 = temp2.Remove(int3, temp2.Length - int3)

        temp3 = temp3.Replace("Windows", Nothing)
        temp3 = temp3.Replace("Zune", Nothing)
        temp3 = temp3.Replace("Bing", Nothing)

        If temp3.Contains(".") Then
            Dim int4 As Integer = temp3.LastIndexOf(".")
            temp3 = temp3.Remove(0, int4 + 1)
        End If

        If Not FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config.ini", "Games", temp3) = Nothing Then
            temp3 = FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config.ini", "Games", temp3)
        End If

        If Not FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config.ini", "Apps", temp3) = Nothing Then
            temp3 = FicherosINI.Leer(My.Application.Info.DirectoryPath + "\Config.ini", "Apps", temp3)
        End If

        Return temp3
    End Function

    Private Function SacarImagen(lineas As String, path As String)

        Dim temp, temp2, temp3 As String
        Dim int, int2, int3 As Integer

        int = lineas.IndexOf("<Logo>")
        temp = lineas.Remove(0, int)

        int2 = temp.IndexOf(">")
        temp2 = temp.Remove(0, int2 + 1)

        int3 = temp2.IndexOf("</Logo>")
        temp3 = temp2.Remove(int3, temp2.Length - int3)

        temp3 = path + "\" + temp3

        If Not File.Exists(temp3) Then
            Dim int4 As Integer = temp3.LastIndexOf(".")
            temp3 = temp3.Insert(int4, ".scale-100")
        End If

        If Not File.Exists(temp3) Then
            temp3 = temp3.Replace(".scale-100", Nothing)
            Dim int4 As Integer = temp3.LastIndexOf(".")
            temp3 = temp3.Insert(int4, ".scale-125")
        End If

        If Not File.Exists(temp3) Then
            temp3 = temp3.Replace(".scale-125", Nothing)
            Dim int4 As Integer = temp3.LastIndexOf(".")
            temp3 = temp3.Insert(int4, ".scale-150")
        End If

        If Not File.Exists(temp3) Then
            temp3 = temp3.Replace(".scale-150", Nothing)
            Dim int4 As Integer = temp3.LastIndexOf(".")
            temp3 = temp3.Insert(int4, ".scale-200")
        End If

        If Not File.Exists(temp3) Then
            temp3 = temp3.Replace(".scale-200", Nothing)
            Dim int4 As Integer = temp3.LastIndexOf(".")
            temp3 = temp3.Insert(int4, ".scale-400")
        End If

        temp3 = temp3.Trim

        If Not File.Exists(temp3) Then
            temp3 = Nothing
        End If

        Return temp3
    End Function

End Module
