namespace DataTransferApp
{
    using System;
    using System.Data.SqlClient;
    using System.Diagnostics;

    using DataTransferApp.Library;

    /// <summary>
    /// The program.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The source connection string.
        /// </summary>
        private const string SourceConnectionString = "";

        /// <summary>
        /// The destination connection string.
        /// </summary>
        private const string DestinationConnectionString = "Server=(localdb)\\mssqllocaldb;Database=test-transfer-db;Trusted_Connection=True;MultipleActiveResultSets=true";

        /// <summary>
        /// The main.
        /// </summary>
        private static void Main()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            
            // Events, Branches, Substrates, Orders, ContainerTypes, Containers, Equipments, Vendors
            var tableName = "[dbo].[Vendors]";
            ImportData(tableName);

            stopWatch.Stop();
            Writer.WriteLineInformation($"\nPress any key to exit. Elapsed: {stopWatch.Elapsed} s.");
            Console.Read();
        }

        /// <summary>
        /// The import data.
        /// </summary>
        /// <param name="tableName">
        /// The table name.
        /// </param>
        private static void ImportData(string tableName)
        {
            var sourceConnBuilder = new SqlConnectionStringBuilder(SourceConnectionString);
            var destinationConnBuilder = new SqlConnectionStringBuilder(DestinationConnectionString);
            
            Writer.WriteLineInformation($"This process will copy {tableName} records from {sourceConnBuilder.InitialCatalog} to {destinationConnBuilder.InitialCatalog}.");
            
            using (var sourceConnection = new SqlConnection(sourceConnBuilder.ConnectionString))
            {
                sourceConnection.Open();

                var sourceCommand = new SqlCommand($"SELECT * FROM {tableName}", sourceConnection);
                var sourceDataReader = sourceCommand.ExecuteReader();

                using (var destinationConnection = new SqlConnection(destinationConnBuilder.ConnectionString))
                {
                    destinationConnection.Open();

                    // ToDo: Check if dest table already has records
                    var destinationCommand = new SqlCommand($"SELECT COUNT(1) FROM {tableName}", destinationConnection);
                    var count = Convert.ToInt32(destinationCommand.ExecuteScalar());

                    if (count == 0)
                    {
                        using (var bulkCopy = new SqlBulkCopy(destinationConnection))
                        {
                            bulkCopy.DestinationTableName = tableName;

                            try
                            {
                                bulkCopy.WriteToServer(sourceDataReader);
                            }
                            catch (Exception e)
                            {
                                Writer.WriteLineException(e.ToString());
                            }
                            finally
                            {
                                sourceDataReader.Close();
                            }
                        }
                    }
                    else
                    {
                        Writer.WriteLineWarning($"Table {tableName} in destination database ({destinationConnBuilder.InitialCatalog}) has {count} records. Data import process will NOT continue.");
                    }
                }
            }
        }
    }
}
