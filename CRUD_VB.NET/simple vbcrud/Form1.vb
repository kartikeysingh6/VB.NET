Imports MySql.Data.MySqlClient

Public Class Form1
    Dim myconnection As String = "server=localhost;user id=root;password=mysql123;database=db_vbcrud"
    Dim conn As New MySqlConnection(myconnection)
    Dim cmd As New MySqlCommand
    Dim sql As String
    Private rowindex As Integer
    Dim da As New MySqlDataAdapter


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim query As String = "Select id as'Std.ID',name as 'Student Name',address as'Address',join as 'join' from tbl_employee where status=0"
            Dim adpt As New MySqlDataAdapter(query, conn)
            Dim ds As New DataSet()
            adpt.Fill(ds, "Std")
            DataGridView1.DataSource = ds.Tables(0)
            conn.Close()



        Catch ex As Exception
            MessageBox.Show(ex.ToString())
            Exit Sub
        End Try

    End Sub
    Sub clear()
        txtname.Text = ""
        txtaddress.Text = ""
        cmbjoin.SelectedIndex = -1
        txtsearch.Text = ""

    End Sub

    Private Sub btnclear_Click(sender As Object, e As EventArgs) Handles btnclear.Click
        clear()

    End Sub

    Private Sub btnrefresh_Click(sender As Object, e As EventArgs) Handles btnrefresh.Click
        Form1_Load(sender, e)
    End Sub

    Private Sub DataGridView1_CellMouseUp(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseUp
        If DataGridView1.RowCount <= 0 Then
            Exit Sub
        End If
        Try
            If e.Button = MouseButtons.Right Then
                Me.DataGridView1.Rows(e.RowIndex).Selected = True
                Me.rowindex = e.RowIndex
                Me.DataGridView1.CurrentCell = Me.DataGridView1.Rows(e.RowIndex).Cells(1)
                Me.ContextMenuStrip1.Show(Me.DataGridView1, e.Location)
                ContextMenuStrip1.Show(Cursor.Position)
            End If
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Private Sub EditToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditToolStripMenuItem.Click
        txtsearch.Text = DataGridView1.CurrentRow.Cells(0).Value.ToString()

    End Sub

    Private Sub txtsearch_TextChanged(sender As Object, e As EventArgs) Handles txtsearch.TextChanged
        If txtsearch.Text = "" Then
            Exit Sub
        End If
        Try
            sql = "select * from tbl_employee where id=" & Val(txtsearch.Text)
            conn.ConnectionString = myconnection
            conn.Open()
            With cmd
                .Connection = conn
                .CommandText = sql
            End With
            Dim publictable As New DataTable
            da.SelectCommand = cmd
            da.Fill(publictable)
            txtname.Text = publictable.Rows(0).Item(1)
            txtaddress.Text = publictable.Rows(0).Item(2)
            cmbjoin.Text = publictable.Rows(0).Item(3)
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        Finally
            da.Dispose()
            conn.Close()

        End Try
    End Sub

    Private Sub btnupdate_Click(sender As Object, e As EventArgs) Handles btnupdate.Click
        Try
            conn.ConnectionString = myconnection
            conn.Open()
            sql = "Update tbl_employee set name=@name,address=@address,join=@join where id=" & Val(txtsearch.Text)
            cmd.Connection = conn
            cmd.CommandText = sql
            cmd.Parameters.AddWithValue("@name", txtname.Text)
            cmd.Parameters.AddWithValue("@address", txtaddress.Text)
            cmd.Parameters.AddWithValue("@join", cmbjoin.Text)
            Dim r As Integer
            r = cmd.ExecuteNonQuery
            If r > 0 Then
                MsgBox("Join Record Updated....!")
                clear()
                conn.Close()
                cmd.Parameters.Clear()

            End If
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Private Sub DeleteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem.Click
        Try
            conn.ConnectionString = myconnection
            conn.Open()
            sql = "Update tbl_employee set status=1 where id=" & Val(DataGridView1.CurrentRow.Cells(0).Value.ToString())
            cmd.Connection = conn
            cmd.CommandText = sql
            Dim r As Integer
            r = cmd.ExecuteNonQuery
            If r > 0 Then
                MsgBox("Join Record Deleted....!")
                clear()
                conn.Close()
            End If
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
End Class
