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
    public class Database
    {
        private string _ConnectionString;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connStringName"></param>
        public Database(string connStringName)
        {
            this._ConnectionString = ConfigurationManager.ConnectionStrings[connStringName].ConnectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public dynamic Query(string query, object parameters)
        {
            dynamic res = new List<QueryResult>();
            using(SQLiteConnection conn = new SQLiteConnection(this._ConnectionString))
            {
                conn.Open();
                SQLiteCommand comm = new SQLiteCommand(query, conn);
                if (parameters != null)
                {                    
                    Dictionary<string, object> paRams = this.CreateParameters(parameters);
                    foreach (string key in paRams.Keys)
                    {
                        SQLiteParameter p = new SQLiteParameter(key, paRams[key]);
                        comm.Parameters.Add(p);
                    }
                }
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
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public dynamic Query(string query)
        {
            return Query(query, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int Execute(string query, object parameters)
        {
            int res;
            using (SQLiteConnection conn = new SQLiteConnection(this._ConnectionString))
            {
                conn.Open();
                SQLiteCommand comm = new SQLiteCommand(query, conn);
                if (parameters != null)
                {
                    Dictionary<string, object> paRams = this.CreateParameters(parameters);
                    foreach (string key in paRams.Keys)
                    {
                        SQLiteParameter p = new SQLiteParameter(key, paRams[key]);
                        comm.Parameters.Add(p);
                    }
                }
                res = comm.ExecuteNonQuery();                
            }
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int Execute(string query)
        {
            return Execute(query, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected Dictionary<string, object> CreateParameters(object parameters)
        {
            return parameters.GetType()
                .GetProperties()
                .ToDictionary(x => x.Name, x => x.GetValue(parameters, null));
        }
    }
}
