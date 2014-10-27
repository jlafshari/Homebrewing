using System;
using System.Data.SQLite;
using BeerRecipeCore.Data.Models;

namespace BeerRecipeCore.Data
{
    public static class BatchUtility
    {
        public static void SaveBatch(BatchDataModel batch)
        {
            using (SQLiteConnection connection = DatabaseUtility.GetNewConnection())
            {
                using (SQLiteCommand updateBatchCommand = connection.CreateCommand())
                {
                    updateBatchCommand.CommandText = "UPDATE Batches SET brewerName = @brewerName, assistantBrewerName = @assistantBrewerName, brewingDate = @brewingDate WHERE id = @id";
                    updateBatchCommand.Parameters.AddWithValue("id", batch.BatchId);
                    updateBatchCommand.Parameters.AddWithValue("brewerName", batch.BrewerName);
                    updateBatchCommand.Parameters.AddWithValue("assistantBrewerName", batch.AssistantBrewerName);
                    updateBatchCommand.Parameters.AddWithValue("brewingDate", batch.BrewingDate.ToString());
                    updateBatchCommand.ExecuteNonQuery();
                }

                foreach (GravityReadingDataModel gravityReading in batch.RecordedGravityReadings)
                {
                    using (SQLiteCommand updateGravityReadingCommand = connection.CreateCommand())
                    {
                        updateGravityReadingCommand.CommandText = "UPDATE GravityReadings SET specificGravity = @value, date = @date WHERE id = @id";
                        updateGravityReadingCommand.Parameters.AddWithValue("id", gravityReading.GravityReadingId);
                        updateGravityReadingCommand.Parameters.AddWithValue("value", gravityReading.Value);
                        updateGravityReadingCommand.Parameters.AddWithValue("date", gravityReading.Date);
                        updateGravityReadingCommand.ExecuteNonQuery();
                    }
                }

                connection.Close();
            }
        }

        public static BatchDataModel CreateBatch(int recipeId)
        {
            BatchDataModel batch = null;
            using (SQLiteConnection connection = DatabaseUtility.GetNewConnection())
            {
                using (SQLiteCommand insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText = "INSERT INTO Batches (brewerName, assistantBrewerName, brewingDate, recipeInfo) VALUES ('', '', '', @recipeInfo)";
                    insertCommand.Parameters.AddWithValue("recipeInfo", recipeId);
                    insertCommand.ExecuteNonQuery();
                }
                batch = new BatchDataModel(DatabaseUtility.GetLastInsertedRowId(connection));
                connection.Close();
            }
            return batch;
        }

        public static GravityReadingDataModel CreateGravityReading(int batchId)
        {
            GravityReadingDataModel gravityReading = null;
            using (SQLiteConnection connection = DatabaseUtility.GetNewConnection())
            {
                gravityReading = CreateGravityReading(batchId, connection);
                connection.Close();
            }
            return gravityReading;
        }

        internal static GravityReadingDataModel CreateGravityReading(int batchId, SQLiteConnection connection)
        {
            DateTime date = DateTime.Now;
            using (SQLiteCommand insertGravityReadingCommand = connection.CreateCommand())
            {
                insertGravityReadingCommand.CommandText = "INSERT INTO GravityReadings (specificGravity, date) VALUES(0, @date)";
                insertGravityReadingCommand.Parameters.AddWithValue("date", date.ToString());
                insertGravityReadingCommand.ExecuteNonQuery();
            }
            GravityReadingDataModel gravityReading = new GravityReadingDataModel(DatabaseUtility.GetLastInsertedRowId(connection)) { Date = date };

            using (SQLiteCommand insertJunctionCommand = connection.CreateCommand())
            {
                insertJunctionCommand.CommandText = "INSERT INTO GravityReadingsInBatches (gravityReading, batch) VALUES(@gravityReading, @batch)";
                insertJunctionCommand.Parameters.AddWithValue("gravityReading", gravityReading.GravityReadingId);
                insertJunctionCommand.Parameters.AddWithValue("batch", batchId);
                insertJunctionCommand.ExecuteNonQuery();
            }

            return gravityReading;
        }
    }
}
