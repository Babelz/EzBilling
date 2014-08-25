using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

namespace EzBilling.DatabaseObjects
{
    public sealed class DatabaseHelper
    {
        #region Vars
        private readonly SQLiteConnection connection;

        private readonly SQLiteCommand command;
        #endregion

        /// <summary>
        /// Constructs new database
        /// </summary>
        /// <param name="path">Database file path</param>
        public DatabaseHelper(string path)
        {
            connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", path));
            command = new SQLiteCommand(connection);
        }
        /// <summary>
        /// Opens database connection
        /// </summary>
        public void Open()
        {
            connection.Open();
        }

        /// <summary>
        /// Closes database connection
        /// </summary>
        public void Close()
        {
            connection.Close();
        }

        public int ExecuteFile(string file)
        {
            string contents = File.ReadAllText(file);
            command.CommandText = contents;
            connection.Open();
            int rows = command.ExecuteNonQuery();
            connection.Close();
            return rows;
        }

        #region Query

        #region Misc

        /// <summary>
        /// Gets the last inserted row id
        /// </summary>
        /// <returns>Last inserted row id</returns>
        public long LastInsertRowId()
        {
            return GetValue<long>("select last_insert_rowid();");
        }

        #endregion

        #region GetValue
        /// <summary>
        /// Retrieve one variable from the database.
        /// Executes a SQL query and returns the value from the SQL result.
        /// </summary>
        /// <param name="sql">Query</param>
        /// <returns></returns>
        public object GetValue(string sql)
        {
            command.CommandText = sql;
            return command.ExecuteScalar();
        }

        /// <summary>
        /// Retrieve one variable from the database.
        /// Executes a SQL query and returns the value from the SQL result.
        /// </summary>
        /// <typeparam name="TType">Output type</typeparam>
        /// <param name="sql">Query</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public TType GetValue<TType>(string sql, Dictionary<string, object> parameters = null)
        {
            
            return GetValue<TType>(sql, GetParameters(parameters));
        }

        /// <summary>
        /// Retrieve one variable from the database.
        /// Executes a SQL query and returns the value from the SQL result.
        /// </summary>
        /// <typeparam name="T">Output type</typeparam>
        /// <param name="sql">Query</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T GetValue<T>(string sql, IEnumerable<SQLiteParameter> parameters)
        {
            command.CommandText = sql;
            if (parameters != null)
            {
                foreach (var sqlParameter in parameters)
                {
                    command.Parameters.Add(sqlParameter);
                }
            }
            return (T) Convert.ChangeType(command.ExecuteScalar(), typeof (T));
        }
        #endregion

        #region Update

        /// <summary>
        /// Updates table
        /// </summary>
        /// <param name="tableName">What table to update</param>
        /// <param name="parameters">Fields to update with values</param>
        /// <param name="fieldCondition">Where clauses field</param>
        /// <param name="valueCondition">Where clauses field value</param>
        /// <returns>How many rows affected</returns>
        public int Update(string tableName, Dictionary<string, object> parameters, string fieldCondition,
            string valueCondition)
        {
            return Update(tableName, parameters,
                new Dictionary<string, string>
                {
                    {fieldCondition, valueCondition}
                });
        }

        /// <summary>
        /// Updates table
        /// </summary>
        /// <param name="tableName">What table to update</param>
        /// <param name="parameters">Fields to update with values</param>
        /// <param name="condition">Possible conditions</param>
        /// <returns>How many rows affected</returns>
        public int Update(string tableName, Dictionary<string, object> parameters, Dictionary<string, string> condition)
        {
            if (condition.Count == 0) throw new ArgumentException("condition is empty");
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append(string.Format("UPDATE {0} SET ", tableName));
            foreach (var parameter in parameters)
            {
                queryBuilder.Append(parameter.Key);
                queryBuilder.Append("=");
                queryBuilder.Append("@v" + parameter.Key);
                queryBuilder.Append(",");
            }
            queryBuilder.Remove(queryBuilder.Length - 1, 1);
            queryBuilder.Append(" WHERE ");
            queryBuilder.Append(CreateConditionString("@x", condition));
            queryBuilder.Append(";");

            command.CommandText = queryBuilder.ToString();
            foreach (var parameter in parameters)
            {
                command.Parameters.AddWithValue("@v" + parameter.Key, parameter.Value);
            }

            foreach (var cond in condition)
            {
                command.Parameters.AddWithValue("@x" + cond.Key, cond.Value);
            }

            return command.ExecuteNonQuery();
        }

        #endregion

        #region Delete

        public int Delete(string table, Dictionary<string, string> condition)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE FROM ");
            sb.Append(table);
            sb.Append(" WHERE ");
            sb.Append(CreateConditionString("@x", condition));

            command.CommandText = sb.ToString();
            return command.ExecuteNonQuery();

        }

        #endregion

        #region Insert

        /// <summary>
        /// Insert a row into a table.
        /// </summary>
        /// <param name="table">Table name</param>
        /// <param name="parameters">Data to insert</param>
        public void Insert(string table, Dictionary<string, object> parameters)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO " + table);
            sb.Append("(`" + String.Join("`,`", parameters.Keys) + "`)");
            sb.Append(" VALUES(");
            sb.Append("@v" + String.Join(",@v", parameters.Keys));
            sb.Append(");");
            
           
            command.CommandText = sb.ToString();
            foreach (var kv in parameters)
            {
                command.Parameters.AddWithValue("@v" + kv.Key, kv.Value);
            }

            command.ExecuteNonQuery();
        }

        #endregion

        #region Select
        /// <summary>
        /// Selects a database using the current database connection.
        /// </summary>
        /// <param name="sql">Query</param>
        /// <param name="parameters">Data</param>
        /// <returns></returns>
        public DataTable Select(string sql, Dictionary<string, object> parameters = null)
        {
            return Select(sql, GetParameters(parameters));
        }

        /// <summary>
        /// Selects a database using the current database connection.
        /// </summary>
        /// <param name="sql">Query</param>
        /// <param name="parameters">Data</param>
        /// <returns></returns>
        public DataTable Select(string sql, IEnumerable<SQLiteParameter> parameters)
        {
            command.CommandText = sql;
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        #endregion
        #endregion

        #region Util
        private List<SQLiteParameter> GetParameters(Dictionary<string, object> parameters)
        {
            if (parameters != null)
            {
                return parameters.Select(kv => new SQLiteParameter(kv.Key, kv.Value)).ToList();
            }

            return new List<SQLiteParameter>();
        }

        /// <summary>
        /// Creates a string which can be used in where clauses
        /// </summary>
        /// <param name="prefix">Prefix for variable (eg. @v)</param>
        /// <param name="parameters">Parameters to store in string</param>
        /// <returns></returns>
        private string CreateConditionString(string prefix, Dictionary<string, string> parameters)
        {
            if (parameters.Count == 0) throw new ArgumentException("parameters is empty", "parameters");
            StringBuilder sb = new StringBuilder();
            foreach (var v in parameters)
            {
                sb.Append(v.Key);
                sb.Append(" = ");
                sb.Append(string.Format("{0}{1}", prefix, v.Key));
                sb.Append(" AND ");
            }
            sb.Remove(sb.Length - 5, 5);
            return sb.ToString();
        }

        #endregion
    }
}
