Imports System.Runtime.InteropServices
Imports System.Text

Public Class FicherosINI

    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Shared Function GetPrivateProfileString(ByVal lpAppName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As StringBuilder, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Shared Function WritePrivateProfileString(ByVal lpAppName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Boolean
    End Function

    Public Shared Function Leer(ByVal File As String, ByVal Section As String, ByVal Key As String) As String

        Dim sb As New StringBuilder(10000)

        GetPrivateProfileString(Section, Key, "", sb, sb.Capacity, File)

        Return sb.ToString
    End Function

    Public Shared Sub Escribir(ByVal File As String, ByVal Section As String, ByVal Key As String, ByVal Value As String)

        Try
            WritePrivateProfileString(Section, Key, Value, File)
        Catch ex As Exception

        End Try

    End Sub

End Class
