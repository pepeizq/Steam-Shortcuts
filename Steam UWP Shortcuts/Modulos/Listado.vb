Imports System.IO
Imports System.Text

Module Listado

    Public Sub Carga(lista As List(Of Aplicacion), lv As ListView, transparente As Boolean)

        If lista.Count > 0 Then
            lista.Sort(Function(x, y) x.Nombre.CompareTo(y.Nombre))

            Dim i As Integer = 0
            While i < lista.Count
                Dim grid As New Grid

                '----------------------------------------------------

                Dim col1 As New ColumnDefinition
                Dim col2 As New ColumnDefinition
                Dim col3 As New ColumnDefinition

                col1.Width = New GridLength(1, GridUnitType.Auto)
                col2.Width = New GridLength(1, GridUnitType.Auto)
                col3.Width = New GridLength(1, GridUnitType.Star)

                grid.ColumnDefinitions.Add(col1)
                grid.ColumnDefinitions.Add(col2)
                grid.ColumnDefinitions.Add(col3)

                grid.Margin = New Thickness(10, 5, 10, 5)

                '----------------------------------------------------

                Dim borde As New Border
                borde.Width = 50
                borde.Height = 50

                If transparente = False Then
                    If lista(i).ColorFondo = Nothing Then
                        borde.Background = Brushes.DarkSlateBlue
                    Else
                        Dim brush As Brush = New SolidColorBrush(ColorConverter.ConvertFromString(lista(i).ColorFondo))
                        borde.Background = brush
                    End If
                Else
                    borde.Background = Brushes.Transparent
                End If

                Dim imagen As New Image
                imagen.Width = 50
                imagen.Height = 50

                If Not lista(i).Imagen = Nothing Then
                    Dim bi3 As New BitmapImage
                    bi3.BeginInit()
                    bi3.UriSource = New Uri(lista(i).Imagen, UriKind.RelativeOrAbsolute)
                    bi3.EndInit()
                    imagen.Source = bi3
                End If

                borde.Child = imagen
                Grid.SetColumn(borde, 0)
                grid.Children.Add(borde)

                '----------------------------------------------------

                Dim textoBloque As New TextBlock
                textoBloque.Text = lista(i).Nombre
                textoBloque.VerticalAlignment = VerticalAlignment.Center
                textoBloque.Margin = New Thickness(10, 0, 0, 0)
                textoBloque.FontSize = 15

                Grid.SetColumn(textoBloque, 1)
                grid.Children.Add(textoBloque)

                '----------------------------------------------------

                Dim checkBox As New CheckBox
                checkBox.HorizontalAlignment = HorizontalAlignment.Right
                checkBox.VerticalAlignment = VerticalAlignment.Center
                checkBox.Tag = lista(i)
                checkBox.Margin = New Thickness(0, 0, 10, 0)

                Dim escala As ScaleTransform = New ScaleTransform(1.5, 1.5)
                checkBox.RenderTransformOrigin = New Point(0.5, 0.5)
                checkBox.RenderTransform = escala

                AddHandler checkBox.Checked, AddressOf cbChecked
                AddHandler checkBox.Unchecked, AddressOf cbUnChecked
                AddHandler checkBox.MouseEnter, AddressOf cbMouseEnter
                AddHandler checkBox.MouseLeave, AddressOf cbMouseLeave

                Grid.SetColumn(checkBox, 2)
                grid.Children.Add(checkBox)

                '----------------------------------------------------

                lv.Items.Add(grid)
                i += 1
            End While
        End If

    End Sub

    Private Sub cbChecked(ByVal sender As Object, ByVal e As RoutedEventArgs)

        Dim cb As CheckBox = e.Source
        Dim appCb As Aplicacion = TryCast(cb.Tag, Aplicacion)

        appCb.Añadir = True
        BotonCrearDisponible()

    End Sub

    Private Sub cbUnChecked(ByVal sender As Object, ByVal e As RoutedEventArgs)

        Dim cb As CheckBox = e.Source
        Dim appCb As Aplicacion = TryCast(cb.Tag, Aplicacion)

        appCb.Añadir = False
        BotonCrearDisponible()

    End Sub

    Private Sub BotonCrearDisponible()

        For Each wnd As Window In Application.Current.Windows
            If wnd.GetType Is GetType(MainWindow) Then
                Dim botonDisponible As Boolean = False

                For Each app As Aplicacion In DirectCast(wnd, MainWindow).listaAppsUWP
                    If app.Añadir = True Then
                        botonDisponible = True
                    End If
                Next

                If botonDisponible = True Then
                    DirectCast(wnd, MainWindow).botonCrearAccesosUWP.IsEnabled = True
                Else
                    DirectCast(wnd, MainWindow).botonCrearAccesosUWP.IsEnabled = False
                End If
            End If
        Next

    End Sub

    Private Sub cbMouseEnter(ByVal sender As Object, ByVal e As RoutedEventArgs)

        For Each wnd As Window In Application.Current.Windows
            If wnd.GetType Is GetType(MainWindow) Then
                DirectCast(wnd, MainWindow).Cursor = Cursors.Hand
            End If
        Next

    End Sub

    Private Sub cbMouseLeave(ByVal sender As Object, ByVal e As RoutedEventArgs)

        For Each wnd As Window In Application.Current.Windows
            If wnd.GetType Is GetType(MainWindow) Then
                DirectCast(wnd, MainWindow).Cursor = Cursors.Arrow
            End If
        Next

    End Sub

End Module
