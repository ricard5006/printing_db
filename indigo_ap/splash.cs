using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace indigo_ap
{
    public partial class splash : Form
    {

        //
        [DllImport("user32.dll")]
        public static extern long ShowWindow(IntPtr hwnd, uint nCmdShow);

        //Función para pasar a primer plano una ventana y activarla
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hwnd);
        private bool prev_instances = false;
        //
        public splash()
        {
            InitializeComponent();

            timer1.Interval = 2000;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            Form1 frm1 = new Form1();

            frm1.Show();
               
                   
            this.Visible = false;
        }

        private void splash_Load(object sender, EventArgs e)
        {
            if (Process.GetProcessesByName("indigo_ap").Length > 1)
            {
                prev_instances = true;
                Close();

            }

        }

        private void splash_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (prev_instances)
            {
                //Obtengo el proceso principal de la primera instancia de mi app
                Process p = Process.GetProcessesByName("indigo_ap").Where(it => it.Id != Process.GetCurrentProcess().Id).First();


                //Muestro la ventana
                ShowWindow(p.MainWindowHandle, 1);
                //La activo y la paso a primer plano
                SetForegroundWindow(p.MainWindowHandle);
            }
        }
    }
}
