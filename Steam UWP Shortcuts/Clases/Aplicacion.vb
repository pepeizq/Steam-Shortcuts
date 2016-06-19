Public Class Aplicacion

    Private _Nombre As String
    Private _AccesoDirecto As String
    Private _Imagen As String
    Private _ColorFondo As String
    Private _Añadir As Boolean
    Private _Categoria As String

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

    Public Property Añadir() As Boolean
        Get
            Return _Añadir
        End Get
        Set(ByVal valor As Boolean)
            _Añadir = valor
        End Set
    End Property

    Public ReadOnly Property Categoria() As String
        Get
            Return _Categoria
        End Get
    End Property

    Public Sub New(ByVal nombre As String, ByVal accesodirecto As String, ByVal imagen As String, ByVal colorfondo As String, ByVal añadir As Boolean, ByVal categoria As String)
        _Nombre = nombre
        _AccesoDirecto = accesodirecto
        _Imagen = imagen
        _ColorFondo = colorfondo
        _Añadir = añadir
        _Categoria = categoria
    End Sub

End Class
