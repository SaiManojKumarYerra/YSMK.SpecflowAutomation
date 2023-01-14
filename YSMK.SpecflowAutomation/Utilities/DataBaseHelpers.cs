using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Text;

namespace YSMK.SpecflowAutomation.Utilities
{
    public static class DataBaseHelpers
    {
        /// <summary>
        /// Open SQL connection
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="connectionString">DB Connection string</param>
        /// <returns>SqlConnection</returns>
        public static SqlConnection DBConnect(this SqlConnection sqlConnection, string connectionString)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                return sqlConnection;
            }
            catch (Exception e)
            {
                //throw new Exception("Exception in DBConnect Method : " + e);
               LogHelpers.Write("ERROR :: " + e.Message);
            }

            return null;
        }

        /// <summary>
        /// Execute SQL Select command
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="queryString">Query to be executed</param>
        /// <returns>Table Results are Dataset</returns>
        public static DataTable ExecuteQuery(this SqlConnection sqlConnection, string queryString)
        {

            DataSet dataset;
            try
            {
                //Checking the state of the connection
                if (sqlConnection == null || ((sqlConnection != null && (sqlConnection.State == ConnectionState.Closed ||
                    sqlConnection.State == ConnectionState.Broken))))
                    sqlConnection.Open();

                SqlDataAdapter dataAdaptor = new SqlDataAdapter
                {
                    SelectCommand = new SqlCommand(queryString, sqlConnection)
                };
                dataAdaptor.SelectCommand.CommandType = CommandType.Text;

                dataset = new DataSet();
                dataAdaptor.Fill(dataset, "table");
                sqlConnection.Close();
                return dataset.Tables["table"];
            }
            catch (Exception e)
            {
                dataset = null;
                sqlConnection.Close();
                //LogHelpers.Write("ERROR :: " + e.Message);
                if (e.Message.Contains("The SELECT permission was denied on the object")) throw new Exception(e.Message);
                return null;
            }
            finally
            {
                sqlConnection.Close();
            }


        }

        public static DataTable ExecuteProcWithParamsDT(this SqlConnection Conn, string procname, Hashtable parameters)
        {
            DataSet dataSet;
            try
            {
                SqlDataAdapter dataAdaptor = new SqlDataAdapter
                {
                    SelectCommand = new SqlCommand(procname, Conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    }
                };
                if (parameters != null)
                    foreach (DictionaryEntry de in parameters)
                    {
                        SqlParameter sp = new SqlParameter(de.Key.ToString(), de.Value.ToString());
                        dataAdaptor.SelectCommand.Parameters.Add(sp);
                    }

                dataSet = new DataSet();
                dataAdaptor.Fill(dataSet, "table");
                Conn.Close();
                return dataSet.Tables["table"];
            }
            catch (Exception e)
            {
                dataSet = null;
                Conn.Close();
                LogHelpers.Write("ERROR :: " + e.Message, e.InnerException);
                return null;
            }
            finally
            {
                Conn.Close();
            }
        }

        /// <summary>
        /// Executes a Transact-SQL statement against the connection and returns the number of rows affected.
        /// (Example: Update, Insert, Delete)
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="queryString">Query to be executed</param>
        /// <returns>returns the number of rows affected</returns>
        public static int ExecuteNonQuery(this SqlConnection sqlConnection, string queryString)
        {
            try
            {
                //Checking the state of the connection
                if (sqlConnection == null || ((sqlConnection != null && (sqlConnection.State == ConnectionState.Closed ||
                    sqlConnection.State == ConnectionState.Broken))))
                    sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand(queryString, sqlConnection);
                int totalRows = sqlCommand.ExecuteNonQuery();
                return totalRows;
            }
            catch (Exception e)
            {
                sqlConnection.Close();
                //LogHelpers.Write("ERROR :: " + e.Message);
                if (!e.Message.Contains("refused because the job is already running"))
                    throw new Exception(e.Message);
                else
                    return -2;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public static string GetConnectionString(string serverName, string dbName)
        {
            // To avoid storing the connection string in your code,
            // you can retrieve it from a configuration file.
            return "Data Source=" + serverName + ";Initial Catalog=" + dbName + ";"
                + "Integrated Security=true;";
        }
    }
}
