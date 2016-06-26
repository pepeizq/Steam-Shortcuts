Imports System.Drawing

Module Iconos

    Public Function Generar(exe As String, nombre As String) As String

        If Not exe = Nothing Then
            exe = exe.Replace(Chr(34), Nothing)
            exe = exe.Replace(vbNullChar, Nothing)
            exe = exe.Trim
        End If

        Dim iconoTemp As Icon = Icon.ExtractAssociatedIcon(exe)
        Dim imagenIcono As Image = iconoTemp.ToBitmap

        Dim nombreTemp As String = nombre

        nombreTemp = nombreTemp.Replace(":", Nothing)
        nombreTemp = nombreTemp.Replace("\", Nothing)
        nombreTemp = nombreTemp.Replace("/", Nothing)
        nombreTemp = nombreTemp.Replace("*", Nothing)
        nombreTemp = nombreTemp.Replace("?", Nothing)
        nombreTemp = nombreTemp.Replace(Chr(34), Nothing)
        nombreTemp = nombreTemp.Replace("<", Nothing)
        nombreTemp = nombreTemp.Replace(">", Nothing)
        nombreTemp = nombreTemp.Replace("|", Nothing)

        imagenIcono.Save(My.Application.Info.DirectoryPath + "\Temp\" + nombreTemp + ".png", Imaging.ImageFormat.Png)
        Return My.Application.Info.DirectoryPath + "\Temp\" + nombreTemp + ".png"
    End Function

End Module
