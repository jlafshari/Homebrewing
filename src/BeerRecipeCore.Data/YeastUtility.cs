using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace BeerRecipeCore.Data
{
    public static class YeastUtility
    {
        public static IEnumerable<Yeast> GetAvailableYeasts(SQLiteConnection connection)
        {
            SQLiteCommand selectYeastCommand = connection.CreateCommand();
            selectYeastCommand.CommandText = "SELECT name, type, form, laboratory, productId, minTemperature, maxTemperature, flocculation, attenuation, notes FROM Yeasts";
            using (SQLiteDataReader reader = selectYeastCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    string name = reader[0].ToString();
                    string type = reader[1].ToString();
                    string form = reader[2].ToString();
                    string laboratory = reader[3].ToString();
                    string productId = reader[4].ToString();
                    float minTemperature = (float) Convert.ToDouble(reader[5]);
                    float maxTemperature = (float) Convert.ToDouble(reader[6]);
                    string flocculation = reader[7].ToString();
                    float attenuation = (float) Convert.ToDouble(reader[8]);
                    string notes = reader[9].ToString();

                    YeastCharacteristics characteristics = new YeastCharacteristics(type, flocculation, form)
                    {
                        Attenuation = attenuation,
                        MinTemperature = minTemperature,
                        MaxTemperature = maxTemperature
                    };
                    yield return new Yeast(name, characteristics, notes, laboratory, productId);
                }
            }
        }
    }
}
