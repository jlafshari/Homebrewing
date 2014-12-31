using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using BeerRecipeCore.Data.Models;

namespace BeerRecipeCore.Data
{
    public static class BatchUtility
    {
        public static IEnumerable<BatchDataModel> GetSavedBatches(IList<RecipeDataModel> availableRecipes)
        {
            using (SQLiteConnection connection = DatabaseUtility.GetNewConnection())
            {
                using (SQLiteCommand getBatchesCommand = connection.CreateCommand())
                {
                    getBatchesCommand.CommandText = "SELECT * FROM Batches";
                    using (SQLiteDataReader reader = getBatchesCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int batchId = reader.GetInt32(0);
                            int recipeId = reader.GetInt32(4);
                            BatchDataModel batch = new BatchDataModel(batchId)
                            {
                                BrewerName = reader.GetString(1),
                                AssistantBrewerName = reader.GetString(2),
                                Recipe = availableRecipes.FirstOrDefault(recipe => recipe.RecipeId == recipeId)
                            };

                            DateTime brewingDate;
                            if (DateTime.TryParse(reader.GetString(3), out brewingDate))
                                batch.BrewingDate = brewingDate;

                            // get gravity readings for batch
                            using (SQLiteCommand getGravityReadingsForBatchCommand = connection.CreateCommand())
                            {
                                getGravityReadingsForBatchCommand.CommandText = "SELECT GravityReadings.id, GravityReadings.specificGravity, GravityReadings.date FROM GravityReadings " +
                                    "JOIN GravityReadingsInBatch ON GravityReadingsInBatch.gravityReading = GravityReadings.id AND GravityReadingsInBatch.batch = @batchId";
                                getGravityReadingsForBatchCommand.Parameters.AddWithValue("batchId", batchId);
                                using (SQLiteDataReader gravityReadingsReader = getGravityReadingsForBatchCommand.ExecuteReader())
                                {
                                    while (gravityReadingsReader.Read())
                                    {
                                        GravityReadingDataModel gravityReading = new GravityReadingDataModel(gravityReadingsReader.GetInt32(0)) { Value = gravityReadingsReader.GetDouble(1) };
                                        DateTime gravityReadingDate;
                                        if (DateTime.TryParse(gravityReadingsReader.GetString(2), out gravityReadingDate))
                                            gravityReading.Date = gravityReadingDate;

                                        batch.RecordedGravityReadings.Add(gravityReading);
                                    }
                                }
                            }

                            yield return batch;
                        }
                    }
                }
                connection.Close();
            }
        }
        
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

        public static BatchDataModel CreateBatch(RecipeDataModel recipe)
        {
            BatchDataModel batch = null;
            using (SQLiteConnection connection = DatabaseUtility.GetNewConnection())
            {
                DateTime currentDate = DateTime.Now;
                using (SQLiteCommand insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText = "INSERT INTO Batches (brewerName, assistantBrewerName, brewingDate, recipeInfo) VALUES ('', '', @brewingDate, @recipeInfo)";
                    insertCommand.Parameters.AddWithValue("brewingDate", currentDate.ToString());
                    insertCommand.Parameters.AddWithValue("recipeInfo", recipe.RecipeId);
                    insertCommand.ExecuteNonQuery();
                }
                batch = new BatchDataModel(DatabaseUtility.GetLastInsertedRowId(connection)) { BrewingDate = currentDate, Recipe = recipe };
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
                insertJunctionCommand.CommandText = "INSERT INTO GravityReadingsInBatch (gravityReading, batch) VALUES(@gravityReading, @batch)";
                insertJunctionCommand.Parameters.AddWithValue("gravityReading", gravityReading.GravityReadingId);
                insertJunctionCommand.Parameters.AddWithValue("batch", batchId);
                insertJunctionCommand.ExecuteNonQuery();
            }

            return gravityReading;
        }
    }
}
