using System;
using System.Collections.Generic;
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
        private static string connectionString = ConfigurationManager.AppSettings["connection_string"].ToString();

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
                    Program.log.write(e.Message);
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
                        Program.log.write(e.Message);
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
                    Program.log.write(e.Message);
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
