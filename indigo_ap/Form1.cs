using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlServerCe;


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
            conexion con = new conexion();
            con.conectar();
            con.crear_tablas();
            con.desconectar();

            cargarDatagridv();
            cargaFormatos();

           
            log.LogWrite("Load");
            
    }

        private void cargarDatagridv()
        {
            conexion con = new conexion();
            con.conectar();

            dgvData.DataSource = con.select_custom("IF OBJECT_ID(N'dbo.t003_custom', N'U') IS NOT NULL "+
                                                    " select * from t003_custom;") ;

            con.desconectar();
        }

        private void cargaFormatos() {

            try
            {

                conexion con = new conexion();
                con.conectar();

                cmb_prn.DataSource = con.select_custom("select f004_nombre_formato from t004_formatos;");
                cmb_prn.DisplayMember = "f004_nombre_formato";
                cmb_prn.ValueMember = "f004_nombre_formato";

                con.desconectar();


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error [custom_prn_Load] " + ex, "Indigo Apps - light applications");
                
            }

        }

        private void btnAbrirArchivo_Click(object sender, EventArgs e)
        {
            

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CSV files|*.csv|Excel files|*xls";

            

            if (ofd.ShowDialog() == DialogResult.OK) {

                var fileName = ofd.FileName;
                tbNombreArchivo.Text = ofd.SafeFileName;

                if (fileName.Contains(".xls") || fileName.Contains(".XLS"))
                {
                    try {


                      //falta verificar si el archivo esta siendo usado/abierto para que no genere error si es el caso.


                        string connectionStringExcel = @"provider=microsoft.jet.oledb.4.0;data source=" + fileName +
                                                    ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1;'";

                    OleDbConnection oledbconn = new OleDbConnection(connectionStringExcel);
                    
                    oledbconn.Open();
                    DataTable dbSchema = oledbconn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                    if (dbSchema == null || dbSchema.Rows.Count < 1)
                    {
                        throw new Exception("Error: No se puede determinar el nombre de la primera hoja.");
                    }

                    string firstSheetName = dbSchema.Rows[0]["TABLE_NAME"].ToString();

                    string excelQuery = "select * from [" + firstSheetName + "]";

                    OleDbCommand oledbcmd = new OleDbCommand(excelQuery, oledbconn);

                    OleDbDataReader dr = oledbcmd.ExecuteReader();

                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable("datos");

                    while (!dr.IsClosed)
                    {

                        Console.WriteLine("datos " + dr.ToString());
                        dt.Load(dr);

                    }

                    ds.Tables.Add(dt);
                        dgvData.DataSource = null;
                    dgvData.DataSource = ds.Tables["datos"];

                    }
                    catch (Exception ex) {
                        MessageBox.Show("Error [Archivo]: "+ ex, "Indigo Apps - light applications");
                        log.LogWrite("Error [Archivo]: " + ex);
                    }

                }
                else {

                    //archivos csv
                }

            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {

            guardar_datos(tbNombreArchivo.Text.ToString(),"","");


        }
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }


        private void guardar_datos(string tabla,string cabeceras,string datos) {
            conexion con = new conexion();

           

            tabla = tabla.Replace(".xls", "").Replace(".XLS","").Replace(".csv","").Replace(".CSV","");

            con.conectar();

            con.delete("t001_parametros",null);
            con.delete("t002_campos", null);
            con.drop("t003_custom");

            //nombre de la tabla t001_parametros
            con.insertar_datos("t001_parametros (f001_parametro,f001_valor)", "'nombre_tabla','"+tabla+"'");

            //header de la tabla
            var column_name = "";
            var qry = "";
            var column_name2 = "";
            for (int x = 0; x < dgvData.Columns.Count; x++ ) {
                column_name = dgvData.Columns[x].Name.ToString().Replace(' ','_');
                con.insertar_datos("t002_campos (f002_nombre,f002_valor)", "'"+ column_name + "', 'text'");
                column_name2 += column_name;
                qry += ","+column_name + " nvarchar(500) ";
            }

            //crear tabla custom para insertar datos del archivo
            

            qry = "IF OBJECT_ID(N'dbo.t003_custom', N'U') IS NULL "
                + "create table t003_custom (id INT IDENTITY(1,1) PRIMARY KEY " + qry+");";
            con.custom_query_set(qry);


            //insertar datos del archivo (datagridview)
            for (int n=0;n < dgvData.Rows.Count-1;n++) {
                pgbEstado.Maximum = dgvData.Rows.Count;
                pgbEstado.Value = 0;
                var y = "";
                for (int m=0;m < dgvData.Rows[n].Cells.Count;m++) {

                     y += "'"+dgvData.Rows[n].Cells[m].Value.ToString()+ "'";

                    if (m == dgvData.Rows[n].Cells.Count-1)
                    {

                    }
                    else {
                        y += ",";
                    }
                }

                //leer columnas almacenadas
                column_name = "";
                for (int x = 0; x < dgvData.Columns.Count; x++)
                {
                    column_name += dgvData.Columns[x].Name.ToString().Replace(' ', '_');

                    if (x == dgvData.Columns.Count - 1)
                    {
                        column_name += ")";
                    }
                    else
                    {
                        column_name += ",";
                    }

                }

                    con.insertar_datos("t003_custom ("+ column_name,  y);
                    pgbEstado.Value = n;
                    
            }
            MessageBox.Show("Informacion almacenada!", "Indigo Apps - light applications");
            con.desconectar();
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

                conexion con = new conexion();

                con.conectar();

                con.drop("t003_custom");

                con.desconectar();

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
                    conexion conn = new conexion();
                    conn.conectar();
                    //muestra los campos en el combobox
                    cmb_campos.DataSource = conn.select_custom("select f002_nombre from t002_campos;");
                    cmb_campos.DisplayMember = "f002_nombre";
                    cmb_campos.ValueMember = "f002_nombre";

                    //oculta el progressbar
                    pgbEstado.Visible = false;

                    //Button print visible
                    btnPrint.Visible = true;

                    conn.desconectar();

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
            conexion conn = new conexion();
            conn.conectar();

            dgvImpresion.Columns.Clear();

            dgvImpresion.DataSource = conn.select_custom("select * from t003_custom where " + cmb_campos.SelectedValue.ToString() + " like '%" + tb_buscar.Text.ToString() + "%'");

            conn.desconectar();

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
            if (printDialog1.ShowDialog() == DialogResult.OK) {

                if (cmb_prn.SelectedIndex >= 0) {





                    RawPrinterHelper.SendStringToPrinter(printDialog1.PrinterSettings.PrinterName, archivo_prn(cmb_prn.SelectedValue.ToString()));
                    log.LogWrite("[Proceso de impresion realizado]");
                }                

            }


        }

        private string archivo_prn(string formato) {

            string prn = "";
            string prn_salida = "";
            string cant = "1";
            int cant_log = 0;
            conexion conn = new conexion();
            DataTable dt_prn = new DataTable();
            DataTable dt_t002 = new DataTable();
            conn.conectar();
            dt_prn = conn.select_custom("SELECT f004_formato from t004_formatos where f004_nombre_formato = '"+formato+"';");
            dt_t002 = conn.select_custom("SELECT f002_nombre from t002_campos;");
            conn.desconectar();

            if (dt_prn.Rows.Count > 0)
            {
                if (dt_t002.Rows.Count > 0)
                {
                    //////////////solo imprime uno

                    foreach (DataGridViewRow row in dgvImpresion.Rows)
                    {
                        bool isSelected = Convert.ToBoolean(row.Cells["Imp."].Value);
                        if (isSelected)
                        {

                            cant = row.Cells["Cant."].Value.ToString();


                            //////////////////
                            prn = dt_prn.Rows[0][0].ToString();

                            for (int i = 0; i < dt_t002.Rows.Count; i++)
                            {
                               
                                var header = dt_t002.Rows[i][0].ToString();
                                var dato = row.Cells[header.ToString()].Value.ToString();

                                prn = prn.Replace("$" + header + "$", dato);
                            }

                            prn_salida += prn.Replace("$cantidad$", cant);
                            cant_log += int.Parse(cant);
                            ////////////////////


                        }

                        
                    }

                    //////////////
                    

                    

                }
                else {
                    MessageBox.Show("no existen cabecera o campos para relacionar", "Indigo Apps - light applications");
                }
            }
            else {
                MessageBox.Show("Formato de impresion no encontrado", "Indigo Apps - light applications ");
            }
            log.LogWrite("Cantidad impresa: "+cant_log.ToString());
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
    
    }
}
