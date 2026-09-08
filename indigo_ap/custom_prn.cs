using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace indigo_ap
{
    public partial class custom_prn : Form
    {
        public LogWriter log = new LogWriter();
        public custom_prn()
        {
            InitializeComponent();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void custom_prn_Load(object sender, EventArgs e)
        {
            cargaFormatos();
            cargarCampos();
        }

        private void cargaFormatos()
        {

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
                MessageBox.Show("Error [cargaFormatos] " + ex, "Indigo Apps - light applications");
                log.LogWrite("Error[cargaFormatos]");
            }

        }

        private void cargarCampos() {

            try
            {
                using (conexion conn = new conexion())
                {
                    conn.conectar();
                    DataTable dt = new DataTable();
                    dt = conn.select_custom("select f002_nombre from t002_campos;");

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            tb_campos.Text += "$" + dt.Rows[i][0].ToString() + "$ \r\n";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error [cargarCampos] " + ex, "Indigo Apps - light applications");
                log.LogWrite("Error [cargarCampos]");
            }


        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            try
            {

                using (conexion con = new conexion())
                {
                    con.conectar();

                    if (cmb_prn.FindStringExact(cmb_prn.Text.ToString()) >= 0)
                    {
                        con.update_datos("t004_formatos",
                            new string[] { "f004_formato" },
                            new object[] { tb_prn.Text },
                            "f004_nombre_formato = @w0",
                            new object[] { cmb_prn.Text });
                        MessageBox.Show("Formato actualizado", "Indigo Apps - light applications");
                    }
                    else
                    {
                        con.insertar_datos("t004_formatos",
                            new string[] { "f004_nombre_formato", "f004_formato" },
                            new object[] { cmb_prn.Text, tb_prn.Text });
                        MessageBox.Show("Formato nuevo almacenado", "Indigo Apps - light applications");
                    }

                    cmb_prn.DataSource = con.select_custom("select f004_nombre_formato from t004_formatos;");
                    cmb_prn.DisplayMember = "f004_nombre_formato";
                    cmb_prn.ValueMember = "f004_nombre_formato";
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error [btn_ok_Click] " + ex, "Indigo Apps - light applications");
                log.LogWrite("Error [btn_ok_Click]");
            }
        }

        private void cmb_prn_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        private void cmb_prn_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                using (conexion con = new conexion())
                {
                    con.conectar();

                    SqlParameter[] parametros = new SqlParameter[] { new SqlParameter("@formato", cmb_prn.Text) };
                    DataTable dt = con.select_custom("select f004_formato from t004_formatos where f004_nombre_formato = @formato", parametros);

                    if (dt.Rows.Count > 0)
                    {
                        tb_prn.Text = dt.Rows[0][0].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error [cmb_prn_SelectedIndexChanged] " + ex, "Indigo Apps - light applications");
                log.LogWrite("Error [cmb_prn_SelectedIndexChanged]");
            }
        }
    }
}
