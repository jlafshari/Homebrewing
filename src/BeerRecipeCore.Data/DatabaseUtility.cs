using System;
using System.Data.SQLite;

namespace BeerRecipeCore.Data
{
    internal static class DatabaseUtility
    {
        internal static int GetLastInsertedRowId(SQLiteConnection connection)
        {
            using SQLiteCommand getRowIdCommand = connection.CreateCommand();
            getRowIdCommand.CommandText = "SELECT last_insert_rowid()";
            return Convert.ToInt32(getRowIdCommand.ExecuteScalar());
        }

        internal static SQLiteConnection GetNewConnection()
        {
            var connection = new SQLiteConnection(DatabaseConnectionString);
            connection.Open();
            return connection;
        }

        internal static readonly string DatabaseConnectionString = "Data Source=" + Properties.Settings.Default.DatabaseLocation;
    }
}
