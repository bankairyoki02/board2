Imports System.Xml
Imports System.IO

Public Class BusinessObjectProcessor

    Private _pClient As SysproPClient
    Private _sUserId As String
    Private _sSysproConnectionString As String
    Private _dtBOErrorRows As New DataTable

    ''' <summary>
    ''' Outputs a datatable containing rows that were associated with a business object error or errors.
    ''' The BOErrorValues datatable rows and columns should be cleared if you will do subsequent BO calls in the same instance of this class.
    ''' </summary>
    ''' <returns>BOErrorValues datatable containing all the errors since the last ResetBOErrorRowsDT() method were called</returns>
    Public Property BOErrorRows As DataTable
        Get
            BOErrorRows = _dtBOErrorRows
        End Get
        Set(value As DataTable)
            _dtBOErrorRows = value
        End Set
    End Property

    Public Sub New(SysproServer As String, SysproPort As String, SysproOperator As String, SysproConnectionString As String)
        _pClient = New SysproPClient(SysproServer, SysproPort)
        _sSysproConnectionString = SysproConnectionString

        Try
            _sUserId = _pClient.GetSysproUserID(SysproOperator, _sSysproConnectionString)
            If _sUserId = "" Then
                Throw New Exception("No UserId for this company.")
            End If
        Catch ex As Exception
            MsgBox("An error occured when retrieving an existing Syspro Connection from AdmState." + vbCrLf + vbCrLf + "Please log into Syspro {9}." + vbCrLf + vbCrLf + ex.Message, MsgBoxStyle.Exclamation)
            Me.Finalize()
        End Try

    End Sub

    Public Sub ResetBOErrorRowsDT()
        _dtBOErrorRows.Rows.Clear()
        _dtBOErrorRows.Columns.Clear()
    End Sub

    Enum BOSetupMethod
        Add
        Update
        Delete
    End Enum

    ''' <summary>
    ''' Takes the schema of the source datatable and translates that into the execution of add/update/delete business object calls
    ''' </summary>
    ''' <param name="tSource">Datatable - primary key values must be assigned in dataset designer. All non readOnly columns will be passed to the BO. Column Names must match the business object.</param>
    ''' <param name="BORootXmlNodeName">The root node name for the business object. This will be used when building the input xml for the BO call.</param>
    ''' <param name="BOItemXmlNodeName">Each datatable row will be enclosed in a node with this name. ex. Item</param>
    ''' <returns></returns>
    Public Function DatatableToBOSetupCall(tSource As DataTable, businessObject As String, BORootXmlNodeName As String, BOItemXmlNodeName As String, Optional xmlParam As String = "") As Boolean
        If _sUserId = "" Then
            Return False
        End If

        'get the rowstate rows
        'build the item xml
        'do the BO call
        'if success - do rowstate accept changes
        'if fail - highlight the error row
        'repeat for add/update/delete

        'setup tables for each setup method
        'dt rows will be assigned to these tables based on their DataRowState
        Dim dsAddRows = New DataSet(BORootXmlNodeName),
        dtAddRows = dsAddRows.Tables.Add(BOItemXmlNodeName),
        dsUpdateRows = New DataSet(BORootXmlNodeName), 'need seperate ds becaues the datatable names are all the same
        dtUpdateRows = dsUpdateRows.Tables.Add(BOItemXmlNodeName),
        dsDeleteRows = New DataSet(BORootXmlNodeName),
        dtDeleteRows = dsDeleteRows.Tables.Add(BOItemXmlNodeName)

        'get the primary key columns
        'CONFIG: primary keys are set in dataset designer
        Dim pKey = tSource.PrimaryKey,
        pKeyColumns = From c As DataColumn In tSource.Columns
                      Where pKey.Contains(c)

        'grab the non-readonly columns from the dt schema
        'CONFIG: column readonly status' are set in dataset designer
        Dim WriteableColumns = From c As DataColumn In tSource.Columns
                               Where Not c.ReadOnly ' assuming that read-only columns will by calculated automatically by the DB

        'setup the datatable schemas
        'add primary key coluns
        For Each c In pKey
            dtAddRows.Columns.Add(c.ColumnName, c.DataType)
            dtUpdateRows.Columns.Add(c.ColumnName, c.DataType)
            'special case: deletes only require the primary keys to be passed in the xml
            dtDeleteRows.Columns.Add(c.ColumnName, c.DataType)
        Next
        For Each c In WriteableColumns
            dtAddRows.Columns.Add(c.ColumnName, c.DataType)
            dtUpdateRows.Columns.Add(c.ColumnName, c.DataType)
        Next

        'seperate out the source datatable rows into individual datatables based on their row state
        For Each r As DataRow In tSource.Rows
            Dim AddRowItems = From c In pKeyColumns.Union(WriteableColumns)
                              Select r(c, DataRowVersion.Current)

            Dim updateRowItems = From c In pKeyColumns.Union(WriteableColumns)
                                 Select If(Not IsDBNull(r(c, DataRowVersion.Current)) AndAlso (r(c, DataRowVersion.Current) <> r(c, DataRowVersion.Original) Or pKey.Contains(c)), r(c, DataRowVersion.Current), DBNull.Value)

            Dim deleteRowItems = From c In pKeyColumns
                                 Select r(c, DataRowVersion.Original)

            Select Case r.RowState
                Case DataRowState.Added
                    dtAddRows.Rows.Add(AddRowItems.ToArray)
                Case DataRowState.Deleted
                    'deletes only require the primary key fields
                    dtDeleteRows.Rows.Add(deleteRowItems.ToArray)
                Case DataRowState.Modified
                    'updates will only have the key and changed fields populated
                    dtUpdateRows.Rows.Add(updateRowItems.ToArray)
                Case Else
                    Debug.Assert(r.RowState = DataRowState.Unchanged)
            End Select
        Next

        'convert the datatables to xml
        Dim writerXmlAdd = New StringWriter(),
            writerXmlUpdate = New StringWriter(),
            writerXmlDelete = New StringWriter(),
            XmlAdd = "",
            XmlUpdate = "",
            XmlDelete = ""

        'build xml - only if there is BO data to process
        If dtAddRows.Rows.Count > 0 Then
            dtAddRows.WriteXml(writerXmlAdd, XmlWriteMode.IgnoreSchema, True)
            Debug.Print(writerXmlAdd.ToString)
            XmlAdd = nestPKeyNodes(BOItemXmlNodeName, pKey, writerXmlAdd)
            Debug.Print(XmlAdd)
        End If
        If dtUpdateRows.Rows.Count > 0 Then
            dtUpdateRows.WriteXml(writerXmlUpdate)
            Debug.Print(writerXmlUpdate.ToString)
            XmlUpdate = nestPKeyNodes(BOItemXmlNodeName, pKey, writerXmlUpdate)
            Debug.Print(XmlUpdate)
        End If
        If dtDeleteRows.Rows.Count > 0 Then
            dtDeleteRows.WriteXml(writerXmlDelete)
            Debug.Print(writerXmlDelete.ToString)
            XmlDelete = nestPKeyNodes(BOItemXmlNodeName, pKey, writerXmlDelete)
            Debug.Print(XmlDelete)
        End If

        'TODO: async each BO call if save performance is slow
        'call the BOs
        'these will return true if the xml being passed in is empty (meaning there is nothing for the BO to do)
        If GenericBOSetupCall(businessObject, "Add", XmlAdd, xmlParam) And GenericBOSetupCall(businessObject, "Update", XmlUpdate, xmlParam) And GenericBOSetupCall(businessObject, "Delete", XmlDelete, xmlParam) Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Shared Function nestPKeyNodes(BOItemXmlNodeName As String, pKey() As DataColumn, ByRef writerXml As StringWriter) As String
        Debug.Print(writerXml.ToString)

        Dim xmlDoc = New XmlDocument
        xmlDoc.LoadXml(writerXml.ToString)

        'grab all the item nodes and move the key fields into a new key node
        Dim itemNodes = xmlDoc.SelectNodes($"//{BOItemXmlNodeName}")
        For Each i In itemNodes
            'create new Key node and add it to the item node
            Dim keyNode = xmlDoc.CreateElement("key")
            i.PrependChild(keyNode)

            'Move the pKey nodes into the new keyNode
            For Each c In pKey
                Dim moveNode = i.SelectSingleNode($"{c.ColumnName}")
                keyNode.AppendChild(moveNode)
            Next
        Next

        Return xmlDoc.OuterXml
    End Function

    ''' <summary>
    ''' Does all the magic required to call a BO Setup Call (Add, Update, or Delete operations).
    ''' </summary>
    ''' <param name="businessObject">business object being called - 6 character designation - ex. PORSSX</param>
    ''' <param name="BOSetupMethod">Enum Add, Update, or Delete</param>
    ''' <param name="xmlIn">Input XML</param>
    ''' <param name="xmlParam">Optional: Parameter XML for the specific BO being called. Generic default might work.</param>
    ''' <returns>True or false depending on success of BO call. Also returns true if there is no xmlIn for the BO to process. BO Results can be retrieved through property BOErrorRows</returns>
    Public Function GenericBOSetupCall(businessObject As String, BOSetupMethod As String, xmlIn As String, Optional xmlParam As String = "") As Boolean
        'no data to update so bail
        If xmlIn.Length = 0 Then
            Return True
        End If

        If _sUserId = "" Then
            MessageBox.Show("Uhoh, no UserId has been retrieved for use in the business object calls.", "No UserId", MessageBoxButtons.OK)
            Return False
        End If

        Dim xmlOutput As String = "",
            xmldoc As New XmlDocument()

        xmldoc.LoadXml(xmlIn)
        Dim rootNodeName = xmldoc.DocumentElement.Name

        'if none was provided use default BO parameter XML
        If xmlParam = "" Then
            xmlParam = $"<{rootNodeName}>
	                    <Parameters>
		                    <IgnoreWarnings>N</IgnoreWarnings>
		                    <ApplyIfEntireDocumentValid>Y</ApplyIfEntireDocumentValid>
		                    <ValidateOnly>N</ValidateOnly>
	                    </Parameters>
                    </{rootNodeName}>
                    "
        End If

        'setup results datatable
        If _dtBOErrorRows.Columns.Count = 0 Then
            Dim keyNode = xmldoc.SelectSingleNode("//key")
            For Each keyColumn As XmlNode In keyNode.ChildNodes
                _dtBOErrorRows.Columns.Add(keyColumn.Name)
            Next
            _dtBOErrorRows.Columns.Add("ErrorColumn") 'string
            _dtBOErrorRows.Columns.Add("ErrorValue") 'string
            _dtBOErrorRows.Columns.Add("ErrorDescription") 'string
            _dtBOErrorRows.Columns.Add("SetupMethod") 'string
            _dtBOErrorRows.Columns.Add("BOCallHadErrors") 'bool
        End If

        Try
            Select Case BOSetupMethod
                Case "Delete" 'delete needs to be first in the event that the user deletes a record and remakes another with the same primary key
                    xmlOutput = _pClient.SetupDelete(_sUserId, businessObject, xmlParam, xmlIn)
                Case "Add"
                    xmlOutput = _pClient.SetupAdd(_sUserId, businessObject, xmlParam, xmlIn)
                Case "Update"
                    xmlOutput = _pClient.SetupUpdate(_sUserId, businessObject, xmlParam, xmlIn)
                Case Else
                    Throw New Exception($"Unknown business object setup method '{BOSetupMethod}' being passed to SetupSupplierStockXref().")
                    Return False
            End Select

            'Check output
            xmldoc.LoadXml(xmlOutput)

            '-------EXAMPLE OF XML OUT------------------------
            '<?xml version="1.0" encoding="Windows-1252"?>
            '<setupsupplierstockxref Language="05" Language2="EN" CssStyle="" DecFormat="1" DateFormat="01" Role="01" Version="8.0.006" OperatorPrimaryRole="   ">
            '  <item>
            '    <key>
            '      <supplier>1000</supplier>
            '      <stockcode>12345111111111116</stockcode>
            '    </key>
            '    <stockcode>
            '      <Value>12345111111111116</Value>
            '      <ErrorNumber>240004</ErrorNumber>
            '      <ErrorDescription>Stock code '12345111111111116' not found</ErrorDescription>
            '    </stockcode>
            '  </item>
            '  <item>
            '    <key>
            '      <supplier>1000</supplier>
            '      <stockcode>123456</stockcode>
            '    </key>
            '    <demoleadtime>
            '      <Value>5f</Value>
            '      <ErrorNumber>100031</ErrorNumber>
            '      <ErrorDescription>XML element 'demoleadtime' must be numeric</ErrorDescription>
            '    </demoleadtime>
            '  </item>
            '  <StatusOfItems>
            '    <RecordsRead>2</RecordsRead>
            '    <RecordsInvalid>2</RecordsInvalid>
            '  </StatusOfItems>
            '</setupsupplierstockxref>
            '------------------------------------------


            Dim items = xmldoc.SelectNodes("//item"),
                hasErrors = xmldoc.SelectSingleNode("//RecordsInvalid").InnerText <> "0"

            If hasErrors Then
                For Each item As XmlNode In items
                    Dim newRow As DataRow = _dtBOErrorRows.NewRow

                    For Each node As XmlNode In item.ChildNodes
                        'get the primary key values and load them into the new row
                        If node.Name = "key" Then
                            For Each keyColumn As XmlNode In node.ChildNodes
                                newRow.Item(keyColumn.Name) = keyColumn.InnerText
                            Next
                        End If

                        If node.SelectSingleNode("ErrorDescription") IsNot Nothing Then
                            newRow.Item("ErrorColumn") = node.Name
                            newRow.Item("ErrorValue") = node.SelectSingleNode("Value").InnerText
                            newRow.Item("ErrorDescription") = "Setup " & BOSetupMethod & " - " & node.SelectSingleNode("ErrorDescription").InnerText
                            newRow.Item("SetupMethod") = BOSetupMethod
                        End If

                        newRow.Item("BOCallHadErrors") = hasErrors
                    Next

                    _dtBOErrorRows.Rows.Add(newRow)
                Next

                Return False
            End If

        Catch ex As Exception
            MessageBox.Show($"An error occurred while running setup BO using the '{LCase(BOSetupMethod)}' method. " & vbCrLf & vbCrLf & ex.Message)
            Return False
        End Try

        Return True
    End Function

End Class
