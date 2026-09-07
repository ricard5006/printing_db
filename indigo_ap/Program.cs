using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;

namespace indigo_ap
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            //Application.Run(new activacion());
            //Application.Run(new custom_prn());

            string path = @"Activacion.txt";
            if (File.Exists(path))
            {
                IPHostEntry hostInfo = Dns.GetHostEntry("localhost");
                string Id = hostInfo.HostName.ToString();
                String Mac = getMotherBoardID();

                MD5 stringMd5 = new MD5();
                string cadena = "Et1m4rc4s." + Id + "-" + Mac + "-DbPrinting";

                string key = stringMd5.GetMd5Hash(cadena);

                StreamReader reader = new StreamReader(path);
                var linea = reader.ReadLine();
                reader.Close();

                String clave = linea;


                if (key == clave)
                {

                    Application.Run(new splash());
                }
                else
                {

                    Application.Run(new activacion());

                }

            }
            else
            {

                Application.Run(new activacion());
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            MessageBox.Show("Error: " + ex.InnerException.ToString());
        }

        public static String getMotherBoardID()
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
    }
}
