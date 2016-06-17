Public Class Aplicacion

    Private _Nombre As String
    Private _AccesoDirecto As String
    Private _Imagen As String
    Private _ColorFondo As String

    Public ReadOnly Property Nombre() As String
        Get
            Return _Nombre
        End Get
    End Property

    Public ReadOnly Property AccesoDirecto() As String
        Get
            Return _AccesoDirecto
        End Get
    End Property

    Public ReadOnly Property Imagen() As String
        Get
            Return _Imagen
        End Get
    End Property

    Public ReadOnly Property ColorFondo() As String
        Get
            Return _ColorFondo
        End Get
    End Property

    Public Sub New(ByVal nombre As String, ByVal accesodirecto As String, ByVal imagen As String, ByVal colorfondo As String)
        _Nombre = nombre
        _AccesoDirecto = accesodirecto
        _Imagen = imagen
        _ColorFondo = colorfondo
    End Sub

End Class
