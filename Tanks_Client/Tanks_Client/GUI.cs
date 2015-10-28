﻿using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MetroFramework.Controls;
using System.Drawing;
using System.Collections.Generic;
using System.Collections;

namespace ClientApplication
{
    public partial class GUI : MetroForm
    {
        private TcpClient client;
        private TcpListener listener;
        private Label[][] cellList;
        private Dictionary<String, int> tankOrientation;
        private String myTank;
        private int[][] grid;
        private int[] position;

        public GUI()
        {
            InitializeComponent();
            txtCmd.Enabled = false;
            btnSend.Enabled = false;
            txtDes.Enabled = false;

            position = new int[] { 0, 0 }; //Track the player coordinates

            tankOrientation = new Dictionary<String, int>(); //orinetations of tanks

            grid = new int[10][];
            for (int i = 0; i < 10; i++)
            {
                grid[i] = new int[10]; //position of coins,healthpack,obstacles
            }

            cellList = new Label[][]{ new Label[]{ pos1_1, pos1_2, pos1_3,pos1_4,pos1_5,pos1_6,pos1_7,pos1_8,pos1_9,pos1_10 }
                                           ,new Label[]{ pos2_1, pos2_2, pos2_3, pos2_4, pos2_5, pos2_6, pos2_7,pos2_8,pos2_9,pos2_10 }
                                           ,new Label[]{ pos3_1, pos3_2, pos3_3, pos3_4, pos3_5, pos3_6, pos3_7,pos3_8,pos3_9,pos3_10 }
                                           ,new Label[]{ pos4_1, pos4_2, pos4_3,pos4_4,pos4_5,pos4_6,pos4_7,pos4_8,pos4_9,pos4_10 }
                                           ,new Label[]{ pos5_1, pos5_2, pos5_3,pos5_4,pos5_5,pos5_6,pos5_7,pos5_8,pos5_9,pos5_10 }
                                           ,new Label[]{ pos6_1, pos6_2, pos6_3,pos6_4,pos6_5,pos6_6,pos6_7,pos6_8,pos6_9,pos6_10 }
                                           ,new Label[]{ pos7_1, pos7_2, pos7_3,pos7_4,pos7_5,pos7_6,pos7_7,pos7_8,pos7_9,pos7_10 }
                                           ,new Label[]{ pos8_1, pos8_2, pos8_3,pos8_4,pos8_5,pos8_6,pos8_7,pos8_8,pos8_9,pos8_10 }
                                           ,new Label[]{ pos9_1, pos9_2, pos9_3,pos9_4,pos9_5,pos9_6,pos9_7,pos9_8,pos9_9,pos9_10 }
                                           ,new Label[]{ pos10_1, pos10_2, pos10_3,pos10_4,pos10_5,pos10_6,pos10_7,pos10_8,pos10_9,pos10_10 }                                        
                                            };

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    cellList[i][j].BackColor = System.Drawing.Color.White;
                }
            }

        }


        /**
        * This method will update the position of the player
        * according to commands given in the text box
        * */

        private void btnJoin_Click(object sender, EventArgs e)
        {
            SendCmd("127.0.0.1", 6000, "JOIN#"); //player connect with server
            btnJoin.Enabled = false;
            txtCmd.Enabled = true;
            btnSend.Enabled = true;
            txtDes.Enabled = true;
            Thread thread = new Thread(() => StartListening("127.0.0.1", 7000)); //listeneing thread
            thread.Start();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            String command = txtCmd.Text.ToString();
            int orientation = tankOrientation[myTank];
            Label positionLabel = cellList[position[0]][position[1]]; //current position of the player

            if (command.Equals("UP#"))
            {
                switch (orientation)
                {
                    case 0:
                        if (position[0] - 1 >= 0)
                        {
                            if (grid[position[0] - 1][position[1]] != 1)
                            {
                                positionLabel.BackColor = System.Drawing.Color.White;
                                positionLabel.Image = null;
                                cellList[position[0] - 1][position[1]].Image = new Bitmap("tank.jpg");
                                position[0] = position[0] - 1;
                            }
                        }
                        break;
                    case 1:
                        positionLabel.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        tankOrientation[myTank] = 0;
                        break;
                    case 2:
                        positionLabel.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        tankOrientation[myTank] = 0;
                        break;
                    case 3:
                        positionLabel.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        tankOrientation[myTank] = 0;
                        break;
                }
            }
            else if (command.Equals("DOWN#"))
            {
                switch (orientation)
                {
                    case 0:
                        positionLabel.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        tankOrientation[myTank] = 2;
                        break;
                    case 1:
                        positionLabel.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        tankOrientation[myTank] = 2;
                        break;
                    case 2:
                        if (position[0] + 1 <= 9)
                        {
                            if (grid[position[0] + 1][position[1]] != 1)
                            {
                                positionLabel.BackColor = System.Drawing.Color.White;
                                positionLabel.Image = null;
                                cellList[position[0] + 1][position[1]].Image = new Bitmap("tank.jpg");
                                cellList[position[0] + 1][position[1]].Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                position[0] = position[0] + 1;
                            }
                        }
                        break;
                    case 3:
                        positionLabel.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        tankOrientation[myTank] = 2;
                        break;
                }
            }
            else if (command.Equals("RIGHT#"))
            {
                switch (orientation)
                {
                    case 0:
                        positionLabel.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        tankOrientation[myTank] = 1;
                        break;
                    case 1:

                        if (position[1] + 1 <= 9)
                        {
                            if (grid[position[0]][position[1] + 1] != 1)
                            {
                                positionLabel.BackColor = System.Drawing.Color.White;
                                positionLabel.Image = null;
                                cellList[position[0]][position[1] + 1].Image = new Bitmap("tank.jpg");
                                cellList[position[0]][position[1] + 1].Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                position[1] = position[1] + 1;
                            }

                        }
                        break;

                    case 2:
                        positionLabel.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        tankOrientation[myTank] = 1;
                        break;
                    case 3:
                        positionLabel.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        tankOrientation[myTank] = 1;
                        break;
                }
            }
            else if (command.Equals("LEFT#"))
            {
                switch (orientation)
                {
                    case 0:
                        positionLabel.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        tankOrientation[myTank] = 3;
                        break;
                    case 1:
                        positionLabel.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        tankOrientation[myTank] = 3;
                        break;

                    case 2:
                        positionLabel.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        tankOrientation[myTank] = 3;
                        break;
                    case 3:
                        if (position[1] - 1 >= 0)
                        {
                            if (grid[position[0]][position[1] - 1] != 1)
                            {
                                positionLabel.BackColor = System.Drawing.Color.White;
                                positionLabel.Image = null;
                                cellList[position[0]][position[1] - 1].Image = new Bitmap("tank.jpg");
                                cellList[position[0]][position[1] - 1].Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                position[1] = position[1] - 1;
                            }
                        }
                        break;
                }
            }
            else if (command.Equals("SHOOT"))
            {

            }
            positionLabel.Refresh();
            String cmd = txtCmd.Text;
            SendCmd("127.0.0.1", 6000, cmd);
        }

        public void SendCmd(String ip, int port, String data)
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

        public void StartListening(String ip, int port)
        {
            //The socket that is listened to 
            Socket connection = null;
            try
            {
                if (listener == null)
                {
                    //Creating listening Socket
                    this.listener = new TcpListener(IPAddress.Parse(ip), port);
                    //Starts listening
                    this.listener.Start();
                }
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
                        AppendTextBox(serverIp + "$" + reply);
                        CreateObstacles(reply);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("\nError at myPro:StartListening().....\n" + e.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /**
         * This method append string phrase sent by the server to TextArea
         * */
        public void AppendTextBox(string msg)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendTextBox), new object[] { msg });
                return;
            }
            txtDes.AppendText("\nServer IP " + msg.Split('$')[0] + " : " + msg.Split('$')[1] + "\n");
        }


        /**
        * This method update obstacle in grid according to messages sent by server
        * */
        public void CreateObstacles(string msg)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(CreateObstacles), new object[] { msg });
                return;
            }
            string[] details = msg.Split(':');

            if (details[0].Equals("I"))
            {
                string[] brickCordicates = details[2].Split(';');
                for (int i = 0; i < brickCordicates.Length; i++)
                {
                    String[] codinatesForEach = brickCordicates[i].Split(',');
                    Label brick = cellList[int.Parse(codinatesForEach[1])][int.Parse(codinatesForEach[0])];
                    brick.Image = new Bitmap("brick.jpg");
                    grid[int.Parse(codinatesForEach[1])][int.Parse(codinatesForEach[0])] = 1;
                }
                string[] stoneCordicates = details[3].Split(';');
                for (int i = 0; i < brickCordicates.Length; i++)
                {
                    String[] codinatesForEach = stoneCordicates[i].Split(',');
                    Label stone = cellList[int.Parse(codinatesForEach[1])][int.Parse(codinatesForEach[0])];
                    stone.Image = new Bitmap("stone.jpg");
                    grid[int.Parse(codinatesForEach[1])][int.Parse(codinatesForEach[0])] = 1;
                }
                string[] waterCordicates = details[4].Split(';');
                for (int i = 0; i < waterCordicates.Length; i++)
                {
                    String[] codinatesForEach = waterCordicates[i].Split(',');
                    Label water = cellList[int.Parse(codinatesForEach[1].Split('#')[0])][int.Parse(codinatesForEach[0])];
                    water.Image = new Bitmap("water.jpg");
                    grid[int.Parse(codinatesForEach[1].Split('#')[0])][int.Parse(codinatesForEach[0])] = 1;
                }
                txtData.AppendText("---------------------------------------------------------------------------------------------------------------------------------------- \n");
                txtData.AppendText("Game Initiating : \n");
                txtData.AppendText("---------------------------------------------------------------------------------------------------------------------------------------- \n");
                txtData.AppendText("Player Name : " + details[1] + "\n");
                txtData.AppendText("Brick Co-ordinates : " + details[2] + "\n");
                txtData.AppendText("Stone Co-ordinates : " + details[3] + "\n");
                txtData.AppendText("Water Co-ordinates : " + details[4].Split('#')[0] + "\n \n");
            }
            else if (details[0].Equals("S"))
            {
                myTank = details[1].Split(';')[0];
                String[] locationDetails = details[1].Split(';')[1].Split(',');
                Label tank = cellList[int.Parse(locationDetails[1])][int.Parse(locationDetails[0])];
                tank.Image = new Bitmap("tank.jpg");

                position[0] = int.Parse(locationDetails[1]);
                position[1] = int.Parse(locationDetails[0]);

                tankOrientation.Add(myTank, 0);
                int direction = int.Parse(details[1].Split(';')[2].Split('#')[0]);
                if (direction == 1)
                {
                    tank.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    tankOrientation[myTank] = 1;
                }
                else if (direction == 2)
                {
                    tank.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    tankOrientation[myTank] = 2;
                }
                else if (direction == 3)
                {
                    tank.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    tankOrientation[myTank] = 3;
                }
                txtData.AppendText("---------------------------------------------------------------------------------------------------------------------------------------- \n");
                txtData.AppendText("Game Starting : \n");
                txtData.AppendText("---------------------------------------------------------------------------------------------------------------------------------------- \n");
                txtData.AppendText("Player 1 Details : " + details[1] + "\n");
                txtData.AppendText("Player 2 Details : " + details[2].Split('#')[0] + "\n \n");
            }
            else if (details[0].Equals("G"))
            {
                txtData.AppendText("---------------------------------------------------------------------------------------------------------------------------------------- \n");
                txtData.AppendText("Global Updates : \n");
                txtData.AppendText("---------------------------------------------------------------------------------------------------------------------------------------- \n");
                txtData.AppendText("Player 1 Location + Direction : " + details[1].Split(';')[1] + "\n");
                txtData.AppendText("Player 1 Whether Shot : " + details[1].Split(';')[2] + "\n");
                txtData.AppendText("Player 1 Health : " + details[1].Split(';')[3] + "\n");
                txtData.AppendText("Player 1 Coins : " + details[1].Split(';')[4] + "\n");
                txtData.AppendText("Player 1 Points : " + details[1].Split(';')[5] + "\n \n");
                txtData.AppendText("Player 2 Location + Direction : " + details[2].Split(';')[1] + "\n");
                txtData.AppendText("Player 2 Whether Shot : " + details[2].Split(';')[2] + "\n");
                txtData.AppendText("Player 2 Health : " + details[2].Split(';')[3] + "\n");
                txtData.AppendText("Player 2 Coins : " + details[2].Split(';')[4] + "\n");
                txtData.AppendText("Player 2 Points : " + details[2].Split(';')[5] + "\n \n");
                txtData.AppendText("X,Y Damage Levels : " + details[3].Split('#')[0] + "\n \n");
            }
            else if (details[0].Equals("C"))
            {
                txtData.AppendText("---------------------------------------------------------------------------------------------------------------------------------------- \n");
                txtData.AppendText("Coins : \n");
                txtData.AppendText("---------------------------------------------------------------------------------------------------------------------------------------- \n");
                txtData.AppendText("Coin Co-ordinates : " + details[1] + "\n");
                txtData.AppendText("Time of Coins : " + details[2] + "\n");
                txtData.AppendText("Value of Coins : " + details[3].Split('#')[0] + "\n\n");
            }
            else if (details[0].Equals("L"))
            {
                txtData.AppendText("---------------------------------------------------------------------------------------------------------------------------------------- \n");
                txtData.AppendText("Life Packs : \n");
                txtData.AppendText("---------------------------------------------------------------------------------------------------------------------------------------- \n");
                txtData.AppendText("Life Pack Co-ordinates : " + details[1] + "\n");
                txtData.AppendText("Time of Life Packs : " + details[2].Split('#')[0] + "\n\n");
            }

        }

    }
}