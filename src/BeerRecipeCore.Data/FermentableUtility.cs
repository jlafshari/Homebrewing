using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace BeerRecipeCore.Data
{
    public static class FermentableUtility
    {
        public static IEnumerable<Fermentable> GetAvailableFermentables(SQLiteConnection connection)
        {
            SQLiteCommand selectFermentablesCommand = connection.CreateCommand();
            selectFermentablesCommand.CommandText = "SELECT name, yield, color, origin, notes, diastaticPower FROM Fermentables";
            using (SQLiteDataReader reader = selectFermentablesCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    string name = reader[0].ToString();
                    float yield = (float) Convert.ToDouble(reader[1]);
                    float color = (float) Convert.ToDouble(reader[2]);
                    string origin = reader[3].ToString();
                    string notes = reader[4].ToString();
                    string diastaticPowerValue = reader[5].ToString();
                    float? diastaticPower = !string.IsNullOrEmpty(diastaticPowerValue) ? (float?) Convert.ToDouble(diastaticPowerValue) : null;

                    FermentableCharacteristics characteristics = new FermentableCharacteristics(yield, color, diastaticPower);
                    yield return new Fermentable(name, characteristics, notes, origin);
                }
            }
        }
    }
}
