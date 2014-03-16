using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Utility;

namespace BeerRecipeCore.Data
{
    public static class FermentableUtility
    {
        public static IEnumerable<Fermentable> GetAvailableFermentables(SQLiteConnection connection)
        {
            SQLiteCommand selectFermentablesCommand = connection.CreateCommand();
            selectFermentablesCommand.CommandText = "SELECT name, yield, yieldByWeight, color, origin, notes, diastaticPower, type, gravityPoint FROM Fermentables";
            using (SQLiteDataReader reader = selectFermentablesCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    string yieldValue = reader[1].ToString();
                    float? yield = !yieldValue.IsNullOrEmpty() ? (float?) float.Parse(yieldValue) : null;
                    string yieldByWeightValue = reader[2].ToString();
                    float? yieldByWeight = !yieldByWeightValue.IsNullOrEmpty() ? (float?) float.Parse(yieldByWeightValue) : null;
                    float color = reader.GetFloat(3);
                    string origin = reader.GetString(4);
                    string notes = reader.GetString(5);
                    string diastaticPowerValue = reader[6].ToString();
                    float? diastaticPower = !diastaticPowerValue.IsNullOrEmpty() ? (float?) float.Parse(diastaticPowerValue) : null;
                    FermentableType type = (FermentableType) EnumConverter.Parse(typeof(FermentableType), reader.GetString(7));
                    int gravityPoint = reader.GetInt32(8);

                    FermentableCharacteristics characteristics = new FermentableCharacteristics(yield, color, diastaticPower) {
                        YieldByWeight = yieldByWeight, Type = type, GravityPoint = gravityPoint };
                    yield return new Fermentable(name, characteristics, notes, origin);
                }
            }
        }
    }
}
