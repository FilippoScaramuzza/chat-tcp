using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
namespace TCP_Client
{
    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string input = Interaction.InputBox("Inserisci l'indirizzo IP del server", "IP Server", "127.0.0.1", -1, -1);
            if(input.Length == 0)
            {
                return;
            }
            string serverPort = Interaction.InputBox("Inserisci la porta del server", "Server Port", "5000", -1, -1);
            if (serverPort.Length == 0)
            {
                return;
            }
            string localIp = Interaction.InputBox("Inserisci il tuo indirizzo IP", "Local IP", GetLocalIPAddress().ToString() , -1, -1);
            if (serverPort.Length == 0)
            {
                return;
            }

            if (input.Length > 0 && serverPort.Length > 0)
                Application.Run(new Form1(input, serverPort, localIp));
        }

        public static IPAddress GetLocalIPAddress() //ottengo il mio ip
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress res = IPAddress.Loopback;
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    res = ip;
                }
            }
            return res;
        }
    }
}
