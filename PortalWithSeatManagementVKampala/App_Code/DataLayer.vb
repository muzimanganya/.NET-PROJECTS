Imports System.Data.OleDb
Imports System.Data
Imports System.Data.SqlClient
Imports System
Imports System.IO

Public Class Datalayer
    Public myCon As SqlConnection
    Public myConTwo As SqlConnection
    Private mySql As String
    Private ServerName As String
    Private UserID As String
    Private Password As String
    Private Database As String = "International"
    Private MsgHeader As String = "Saving Data"
    Private myColumns As Collection
    Private myColumns2 As Collection
    Private myWhere As String
    Private myWhere2 As String = ""
    Private myWhere3 As String = ""
    Private SelectMaster As String
    Private Shared CUser As String = ""
    Private Shared WorkDate As String
    Public isSwitchingDB As Boolean = False

    Public Sub New()
        'If ServerName = "" or mySql = "" Then
        If IsNothing(myCon) Then
            ' ConnectDB()
        End If
    End Sub

    Protected Sub Dispose(ByVal disposing As Boolean)
        myCon = Nothing
    End Sub
    Public Shared Property WorkingDate As String
        Get
            Return WorkDate
        End Get
        Set(ByVal value As String)
            WorkDate = value
        End Set
    End Property
    Public Shared Property CurrentUser() As String
        Get
            Return CUser
        End Get
        Set(ByVal value As String)
            CUser = value
        End Set
    End Property


    Public ReadOnly Property CurrentGroup() As Int32
        Get
            ' Return Convert.ToInt32(ReturnSingleValue("select GNo from security where UID='" & CurrentUser & "'"))
            Return "kbs"
        End Get
    End Property
    Public Property SelectStatement() As String
        Get
            Return mySql
        End Get
        Set(ByVal value As String)
            mySql = value
        End Set
    End Property

    Public Property WhereClause() As String
        Get
            Return myWhere
        End Get
        Set(ByVal value As String)
            myWhere = ""
            myWhere = " and " & value
        End Set
    End Property

    Public Property WhereClause2() As String

        Get
            Return myWhere2
        End Get
        Set(ByVal value As String)
            myWhere2 = ""
            If value <> "" Then
                If Mid(SelectStatement, 8, 3) = "TOP" Or Mid(SelectStatement, 8, 3) = "TO" Or Mid(SelectStatement, 8, 3) = "OP" Or Mid(SelectStatement, 8, 3) = "TOP " Or Mid(SelectStatement, 8, 3) = " TOP" Then
                    SelectStatement = Mid(SelectStatement, 1, 7) & Mid(SelectStatement, 14, SelectStatement.Length() - 13)
                End If
                myWhere2 = " and " & value
            Else
                myWhere2 = " "
            End If
        End Set
    End Property

    Public Property myWhereClause3() As String
        Get
            Return myWhere3
        End Get
        Set(ByVal value As String)
            myWhere3 = ""
            myWhere3 = " and " & value
        End Set
    End Property
    Public Property SqlServerName() As String
        Get
            Return ServerName
        End Get
        Set(ByVal value As String)
            ServerName = value
        End Set
    End Property

    Public Property sqlUserId() As String
        Get
            Return UserID
        End Get
        Set(ByVal value As String)
            UserID = value
        End Set
    End Property

    Public Property SqlPassword() As String
        Get
            Return Password
        End Get
        Set(ByVal value As String)
            Password = value
        End Set
    End Property

    Public Property SQLDatabase() As String
        Get
            Return Database
        End Get
        Set(ByVal value As String)
            Database = value
        End Set
    End Property

    Public Property IgnoreColumns() As Collection
        Get
            Return myColumns
        End Get
        Set(ByVal value As Collection)
            myColumns = value
        End Set
    End Property

    Public Property MasterStmt() As String
        Get
            Return SelectMaster
        End Get
        Set(ByVal value As String)
            SelectMaster = value
        End Set
    End Property
    
    Public Sub ConnectDB()
        ' Try
        'If ServerName = "" Or UserID = "" Then
        '    Call ReadRegistry()
        'End If
        'If ServerName <> "" Then
        '    myCon = Nothing
        '    myCon = New SqlConnection("User Id=" & UserID & ";password=" & Password & ";data source=" & ServerName & ";database=" & Database & "")
        '    myCon.Open()
        'End If
        'Catch ex As Exception
        'MsgBox("Error Encountered while connecting to database. " + ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error Connecting to DB!")
        'End Try

    End Sub
    Public Function ReturnConnection(Optional ndatabase As String = "") As SqlConnection
        If ndatabase.Length > 0 Then
            'We set a database to work with
            Dim theSetCon As New SqlConnection
            If ServerName = "" Or UserID = "" Then
                Call ReadRegistry()
            End If
            If ServerName <> "" Then
                theSetCon = New SqlConnection("User Id=" & UserID & ";password=" & Password & ";data source=" & ServerName & ";database=" & ndatabase & "")
            End If
            theSetCon.Open()
            Return theSetCon
        Else
            Dim theCon As New SqlConnection
            If ServerName = "" Or UserID = "" Then
                Call ReadRegistry()
            End If
            If ServerName <> "" Then
                theCon = New SqlConnection("User Id=" & UserID & ";password=" & Password & ";data source=" & ServerName & ";database=" & Database & "")
                'theCon = New SqlConnection(ConfigurationManager.ConnectionStrings("DatabaseConn").ConnectionString)
            End If
            theCon.Open()
            Return theCon
        End If
        
    End Function
    Public Sub ConnectForProcsAudit()
        Try
            If ServerName = "" Or UserID = "" Then
                Call ReadRegistry()
            End If
            If ServerName <> "" Then
                myConTwo = Nothing
                myConTwo = New SqlConnection("User Id=" & UserID & ";password=" & Password & ";data source=" & ServerName & ";database=" & Database & "")
                myConTwo.Open()
            End If
        Catch ex As Exception
            LogException(ex, SQLDatabase)
        End Try

    End Sub
    Public Sub CloseDBProcs()
        On Error Resume Next
        myConTwo.Close()
        myConTwo = Nothing
    End Sub
    Public Sub CloseDB()
        On Error Resume Next
        myCon.Close()
        myCon.Dispose()
        myCon = Nothing

    End Sub

    Public Function ReturnAdapter(ByVal mySql As String) As SqlDataAdapter
        Dim myDataAdapter As SqlDataAdapter
        Using aCon As SqlConnection = ReturnConnection()
            myDataAdapter = New SqlDataAdapter(mySql, aCon)
            aCon.Close()
        End Using

        Return myDataAdapter
    End Function
    Public Sub ReturnBlessedAdapter(ByRef adp As Object, ByVal mySql As String)
        ConnectDB()
        adp.Connection = myCon
    End Sub

    Public Function ReturnAdapter(ByVal cmd As SqlCommand) As SqlDataAdapter
        Try
            'ConnectDB()
            Dim myDataAdapter As SqlDataAdapter
            Using acon As SqlConnection = ReturnConnection()
                cmd.Connection = acon
                myDataAdapter = New SqlDataAdapter(cmd)
                acon.Close()
            End Using


            ' Try

            'CloseDB()
            Return myDataAdapter
            PerformAuditTrail(" ", mySql)
        Catch ex As Exception
            LogException(ex, SQLDatabase)
        Finally

        End Try
    End Function

    Public Function ReturnSingleValue(ByVal Selectstmt As String, Optional db As String = "") As String
        Dim myval As String = ""
        Try
            'Using aCon As SqlConnection = ReturnConnection()
            '    Dim adp As SqlDataAdapter = ReturnAdapter(Selectstmt)
            '    Dim ds As New DataSet
            '    adp.Fill(ds)

            '    If ds.Tables(0).Rows.Count > 0 Then
            '        myval = ds.Tables(0).Rows(0).Item(0).ToString()
            '    Else
            '        myval = ""
            '    End If
            '    aCon.Close()
            '    PerformAuditTrail(" ", Selectstmt)
            'End Using
            If db.Length > 0 Then
                Using aCon As SqlConnection = ReturnConnection(db)
                    Dim cmd As New SqlCommand(Selectstmt, aCon)
                    Dim dr As SqlDataReader
                    dr = cmd.ExecuteReader()
                    If dr.Read() Then
                        myval = dr.Item(0)
                        'CloseDB()
                    Else
                        myval = ""
                    End If
                    PerformAuditTrail(" ", Selectstmt)
                    aCon.Close()
                End Using
            Else
                Using aCon As SqlConnection = ReturnConnection()
                    Dim cmd As New SqlCommand(Selectstmt, aCon)
                    Dim dr As SqlDataReader
                    dr = cmd.ExecuteReader()
                    If dr.Read() Then
                        myval = dr.Item(0)
                        'CloseDB()
                    Else
                        myval = ""
                    End If
                    PerformAuditTrail(" ", Selectstmt)
                    aCon.Close()
                End Using
            End If


        Catch ex As Exception
            LogException(ex, SQLDatabase)
            myval = ""
        End Try
        Return myval
    End Function

    Public Function ReturnSingleNumeric(ByVal Selectstmt As String) As Double
        Try
            Dim myval As Double = 0

            Using aCon As SqlConnection = ReturnConnection()

                Dim cmd As New SqlCommand(Selectstmt, aCon)
                Dim dr As SqlDataReader
                dr = cmd.ExecuteReader()
                If dr.Read() Then
                    myval = dr.Item(0)
                    '   CloseDB()
                Else
                    myval = 0
                End If
                aCon.Close()
                'PerformAuditTrail(" ", Selectstmt)
            End Using

            Return myval
        Catch ex As Exception
            LogException(ex, SQLDatabase)
            Return 0
        End Try

    End Function

    Public Function ReturnSingleNumeric(ByVal Selectstmt As String, Database As String) As Double
        Try
            Dim myval As Double = 0

            Using aCon As SqlConnection = ReturnConnection(Database)

                Dim cmd As New SqlCommand(Selectstmt, aCon)
                Dim dr As SqlDataReader
                dr = cmd.ExecuteReader()
                If dr.Read() Then
                    myval = dr.Item(0)
                    '   CloseDB()
                Else
                    myval = 0
                End If
                aCon.Close()
                'PerformAuditTrail(" ", Selectstmt)
            End Using

            Return myval
        Catch ex As Exception
            LogException(ex, SQLDatabase)
            Return 0
        End Try

    End Function

    Public Function ReturnDataReader(ByVal Selectstmt As String) As SqlDataReader
        Dim dr As SqlDataReader
        Try
            Using aCon As SqlConnection = ReturnConnection()
                Dim cmd As New SqlCommand(Selectstmt, aCon)
                dr = cmd.ExecuteReader()
                PerformAuditTrail(" ", Selectstmt)
                aCon.Close()
            End Using

        Catch ex As Exception
            LogException(ex, SQLDatabase)
        End Try
        Return dr
    End Function

    Public Sub FillDataSet(ByRef dt As DataSet, ByVal mySql As String, ByVal TableName As String)
        Try
            Dim da As SqlDataAdapter = ReturnAdapter(mySql)
            da.Fill(dt, TableName)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Error Filling DataSet")
        End Try
        PerformAuditTrail(TableName, mySql)
    End Sub

    Public Sub ExecuteProc(ByRef cmd As SqlCommand)

        Using aCon As SqlConnection = ReturnConnection()
            cmd.Connection = aCon
            cmd.ExecuteNonQuery()
            aCon.Close()
        End Using

    End Sub

    Public Sub executeCommand(ByRef sqlstmt As String)
        Dim cmd As New SqlCommand(sqlstmt)

        cmd.CommandType = CommandType.Text

        ExecuteProc(cmd)

        PerformAuditTrail(" ", sqlstmt)
    End Sub


    Public Function ReturnDataTable(ByVal mysql As String, Optional db As String = "") As DataTable
        Dim table As New DataTable()
        'Try
        Dim command As SqlCommand
        If db.Length > 0 Then
            Using aCon As SqlConnection = ReturnConnection(db)
                command = New SqlCommand(mysql, aCon)
                Dim adapter As New SqlDataAdapter()
                adapter.SelectCommand = command
                table.Locale = System.Globalization.CultureInfo.InvariantCulture
                adapter.Fill(table)
                'CloseDB()
                aCon.Close()
                PerformAuditTrail(table.TableName, mysql)
            End Using
        Else
            Using aCon As SqlConnection = ReturnConnection()
                command = New SqlCommand(mysql, aCon)
                Dim adapter As New SqlDataAdapter()
                adapter.SelectCommand = command
                table.Locale = System.Globalization.CultureInfo.InvariantCulture
                adapter.Fill(table)
                'CloseDB()
                aCon.Close()
                PerformAuditTrail(table.TableName, mysql)
            End Using
        End If

        'Catch ex As Exception
        'LogException(ex, SQLDatabase)
        'End Try
        Return table
    End Function

    Private Sub ReadRegistry()
        Dim cfg As New Config
        If isSwitchingDB Then
            'No need to change the database 
            SqlServerName = cfg.SQLServer
            sqlUserId = cfg.SQLUser
            SqlPassword = cfg.SQLPassword
            'Database = cfg.SecurityDB
        Else
            SqlServerName = cfg.SQLServer
            sqlUserId = cfg.SQLUser
            SqlPassword = cfg.SQLPassword
            Database = cfg.SecurityDB
        End If

        
    End Sub
    Private Function CheckRightsForEditing(ByVal tableName As String) As String
        Dim clsDataLayer As New Datalayer
        Dim query As String = String.Format("SELECT * FROM AdvancedUserRights WHERE UserId='{0}' AND TableName='{1}'", CurrentUser, tableName)
        Dim dt As DataTable = clsDataLayer.ReturnDataTable(query)
        Dim editing As String = "N"
        If (dt.Rows.Count > 0) Then
            editing = dt.Rows(0).Item("editing").ToString
            If (editing.Length = 0) Then
                editing = "N"
            End If
        End If
        Return editing
    End Function
    Private Function CheckRightsForDeleting(ByVal tableName As String) As String
        Dim clsDataLayer As New Datalayer
        Dim query As String = String.Format("SELECT * FROM AdvancedUserRights WHERE UserId='{0}' AND TableName='{1}'", CurrentUser, tableName)
        Dim dt As DataTable = clsDataLayer.ReturnDataTable(query)
        Dim deleting As String = "N"
        If (dt.Rows.Count > 0) Then
            deleting = dt.Rows(0).Item("deleting").ToString
            If (deleting.Length = 0) Then
                deleting = "N"
            End If
        End If
        Return deleting
    End Function
    Public Function SaveChanges(ByRef ds As DataSet, ByVal tableName As String, Optional ByVal ImportData As Boolean = False, Optional ByVal spName As String = "") As Boolean
        Dim myds As New DataSet
        Dim comm As New SqlCommand
        Dim i As Integer
        Dim InsertSQL As String
        Dim DeleteSQL As String
        Dim UpdateSQL As String
        Dim InsertValues As String
        Dim myTrans As SqlTransaction
        Dim IgnoredRecords As Boolean = False
        'Try
        If ImportData = False Then
            If ds.HasChanges Then
                ConnectDB()

                myTrans = myCon.BeginTransaction("myTrans")

                For Each dr As DataRow In ds.GetChanges.Tables(0).Rows
                    If dr.RowState = DataRowState.Deleted Then
                        comm.CommandType = CommandType.Text
                        comm.Connection = myCon
                        DeleteSQL = "DELETE FROM " & tableName & " WHERE SERIAL=" & dr.Item(0, DataRowVersion.Original) & ""
                        comm.CommandText = DeleteSQL
                        comm.Transaction = myTrans
                        comm.ExecuteNonQuery()
                        PerformAuditTrail(tableName, DeleteSQL)
                    Else

                        If dr.Item(1).GetType.ToString <> "System.DBNull" Then
                            If dr.RowState = DataRowState.Added Then

                                InsertSQL = "INSERT INTO " & tableName & "("
                                InsertValues = " VALUES ("
                                For i = 1 To dr.ItemArray.Length - 1

                                    If IgnoreColumns.Contains(ds.Tables(0).Columns(i).Caption) = False Then
                                        If dr.Item(i).GetType().ToString = "System.DateTime" Or dr.Item(i).GetType().ToString = "System.String" Then
                                            InsertValues = InsertValues & "'" & dr.Item(i) & "',"
                                        ElseIf dr.Item(i).GetType.ToString = "System.DBNull" Or dr.Item(i).GetType.ToString = "System.DBNull" Then
                                            InsertValues = InsertValues & "'" & "'" & ","
                                        ElseIf dr.Item(i).GetType().ToString = "System.Boolean" Then
                                            If (dr.Item(i) = "True") Then
                                                InsertValues = InsertValues & 1 & ","
                                            ElseIf (dr.Item(i) = "False") Then
                                                InsertValues = InsertValues & 0 & ","
                                            End If
                                        ElseIf dr.Item(i).GetType().ToString = "System.Byte()" Then

                                            'Continue For
                                        Else
                                            InsertValues = InsertValues & "" & dr.Item(i) & ","
                                        End If
                                        InsertSQL = InsertSQL & ds.Tables(0).Columns(i).ColumnName & ","
                                    End If

                                Next i
                                InsertSQL = Mid(InsertSQL, 1, Len(InsertSQL) - 1)
                                InsertValues = Mid(InsertValues, 1, Len(InsertValues) - 1)
                                InsertSQL = InsertSQL & ")"
                                InsertValues = InsertValues & ")"

                                InsertSQL = InsertSQL & " " & InsertValues

                                comm.CommandType = CommandType.Text
                                comm.Connection = myCon
                                comm.CommandText = InsertSQL
                                comm.Transaction = myTrans
                                comm.ExecuteNonQuery()
                                PerformAuditTrail(tableName, InsertSQL)
                            ElseIf dr.RowState = DataRowState.Modified Then
                                UpdateSQL = "UPDATE " & tableName & " SET "

                                For i = 1 To dr.ItemArray.Length - 1
                                    If IgnoreColumns.Contains(ds.Tables(0).Columns(i).Caption) = False Then
                                        'MsgBox(ds.Tables(0).Columns(i).DataType.ToString)
                                        If dr.Item(i).GetType().ToString = "System.DateTime" Or dr.Item(i).GetType().ToString = "System.String" Then
                                            UpdateSQL = UpdateSQL & ds.Tables(0).Columns(i).Caption & "='" & dr.Item(i) & "',"
                                        ElseIf dr.Item(i).GetType.ToString = "System.DBNull" Or dr.Item(i).GetType.ToString = "System.DBNull" Then
                                            UpdateSQL = UpdateSQL & ds.Tables(0).Columns(i).Caption & "=''" & ","
                                        ElseIf dr.Item(i).GetType().ToString = "System.Boolean" Then
                                            If (dr.Item(i) = "True") Then
                                                UpdateSQL = UpdateSQL & ds.Tables(0).Columns(i).Caption & "=" & 1 & ","
                                            ElseIf (dr.Item(i) = "False") Then
                                                UpdateSQL = UpdateSQL & ds.Tables(0).Columns(i).Caption & "=" & 0 & ","
                                            End If
                                        Else
                                            UpdateSQL = UpdateSQL & ds.Tables(0).Columns(i).ColumnName & "=" & dr.Item(i) & ","
                                        End If
                                    End If

                                Next i
                                UpdateSQL = Mid(UpdateSQL, 1, Len(UpdateSQL) - 1)
                                UpdateSQL = UpdateSQL & " WHERE Serial = " & dr.Item(0) & ""
                                comm.CommandType = CommandType.Text
                                comm.Connection = myCon
                                comm.CommandText = UpdateSQL
                                comm.Transaction = myTrans
                                comm.ExecuteNonQuery()
                                PerformAuditTrail(tableName, UpdateSQL)
                            End If
                        Else
                            IgnoredRecords = True
                        End If
                    End If
                Next dr
                ds.AcceptChanges()

                myTrans.Commit()
                If IgnoredRecords = True Then
                    MsgBox("Record with Null first column were not saved", MsgBoxStyle.Information, "Saving")
                End If
                MsgBox("Data Update successful", MsgBoxStyle.OkOnly, tableName)
            Else
                MsgBox("No changes were made", MsgBoxStyle.OkOnly, MsgHeader)
            End If
        Else
            'Save import data

            ConnectDB()
            myTrans = myCon.BeginTransaction("myTrans")

            For Each dr As DataRow In ds.Tables(0).Rows

                InsertSQL = "INSERT INTO " & tableName & "("
                InsertValues = " VALUES ("
                For i = 0 To dr.ItemArray.Length - 1
                    If IgnoreColumns.Contains(ds.Tables(0).Columns(i).Caption) = False Then
                        If dr.Item(i).GetType().ToString = "System.DateTime" Or dr.Item(i).GetType().ToString = "System.String" Then
                            InsertValues = InsertValues & "'" & dr.Item(i) & "',"
                        ElseIf dr.Item(i).GetType.ToString = "System.DBNull" Or dr.Item(i).GetType.ToString = "System.DBNull" Then
                            InsertValues = InsertValues & "'" & "'" & ","
                        Else
                            InsertValues = InsertValues & "" & dr.Item(i) & ","
                        End If
                        InsertSQL = InsertSQL & ds.Tables(0).Columns(i).ColumnName & ","
                    End If

                Next i
                InsertSQL = Mid(InsertSQL, 1, Len(InsertSQL) - 1)
                InsertValues = Mid(InsertValues, 1, Len(InsertValues) - 1)
                InsertSQL = InsertSQL & ")"
                InsertValues = InsertValues & ")"

                InsertSQL = InsertSQL & " " & InsertValues

                comm.CommandType = CommandType.Text
                comm.Connection = myCon
                comm.CommandText = InsertSQL
                comm.Transaction = myTrans
                comm.ExecuteNonQuery()
                PerformAuditTrail(tableName, InsertSQL)
                'InsertSQL = "INSERT INTO " & tableName & " VALUES ("
                'For i = 0 To dr.ItemArray.Length - 1
                '    'MsgBox(dr.Item(i).GetType.ToString)
                '    If dr.Item(i).GetType().ToString = "System.DateTime" Or dr.Item(i).GetType().ToString = "System.String" Then
                '        InsertSQL = InsertSQL & "'" & dr.Item(i) & "',"
                '    ElseIf dr.Item(i).GetType.ToString = "System.DBNull" Or dr.Item(i).GetType.ToString = "System.DBNull" Then
                '        InsertSQL = InsertSQL & "'" & "'" & ","
                '    Else
                '        InsertSQL = InsertSQL & "" & dr.Item(i) & ","

                '    End If

                'Next i

                'InsertSQL = Mid(InsertSQL, 1, Len(InsertSQL) - 1)
                'InsertSQL = InsertSQL & ")"
                ''MsgBox(InsertSQL)

                'comm.CommandType = CommandType.Text
                'comm.Connection = myCon
                'comm.CommandText = InsertSQL
                'comm.Transaction = myTrans
                'comm.ExecuteNonQuery()
            Next dr
            myTrans.Commit()
            If spName <> "" Then
                UpdateBatches(spName)
            End If
            MsgBox("Data Import for " & tableName & " successful", MsgBoxStyle.OkOnly, tableName)
        End If
        SaveChanges = True
        'Catch ex As Exception
        'MsgBox("The data you entered caused an error while saving. " & ex.Message, MsgBoxStyle.OkOnly, MsgHeader)
        'Try
        '    myTrans.Rollback()
        'Catch eE As Exception

        'End Try

        'Finally
        '    CloseDB()
        'End Try
    End Function
    Private Sub TrimTrailingComma(ByRef sql As String)
        Dim buff As Char() = sql.ToCharArray
        Dim len As Integer = buff.Length
        MsgBox(buff((len - 1)).ToString)
        If (buff((len - 1)) = ",") Then
            sql.TrimEnd()
        End If
        ' MsgBox(sql)
    End Sub
    Public Sub PerformAuditTrail(ByVal tableName As String, ByVal change As String)
        Try
            'Dim cmd As New SqlCommand("spAuditTrail")
            'cmd.CommandType = CommandType.StoredProcedure
            'cmd.Parameters.Add(New SqlParameter("@table", SqlDbType.VarChar)).Value = tableName
            'cmd.Parameters.Add(New SqlParameter("@change", SqlDbType.VarChar)).Value = change
            'cmd.Parameters.Add(New SqlParameter("@user", SqlDbType.VarChar)).Value = CurrentUser
            'ExecuteProc(cmd)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Error Performing Audit Trail")
        End Try
    End Sub
    Private Sub UpdateBatches(ByVal spName As String)
        Dim cmd As New SqlCommand(spName)

        cmd.CommandType = CommandType.StoredProcedure

        cmd.Parameters.Add(New SqlParameter("@UUser", SqlDbType.VarChar, 20)).Value = CurrentUser
        ExecuteProc(cmd)
    End Sub
    Public Sub LogException(ex As Exception, dbpath As [String])
        Try
            Using tx As TextWriter = New StreamWriter("C\logs\exceptions.txt", True)
                Dim logLine As [String] = [String].Format("{0}  :  {1}  : {2} : {3}", DateTime.Now.ToString(), dbpath, ex.Message.ToString(), ex.Source.ToString())
                tx.WriteLine(logLine)
                tx.WriteLine(ex.StackTrace.ToString())
                tx.WriteLine("")
                tx.WriteLine("------------------------------------------------------------------------------------------------------")
                tx.WriteLine("")
                tx.Close()

            End Using
        Catch iex As IOException
        Catch e As Exception
        End Try
    End Sub
    Public Sub LogException(ex As Exception)
        Dim dbpath As String = SQLDatabase
        Try
            Using tx As TextWriter = New StreamWriter("C:\logs\exceptions.txt", True)
                Dim logLine As [String] = [String].Format("{0}  :  {1}  : {2} : {3}", DateTime.Now.ToString(), dbpath, ex.Message.ToString(), ex.Source.ToString())
                tx.WriteLine(logLine)
                tx.WriteLine(ex.StackTrace.ToString())
                tx.WriteLine("")
                tx.WriteLine("------------------------------------------------------------------------------------------------------")
                tx.WriteLine("")
                tx.Close()

            End Using
        Catch iex As IOException
        Catch e As Exception
        End Try
    End Sub
    Public Function ReturnConnectionString() As String
        Dim mString As String = ""
        If ServerName = "" Or UserID = "" Then
            Call ReadRegistry()
        End If
        If ServerName <> "" Then
            mString = "User Id=" & UserID & ";password=" & Password & ";data source=" & ServerName & ";database=" & Database & ""
        End If
        Return mString
    End Function
    Public Sub LogString(s As String)
        Try
            Using tx As TextWriter = New StreamWriter("C:\logs\trail0001.txt", True)
                Dim logLine As [String] = [String].Format("{0}  :  {1}", DateTime.Now.ToString(), s)
                tx.WriteLine(logLine)
                tx.WriteLine("")
                tx.WriteLine("------------------------------------------------------------------------------------------------------")
                tx.WriteLine("")
                tx.Close()
            End Using
        Catch iex As IOException
        Catch e As Exception
            LogException(e)
        End Try
    End Sub
End Class



