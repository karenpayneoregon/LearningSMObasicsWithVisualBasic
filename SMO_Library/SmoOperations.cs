using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMO_Library 
{
    /// <summary>
    /// Various methods to demo working with smo objects.
    /// </summary>
    /// <remarks>
    /// References needed are under the following folder where may be different
    /// depending on the version of SQL-Server installed, most likely at the 
    /// 130 part.
    /// C:\Program Files\Microsoft SQL Server\130\SDK\Assemblies
    /// 
    /// </remarks>
    public class SmoOperations : BaseExceptionProperties
    {

        /// <summary>
        /// Your server name e.g. could be (local) or perhaps .\SQLEXPRESS
        /// </summary>
        public string ServerName { get => "KARENS-PC"; }
        private Server mServer;
        public Server Server { get { return mServer; } }
        public SmoOperations()
        {
            mServer = InitializeServer();
        }
        Server InitializeServer()
        {
            ServerConnection connection = new ServerConnection(ServerName);
            Server sqlServer = new Server(connection);
            return sqlServer;
        }
        public DataTable AvailableServers()
        {
            return SmoApplication.EnumAvailableSqlServers(true);
        }
        public List<LocalServer> LocalServers()
        {
            return SmoApplication.EnumAvailableSqlServers(true).AsEnumerable()
                .Select(row => new LocalServer()
                {
                    Name = row.Field<string>("Name"),
                    Instance = row.Field<string>("Instance"),
                    ServerName = row.Field<string>("Server")
                }).ToList();
        }
        public List<string> DatabaseNames()
        {
            return mServer.Databases.OfType<Database>().Select(db => db.Name).ToList();
        }

        /// <summary>
        /// Determine if database exists on the server.
        /// </summary>
        /// <param name="pDatabaseName"></param>
        /// <returns></returns>
        public bool DatabaseExists(string pDatabaseName)
        {
            var databaseNames = new List<string>();
            var item = mServer.Databases.OfType<Database>().FirstOrDefault(db => db.Name == pDatabaseName);
            if (item != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Return a valid Database based on a database name
        /// </summary>
        /// <param name="pDatabaseName"></param>
        /// <returns></returns>
        public Database GetDatabase(string pDatabaseName)
        {
            return mServer.Databases.OfType<Database>().FirstOrDefault(db => db.Name == pDatabaseName);
        }
        /// <summary>
        /// Create a new database
        /// </summary>
        /// <param name="pDatabaseName">Name of new database</param>
        /// <returns></returns>
        public SqlDatabase CreateDatabase(string pDatabaseName)
        {
            var database = new SqlDatabase() { Name = pDatabaseName, Exists = false };
            try
            {
                Database db;
                db = new Database(mServer, pDatabaseName);
                db.Create();
                db = mServer.Databases[pDatabaseName];
                database.Database = db;
                database.Exists = true;

            }
            catch (Exception ex)
            {
                mHasException = true;
                mLastException = ex;
            }

            return database;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDatabaseName"></param>
        /// <param name="pKill">If true Deletes the specified database and drops any active connection. else drop the database </param>
        /// <returns></returns>
        /// <remarks>
        /// Database.Drop can fail it there is an open connection e.g. running a query
        /// in SSMS or perhaps some code that recently ran did not release it's connection
        /// to the database to be dropped.
        /// </remarks>
        public bool DropDatabase(string pDatabaseName, bool pKill = false)
        {
            bool success = true;
            try
            {
                if (pKill)
                {
                    mServer.KillDatabase(pDatabaseName);
                }
                else
                {
                    GetDatabase(pDatabaseName).Drop();
                }
                
            }
            catch (Exception ex)
            {
                mHasException = true;
                mLastException = ex;
                success = false;
            }

            return success;
        }
        /// <summary>
        /// Get table names for database
        /// </summary>
        /// <param name="pDatabaseName">Exists SQL-Server database</param>
        /// <returns></returns>
        /// <remarks>System objects/tables are filtered out</remarks>
        public List<string> TableNames(string pDatabaseName)
        {
            var tableNames = new List<string>(); 
            var database = mServer.Databases.OfType<Database>().FirstOrDefault(tbl => tbl.Name == pDatabaseName);

            if (database != null)
            {
                tableNames = database.Tables.OfType<Table>().Where(tbl => !tbl.IsSystemObject).Select(tbl => tbl.Name).ToList();
            }

            return tableNames;

        }
        public Table GetTableByName(string pDatabaseName, string pTableName)
        {
            Table tblResult = null;
            var database = mServer.Databases.OfType<Database>().FirstOrDefault(tbl => tbl.Name == pDatabaseName);
            if (database != null)
            {
                tblResult = database.Tables.OfType<Table>().Where(tbl => !tbl.IsSystemObject).Select(tbl => tbl).FirstOrDefault();
            }

            return tblResult;
        }
        /// <summary>
        /// Does the table exists in the specified database
        /// </summary>
        /// <param name="pDatabaseName">valid SQL-Server database</param>
        /// <param name="pTableName">Table name to see if it exists in pDatabaseName</param>
        /// <returns></returns>
        public bool TableExists(string pDatabaseName, string pTableName)
        {
            bool exists = false;
            var database = mServer.Databases.OfType<Database>().FirstOrDefault(tbl => tbl.Name == pDatabaseName);

            if (database != null)
            {
                exists = (database.Tables.OfType<Table>().Where(tbl => !tbl.IsSystemObject).FirstOrDefault(tbl => tbl.Name == pTableName) != null);
            }

            return exists;
        }
        /// <summary>
        /// Get column names for table in database.
        /// </summary>
        /// <param name="pDatabaseName">valid SQL-Server database</param>
        /// <param name="pTableName">Exists table in pDatabaseName</param>
        /// <returns></returns>
        public List<string> TableColumnNames(string pDatabaseName, string pTableName)
        {
            var columnNames = new List<string>();
            var database = mServer.Databases.OfType<Database>().FirstOrDefault(db => db.Name == pDatabaseName);

            if (database != null)
            {
                var table = database.Tables.OfType<Table>().FirstOrDefault(tbl => tbl.Name == pTableName);

                if (table != null)
                {
                    
                    columnNames = table.Columns.OfType<Column>().Select(col => col.Name).ToList();
                }
            }

            return columnNames;
        }
        /// <summary>
        /// An example for creating a database with one table
        /// </summary>
        /// <returns></returns>
        public bool CreateTable(string pDatabaseName)
        {
            try
            {
                SqlDatabase db = CreateDatabase(pDatabaseName);
                var tblCustomer = new Table(db.Database, "Customer");
                var colPrimaryKey = new Column(tblCustomer, "Id", DataType.Int)
                {
                    Identity = true,
                    IdentityIncrement = 1,
                    IdentitySeed = 1,
                    Nullable = false
                };              
                tblCustomer.Columns.Add(colPrimaryKey);

                var idxPrimary = new Index(tblCustomer, "PK_Customer_id");
                tblCustomer.Indexes.Add(idxPrimary);

                idxPrimary.IndexedColumns.Add(new IndexedColumn(idxPrimary, colPrimaryKey.Name));
                idxPrimary.IsClustered = true;
                idxPrimary.IsUnique = true;
                idxPrimary.IndexKeyType = IndexKeyType.DriPrimaryKey;

                var colFirstName = new Column(tblCustomer, "FirstName", DataType.NVarCharMax)
                {
                    Nullable = true
                };
                tblCustomer.Columns.Add(colFirstName);

                var colLastName = new Column(tblCustomer, "LastName", DataType.NVarCharMax)
                {
                    Nullable = true
                };
                tblCustomer.Columns.Add(colLastName);

                var colBirthDate = new Column(tblCustomer, "BirthDate", DataType.Date)
                {
                    Nullable = true
                } ;
                tblCustomer.Columns.Add(colBirthDate);


                tblCustomer.Create();

                return true;
            }
            catch (Exception ex)
            {
                mHasException = true;
                mLastException = ex;
                return false;
            }
        }
        /// <summary>
        /// Return default server name
        /// </summary>
        /// <returns></returns>
        public string GetDefaultServerName()
        {
            return mServer.Name;
        }
        public string DefaultServerName()
        {
            ServerConnection connection = new ServerConnection();
            return connection.TrueName;

        }
        /// <summary>
        /// Return SQL-Server install path
        /// </summary>
        /// <returns></returns>
        public string SqlServerInstallPath()
        {
            return mServer.RootDirectory;
        }
        /// <summary>
        /// Does a column name exists in a table within a specific database
        /// </summary>
        /// <param name="pDatabaseName">valid SQL-Server database</param>
        /// <param name="pTableName">Exists table in pDatabaseName</param>
        /// <param name="pColumnName">Column to check if it exists in pTableName in pDatabaseName</param>
        /// <returns></returns>
        public bool ColumnExists(string pDatabaseName, string pTableName, string pColumnName)
        {
            bool exists = false;
            var database = mServer.Databases.OfType<Database>().FirstOrDefault(db => db.Name == pDatabaseName);

            if (database != null)
            {
                var table = database.Tables.OfType<Table>().FirstOrDefault(tbl => tbl.Name == pTableName);
                if (table != null)
                {
                    exists = (table.Columns.OfType<Column>().FirstOrDefault(col => col.Name == pColumnName) != null);
                }
            }

            return exists;
        }
        /// <summary>
        /// Get details for each column in a table within a database.
        /// There are more details then returned here so feel free to explore.
        /// </summary>
        /// <param name="pDatabaseName">valid SQL-Server database</param>
        /// <param name="pTableName">Exists table in pDatabaseName</param>
        /// <returns></returns>
        public List<ColumnDetails> GetColumnDetails(string pDatabaseName, string pTableName)
        {
            var columnDetails = new List<ColumnDetails>();
            var columnNames = new List<string>();
            var database = mServer.Databases.OfType<Database>().FirstOrDefault(db => db.Name == pDatabaseName);

            if (database != null)
            {
                var table = database.Tables.OfType<Table>().FirstOrDefault(tbl => tbl.Name == pTableName);

                if (table != null)
                {
                    columnDetails = table.Columns.OfType<Column>().Select(col => new ColumnDetails()
                    {
                        Identity = col.Identity,
                        DataType = col.DataType,
                        Name = col.Name,
                        InPrimaryKey = col.InPrimaryKey,
                        Nullable = col.Nullable
                    }).ToList();
                }
            }

            return columnDetails;
        }
        /// <summary>
        /// Get foreign key details for specified table in specified database
        /// </summary>
        /// <param name="pDatabaseName">valid SQL-Server database</param>
        /// <param name="pTableName">Exists table in pDatabaseName</param>
        /// <returns></returns>
        public List<ForeignKeysDetails> TableKeys(string pDatabaseName, string pTableName)
        {
            var keyList = new List<ForeignKeysDetails>();
            var database = mServer.Databases.OfType<Database>().FirstOrDefault(db => db.Name == pDatabaseName);
            if (database != null)
            {
                var table = database.Tables.OfType<Table>().FirstOrDefault(tbl => tbl.Name == pTableName);
                if (table != null)
                {
                    foreach (Column item in table.Columns.OfType<Column>())
                    {
                        List< ForeignKeysDetails> fkds = item.EnumForeignKeys().AsEnumerable().Select(row => new ForeignKeysDetails
                        {
                            TableSchema = row.Field<string>("Table_Schema"),
                            TableName = row.Field<string>("Table_Name"),
                            SchemaName = row.Field<string>("Name")
                        }).ToList();

                        foreach (ForeignKeysDetails ts in fkds)
                        {
                            keyList.Add(ts);
                        }
                    }
                }
            }

            return keyList;

        }

        #region Experimenting (not in code sample)

        public void CreateTableScriptsForSpecificDatabase()
        {
            String dbName = "NorthWindAzure"; // database name, Connect to the local, default instance of SQL Server.
            Server srv = new Server();
            // Reference the database.
            Database db = srv.Databases[dbName];
            // Define a Scripter object and set the required scripting options.
            Scripter scrp = new Scripter(srv);
            scrp.Options.ScriptDrops = false;
            scrp.Options.WithDependencies = true;
            scrp.Options.Indexes = true; // To include indexes
            scrp.Options.DriAllConstraints = true; // to include referential constraints in the script
                                                   // Iterate through the tables in database and script each one. Display the script.
            foreach (Table tb in db.Tables)
            {
                // check if the table is not a system table
                if (tb.IsSystemObject == false)
                {
                    Console.WriteLine("-- Scripting for table " + tb.Name);
                    // Generating script for table tb
                    System.Collections.Specialized.StringCollection sc = scrp.Script(new Urn[] { tb.Urn });
                    foreach (string st in sc)
                    {
                        Console.WriteLine(st);
                    }
                    Console.WriteLine("--");
                }
            }

        }
        public void DependencyWalkerExperimenting()
        {
            String srvName = "KARENS-PC";
            String dbName = "NorthWindAzure";
            Server srv = new Server(new ServerConnection() { ServerInstance = srvName });
            Database db = srv.Databases[dbName];

            DependencyWalker dependencyWalker = new DependencyWalker(srv);
            DependencyTree dependencyTree = dependencyWalker.DiscoverDependencies(
                new Urn[] { db.Tables["Orders","dbo"].Urn }, DependencyType.Parents);


            DependencyCollection dependencyCollection = dependencyWalker.WalkDependencies(dependencyTree);
            foreach (DependencyCollectionNode node in dependencyCollection.OrderBy(n => n.Urn.Value))
            {
                Console.WriteLine("{0}\t{1}", node.Urn.Type, node.Urn.GetAttribute("Name"));
                Console.WriteLine("\t{0}\n", node.Urn.Value);
                Console.WriteLine("\t{0}", node.Urn.Parent);
            }
        }
        #endregion
    }
}
