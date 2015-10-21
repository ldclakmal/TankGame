using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ClientApplication
{
    public partial class GUI : MetroForm
    {
        private TcpClient client;
        private TcpListener listener;

        public GUI()
        {
            InitializeComponent();
            txtCmd.Enabled = false;
            btnSend.Enabled = false;
        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            sendCmd("127.0.0.1", 6000, "JOIN#");
            btnJoin.Enabled = false;
            txtCmd.Enabled = true;
            btnSend.Enabled = true;
            startListening("127.0.0.1", 7000);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            String cmd = txtCmd.Text;
            sendCmd("127.0.0.1", 6000, cmd);
            //Thread thread = new Thread(() => startListening("127.0.0.1", 7000));
            //thread.Start();
        }

        public void sendCmd(String ip, int port, String data)
        {
            try
            {
                this.client = new TcpClient();
                this.client.Connect(ip, port);

                if (this.client.Connected)
                {
                    //To write to the socket
                    NetworkStream clientStream = client.GetStream();
                    BinaryWriter writer = new BinaryWriter(clientStream);
                    //Create objects for writing across stream
                    writer = new BinaryWriter(clientStream);
                    Byte[] tempStr = Encoding.ASCII.GetBytes(data);

                    //writing to the port                
                    writer.Write(tempStr);
                    writer.Close();
                    clientStream.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("\nError at myPro:sendCmd.....\n" + e.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void startListening(String ip, int port)
        {
            try
            {
                //Creating listening Socket
                this.listener = new TcpListener(IPAddress.Parse(ip), port);
                //Starts listening
                this.listener.Start();
                //Establish connection upon client request
                //listen();
                Thread thread = new Thread(() => listen());
                thread.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show("\nError at myPro:startListening().....\n" + e.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void listen()
        {
            try
            {
                //The socket that is listened to 
                Socket connection = null;
                while (true)
                {
                    //connection is connected socket
                    connection = listener.AcceptSocket();
                    if (connection.Connected)
                    {
                        //To read from socket create NetworkStream object associated with socket
                        NetworkStream serverStream = new NetworkStream(connection);

                        SocketAddress sockAdd = connection.RemoteEndPoint.Serialize();
                        string s = connection.RemoteEndPoint.ToString();
                        List<Byte> inputStr = new List<byte>();

                        int asw = 0;
                        while (asw != -1)
                        {
                            asw = serverStream.ReadByte();
                            inputStr.Add((Byte)asw);
                        }

                        String reply = Encoding.UTF8.GetString(inputStr.ToArray());
                        serverStream.Close();
                        string serverIp = s.Substring(0, s.IndexOf(":"));
                        AppendTextBox(serverIp + "$" + reply);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("\nError at myPro:listen().....\n" + e.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AppendTextBox(string msg)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendTextBox), new object[] { msg });
                return;
            }
            txtDes.AppendText("\nServer IP " + msg.Split('$')[0] + " : " + msg.Split('$')[1] + "\n");
        }
    }
}
