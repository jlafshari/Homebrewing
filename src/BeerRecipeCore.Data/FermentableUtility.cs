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
                    string name = reader[0].ToString();
                    string yieldValue = reader[1].ToString();
                    float? yield = !string.IsNullOrEmpty(yieldValue) ? (float?) Convert.ToDouble(yieldValue) : null;
                    string yieldByWeightValue = reader[2].ToString();
                    float? yieldByWeight = !string.IsNullOrEmpty(yieldByWeightValue) ? (float?) Convert.ToDouble(yieldByWeightValue) : null;
                    float color = (float) Convert.ToDouble(reader[3]);
                    string origin = reader[4].ToString();
                    string notes = reader[5].ToString();
                    string diastaticPowerValue = reader[6].ToString();
                    float? diastaticPower = !string.IsNullOrEmpty(diastaticPowerValue) ? (float?) Convert.ToDouble(diastaticPowerValue) : null;
                    FermentableType type = (FermentableType) EnumConverter.Parse(typeof(FermentableType), reader[7].ToString());
                    int gravityPoint = Convert.ToInt32(reader[8].ToString());

                    FermentableCharacteristics characteristics = new FermentableCharacteristics(yield, color, diastaticPower) {
                        YieldByWeight = yieldByWeight, Type = type, GravityPoint = gravityPoint };
                    yield return new Fermentable(name, characteristics, notes, origin);
                }
            }
        }
    }
}
