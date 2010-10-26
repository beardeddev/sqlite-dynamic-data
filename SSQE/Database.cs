/*
 * Copyright (c) 2010, Vitaliy Litvinenko
 * All rights reserved.
 * Find me on http://beardeddev.pp.ua
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.Configuration;

namespace SSQE
{
    /// <summary>
    /// Base class for querying the database
    /// </summary>
    public class Database
    {
        private string _ConnectionString;


        /// <summary>
        /// Get instance of SQLiteCommand with query params
        /// </summary>
        /// <param name="conn">Connection to database</param>
        /// <param name="query">SQL Query string</param>
        /// <param name="parameters">Query parameters</param>
        /// <returns>Instance of SQLiteCommand to be executed</returns>
        private SQLiteCommand GetCommand(SQLiteConnection conn, string query, params object[] parameters)
        {
            SQLiteCommand comm = new SQLiteCommand(query, conn);
            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    string parameterName = string.Format("@{0}", i);
                    SQLiteParameter p = new SQLiteParameter(parameterName, parameters[i]);
                    comm.Parameters.Add(p);
                }
            }
            return comm;
        }

        /// <summary>
        /// Creates new instance of database for querying
        /// </summary>
        /// <param name="connStringName">Name of connection string</param>
        /// <example>
        /// <code>
        /// public class DemoDb : SSQE.Database
        /// {
        ///     public DemoDb()
        ///         : base("demo")
        ///     {
        ///     }
        /// }
        /// </code>
        /// <remarks>
        /// &lt;connectionStrings&gt;
        /// &lt;add name="test" connectionString="Data Source=:memory:;New=True;Pooling=True;Max Pool Size=1;Journal Mode=Off;Version=3;" /&gt;
        /// &lt;/connectionStrings&gt;
        /// </remarks>
        /// </example>
        public Database(string connStringName)
        {
            this._ConnectionString = ConfigurationManager.ConnectionStrings[connStringName].ConnectionString;
        }

        /// <summary>
        /// Open database by connection string name and return new instance of database for querying
        /// </summary>
        /// <param name="connStringName">Name of connection string</param>
        /// <returns>Instance of database for querying</returns>
        /// <example>
        /// <code>
        /// var db = Database.Open("test");
        /// </code>
        ///<remarks>
        /// &lt;connectionStrings&gt;
        /// &lt;add name="test" connectionString="Data Source=:memory:;New=True;Pooling=True;Max Pool Size=1;Journal Mode=Off;Version=3;" /&gt;
        /// &lt;/connectionStrings&gt;
        ///</remarks>
        /// </example>
        public static Database Open(string connStringName)
        {
            return new Database(connStringName);
        }

        /// <summary>
        /// Run query against database
        /// </summary>
        /// <param name="query">Query string</param>
        /// <param name="parameters">Parameters for query command</param>
        /// <returns>Dynamic result set object of query</returns>
        /// <example>
        /// <code>
        /// var db = Database.Open("demo");
        /// 
        /// var ruby = db.Query("SELECT * FROM dictionary WHERE title = @0", "Ruby");
        /// foreach (var r in ruby)
        /// {
        ///     Console.WriteLine("{0}\t{1}\t...", r.id, r.title);
        /// }
        /// </code>
        /// </example>
        public dynamic Query(string query, params object[] parameters)
        {
            dynamic res = new List<QueryResult>();
            using(SQLiteConnection conn = new SQLiteConnection(this._ConnectionString))
            {
                conn.Open();
                SQLiteCommand comm = GetCommand(conn, query, parameters);
                SQLiteDataReader dr = comm.ExecuteReader();
                if (dr.HasRows)
                {
                    ArrayList columnNames = new ArrayList();
                    foreach (DataRow row in dr.GetSchemaTable().Rows)
                    {
                        columnNames.Add(row["ColumnName"]);
                    }
                    while (dr.Read())
                    {
                        Dictionary<string, object> drRow = new Dictionary<string, object>();
                        foreach (string key in columnNames)
                        {
                            drRow.Add(key, dr[key]);                  
                        }
                        dynamic row = new QueryResult(drRow);
                        res.Add(row);
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Run query against database
        /// </summary>
        /// <param name="query">Query string</param>
        /// <returns>Dynamic result set object of query</returns>
        /// <example>
        /// <code>
        /// var db = Database.Open("demo");
        /// var res = db.Query("SELECT * FROM dictionary");
        /// 
        /// foreach (var r in res)
        /// {
        ///     Console.WriteLine("{0}\t{1}\t...", r.id, r.title);
        /// }
        /// </code>
        /// </example>
        public dynamic Query(string query)
        {
            return Query(query, null);
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the result set returned by the query. 
        /// Additional columns or rows are ignored.
        /// </summary>
        /// <param name="query">Query string</param>
        /// <param name="parameters">Parameters for query command</param>
        /// <returns>
        /// The first column of the first row in the result set, or a null reference (Nothing in Visual Basic) if the result set is empty.
        /// Returns a maximum of 2033 characters.
        /// </returns>
        /// <example>
        /// <code>
        /// var db = Database.Open("demo");
        /// var c = db.Query("SELECT COUNT(*) FROM dictionary WHERE title = @0", "ruby");
        /// 
        /// Console.WriteLine("Count of record is {0}", c);
        /// </code>
        /// </example>
        public object ScalarQuery(string query, params object[] parameters)
        {
            object res = null;
            using (SQLiteConnection conn = new SQLiteConnection(this._ConnectionString))
            {
                conn.Open();
                SQLiteCommand comm = GetCommand(conn, query, parameters);
                res = comm.ExecuteScalar();
            }
            return res;
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the result set returned by the query. 
        /// Additional columns or rows are ignored.
        /// </summary>
        /// <param name="query">Query string</param>
        /// <returns>
        /// The first column of the first row in the result set, or a null reference (Nothing in Visual Basic) if the result set is empty.
        /// Returns a maximum of 2033 characters.
        /// </returns>
        /// <example>
        /// <code>
        /// var db = Database.Open("demo");
        /// var c = db.Query("SELECT COUNT(*) FROM dictionary");
        /// 
        /// Console.WriteLine("Count of record is {0}", c);
        /// </code>
        /// </example>
        public object ScalarQuery(string query)
        {
            return ScalarQuery(query, null);
        }

        /// <summary>
        /// Executes query against database
        /// </summary>
        /// <param name="query">Query string</param>
        /// <param name="parameters">Query command argumets</param>
        /// <returns>Count of affected rows</returns>
        /// <example>
        /// <code>
        /// var db = Database.Open("demo");
        /// 
        /// int affectedRows = db.Execute("INSERT INTO dictionary(title, description) VALUES(@0, @1)", 
        ///                    "Java", 
        ///                    @"Java is a programming language originally developed by 
        ///                      James Gosling at Sun Microsystems (which is now a subsidiary 
        ///                      of Oracle Corporation) and released in 1995 as a core component 
        ///                      of Sun Microsystems' Java platform. The language derives much of 
        ///                      its syntax from C and C++ but has a simpler object model and fewer 
        ///                      low-level facilities.");
        /// </code>
        /// </example>
        public int Execute(string query, params object[] parameters)
        {
            int res;
            using (SQLiteConnection conn = new SQLiteConnection(this._ConnectionString))
            {
                conn.Open();
                SQLiteCommand comm = GetCommand(conn, query, parameters);                
                res = comm.ExecuteNonQuery();                
            }
            return res;
        }

        /// <summary>
        /// Executes query against database
        /// </summary>
        /// <param name="query">Query string</param>
        /// <returns>Count of affected rows</returns>
        /// <example>
        /// <code>
        /// var db = Database.Open("demo");
        /// 
        /// db.Execute(@"CREATE TABLE dictionary(
        ///                    id INTEGER PRIMARY KEY AUTOINCREMENT,
        ///                    title VARCHAR(256),
        ///                    description TEXT
        ///                 )
        ///                 ;");
        /// 
        /// Console.WriteLine("Table created");
        /// </code>
        /// </example>
        public int Execute(string query)
        {
            return Execute(query, null);
        }
    }
}
