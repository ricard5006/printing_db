using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace indigo_ap
{
    public partial class activacion : Form
    {

        //
        [DllImport("user32.dll")]
        public static extern long ShowWindow(IntPtr hwnd, uint nCmdShow);

        //Función para pasar a primer plano una ventana y activarla
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hwnd);
        private bool prev_instances = false;
        public activacion()
        {
            InitializeComponent();
            IPHostEntry hostInfo = Dns.GetHostEntry("localhost");
            TbID.Text = hostInfo.HostName.ToString();
            TbMAC.Text = getMotherBoardID();



            demoVersion();
        }

        public String getMotherBoardID()
        {

            string serial = "";

            IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            Console.WriteLine("Interface information for {0}.{1}     ",
                    computerProperties.HostName, computerProperties.DomainName);
            foreach (NetworkInterface adapter in nics)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();

                if (adapter.NetworkInterfaceType.ToString().Equals("Ethernet"))
                {
                    serial = adapter.GetPhysicalAddress().ToString();
                    break;
                }

            }

            return serial;

        }

       

        private void activar_Click(object sender, EventArgs e)
        {
            MD5 stringMd5 = new MD5();


            string cadena = "Et1m4rc4s." + TbID.Text.ToString() + "-" + TbMAC.Text.ToString() + "-" + TbApp.Text.ToString();
            string clave = TbActv.Text.ToString();
            string key = stringMd5.GetMd5Hash(cadena);
            if (key == TbActv.Text)
            {
                using (StreamWriter sw = File.CreateText(conexion.ObtenerRutaActivacion()))
                {
                    sw.WriteLine(key);
                }
                //Thread t = new Thread(new ThreadStart(frm_Principal));
                //t.Start();
                //Close();
                Form1 frm = new Form1();
                frm.Show();
                this.Visible = false;

            }
            else
            {
                MessageBox.Show("Error en el Codigo de Activación, Por Favor Verifique e ingrese de nuevo");
                TbActv.Text = "";
            }
        }



        private void frm_Principal()
        {
            // no usado
            Application.Run(new Form1());
        }


        private void btnDemo_Click(object sender, EventArgs e)
        {
            String archivo = conexion.ObtenerRutaActivacion();
            if (File.Exists(archivo))
            {

                ////////////////
                StreamReader reader = new StreamReader(archivo);
                var linea = reader.ReadLine();
                reader.Close();

                if ((int.Parse(fecha()) < int.Parse(linea)))
                {

                    MessageBox.Show("Error en activacion Demo.");
                    this.Close();
                }

                else
                {

                    //Thread t = new Thread(new ThreadStart(frm_Principal));
                    //t.Start();                   
                    //Close();
                    Form1 frm = new Form1();
                    frm.Show();
                    this.Visible = false;

                }





            }
            else
            {

                using (StreamWriter sw = File.CreateText(archivo))
                {
                    sw.WriteLine(fecha());
                    sw.Close();
                }
                //Thread t = new Thread(new ThreadStart(frm_Principal));
                //t.Start();

                //Close();
                Form1 frm = new Form1();
                frm.Show();
                this.Visible = false;

            }
        }

        public string fecha()
        {
            string fecha = DateTime.Now.ToString("ddMMyyyy");

            return fecha + "1";
        }

        private void demoVersion()
        {
            try
            {
                string path = conexion.ObtenerRutaActivacion();
                if (File.Exists(path))
                {
                    StreamReader reader = new StreamReader(path);
                    var linea = reader.ReadLine();
                    reader.Close();

                    if ((int.Parse(fecha()) - int.Parse(linea)) > 30000000)
                    {

                        btnDemo.Enabled = false;
                    }


                }
            }
            catch (Exception e) {
                MessageBox.Show("Error en activacion: "+e);
            }

        }

        private void activacion_Load(object sender, EventArgs e)
        {
            if (Process.GetProcessesByName("indigo_ap").Length > 1)
            {
                prev_instances = true;
                Close();

            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
