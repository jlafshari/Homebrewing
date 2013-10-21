﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace BeerRecipeCore.Data
{
    public static class HopsUtility
    {
        public static IEnumerable<Hops> GetAvailableHopsVarieties(SQLiteConnection connection)
        {
            SQLiteCommand selectHopsCommand = connection.CreateCommand();
            //selectHopsCommand.CommandText = "SELECT (name, alpha, use, notes, beta, hsi, origin) FROM Hops";
            selectHopsCommand.CommandText = "SELECT * FROM Hops";
            using (SQLiteDataReader reader = selectHopsCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    string name = reader[1].ToString();
                    float alphaAcid = (float) Convert.ToDouble(reader[2]);
                    string use = reader[3].ToString();
                    string notes = reader[4].ToString();
                    float betaAcid = (float) Convert.ToDouble(reader[5]);
                    float hsi = (float) Convert.ToDouble(reader[6]);
                    string origin = reader[7].ToString();

                    HopsCharacteristics characteristics = new HopsCharacteristics(alphaAcid, betaAcid) { Hsi = hsi };
                    yield return new Hops(name, characteristics, use, notes, origin);
                }
            }
        }
    }
}
