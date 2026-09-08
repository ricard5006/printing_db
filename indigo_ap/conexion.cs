using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace indigo_ap
{
    public class conexion : IDisposable
    {
        public LogWriter log;

        public SqlConnection conn;
        public SqlCommand connComando;
        public SqlTransaction transaccion;

        public conexion()
        {
            log = new LogWriter();
            conn = new SqlConnection(@"Server=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|indigo_app.mdf; Integrated Security=True;");
            connComando = new SqlCommand();
        }

        public void conectar()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error [conectar]: " + ex.Message);
                log.LogWrite("Error [conectar]: " + ex.Message);
            }
        }

        public void desconectar()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error [desconectar]: " + ex.Message);
                log.LogWrite("Error [desconectar]: " + ex.Message);
            }
        }

        public void iniciar_transaccion()
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            transaccion = conn.BeginTransaction();
        }

        public void confirmar()
        {
            if (transaccion != null)
            {
                transaccion.Commit();
                transaccion.Dispose();
                transaccion = null;
            }
        }

        public void revertir()
        {
            if (transaccion != null)
            {
                transaccion.Rollback();
                transaccion.Dispose();
                transaccion = null;
            }
        }

        public void crear_tablas()
        {
            connComando.Connection = conn;
            connComando.Transaction = transaccion;

            ejecutarNonQuery("IF OBJECT_ID(N'dbo.t001_parametros', N'U') IS NULL "
                + "CREATE TABLE t001_parametros (f001_idParametros INT IDENTITY(1,1) PRIMARY KEY, f001_parametro nvarchar(50), f001_valor nvarchar(50));");

            ejecutarNonQuery("IF OBJECT_ID(N'dbo.t002_campos', N'U') IS NULL "
                + "CREATE TABLE t002_campos (f002_idCampos INT IDENTITY(1,1) PRIMARY KEY,f002_nombre_tabla nvarchar(100), f002_nombre nvarchar(100), f002_valor nvarchar(100));");

            ejecutarNonQuery("IF OBJECT_ID(N'dbo.t004_formatos', N'U') IS NULL "
                + "CREATE TABLE t004_formatos (f004_idFormato INT IDENTITY(1,1) PRIMARY KEY,f004_nombre_formato nvarchar(100), f004_formato ntext);");
        }

        /// <summary>
        /// Inserta valores de forma parametrizada. Evita inyeccion SQL.
        /// </summary>
        public void insertar_datos(string tabla, string[] columnas, object[] valores)
        {
            if (columnas == null || valores == null || columnas.Length != valores.Length)
            {
                throw new ArgumentException("columnas y valores deben tener la misma cantidad de elementos.");
            }

            string nombreColumnas = "";
            string nombreParametros = "";
            List<SqlParameter> parametros = new List<SqlParameter>();

            for (int i = 0; i < columnas.Length; i++)
            {
                string col = SanitizarIdentificador(columnas[i]);
                nombreColumnas += (i > 0 ? "," : "") + "[" + col + "]";
                string param = "@p" + i;
                nombreParametros += (i > 0 ? "," : "") + param;
                parametros.Add(new SqlParameter(param, NormalizarValor(valores[i])));
            }

            ejecutarNonQuery("INSERT INTO " + tabla + " (" + nombreColumnas + ") VALUES (" + nombreParametros + ");", parametros.ToArray());
        }

        /// <summary>
        /// Actualiza valores de forma parametrizada.
        /// La condicion debe hacer referencia a los valores con @w0, @w1... (valoresCondicion).
        /// </summary>
        public void update_datos(string tabla, string[] columnas, object[] valores, string condicion, object[] valoresCondicion)
        {
            if (columnas == null || valores == null || columnas.Length != valores.Length)
            {
                throw new ArgumentException("columnas y valores deben tener la misma cantidad de elementos.");
            }

            StringBuilder textoSql = new StringBuilder();
            textoSql.Append("UPDATE " + tabla + " SET ");
            List<SqlParameter> parametros = new List<SqlParameter>();

            for (int i = 0; i < columnas.Length; i++)
            {
                string col = SanitizarIdentificador(columnas[i]);
                string param = "@p" + i;
                textoSql.Append((i > 0 ? "," : "") + "[" + col + "] = " + param);
                parametros.Add(new SqlParameter(param, NormalizarValor(valores[i])));
            }

            textoSql.Append(" WHERE " + condicion + ";");

            if (valoresCondicion != null)
            {
                for (int j = 0; j < valoresCondicion.Length; j++)
                {
                    parametros.Add(new SqlParameter("@w" + j, NormalizarValor(valoresCondicion[j])));
                }
            }

            ejecutarNonQuery(textoSql.ToString(), parametros.ToArray());
        }

        /// <summary>
        /// Elimina informacion de una tabla.
        /// </summary>
        public void delete(string tabla, string valores)
        {
            if (valores == null)
            {
                ejecutarNonQuery("DELETE FROM " + tabla + ";");
            }
            else
            {
                ejecutarNonQuery("DELETE FROM " + tabla + " WHERE " + valores + ";");
            }
        }

        public void drop(string tabla)
        {
            ejecutarNonQuery("IF OBJECT_ID(N'dbo." + tabla + "', N'U') IS NOT NULL DROP TABLE " + tabla + ";");
        }

        public void custom_query_set(string query)
        {
            custom_query_set(query, null);
        }

        public void custom_query_set(string query, SqlParameter[] parametros)
        {
            ejecutarNonQuery(query, parametros);
        }

        public DataTable select_custom(string query)
        {
            return select_custom(query, null);
        }

        public DataTable select_custom(string query, SqlParameter[] parametros)
        {
            DataTable dt = new DataTable();
            try
            {
                connComando.Connection = conn;
                connComando.Transaction = transaccion;
                connComando.CommandText = query;
                connComando.Parameters.Clear();
                if (parametros != null)
                {
                    connComando.Parameters.AddRange(parametros);
                }

                using (SqlDataReader reader = connComando.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error [select_custom]: " + ex.Message);
                log.LogWrite("Error [select_custom]: " + ex.Message.ToString());
            }

            return dt;
        }

        /// <summary>
        /// Limpia nombres de columnas para usarlos como identificadores de SQL Server.
        /// Evita inyeccion SQL a traves de encabezados de archivos.
        /// </summary>
        public static string NormalizarValor(object valor)
        {
            if (valor == null || valor is string || valor is DateTime || valor is byte[])
            {
                return valor == null ? "" : (string)Convert.ToString(valor);
            }

            if (valor is double || valor is float)
            {
                return Convert.ToDouble(valor, CultureInfo.InvariantCulture)
                             .ToString("0.#############################", CultureInfo.InvariantCulture);
            }

            if (valor is decimal)
            {
                return ((decimal)valor).ToString(CultureInfo.InvariantCulture);
            }

            return Convert.ToString(valor, CultureInfo.InvariantCulture);
        }

        public static string SanitizarIdentificador(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                return "columna";
            }

            StringBuilder sb = new StringBuilder();
            foreach (char c in nombre.Trim())
            {
                if (char.IsLetterOrDigit(c) || c == '_' || c == ' ' || c == '-')
                {
                    sb.Append(c);
                }
            }

            return sb.Length == 0 ? "columna" : sb.ToString();
        }

        /// <summary>
        /// Carpeta donde viven los datos de la aplicacion: base de datos, activacion y log.
        /// Esta carpeta es escribible aunque la app este instalada en Program Files.
        /// </summary>
        public static string ObtenerDirectorioDatos()
        {
            string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "IndigoApps");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }

        /// <summary>
        /// Copia el template de base de datos (indigo_app.mdf) instalado junto a la app
        /// hacia la carpeta de datos del usuario, la primera vez que la aplicacion se ejecuta.
        /// </summary>
        public static void PrepararBaseTemplate()
        {
            string dataDir = ObtenerDirectorioDatos();
            string destino = Path.Combine(dataDir, "indigo_app.mdf");
            if (!File.Exists(destino))
            {
                string origen = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "indigo_app.mdf");
                if (File.Exists(origen))
                {
                    File.Copy(origen, destino, false);

                    string ldfOrigen = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "indigo_app_log.ldf");
                    if (File.Exists(ldfOrigen))
                    {
                        File.Copy(ldfOrigen, Path.Combine(dataDir, "indigo_app_log.ldf"), false);
                    }
                }
            }
        }

        /// <summary>
        /// Ruta absoluta del archivo de activacion en la carpeta de datos.
        /// </summary>
        public static string ObtenerRutaActivacion()
        {
            return Path.Combine(ObtenerDirectorioDatos(), "Activacion.txt");
        }

        /// <summary>
        /// Verifica que la instancia (LocalDB)\MSSQLLocalDB este disponible en el equipo.
        /// </summary>
        public static bool VerificarLocalDB()
        {
            try
            {
                string sqlLocalDb = BuscarSqlLocalDb();
                if (sqlLocalDb == null)
                {
                    return false;
                }

                ProcessStartInfo psi = new ProcessStartInfo(sqlLocalDb, "info");
                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                psi.CreateNoWindow = true;

                using (Process p = Process.Start(psi))
                {
                    string salida = p.StandardOutput.ReadToEnd();
                    p.WaitForExit();
                    return salida.IndexOf("MSSQLLocalDB", StringComparison.OrdinalIgnoreCase) >= 0;
                }
            }
            catch
            {
                return false;
            }
        }

        private static string BuscarSqlLocalDb()
        {
            string[] versiones = { "160", "150", "140", "130", "120" };
            string[] raices = { @"C:\Program Files\Microsoft SQL Server\", @"C:\Program Files (x86)\Microsoft SQL Server\" };

            foreach (string raiz in raices)
            {
                foreach (string version in versiones)
                {
                    string ruta = Path.Combine(raiz, version, "Tools", "Binn", "SqlLocalDB.exe");
                    if (File.Exists(ruta))
                    {
                        return ruta;
                    }
                }
            }

            return null;
        }

        private void ejecutarNonQuery(string query, SqlParameter[] parametros = null)
        {
            try
            {
                connComando.Connection = conn;
                connComando.Transaction = transaccion;
                connComando.CommandText = query;
                connComando.Parameters.Clear();
                if (parametros != null)
                {
                    connComando.Parameters.AddRange(parametros);
                }
                connComando.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error [sql]: " + ex.Message);
                log.LogWrite("Error [sql]: " + ex.Message.ToString());
                throw;
            }
        }

        public void Dispose()
        {
            if (transaccion != null)
            {
                try { transaccion.Dispose(); }
                catch { }
                transaccion = null;
            }
            if (connComando != null)
            {
                connComando.Dispose();
                connComando = null;
            }
            if (conn != null)
            {
                conn.Dispose();
                conn = null;
            }
        }
    }
}