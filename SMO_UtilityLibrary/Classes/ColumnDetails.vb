Imports System.ComponentModel
Imports Microsoft.SqlServer.Management.Smo

Namespace Classes
    Public Class ColumnDetails
        ''' <summary>
        ''' Column is a identify column
        ''' </summary>
        <Category("Items"), Description("Indicates if the field is Identity")>
        Public Property Identity() As Boolean
        <Category("General"), Description("Column Name")>
        Public Property Name() As String
        ''' <summary>
        ''' There are plenty of useful properties within DataType as an
        ''' example in the property SqlDataType or IsDate (which we know
        ''' there are multiple data types).
        ''' </summary>
        <Category("Items"), Description("Describes the data type")>
        Public Property DataType() As DataType
        <Category("Items"), Description("Describes the sql data type")>
        Public ReadOnly Property SqlDataType() As SqlDataType
            Get
                Return DataType.SqlDataType
            End Get
        End Property
        '        
        ' * I setup several properties for Dates to show that we can do this but
        ' * generally speaking we don't need to do all of them.
        '         
        <Category("Items"), Description("Indicates if this field is a Date")>
        Public ReadOnly Property IsDate() As Boolean
            Get
                Return DataType.SqlDataType = SqlDataType.Date
            End Get
        End Property
        <Category("Items"), Description("Indicates if this field is a DateTime")>
        Public ReadOnly Property IsDateTime() As Boolean
            Get
                Return DataType.SqlDataType = SqlDataType.DateTime
            End Get
        End Property
        <Category("Items"), Description("Indicates if this field is a DateTime Offset")>
        Public ReadOnly Property IsDateTimeOffset() As Boolean
            Get
                Return DataType.SqlDataType = SqlDataType.DateTimeOffset
            End Get
        End Property
        <Category("Items"), Description("Indicates if this field is Nullable")>
        Public Property Nullable() As Boolean
        <CategoryAttribute("Items"), DescriptionAttribute("Indicates if field is in a primary key")>
        Public Property InPrimaryKey() As Boolean
        ''' <summary>
        ''' get foreign keys
        ''' </summary>
        <Category("Items"), Description("ForeignKeys DataTable")>
        Public Property ForeignKeys() As DataTable
        ''' <summary>
        ''' Contains row data retrieved from EnumForeignKeys
        ''' which represent any foreign key definitions
        ''' </summary>
        <Category("Items"), Description("ForeignKeys break down")>
        Public Property ForeignKeysList() As List(Of ForeignKeysDetails)
        Public Overrides Function ToString() As String
            Return Name
        End Function
    End Class
End Namespace