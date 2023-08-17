using System;
using System.Collections.Generic;
using System.Data;
//using System.Data.SqlServerCe;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace indigo_ap
{
    public class conexion
    {
        public LogWriter log;

        public SqlConnection conn;
        public SqlCommand connComando;
        public SqlDataAdapter adaptadorSqlCe;
        public SqlDataReader readerSqlCe;

        public DataTable dt;

       




        //public conexion() {

        //    conn = new SqlCeConnection();
        //    conn.ConnectionString = "Data Source=indigo_app.sdf;Persist Security Info=False;";

        //    connComando = new SqlCeCommand();
        //    adaptadorSqlCe = new SqlCeDataAdapter();

            

        //}

        public conexion() {

            conn = new SqlConnection();
            conn.ConnectionString = (@"Server=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|indigo_app.mdf; Integrated Security=True;");

            connComando = new SqlCommand();
            adaptadorSqlCe = new SqlDataAdapter();
        }

        

        public void conectar() {
            try {
                

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Open();

                }
                else if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

            } catch (SqlException ex) {
               Console.WriteLine("Error [conectar]: " + ex.Message);
               log.LogWrite("Error [conectar]: " + ex.Message);
            }
            


        }

        public void desconectar() {
            try {

                if(conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

            } catch (SqlException ex) {
                Console.WriteLine("Error [desconectar]: " + ex.Message);
                log.LogWrite("Error [desconectar]: " + ex.Message);
            }
        }


        public void crear_tablas() {
            connComando.Connection = conn;
           
            connComando.CommandText = "IF OBJECT_ID(N'dbo.t001_parametros', N'U') IS NULL "
                + "CREATE TABLE t001_parametros (f001_idParametros INT IDENTITY(1,1) PRIMARY KEY, f001_parametro nvarchar(50), f001_valor nvarchar(50));";
            connComando.ExecuteNonQuery();

            connComando.CommandText = "IF OBJECT_ID(N'dbo.t002_campos', N'U') IS NULL "
                + "CREATE TABLE t002_campos (f002_idCampos INT IDENTITY(1,1) PRIMARY KEY,f002_nombre_tabla nvarchar(100), f002_nombre nvarchar(100), f002_valor nvarchar(100));";
            connComando.ExecuteNonQuery();

            connComando.CommandText = "IF OBJECT_ID(N'dbo.t004_formatos', N'U') IS NULL "
                + "CREATE TABLE t004_formatos (f004_idFormato INT IDENTITY(1,1) PRIMARY KEY,f004_nombre_formato nvarchar(100), f004_formato ntext);";
            connComando.ExecuteNonQuery();
        }

        public void insertar_datos(string tabla,string valores) {

            try {

                connComando.Connection = conn;
                connComando.CommandText = @"INSERT INTO " + tabla + " VALUES (" + valores + " );";
                connComando.ExecuteNonQuery();

            } catch (SqlException ex) {
                Console.WriteLine("Error [insertar_datos]: " + ex.Message);
                log.LogWrite("Error [insertar_datos]: " + ex.Message);
            }
            

        }

        public void update_datos(string tabla, string valores)
        {

            try
            {

                connComando.Connection = conn;
                connComando.CommandText = @"UPDATE " + tabla + " SET " + valores + ";";
                connComando.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error [insertar_datos]: " + ex.Message);
                log.LogWrite("Error [insertar_datos]: " + ex.Message);
            }


        }


        
        /// <summary>
        ///  Elimina informacion de una tabla
        /// </summary>
        /// <param name="tabla">especifica el nombre de la tabla a borrar.</param>
        /// <param name="valores"> especifica el dato a borrar.</param>
        public void delete(string tabla, string valores)
        {
            try
            {
                connComando.Connection = conn;

                if (valores == null) {
                    connComando.CommandText = @"DELETE FROM " + tabla +";";
                    connComando.ExecuteNonQuery();
                } else {
                    connComando.CommandText = @"DELETE FROM " + tabla + " WHERE " + valores + ";";
                    connComando.ExecuteNonQuery();
                }
                
                
                
            }
            catch (SqlException ex)
            {

                Console.WriteLine("Error [delete]: " + ex.Message);
                log.LogWrite("Error [delete]: " + ex.Message);
            }

        }

        public void drop(string tabla)
        {
            try
            {
                connComando.Connection = conn;
                connComando.CommandText = "IF OBJECT_ID(N'dbo." + tabla + "', N'U') IS NOT NULL "+"DROP TABLE " + tabla + ";";
                connComando.ExecuteNonQuery();
                



            }
            catch (SqlException ex)
            {

                Console.WriteLine("Error [truncate]: " + ex.Message);
                log.LogWrite("Error [truncate]: " + ex.Message);
            }

        }

        public void custom_query_set(string query)
        {

            try
            {
                
                connComando.Connection = conn;
                connComando.CommandText = query;
                connComando.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                
                Console.WriteLine("Error [custom_query]: " + ex.Message);
                log.LogWrite("Error [custom_query]: " + ex.Message);
            }


        }

        public DataTable select_custom(string query)
        {
            try
            {

                dt = new DataTable();
                connComando.Connection = conn;
                connComando.CommandText = query;
                readerSqlCe = connComando.ExecuteReader();
                dt.Load(readerSqlCe);



            }
            catch (SqlException ex)
            {
                
                Console.WriteLine("Error [select_custom]: " + ex.Message);
                log.LogWrite("Error [select_custom]: " + ex.Message);
            }

            return dt;
        }

        


    }


}
