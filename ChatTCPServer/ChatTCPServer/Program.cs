using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Net;
using System.Net.Sockets;

namespace ChatTCPServer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string serverPort = Interaction.InputBox("Inserisci la porta del server:", "Porta Locale", "5000");
            if (serverPort == "") return;
            
            Application.Run(new Form1(Convert.ToInt16(serverPort)));
        }
    }
}
