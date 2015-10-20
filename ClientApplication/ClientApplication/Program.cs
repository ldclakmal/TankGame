using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientApplication
{
    class Program
    {
        private TcpClient client;
        private TcpListener listener;

        public static void Main()
        {
            Program myPro = new Program();
            while (true)
            {
                String cmd = Console.ReadLine();
                myPro.sendCmd("127.0.0.1", 6000, cmd);
                myPro.startListening("127.0.0.1", 7000);
            }
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
                    Console.WriteLine("\nData: " + data + " is written to " + ip + " on " + port);
                    writer.Close();
                    clientStream.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\nError at myPro:sendCmd..... " + e.StackTrace);
            }
        }

        public void startListening(String ip, int port)
        {
            //The socket that is listened to 
            Socket connection = null;
            try
            {
                //Creating listening Socket
                this.listener = new TcpListener(IPAddress.Parse(ip), port);
                //Starts listening
                this.listener.Start();
                //Establish connection upon client request
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
                        Console.WriteLine(serverIp + ": " + reply);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\nError at myPro:startListening()..... " + e.StackTrace);
            }
        }
    }
}
