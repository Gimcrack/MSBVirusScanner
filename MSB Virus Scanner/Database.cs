using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace MSB_Virus_Scanner
{
    class Database
    {
        private static NameValueCollection config = ConfigurationManager.AppSettings;

        private static string connectionString = String.Format(
            "Server={0};Database={1};User Id={2}; password={3}",
            config["database_server"].ToString(),
            config["database_name"].ToString(),
            config["database_username"].ToString(),
            config["database_password"].ToString()
        );

        public static void Statement( string command, Dictionary<string, string> parameters = null )
        {
            Statement( new SqlCommand(command), parameters );
        }
        
        public static void Statement( SqlCommand command, Dictionary<string,string> parameters = null )
        {
            using (SqlConnection dbc = new SqlConnection( connectionString ))
            {
                try
                {
                    if (dbc != null && dbc.State != ConnectionState.Open)
                    {
                        dbc.Close();
                        dbc.Open();
                    }

                    command.Connection = dbc;

                    if( parameters != null )
                    {
                        foreach(KeyValuePair<string,string> pair in parameters)
                        {
                            command.Parameters.AddWithValue(pair.Key, pair.Value);
                        }
                    }

                    command.ExecuteNonQuery();

                }

                catch (SqlException e)
                {
                    Program.log.Write(e.Message);
                }

                finally
                {
                    dbc.Close();
                }
            }
        }


        public static DataTable Query( string query, Dictionary<string, string> parameters = null )
        {
            return Query( new SqlCommand(query) );
        }

        public static DataTable Query( SqlCommand query, Dictionary<string, string> parameters = null )
        {
            SqlDataReader reader;
            DataTable dt = new DataTable();


            using (SqlConnection dbc = new SqlConnection(connectionString))
            {
                try
                {
                    if (dbc != null && dbc.State != ConnectionState.Open)
                    {
                        dbc.Close();
                        dbc.Open();
                    }

                    query.Connection = dbc;

                    reader = query.ExecuteReader();

                    try
                    {
                        dt.Load(reader);
                        return dt;
                    }

                    catch (SqlException e)
                    {
                        Program.log.Write(e.Message);
                        reader.Close();
                        return dt; // empty data table
                    }

                    finally
                    {
                        reader.Close();
                    }
                }

                catch (SqlException e)
                {
                    Program.log.Write(e.Message);
                    return dt; // empty data table
                }

                finally
                {
                    dbc.Close();
                }
            }
        }
    }
}
