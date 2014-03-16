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
                    string name = reader.GetString(0);
                    string type = reader.GetString(1);
                    string form = reader.GetString(2);
                    string laboratory = reader.GetString(3);
                    string productId = reader.GetString(4);
                    float minTemperature = reader.GetFloat(5);
                    float maxTemperature = reader.GetFloat(6);
                    string flocculation = reader.GetString(7);
                    float attenuation = reader.GetFloat(8);
                    string notes = reader.GetString(9);

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
