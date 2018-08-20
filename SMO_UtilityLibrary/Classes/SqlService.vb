
Imports Microsoft.SqlServer.Management.Smo.Wmi
Imports System.Data.Sql
Imports Microsoft.SqlServer.Management.Smo
'https://docs.microsoft.com/en-us/previous-versions/sql/sql-server-2005/ms162139(v=sql.90)
'SMO Overview https://docs.microsoft.com/en-us/sql/relational-databases/server-management-objects-smo/overview-smo?view=sql-server-2017
'https://normshield.zendesk.com/hc/en-us/articles/207327055-SQL-Server-Configuration-Error-Cannot-connect-to-WMI-provider-When-Configuring-Database-for-Installation
Namespace Classes
    Public Class SqlService
        Public Sub Demo()
            ''Declare and create an instance of the ManagedComputer object that represents the WMI Provider services.
            Dim mc As ManagedComputer
            mc = New ManagedComputer()
            'mc.ConnectionSettings.ProviderArchitecture = ProviderArchitecture.Use64bit
            'Console.WriteLine(mc.ConnectionSettings.ProviderArchitecture.ToString())
            'Dim test = SmoApplication.EnumAvailableSqlServers(True)

            Console.WriteLine()
            ''Iterate through each service registered with the WMI Provider.
            Try
                Dim svc As Service
                For Each svc In mc.Services
                    Console.WriteLine(svc.Name)
                Next

            Catch ex As Exception
                Console.WriteLine(ex.ToString())
            End Try
            ''Reference the Microsoft SQL Server service.
            'svc = mc.Services("MSSQLSERVER")
            'Dim instance As SqlDataSourceEnumerator = SqlDataSourceEnumerator.Instance
            'Dim table As System.Data.DataTable = instance.GetDataSources()
            Console.WriteLine(System.Environment.Is64BitOperatingSystem)
        End Sub

        Public Sub Demo1()
            Dim mc As ManagedComputer = New ManagedComputer()
            Try
                Dim svc As Service
                For Each svc In mc.Services
                    Console.WriteLine(svc.Name)
                Next

            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
        End Sub


    End Class
End Namespace
