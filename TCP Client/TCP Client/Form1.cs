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

namespace TCP_Client
{
    public partial class Form1 : Form
    {
        string serverIp;
        int serverPort;
        string localIp;
        TcpClient client;
        Thread receiveTh;
        NetworkStream stream; 

        public Form1(string ipServer, string portServer, string localIp)
        {
            InitializeComponent();
            this.serverIp = ipServer;
            this.serverPort = Convert.ToInt32(portServer);
            this.localIp = localIp;
            try
            {
                this.client = new TcpClient(serverIp, serverPort); //creo un tcpclient che serve per la connessione al server
                labelIntro.Text = "Connesso al server " + serverIp + " alla porta " + serverPort;
                stream = client.GetStream(); // apro il flusso per l'invio e la ricezione
                receiveTh = new Thread(receive); //inizializzo il thread per la ricezione
                receiveTh.Start();
            }
            catch (SocketException e)
            {
                MessageBox.Show("Errore: " + e.Message, "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception e)
            {
                MessageBox.Show("Errore: " + e.Message, "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void send()
        {
            try
            {
                string textToSend = tbMsg.Text; //prendo il msg da inviare
                string destIp = tbDestIp.Text; //prendo l'ip del dest
                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(localIp + "/" + destIp + "/" + "s" + "/" + textToSend); //segmento da inviare convertito in byte
                stream.Write(bytesToSend, 0, bytesToSend.Length); //Invio il segmento
                lbChat.Items.Insert(0, "Tu : " + textToSend); //Lo scrivo su textbox
            }
            catch (System.IO.IOException e)
            {
                MessageBox.Show("Errore: " + e.Message, "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (tbMsg.Text != "" && tbDestIp.Text != "") send();
            else lbChat.Items.Insert(0, "Compilare i campi richiesti!");
        }

        private void receive() 
        {
            try
            {
                while(true)
                {
                    byte[] bytesToRead = new byte[client.ReceiveBufferSize]; //Ricevo un segmento in byte
                    int bytesRead = stream.Read(bytesToRead, 0, client.ReceiveBufferSize);//Ricevo un messaggio
                    string[] msgReceived = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead).Split('/'); //Converto il segmento in caratteri ascii ed eseguo lo split per decodificarlo
                    switch(msgReceived[2])
                    {
                        case "c": //Se e' un echo del mio msg appena inviato.
                            {
                                SetText("Il messaggio [ " + msgReceived[3] + " ] e` stato ricevuto.", 'c');
                                break;
                            }
                        case "r": //Se e` un msg di un altro host
                            {
                                SetText(msgReceived[0] + " : " + msgReceived[3], 'r');
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
            }
            catch(System.IO.IOException e)
            {
                MessageBox.Show("Errore: " + e.Message, "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) //Comunico al server che chiudo la connessione
        {
            try
            {
                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes("BB");
                stream.Write(bytesToSend, 0, bytesToSend.Length);
                stream.Close();
                client.Close();
            }
            catch (System.IO.IOException u)
            {
                MessageBox.Show("Errore: " + u.Message, "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch(System.NullReferenceException j)
            {
                MessageBox.Show("Errore: " + j.Message, "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        delegate void SetTextCallback(string text, char c);//Funzione per la gestione dei thread e scrittura
        private void SetText(string text, char c)
        {
            if (lbDebug.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                Invoke(d, new object[] { text , c });
            }
            else
            {
                if( c == 'r')
                    lbChat.Items.Insert(0, text); //scrivo
                else if(c == 'c')
                    lbDebug.Items.Insert(0, text); //scrivo
            }
        }

        private void tbMsg_KeyPress(object sender, KeyPressEventArgs e) //Non permetto l'uso del char '/' essendo usato nel protocollo
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar == '/')
            {
                e.Handled = true;
                MessageBox.Show("Il carattere digitato non e` permesso.", "Warning",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            e.Handled = false;
            return;
        }
    }
}
