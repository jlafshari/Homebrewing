using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace BeerRecipeCore.Data
{
    public static class HopsUtility
    {
        public static IEnumerable<Hops> GetAvailableHopsVarieties(SQLiteConnection connection)
        {
            SQLiteCommand selectHopsCommand = connection.CreateCommand();
            selectHopsCommand.CommandText = "SELECT name, alpha, use, notes, beta, hsi, origin FROM Hops";
            using (SQLiteDataReader reader = selectHopsCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    float alphaAcid = reader.GetFloat(1);
                    string use = reader.GetString(2);
                    string notes = reader.GetString(3);
                    float betaAcid = reader.GetFloat(4);
                    float hsi = reader.GetFloat(5);
                    string origin = reader.GetString(6);

                    HopsCharacteristics characteristics = new HopsCharacteristics(alphaAcid, betaAcid) { Hsi = hsi };
                    yield return new Hops(name, characteristics, use, notes, origin);
                }
            }
        }
    }
}
