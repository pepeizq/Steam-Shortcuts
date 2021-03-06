﻿Public Class Aplicacion

    Private _Nombre As String
    Private _Ejecutable As String
    Private _Argumentos As String
    Private _Icono As String
    Private _ColorFondo As String
    Private _Añadir As Boolean
    Private _Categoria As String

    Public ReadOnly Property Nombre() As String
        Get
            Return _Nombre
        End Get
    End Property

    Public ReadOnly Property Ejecutable() As String
        Get
            Return _Ejecutable
        End Get
    End Property

    Public ReadOnly Property Argumentos() As String
        Get
            Return _Argumentos
        End Get
    End Property

    Public ReadOnly Property Icono() As String
        Get
            Return _Icono
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

    Public Sub New(ByVal nombre As String, ByVal ejecutable As String, ByVal argumentos As String, ByVal icono As String, ByVal colorfondo As String, ByVal añadir As Boolean, ByVal categoria As String)
        _Nombre = nombre
        _Ejecutable = ejecutable
        _Argumentos = argumentos
        _Icono = icono
        _ColorFondo = colorfondo
        _Añadir = añadir
        _Categoria = categoria
    End Sub

End Class
