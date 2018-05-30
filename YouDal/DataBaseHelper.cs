using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using Dapper;

namespace YouDal
{
    public class DataBaseHelper
    {
        //public const string _dataBaseConnectStr = "Database=youhua;Data Source=47.254.42.113;User Id=yan;Password=Yzq@0731qq;pooling=false;CharSet=utf8;port=3306";
        public const string _dataBaseConnectStr = "Database=youhua;Data Source=127.0.0.1;User Id=youhua;Password=y66262996;pooling=false;CharSet=utf8;port=3306";

        public static IDbConnection GetConnection()
        {
            try
            {
                var connection = new MySqlConnection(_dataBaseConnectStr);
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                return connection;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
       
    }
}
