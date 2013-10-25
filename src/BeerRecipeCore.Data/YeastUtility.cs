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
            selectYeastCommand.CommandText = "SELECT name, type, form, amount, amountIsWeight, laboratory, productId, minTemperature, maxTemperature, flocculation, attenuation, notes FROM Yeasts";
            using (SQLiteDataReader reader = selectYeastCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    string name = reader[0].ToString();
                    string type = reader[1].ToString();
                    string form = reader[2].ToString();
                    float amount = (float) Convert.ToDouble(reader[3]);
                    bool amountIsWeight = Convert.ToBoolean(Convert.ToInt32(reader[4]));
                    string laboratory = reader[5].ToString();
                    string productId = reader[6].ToString();
                    float minTemperature = (float) Convert.ToDouble(reader[7]);
                    float maxTemperature = (float) Convert.ToDouble(reader[8]);
                    string flocculation = reader[9].ToString();
                    float attenuation = (float) Convert.ToDouble(reader[10]);
                    string notes = reader[11].ToString();

                    YeastCharacteristics characteristics = new YeastCharacteristics(type, flocculation, form)
                    {
                        Attenuation = attenuation,
                        MinTemperature = minTemperature,
                        MaxTemperature = maxTemperature
                    };
                    yield return new Yeast(name, characteristics, notes, amount, amountIsWeight, laboratory, productId);
                }
            }
        }
    }
}
