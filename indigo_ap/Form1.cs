using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using System.Runtime.InteropServices;

namespace indigo_ap
{
    public partial class Form1 : Form
    {

       
    
    public LogWriter log =  new LogWriter();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (conexion con = new conexion())
            {
                con.conectar();
                con.crear_tablas();
            }

            cargarDatagridv();
            cargaFormatos();

            log.LogWrite("Load");

    }

        private void cargarDatagridv()
        {
            using (conexion con = new conexion())
            {
                con.conectar();
                dgvData.DataSource = con.select_custom("IF OBJECT_ID(N'dbo.t003_custom', N'U') IS NOT NULL " +
                                                        " select * from t003_custom;");
            }
        }

        private void cargaFormatos() {

            try
            {

                using (conexion con = new conexion())
                {
                    con.conectar();
                    cmb_prn.DataSource = con.select_custom("select f004_nombre_formato from t004_formatos;");
                    cmb_prn.DisplayMember = "f004_nombre_formato";
                    cmb_prn.ValueMember = "f004_nombre_formato";
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error [custom_prn_Load] " + ex, "Indigo Apps - light applications");
                
            }

        }

        private void btnAbrirArchivo_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Archivos Excel|*.xls;*.xlsx|Archivos CSV|*.csv";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var fileName = ofd.FileName;
                tbNombreArchivo.Text = ofd.SafeFileName;

                try
                {
                    string extension = Path.GetExtension(fileName).ToLowerInvariant();
                    DataTable dtDatos;

                    if (extension == ".csv")
                    {
                        dtDatos = leerCSV(fileName);
                    }
                    else
                    {
                        dtDatos = leerExcel(fileName, extension);
                    }

                    dgvData.DataSource = null;
                    dgvData.DataSource = dtDatos;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error [Archivo]: " + ex, "Indigo Apps - light applications");
                    log.LogWrite("Error [Archivo]: " + ex);
                }
            }
        }

        private DataTable leerExcel(string filePath, string extension)
        {
            if (extension == ".xlsx")
            {
                string connXlsx = @"provider=microsoft.ace.oledb.12.0;data source=" + filePath +
                                  ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;'";
                return LeerExcelCon(connXlsx);
            }

            // .xls: se intenta primero el proveedor ACE y se usa Jet como respaldo.
            try
            {
                string connAce = @"provider=microsoft.ace.oledb.12.0;data source=" + filePath +
                                 ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1;'";
                return LeerExcelCon(connAce);
            }
            catch
            {
                string connJet = @"provider=microsoft.jet.oledb.4.0;data source=" + filePath +
                                 ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1;'";
                return LeerExcelCon(connJet);
            }
        }

        private DataTable LeerExcelCon(string connectionString)
        {
            using (OleDbConnection oledbconn = new OleDbConnection(connectionString))
            {
                oledbconn.Open();

                DataTable dbSchema = oledbconn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                if (dbSchema == null || dbSchema.Rows.Count < 1)
                {
                    throw new Exception("Error: No se puede determinar el nombre de la primera hoja.");
                }

                string firstSheetName = dbSchema.Rows[0]["TABLE_NAME"].ToString();
                string excelQuery = "select * from [" + firstSheetName + "]";

                using (OleDbCommand oledbcmd = new OleDbCommand(excelQuery, oledbconn))
                using (OleDbDataReader dr = oledbcmd.ExecuteReader())
                {
                    DataTable dt = new DataTable("datos");
                    dt.Load(dr);
                    return dt;
                }
            }
        }

        private DataTable leerCSV(string filePath)
        {
            DataTable dt = new DataTable("datos");
            string delimitador = DetectDelimiter(filePath);

            using (TextFieldParser parser = new TextFieldParser(filePath, Encoding.Default, true))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(delimitador);
                parser.HasFieldsEnclosedInQuotes = true;
                parser.TrimWhiteSpace = true;

                if (parser.EndOfData)
                {
                    return dt;
                }

                string[] cabeceras = parser.ReadFields();
                if (cabeceras == null)
                {
                    return dt;
                }

                foreach (string cabecera in cabeceras)
                {
                    dt.Columns.Add((cabecera ?? "").Trim());
                }

                while (!parser.EndOfData)
                {
                    string[] campos = parser.ReadFields();
                    if (campos == null)
                    {
                        continue;
                    }

                    DataRow fila = dt.NewRow();
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        fila[c] = c < campos.Length ? (campos[c] ?? "") : "";
                    }
                    dt.Rows.Add(fila);
                }
            }

            return dt;
        }

        private string DetectDelimiter(string filePath)
        {
            string primeraLinea;
            using (StreamReader sr = new StreamReader(filePath, Encoding.Default, true))
            {
                primeraLinea = sr.ReadLine();
            }

            if (primeraLinea == null)
            {
                return ",";
            }

            int conteoComa = 0, conteoPuntoComa = 0, conteoTab = 0;
            foreach (char c in primeraLinea)
            {
                if (c == ',') conteoComa++;
                else if (c == ';') conteoPuntoComa++;
                else if (c == '\t') conteoTab++;
            }

            if (conteoPuntoComa > conteoComa && conteoPuntoComa > conteoTab) return ";";
            if (conteoTab > conteoComa) return "\t";
            return ",";
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {

            guardar_datos(tbNombreArchivo.Text.ToString(),"","");


        }
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }


        private void guardar_datos(string tabla, string cabeceras, string datos)
        {
            tabla = tabla.Replace(".xls", "").Replace(".XLS", "").Replace(".csv", "").Replace(".CSV", "")
                         .Replace(".xlsx", "").Replace(".XLSX", "");

            using (conexion con = new conexion())
            {
                con.conectar();
                con.iniciar_transaccion();

                try
                {
                    con.delete("t001_parametros", null);
                    con.delete("t002_campos", null);
                    con.drop("t003_custom");

                    con.insertar_datos("t001_parametros",
                        new string[] { "f001_parametro", "f001_valor" },
                        new object[] { "nombre_tabla", tabla });

                    string[] columnas = new string[dgvData.Columns.Count];
                    for (int x = 0; x < dgvData.Columns.Count; x++)
                    {
                        columnas[x] = conexion.SanitizarIdentificador(dgvData.Columns[x].Name);
                        con.insertar_datos("t002_campos",
                            new string[] { "f002_nombre", "f002_valor" },
                            new object[] { columnas[x], "text" });
                    }

                    string qry = "IF OBJECT_ID(N'dbo.t003_custom', N'U') IS NULL "
                        + "create table t003_custom (id INT IDENTITY(1,1) PRIMARY KEY";
                    for (int x = 0; x < columnas.Length; x++)
                    {
                        qry += ",[" + columnas[x] + "] nvarchar(500)";
                    }
                    qry += ");";
                    con.custom_query_set(qry);

                    pgbEstado.Maximum = Math.Max(1, dgvData.Rows.Count - 1);
                    pgbEstado.Value = 0;

                    for (int n = 0; n < dgvData.Rows.Count - 1; n++)
                    {
                        object[] valores = new object[dgvData.Rows[n].Cells.Count];
                        for (int m = 0; m < dgvData.Rows[n].Cells.Count; m++)
                        {
                            valores[m] = dgvData.Rows[n].Cells[m].Value ?? "";
                        }

                        con.insertar_datos("t003_custom", columnas, valores);

                        pgbEstado.Value = n + 1;
                        Application.DoEvents();
                    }

                    con.confirmar();
                    MessageBox.Show("Informacion almacenada!", "Indigo Apps - light applications");
                }
                catch (Exception ex)
                {
                    con.revertir();
                    MessageBox.Show("Error [guardar_datos]: " + ex, "Indigo Apps - light applications");
                    log.LogWrite("Error [guardar_datos]: " + ex.Message.ToString());
                }
            }
        }

        private void btn_openPRN_Click(object sender, EventArgs e)
        {
            custom_prn frm = new custom_prn();
            frm.Show();
        }

        private void btn_eliminar_Click(object sender, EventArgs e)
        {

            //deshabilitado
            if (MessageBox.Show("Esta seguro de borrar la base de datos actual?. \nPresione aceptar para borrar o presione cancelar para cerrar esta ventana.", "Indigo Apps - light applications") == DialogResult.OK) {

                using (conexion con = new conexion())
                {
                    con.conectar();
                    con.drop("t003_custom");
                }

            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

            switch (tabControl1.SelectedIndex)
            {
                case 0:

                    //pestaña archivo

                    //oculta el progressbar
                    pgbEstado.Visible = true;

                    //Button print visible
                    btnPrint.Visible = false;

                    break;

                case 1:

                    //pestaña impresion
                    using (conexion conn = new conexion())
                    {
                        conn.conectar();
                        //muestra los campos en el combobox
                        cmb_campos.DataSource = conn.select_custom("select f002_nombre from t002_campos;");
                        cmb_campos.DisplayMember = "f002_nombre";
                        cmb_campos.ValueMember = "f002_nombre";
                    }

                    //oculta el progressbar
                    pgbEstado.Visible = false;

                    //Button print visible
                    btnPrint.Visible = true;

                    break;



            }

            



            


        }

        private void lbl_info_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Indigo Apps - light applications \nTe permite la impresion masiva usando listados, compatible solo con archivos de extension .xls y .csv", "Indigo Apps - light applications ®");
        }

        private void btn_buscar_Click(object sender, EventArgs e)
        {

            //busca un datos en al tabla
            using (conexion conn = new conexion())
            {
                conn.conectar();

                dgvImpresion.Columns.Clear();

                string campo = conexion.SanitizarIdentificador(cmb_campos.SelectedValue.ToString());
                string query = "select * from t003_custom where [" + campo + "] like @valor";
                SqlParameter[] parametros = new SqlParameter[] { new SqlParameter("@valor", "%" + tb_buscar.Text + "%") };

                dgvImpresion.DataSource = conn.select_custom(query, parametros);
            }

            //se agregan dos columnas para la impresion
            DataGridViewCheckBoxColumn column_chk = new DataGridViewCheckBoxColumn();
            DataGridViewTextBoxColumn column_cant = new DataGridViewTextBoxColumn();

            column_chk.Name = "Imp.";
            column_cant.Name = "Cant.";

            column_chk.Width = 50;
            column_cant.Width = 80;

            column_cant.MaxInputLength = 4;

            dgvImpresion.Columns.Insert(0, column_chk);            
            dgvImpresion.Columns.Insert(1, column_cant);

            //---------------------------

            
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            cargaFormatos();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                if (cmb_prn.SelectedIndex < 0)
                {
                    MessageBox.Show("Seleccione un formato de impresion.", "Indigo Apps - light applications");
                    return;
                }

                bool haySeleccion = false;
                foreach (DataGridViewRow row in dgvImpresion.Rows)
                {
                    if (row.Cells["Imp."].Value != null && Convert.ToBoolean(row.Cells["Imp."].Value))
                    {
                        haySeleccion = true;
                        break;
                    }
                }

                if (!haySeleccion)
                {
                    MessageBox.Show("Seleccione al menos un registro para imprimir.", "Indigo Apps - light applications");
                    return;
                }

                string contenido = archivo_prn(cmb_prn.SelectedValue.ToString());
                if (string.IsNullOrEmpty(contenido))
                {
                    MessageBox.Show("No se genero contenido para imprimir.", "Indigo Apps - light applications");
                    return;
                }

                bool impreso = RawPrinterHelper.SendStringToPrinter(printDialog1.PrinterSettings.PrinterName, contenido);
                if (impreso)
                {
                    log.LogWrite("[Proceso de impresion realizado]");
                }
                else
                {
                    MessageBox.Show("Error al enviar el documento a la impresora.", "Indigo Apps - light applications");
                    log.LogWrite("[Error al enviar el documento a la impresora]");
                }
            }
        }

        private string archivo_prn(string formato)
        {
            string prn_salida = "";
            int cant_log = 0;

            using (conexion conn = new conexion())
            {
                DataTable dt_prn;
                DataTable dt_t002;
                conn.conectar();

                SqlParameter[] parametrosFormato = new SqlParameter[] { new SqlParameter("@formato", formato) };
                dt_prn = conn.select_custom("SELECT f004_formato from t004_formatos where f004_nombre_formato = @formato", parametrosFormato);
                dt_t002 = conn.select_custom("SELECT f002_nombre from t002_campos;");

                if (dt_prn.Rows.Count > 0)
                {
                    if (dt_t002.Rows.Count > 0)
                    {
                        foreach (DataGridViewRow row in dgvImpresion.Rows)
                        {
                            if (row.Cells["Imp."].Value == null || !Convert.ToBoolean(row.Cells["Imp."].Value))
                            {
                                continue;
                            }

                            int cantNum;
                            if (!int.TryParse(Convert.ToString(row.Cells["Cant."].Value), out cantNum) || cantNum < 1)
                            {
                                cantNum = 1;
                            }

                            string prn = dt_prn.Rows[0][0].ToString();

                            for (int i = 0; i < dt_t002.Rows.Count; i++)
                            {
                                string header = dt_t002.Rows[i][0].ToString();
                                string dato = Convert.ToString(row.Cells[header].Value);
                                prn = prn.Replace("$" + header + "$", dato);
                            }

                            prn_salida += prn.Replace("$cantidad$", Convert.ToString(cantNum));
                            cant_log += cantNum;
                        }
                    }
                    else
                    {
                        MessageBox.Show("no existen cabecera o campos para relacionar", "Indigo Apps - light applications");
                    }
                }
                else
                {
                    MessageBox.Show("Formato de impresion no encontrado", "Indigo Apps - light applications ");
                }
            }

            log.LogWrite("Cantidad impresa: " + cant_log.ToString());
            return prn_salida;
        }

        private void dgvImpresion_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        
            //Check to ensure that the row CheckBox is clicked.
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                //Reference the GridView Row.
                DataGridViewRow row = dgvImpresion.Rows[e.RowIndex];

                //Set the CheckBox selection.
                row.Cells["Imp."].Value = !Convert.ToBoolean(row.Cells["Imp."].EditedFormattedValue);

                //If CheckBox is checked, display Message Box.
                if (Convert.ToBoolean(row.Cells["Imp."].Value))
                {
                    row.Cells["Cant."].Value = "1";
                }
                else {
                    row.Cells["Cant."].Value = "";

                }
            }
        }

        int m, mx, my;

        private void lbl_info_MouseDown(object sender, MouseEventArgs e)
        {
            m = 1;
            mx = e.X;
            my = e.Y;

        }

        private void lbl_info_MouseMove(object sender, MouseEventArgs e)
        {
            if (m == 1) {
                this.SetDesktopLocation(MousePosition.X - mx, MousePosition.Y - my);
        }
        }

        private void lbl_info_MouseUp(object sender, MouseEventArgs e)
        {
            m = 0;
        }
    }
}
