using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Permissions;

namespace ChatTCPServer
{
    public partial class Form1 : Form
    {
        static object _lock = new object(); // serve per evitare che altri thread eseguano il blocco di istruzioni "critico"
        static Dictionary<int, TcpClient> list_clients = new Dictionary<int, TcpClient>(); // lista/dizionario: numeroUtente, TcpClient

        Thread listenThread;

        int count;

        int serverPort; // porta del server

        public Form1(int serverPort)
        {
            InitializeComponent();
            this.serverPort = serverPort;
            
            //calcolo indirizzo IP del server
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    lblIp.Text = ip.ToString();
                }
            }

            lblPort.Text = serverPort.ToString();
            lblUserCount.Text = "0";
        }

        public void startListen()
        {
            count = 0;
            // Comincia il listen()
            TcpListener ServerSocket = new TcpListener(IPAddress.Any, serverPort);
            ServerSocket.Start();
            while (true)
            {
                TcpClient client = ServerSocket.AcceptTcpClient();
                lock (_lock) list_clients.Add(count, client);
                SetText(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() + " connected!");
                // Creo un thread per ogni client per l'ascolto dei messaggi
                Thread t = new Thread(handle_clients);
                t.IsBackground = true; //background così si chiude da solo insieme al form
                t.Start(count);
                count++;
                RefreshLblCount(list_clients.Count.ToString());
            }
        }
        
        //Funzioni strane per gestire le componenti 
        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (lst.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                Invoke(d, new object[] { text });
            }
            else
            {
                lst.Items.Insert(0, text);
            }
        }

        //Aggiorna il numero di utenti (modifica della label richiamabile dal thread)
        private void RefreshLblCount(string count)
        {
            if (lblUserCount.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(RefreshLblCount);
                Invoke(d, new object[] { count });
            }
            else
            {
                lblUserCount.Text = count;
            }
        }

        //richiamata da ogni thread per ogni client
        //gestisce la ricezione dei messaggi
        public void handle_clients(object o)
        {
            int id = (int)o;
            TcpClient client;

            lock (_lock) client = list_clients[id];

            if(client.Connected == true)
            {
                while (true)
                {
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[1024];
                    int byte_count = stream.Read(buffer, 0, buffer.Length);

                    if (byte_count == 0)
                    {
                        break;
                    }
                    // se il messaggio è "BB", significa che il client si è disconnesso
                    string data = Encoding.ASCII.GetString(buffer, 0, byte_count);
                    if (data == "BB")
                    {
                        lock (_lock) list_clients.Remove(id);
                        SetText(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() + " disconnected!");
                        client.Client.Shutdown(SocketShutdown.Both);
                        client.Close();
                        RefreshLblCount(list_clients.Count.ToString());
                        return;
                    }
                    /* Altrimenti viene inviato inoltrato il messaggio normalmente:
                     * per gestire i flussi dei mesasggi abbiamo implementato un protocollo per il componimento del messaggio:
                     * 
                     * FORMATO MESSAGGIO
                     * 
                     * IPMITTENTE/IPDESTINATARIO/TIPOMESSAGGIO/DATI
                     * 
                     * TIPOMESSAGGIO può essere: 
                     * - s : il messaggio è inviato da client a server
                     * - c : il messaggio è inviato dal server al client, ed è la conferma della ricezione del messaggio
                     * - r : il messaggio è inviato da un client ad un altro (inoltrato dal server)
                     * 
                     */
                    else
                    {
                        send(data);
                        SetText(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() + " to " + data.Split('/')[1] + ": " + data.Split('/')[3]);
                    }
                }
            }
        }

        public void send(string data)
        {
            byte[] buffer;

            IPAddress localIp = IPAddress.Parse(data.Split('/')[0]);
            IPAddress remoteIp = IPAddress.Parse(data.Split('/')[1]);

            lock (_lock)
            {
                foreach (TcpClient c in list_clients.Values)
                {
                    //Se IPMITTENTE è uguale all'ip del client invia la conferma della ricezione con TIPOMESSAGGIO: "c"
                    if (((IPEndPoint)c.Client.RemoteEndPoint).Address.ToString() == localIp.ToString())
                    {
                        string[] splittedData = data.Split('/');
                        // c (confirmed) è il codice che il client interpreta come conferma dell'invio del messaggio
                        splittedData[2] = "c";
                        data = string.Join("/", splittedData);
                        buffer = Encoding.ASCII.GetBytes(data + Environment.NewLine);

                        NetworkStream stream = c.GetStream();

                        stream.Write(buffer, 0, buffer.Length);
                    }
                    //ALtrimenti se IPDESTINATARIO è uguale all'ip del client invia il messaggio come messaggio in arrivo, con TIPOMESSAGGIO: "r"
                    if (((IPEndPoint)c.Client.RemoteEndPoint).Address.ToString() == remoteIp.ToString())
                    {
                        string[] splittedData = data.Split('/');
                        // r (receive) è il codice che il client interpreta come messaggio in arrivo
                        splittedData[2] = "r";
                        data = string.Join("/", splittedData);
                        buffer = Encoding.ASCII.GetBytes(data + Environment.NewLine);

                        NetworkStream stream = c.GetStream();

                        stream.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            //Viene creado il thread per l'ascolto
            listenThread = new Thread(startListen);
            listenThread.IsBackground = true;
            listenThread.Start();
            btnListen.Enabled = false;
            lst.Items.Insert(0, "Listening on port " + serverPort);
            btnStopListen.Enabled = true;
        }

        private void btnStopListen_Click(object sender, EventArgs e)
        {
            listenThread.Suspend();
            lst.Items.Insert(0, "Server is no more listening");
            btnRestartListen.Enabled = true;
            btnStopListen.Enabled = false;
        }

        private void Form1_Closing(object sender, CancelEventArgs e)
        {
            Application.Exit();
        }

        private void btnRestartListen_Click(object sender, EventArgs e)
        {
            listenThread.Resume();
            lst.Items.Insert(0, "Listening on port " + serverPort);
            btnStopListen.Enabled = true;
            btnRestartListen.Enabled = false;
        }
    }
}
