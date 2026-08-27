using System;
using Microsoft.Data.Sqlite;

var connectionString = "Data Source=../hymap_local.db";
using var connection = new SqliteConnection(connectionString);
connection.Open();

try {
    using var cmd1 = connection.CreateCommand();
    cmd1.CommandText = "ALTER TABLE Rutes RENAME TO Rutes;";
    cmd1.ExecuteNonQuery();
    Console.WriteLine("Renamed Rutes to Rutes.");
} catch(Exception ex) { Console.WriteLine(ex.Message); }

try {
    using var cmd2 = connection.CreateCommand();
    cmd2.CommandText = "ALTER TABLE Wilayahs RENAME TO Wilayahs;";
    cmd2.ExecuteNonQuery();
    Console.WriteLine("Renamed Wilayahs to Wilayahs.");
} catch(Exception ex) { Console.WriteLine(ex.Message); }

try {
    using var cmd3 = connection.CreateCommand();
    cmd3.CommandText = "ALTER TABLE Wilayahs RENAME COLUMN RuteId TO RuteId;";
    cmd3.ExecuteNonQuery();
    Console.WriteLine("Renamed RuteId to RuteId in Wilayahs.");
} catch(Exception ex) { Console.WriteLine(ex.Message); }
