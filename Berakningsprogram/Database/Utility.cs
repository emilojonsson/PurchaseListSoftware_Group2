﻿using Npgsql;
using System.Data;
using System.Diagnostics;
using System.Globalization;

namespace TestDatabasePostgres
{
    public static class Utility
    {

        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(@"Server=localhost;Port=5432;User Id=postgres;Password=XXXXX;Database=TestDB");
        }
        public static void TestConnection()
        {
            using (NpgsqlConnection con = GetConnection())
            {
                con.Open();
                if (con.State == ConnectionState.Open)
                {
                    Console.WriteLine("Connected");
                }
            }
        }
        public static void InsertCreatureToDatabaseViaProcedure(object theObject)
        {
            using (NpgsqlConnection con = GetConnection())
            {
                string query = $@"call insert_data({theObject.ToString()})";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                con.Open();
                int n = cmd.ExecuteNonQuery();
                if (n == 1 || n == -1)
                {
                    Console.WriteLine($"{theObject.GetType()} inserted to database");
                }
            }
        }
        public static object[] ReadRecord(string typeOfObject, string theCreature)
        {
            using (NpgsqlConnection con = GetConnection())
            {
                var query = $"Select * from {typeOfObject} WHERE Name = '{theCreature}'";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                con.Open();
                var dataReader = cmd.ExecuteReader();
                dataReader.Read();
                int columns = dataReader.FieldCount;
                object[] values = new object[columns];
                dataReader.GetValues(values);
                return values;
            }
        }
        public static void UpdateRecord(string tablename, string id, string data = "NULL", string data1 = "NULL", string data2 = "NULL")
        {
            using (NpgsqlConnection con = GetConnection())
            {
                var query = $@"call update_record({tablename},{id},{data},{data1},{data2})";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                con.Open();
                int n = cmd.ExecuteNonQuery();
                if (n == 1 || n == -1)
                {
                    Console.WriteLine($"updated ID:{id} in the database");
                }
            }

        }
        public static void DeleteRecord(string tablename, int id)
        {
            using (NpgsqlConnection con = GetConnection())
            {
                var query = $@"call delete_record({tablename},{id})";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                con.Open();
                int n = cmd.ExecuteNonQuery();
                if (n == 1 || n == -1)
                {
                    Console.WriteLine($"deleted ID:{id} in the database");
                }
            }
        }
        }
    }

