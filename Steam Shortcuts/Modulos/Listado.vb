Module Listado

    Public Sub Carga(lista As List(Of Aplicacion), transparente As Boolean, tbControl As Dragablz.TabablzControl, titulo As String, icono As String)

        Dim lv As ListView = New ListView

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

                grid.Margin = New Thickness(10, 10, 10, 10)

                '----------------------------------------------------

                Dim checkBox As New CheckBox
                checkBox.HorizontalAlignment = HorizontalAlignment.Right
                checkBox.VerticalAlignment = VerticalAlignment.Center
                checkBox.Tag = lista(i)
                checkBox.Margin = New Thickness(10, 0, 20, 0)

                Dim escala As ScaleTransform = New ScaleTransform(1.5, 1.5)
                checkBox.RenderTransformOrigin = New Point(0.5, 0.5)
                checkBox.RenderTransform = escala

                AddHandler checkBox.Checked, AddressOf cbChecked
                AddHandler checkBox.Unchecked, AddressOf cbUnChecked
                AddHandler checkBox.MouseEnter, AddressOf cbMouseEnter
                AddHandler checkBox.MouseLeave, AddressOf cbMouseLeave

                Grid.SetColumn(checkBox, 0)
                grid.Children.Add(checkBox)

                '----------------------------------------------------

                Dim borde As New Border

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

                If Not lista(i).Icono = Nothing Then
                    Dim bi3 As New BitmapImage
                    bi3.BeginInit()
                    bi3.UriSource = New Uri(lista(i).Icono, UriKind.RelativeOrAbsolute)
                    bi3.EndInit()

                    imagen.Source = bi3
                    imagen.Width = 40
                    imagen.Height = 40

                    borde.Width = 40
                    borde.Height = 40
                Else
                    imagen.Width = 0
                    imagen.Height = 40

                    borde.Width = 0
                    borde.Height = 40
                End If

                borde.Child = imagen
                Grid.SetColumn(borde, 1)
                grid.Children.Add(borde)

                '----------------------------------------------------

                Dim textoBloque As New TextBlock
                textoBloque.Text = lista(i).Nombre
                textoBloque.VerticalAlignment = VerticalAlignment.Center
                textoBloque.Margin = New Thickness(10, 0, 0, 0)
                textoBloque.FontSize = 15

                Grid.SetColumn(textoBloque, 2)
                grid.Children.Add(textoBloque)

                '----------------------------------------------------

                grid.ToolTip = lista(i).Ejecutable + " " + lista(i).Argumentos
                lv.Items.Add(grid)

                i += 1
            End While
        End If

        '----------------------------------------------------

        Dim gridHeader As New Grid

        Dim colh1 As New ColumnDefinition
        Dim colh2 As New ColumnDefinition

        colh1.Width = New GridLength(1, GridUnitType.Auto)
        colh2.Width = New GridLength(1, GridUnitType.Auto)

        gridHeader.ColumnDefinitions.Add(colh1)
        gridHeader.ColumnDefinitions.Add(colh2)

        gridHeader.Margin = New Thickness(2, 2, 2, 2)

        '----------------------------------------------------

        Dim iconoHeader As New Image

        If Not icono = Nothing Then
            iconoHeader.Source = New BitmapImage(New Uri(icono))
        End If

        Grid.SetColumn(iconoHeader, 0)
        gridHeader.Children.Add(iconoHeader)

        '----------------------------------------------------

        Dim tituloHeader As New TextBlock

        tituloHeader.Text = titulo
        tituloHeader.FontSize = 15
        tituloHeader.Margin = New Thickness(5, 0, 5, 0)
        tituloHeader.Foreground = Brushes.White
        tituloHeader.VerticalAlignment = VerticalAlignment.Center

        Grid.SetColumn(tituloHeader, 1)
        gridHeader.Children.Add(tituloHeader)

        '----------------------------------------------------

        Dim tabItem As New TabItem
        tabItem.Header = gridHeader

        tabItem.Content = lv

        tbControl.Items.Add(tabItem)

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

                For Each app As Aplicacion In DirectCast(wnd, MainWindow).listaUWP
                    If app.Añadir = True Then
                        botonDisponible = True
                    End If
                Next

                For Each app As Aplicacion In DirectCast(wnd, MainWindow).listaGOG
                    If app.Añadir = True Then
                        botonDisponible = True
                    End If
                Next

                For Each app As Aplicacion In DirectCast(wnd, MainWindow).listaUplay
                    If app.Añadir = True Then
                        botonDisponible = True
                    End If
                Next

                For Each app As Aplicacion In DirectCast(wnd, MainWindow).listaOrigin
                    If app.Añadir = True Then
                        botonDisponible = True
                    End If
                Next

                For Each app As Aplicacion In DirectCast(wnd, MainWindow).listaBattlenet
                    If app.Añadir = True Then
                        botonDisponible = True
                    End If
                Next

                If botonDisponible = True Then
                    DirectCast(wnd, MainWindow).botonCrearAccesos.IsEnabled = True
                Else
                    DirectCast(wnd, MainWindow).botonCrearAccesos.IsEnabled = False
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
