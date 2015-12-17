using MetroFramework.Forms;
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
using System.Collections;
using Tanks_Client.beans;

namespace ClientApplication
{
    public partial class GUI : MetroForm
    {
        private TcpClient client;
        private TcpListener listener;
        private Socket connection;
        private Label[][] cellList;
        private Dictionary<String, int> tankOrientation;
        private String myTank;
        private String enemyTank1;
        private String enemyTank2;
        private String enemyTank3;
        private String enemyTank4;
        private int[][] grid;
        private int[] position;
        private ArrayList playerArr;
        private ArrayList enemyArr;
        private ArrayList brickArr;
        private ArrayList stoneArr;
        private ArrayList waterArr;
        private ArrayList coinArr;
        private ArrayList lifepackArr;
        private Thread thread;
        private Boolean isStop;
        private Boolean isTooQuick;

        public GUI()
        {
            InitializeComponent();
            txtCmd.Enabled = false;
            btnSend.Enabled = false;
            btnStop.Enabled = false;
            txtDes.Enabled = false;
            txtData.Enabled = false;
            isStop = false;
            isTooQuick = false;

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
            btnStop.Enabled = true;
            thread = new Thread(() => StartListening("127.0.0.1", 7000)); //listeneing thread
            thread.Start();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            String command = txtCmd.Text.ToString();
            //if (!isTooQuick)
            //{
            //    UpdateGui(command);
            //}
            SendCmd("127.0.0.1", 6000, command);
        }

        /**
        * This method will call the method UpdateGui() for update the position of the player
        * of client GUI according to commands given in the text box
        * */
        private void ArrowKeysPressed(String command)
        {
            //if (!isTooQuick)
            //{
            //    UpdateGui(command);
            //}
            SendCmd("127.0.0.1", 6000, command);
        }

        /**
        * This method will update the position of the player
        * of client GUI according to commands given in the text box
        * */

        /**
        private void UpdateGui(String command)
        {
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
                                cellList[position[0] - 1][position[1]].Image = new Bitmap("tank1.jpg");
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
                                cellList[position[0] + 1][position[1]].Image = new Bitmap("tank1.jpg");
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
                                cellList[position[0]][position[1] + 1].Image = new Bitmap("tank1.jpg");
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
                                cellList[position[0]][position[1] - 1].Image = new Bitmap("tank1.jpg");
                                cellList[position[0]][position[1] - 1].Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                position[1] = position[1] - 1;
                            }
                        }
                        break;
                }
            }
            else if (command.Equals("SHOOT#"))
            {

            }
            positionLabel.Refresh();    //refresh the label in order to change to new layout
        }
        **/

        /**
        * This method will send commands to the server
        * */
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
                MessageBox.Show("\nError at myPro:SendCmd.....\n" + e.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /**
        * This method will listen all the messages sent by the server and print them in textboxes
        * */
        public void StartListening(String ip, int port)
        {
            //The socket that is listened to 
            connection = null;
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
                while (!isStop)
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
                    if (isStop)
                    {
                        listener.Stop();
                        break;
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
            if (msg.Contains("TOO_QUICK#"))
            {
                isTooQuick = true;
                MessageBox.Show("Too Quick Commands...");
            }
            else
            {
                isTooQuick = false;
            }
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
                //intiantiate ArrayLists for keep the track of objects
                playerArr = new ArrayList();
                enemyArr = new ArrayList();
                brickArr = new ArrayList();
                stoneArr = new ArrayList();
                waterArr = new ArrayList();
                coinArr = new ArrayList();
                lifepackArr = new ArrayList();

                string[] brickCordinates = details[2].Split(';');

                for (int i = 0; i < brickCordinates.Length; i++)
                {
                    String[] codinatesForEach = brickCordinates[i].Split(',');
                    Label brick = cellList[int.Parse(codinatesForEach[1])][int.Parse(codinatesForEach[0])];
                    brick.Image = new Bitmap("brick.jpg");
                    grid[int.Parse(codinatesForEach[1])][int.Parse(codinatesForEach[0])] = 1;

                    Brick brickOb = new Brick(int.Parse(codinatesForEach[0]), int.Parse(codinatesForEach[1]), 100);
                    brickArr.Add(brickOb);
                }

                string[] stoneCordinates = details[3].Split(';');

                for (int i = 0; i < stoneCordinates.Length; i++)
                {
                    String[] codinatesForEach = stoneCordinates[i].Split(',');
                    Label stone = cellList[int.Parse(codinatesForEach[1])][int.Parse(codinatesForEach[0])];
                    stone.Image = new Bitmap("stone.jpg");
                    grid[int.Parse(codinatesForEach[1])][int.Parse(codinatesForEach[0])] = 1;

                    Stone stoneOb = new Stone(int.Parse(codinatesForEach[0]), int.Parse(codinatesForEach[1]));
                    stoneArr.Add(stoneOb);
                }

                string[] waterCordinates = details[4].Split(';');

                for (int i = 0; i < waterCordinates.Length; i++)
                {
                    String[] codinatesForEach = waterCordinates[i].Split(',');
                    Label water = cellList[int.Parse(codinatesForEach[1].Split('#')[0])][int.Parse(codinatesForEach[0])];
                    water.Image = new Bitmap("water.jpg");
                    grid[int.Parse(codinatesForEach[1].Split('#')[0])][int.Parse(codinatesForEach[0])] = 1;

                    Water waterOb = new Water(int.Parse(codinatesForEach[0]), int.Parse(codinatesForEach[1].Split('#')[0]));
                    waterArr.Add(waterOb);
                }

                txtData.AppendText("--------------------------------------------------------------------------------------------------- \n");
                txtData.AppendText("Game Initiating : \n");
                txtData.AppendText("--------------------------------------------------------------------------------------------------- \n");
                txtData.AppendText("Player Name : " + details[1] + "\n");
                txtData.AppendText("Brick Co-ordinates : " + details[2] + "\n");
                txtData.AppendText("Stone Co-ordinates : " + details[3] + "\n");
                txtData.AppendText("Water Co-ordinates : " + details[4].Split('#')[0] + "\n \n");
            }
            else if (details[0].Equals("S"))
            {
                //--------------- myTank ---------------------------------------------------------
                myTank = details[1].Split(';')[0];
                String[] locationDetails = details[1].Split(';')[1].Split(',');
                Label tank = cellList[int.Parse(locationDetails[1])][int.Parse(locationDetails[0])];
                tank.Image = new Bitmap("tank1.jpg");

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

                Player playerOb = new Player(myTank, "127.0.0.1", 6000, int.Parse(locationDetails[0]), int.Parse(locationDetails[1]), 100);
                playerArr.Add(playerOb);

                //--------------- enemyTank1 ---------------------------------------------------------
                if (details.Length >= 3)
                {
                    enemyTank1 = details[2].Split(';')[0];
                    locationDetails = details[2].Split(';')[1].Split(',');
                    tank = cellList[int.Parse(locationDetails[1])][int.Parse(locationDetails[0])];
                    tank.Image = new Bitmap("tank2.jpg");

                    position[0] = int.Parse(locationDetails[1]);
                    position[1] = int.Parse(locationDetails[0]);

                    tankOrientation.Add(enemyTank1, 0);
                    direction = int.Parse(details[2].Split(';')[2].Split('#')[0]);
                    if (direction == 1)
                    {
                        tank.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        tankOrientation[enemyTank1] = 1;
                    }
                    else if (direction == 2)
                    {
                        tank.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        tankOrientation[enemyTank1] = 2;
                    }
                    else if (direction == 3)
                    {
                        tank.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        tankOrientation[enemyTank1] = 3;
                    }

                    Enemy enemy = new Enemy();
                    enemy.Id = 1;
                    enemy.CurrentPositionX = int.Parse(locationDetails[0]);
                    enemy.CurrentPositionY = int.Parse(locationDetails[1]);
                    enemyArr.Add(enemy);
                }

                //--------------- enemyTank2 ---------------------------------------------------------
                if (details.Length >= 4)
                {
                    enemyTank2 = details[3].Split(';')[0];
                    locationDetails = details[3].Split(';')[1].Split(',');
                    tank = cellList[int.Parse(locationDetails[1])][int.Parse(locationDetails[0])];
                    tank.Image = new Bitmap("tank3.jpg");

                    position[0] = int.Parse(locationDetails[1]);
                    position[1] = int.Parse(locationDetails[0]);

                    tankOrientation.Add(enemyTank2, 0);
                    direction = int.Parse(details[3].Split(';')[2].Split('#')[0]);
                    if (direction == 1)
                    {
                        tank.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        tankOrientation[enemyTank2] = 1;
                    }
                    else if (direction == 2)
                    {
                        tank.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        tankOrientation[enemyTank2] = 2;
                    }
                    else if (direction == 3)
                    {
                        tank.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        tankOrientation[enemyTank2] = 3;
                    }

                    Enemy enemy = new Enemy();
                    enemy.Id = 2;
                    enemy.CurrentPositionX = int.Parse(locationDetails[0]);
                    enemy.CurrentPositionY = int.Parse(locationDetails[1]);
                    enemyArr.Add(enemy);
                }

                //--------------- enemyTank3 ---------------------------------------------------------
                if (details.Length >= 5)
                {
                    enemyTank3 = details[4].Split(';')[0];
                    locationDetails = details[4].Split(';')[1].Split(',');
                    tank = cellList[int.Parse(locationDetails[1])][int.Parse(locationDetails[0])];
                    tank.Image = new Bitmap("tank4.jpg");

                    position[0] = int.Parse(locationDetails[1]);
                    position[1] = int.Parse(locationDetails[0]);

                    tankOrientation.Add(enemyTank3, 0);
                    direction = int.Parse(details[4].Split(';')[2].Split('#')[0]);
                    if (direction == 1)
                    {
                        tank.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        tankOrientation[enemyTank3] = 1;
                    }
                    else if (direction == 2)
                    {
                        tank.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        tankOrientation[enemyTank3] = 2;
                    }
                    else if (direction == 3)
                    {
                        tank.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        tankOrientation[enemyTank3] = 3;
                    }

                    Enemy enemy = new Enemy();
                    enemy.Id = 3;
                    enemy.CurrentPositionX = int.Parse(locationDetails[0]);
                    enemy.CurrentPositionY = int.Parse(locationDetails[1]);
                    enemyArr.Add(enemy);
                }

                //--------------- enemyTank4 ---------------------------------------------------------
                if (details.Length >= 6)
                {
                    enemyTank4 = details[5].Split(';')[0];
                    locationDetails = details[5].Split(';')[1].Split(',');
                    tank = cellList[int.Parse(locationDetails[1])][int.Parse(locationDetails[0])];
                    tank.Image = new Bitmap("tank5.jpg");

                    position[0] = int.Parse(locationDetails[1]);
                    position[1] = int.Parse(locationDetails[0]);

                    tankOrientation.Add(enemyTank4, 0);
                    direction = int.Parse(details[5].Split(';')[2].Split('#')[0]);
                    if (direction == 1)
                    {
                        tank.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        tankOrientation[enemyTank4] = 1;
                    }
                    else if (direction == 2)
                    {
                        tank.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        tankOrientation[enemyTank4] = 2;
                    }
                    else if (direction == 3)
                    {
                        tank.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        tankOrientation[enemyTank4] = 3;
                    }

                    Enemy enemy = new Enemy();
                    enemy.Id = 4;
                    enemy.CurrentPositionX = int.Parse(locationDetails[0]);
                    enemy.CurrentPositionY = int.Parse(locationDetails[1]);
                    enemyArr.Add(enemy);
                }

                txtData.AppendText("--------------------------------------------------------------------------------------------------- \n");
                txtData.AppendText("Game Starting : \n");
                txtData.AppendText("--------------------------------------------------------------------------------------------------- \n");
                txtData.AppendText("MyTank Details : " + details[1] + "\n");
                if (details.Length >= 3)
                {
                    txtData.AppendText("Enemy 1 Details : " + details[2].Split('#')[0] + "\n \n");
                }
                if (details.Length >= 4)
                {
                    txtData.AppendText("Enemy 2 Details : " + details[3].Split('#')[0] + "\n \n");
                }
                if (details.Length >= 5)
                {
                    txtData.AppendText("Enemy 3 Details : " + details[4].Split('#')[0] + "\n \n");
                }
                if (details.Length >= 6)
                {
                    txtData.AppendText("Enemy 4 Details : " + details[5].Split('#')[0] + "\n \n");
                }
            }
            else if (details[0].Equals("G"))
            {
                UpdateTankLocation(details);

                txtData.AppendText("--------------------------------------------------------------------------------------------------- \n");
                txtData.AppendText("Global Updates : \n");
                txtData.AppendText("--------------------------------------------------------------------------------------------------- \n");
                txtData.AppendText("MyTank Location : " + details[1].Split(';')[1] + "\n");
                txtData.AppendText("MyTank Direction : " + details[1].Split(';')[2] + "\n");
                txtData.AppendText("MyTank Whether Shot : " + details[1].Split(';')[3] + "\n");
                txtData.AppendText("MyTank Health : " + details[1].Split(';')[4] + "\n");
                txtData.AppendText("MyTank Coins : " + details[1].Split(';')[5] + "\n");
                txtData.AppendText("MyTank Points : " + details[1].Split(';')[6] + "\n \n");
                if (details.Length >= 4)
                {
                    txtData.AppendText("Enemy 1 Location : " + details[2].Split(';')[1] + "\n");
                    txtData.AppendText("Enemy 1 Direction : " + details[2].Split(';')[2] + "\n");
                    txtData.AppendText("Enemy 1 Whether Shot : " + details[2].Split(';')[3] + "\n");
                    txtData.AppendText("Enemy 1 Health : " + details[2].Split(';')[4] + "\n");
                    txtData.AppendText("Enemy 1 Coins : " + details[2].Split(';')[5] + "\n");
                    txtData.AppendText("Enemy 1 Points : " + details[2].Split(';')[6] + "\n \n");
                    txtData.AppendText("X,Y Damage Levels : " + details[3].Split('#')[0] + "\n \n");
                }
                if (details.Length >= 5)
                {
                    txtData.AppendText("Enemy 2 Location : " + details[3].Split(';')[1] + "\n");
                    txtData.AppendText("Enemy 2 Direction : " + details[3].Split(';')[2] + "\n");
                    txtData.AppendText("Enemy 2 Whether Shot : " + details[3].Split(';')[3] + "\n");
                    txtData.AppendText("Enemy 2 Health : " + details[3].Split(';')[4] + "\n");
                    txtData.AppendText("Enemy 2 Coins : " + details[3].Split(';')[5] + "\n");
                    txtData.AppendText("Enemy 2 Points : " + details[3].Split(';')[6] + "\n \n");
                    txtData.AppendText("X,Y Damage Levels : " + details[4].Split('#')[0] + "\n \n");
                }
                if (details.Length >= 6)
                {
                    txtData.AppendText("Enemy 3 Location : " + details[4].Split(';')[1] + "\n");
                    txtData.AppendText("Enemy 3 Direction : " + details[4].Split(';')[2] + "\n");
                    txtData.AppendText("Enemy 3 Whether Shot : " + details[4].Split(';')[3] + "\n");
                    txtData.AppendText("Enemy 3 Health : " + details[4].Split(';')[4] + "\n");
                    txtData.AppendText("Enemy 3 Coins : " + details[4].Split(';')[5] + "\n");
                    txtData.AppendText("Enemy 3 Points : " + details[4].Split(';')[6] + "\n \n");
                    txtData.AppendText("X,Y Damage Levels : " + details[5].Split('#')[0] + "\n \n");
                }
                if (details.Length >= 7)
                {
                    txtData.AppendText("Enemy 4 Location : " + details[5].Split(';')[1] + "\n");
                    txtData.AppendText("Enemy 4 Direction : " + details[5].Split(';')[2] + "\n");
                    txtData.AppendText("Enemy 4 Whether Shot : " + details[5].Split(';')[3] + "\n");
                    txtData.AppendText("Enemy 4 Health : " + details[5].Split(';')[4] + "\n");
                    txtData.AppendText("Enemy 4 Coins : " + details[5].Split(';')[5] + "\n");
                    txtData.AppendText("Enemy 4 Points : " + details[5].Split(';')[6] + "\n \n");
                    txtData.AppendText("X,Y Damage Levels : " + details[6].Split('#')[0] + "\n \n");
                }
            }
            else if (details[0].Equals("C"))
            {
                txtData.AppendText("--------------------------------------------------------------------------------------------------- \n");
                txtData.AppendText("Coins : \n");
                txtData.AppendText("--------------------------------------------------------------------------------------------------- \n");
                txtData.AppendText("Coin Co-ordinates : " + details[1] + "\n");
                txtData.AppendText("Time of Coins : " + details[2] + "\n");
                txtData.AppendText("Value of Coins : " + details[3].Split('#')[0] + "\n\n");
            }
            else if (details[0].Equals("L"))
            {
                txtData.AppendText("--------------------------------------------------------------------------------------------------- \n");
                txtData.AppendText("Life Packs : \n");
                txtData.AppendText("--------------------------------------------------------------------------------------------------- \n");
                txtData.AppendText("Life Pack Co-ordinates : " + details[1] + "\n");
                txtData.AppendText("Time of Life Packs : " + details[2].Split('#')[0] + "\n\n");
            }
        }

        /**
        * This method update tanks in grid according to messages sent by server
        * */
        public void UpdateTankLocation(string[] details)
        {
            //---------------------------------- myTank - Movements -----------------------------------------------------
            if (details.Length >= 3)
            {
                Player myTank = (Player)playerArr[0];
                String coordinates = details[1].Split(';')[1];
                String _direction = details[1].Split(';')[2];
                String x = coordinates.Split(',')[0];
                String y = coordinates.Split(',')[1];

                Label currentPositionLabel = cellList[myTank.CurrentX][myTank.CurrentY];
                currentPositionLabel.BackColor = System.Drawing.Color.White;
                currentPositionLabel.Image = null;
                currentPositionLabel.Refresh();
                cellList[int.Parse(y)][int.Parse(x)].Image = new Bitmap("tank1.jpg");
                myTank.CurrentX = int.Parse(y);
                myTank.CurrentY = int.Parse(x);

                if (myTank.Direction == 1)
                {
                    cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                }
                else if (myTank.Direction == 2)
                {
                    cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                }
                else if (myTank.Direction == 3)
                {
                    cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                }


                if (myTank.Direction == 0)
                {
                    switch (int.Parse(_direction))
                    {
                        case 0:
                            break;

                        case 1:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            myTank.Direction = 1;
                            break;

                        case 2:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            myTank.Direction = 2;
                            break;

                        case 3:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            myTank.Direction = 3;
                            break;
                    }
                }
                else if (myTank.Direction == 1)
                {
                    switch (int.Parse(_direction))
                    {
                        case 0:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            myTank.Direction = 0;
                            break;

                        case 1:
                            break;

                        case 2:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            myTank.Direction = 2;
                            break;

                        case 3:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            myTank.Direction = 3;
                            break;
                    }
                }
                else if (myTank.Direction == 2)
                {
                    switch (int.Parse(_direction))
                    {
                        case 0:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            myTank.Direction = 0;
                            break;

                        case 1:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            myTank.Direction = 1;
                            break;

                        case 2:
                            break;

                        case 3:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            myTank.Direction = 3;
                            break;
                    }
                }
                else
                {
                    switch (int.Parse(_direction))
                    {
                        case 0:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            myTank.Direction = 0;
                            break;

                        case 1:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            myTank.Direction = 1;
                            break;

                        case 2:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            myTank.Direction = 2;
                            break;

                        case 3:
                            break;
                    }
                }
                cellList[int.Parse(y)][int.Parse(x)].Refresh();
            }

            //---------------------------------- enemyTank1 - Movements -----------------------------------------------------
            if (details.Length >= 4)
            {
                Enemy enemy = (Enemy)enemyArr[0];
                String coordinates = details[2].Split(';')[1];
                String _direction = details[2].Split(';')[2];
                String x = coordinates.Split(',')[0];
                String y = coordinates.Split(',')[1];

                Label currentPositionLabel = cellList[enemy.CurrentPositionX][enemy.CurrentPositionY];
                currentPositionLabel.BackColor = System.Drawing.Color.White;
                currentPositionLabel.Image = null;
                currentPositionLabel.Refresh();
                cellList[int.Parse(y)][int.Parse(x)].Image = new Bitmap("tank2.jpg");
                enemy.CurrentPositionX = int.Parse(y);
                enemy.CurrentPositionY = int.Parse(x);

                if (enemy.TankOrientation == 1)
                {
                    cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                }
                else if (enemy.TankOrientation == 2)
                {
                    cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                }
                else if (enemy.TankOrientation == 3)
                {
                    cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                }


                if (enemy.TankOrientation == 0)
                {
                    switch (int.Parse(_direction))
                    {
                        case 0:
                            break;

                        case 1:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            enemy.TankOrientation = 1;
                            break;

                        case 2:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            enemy.TankOrientation = 2;
                            break;

                        case 3:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            enemy.TankOrientation = 3;
                            break;
                    }
                }
                else if (enemy.TankOrientation == 1)
                {
                    switch (int.Parse(_direction))
                    {
                        case 0:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            enemy.TankOrientation = 0;
                            break;

                        case 1:
                            break;

                        case 2:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            enemy.TankOrientation = 2;
                            break;

                        case 3:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            enemy.TankOrientation = 3;
                            break;
                    }
                }
                else if (enemy.TankOrientation == 2)
                {
                    switch (int.Parse(_direction))
                    {
                        case 0:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            enemy.TankOrientation = 0;
                            break;

                        case 1:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            enemy.TankOrientation = 1;
                            break;

                        case 2:
                            break;

                        case 3:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            enemy.TankOrientation = 3;
                            break;
                    }
                }
                else
                {
                    switch (int.Parse(_direction))
                    {
                        case 0:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            enemy.TankOrientation = 0;
                            break;

                        case 1:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            enemy.TankOrientation = 1;
                            break;

                        case 2:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            enemy.TankOrientation = 2;
                            break;

                        case 3:
                            break;
                    }
                }
                cellList[int.Parse(y)][int.Parse(x)].Refresh();
            }

            //---------------------------------- enemyTank2 - Movements -----------------------------------------------------
            if (details.Length >= 5)
            {
                Enemy enemy = (Enemy)enemyArr[1];
                String coordinates = details[3].Split(';')[1];
                String _direction = details[3].Split(';')[2];
                String x = coordinates.Split(',')[0];
                String y = coordinates.Split(',')[1];

                Label currentPositionLabel = cellList[enemy.CurrentPositionX][enemy.CurrentPositionY];
                currentPositionLabel.BackColor = System.Drawing.Color.White;
                currentPositionLabel.Image = null;
                currentPositionLabel.Refresh();
                cellList[int.Parse(y)][int.Parse(x)].Image = new Bitmap("tank3.jpg");
                enemy.CurrentPositionX = int.Parse(y);
                enemy.CurrentPositionY = int.Parse(x);

                if (enemy.TankOrientation == 1)
                {
                    cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                }
                else if (enemy.TankOrientation == 2)
                {
                    cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                }
                else if (enemy.TankOrientation == 3)
                {
                    cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                }


                if (enemy.TankOrientation == 0)
                {
                    switch (int.Parse(_direction))
                    {
                        case 0:
                            break;

                        case 1:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            enemy.TankOrientation = 1;
                            break;

                        case 2:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            enemy.TankOrientation = 2;
                            break;

                        case 3:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            enemy.TankOrientation = 3;
                            break;
                    }
                }
                else if (enemy.TankOrientation == 1)
                {
                    switch (int.Parse(_direction))
                    {
                        case 0:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            enemy.TankOrientation = 0;
                            break;

                        case 1:
                            break;

                        case 2:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            enemy.TankOrientation = 2;
                            break;

                        case 3:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            enemy.TankOrientation = 3;
                            break;
                    }
                }
                else if (enemy.TankOrientation == 2)
                {
                    switch (int.Parse(_direction))
                    {
                        case 0:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            enemy.TankOrientation = 0;
                            break;

                        case 1:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            enemy.TankOrientation = 1;
                            break;

                        case 2:
                            break;

                        case 3:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            enemy.TankOrientation = 3;
                            break;
                    }
                }
                else
                {
                    switch (int.Parse(_direction))
                    {
                        case 0:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            enemy.TankOrientation = 0;
                            break;

                        case 1:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            enemy.TankOrientation = 1;
                            break;

                        case 2:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            enemy.TankOrientation = 2;
                            break;

                        case 3:
                            break;
                    }
                }
                cellList[int.Parse(y)][int.Parse(x)].Refresh();
            }

            //---------------------------------- enemyTank3 - Movements -----------------------------------------------------
            if (details.Length >= 6)
            {
                Enemy enemy = (Enemy)enemyArr[2];
                String coordinates = details[4].Split(';')[1];
                String _direction = details[4].Split(';')[2];
                String x = coordinates.Split(',')[0];
                String y = coordinates.Split(',')[1];

                Label currentPositionLabel = cellList[enemy.CurrentPositionX][enemy.CurrentPositionY];
                currentPositionLabel.BackColor = System.Drawing.Color.White;
                currentPositionLabel.Image = null;
                currentPositionLabel.Refresh();
                cellList[int.Parse(y)][int.Parse(x)].Image = new Bitmap("tank4.jpg");
                enemy.CurrentPositionX = int.Parse(y);
                enemy.CurrentPositionY = int.Parse(x);

                if (enemy.TankOrientation == 1)
                {
                    cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                }
                else if (enemy.TankOrientation == 2)
                {
                    cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                }
                else if (enemy.TankOrientation == 3)
                {
                    cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                }


                if (enemy.TankOrientation == 0)
                {
                    switch (int.Parse(_direction))
                    {
                        case 0:
                            break;

                        case 1:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            enemy.TankOrientation = 1;
                            break;

                        case 2:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            enemy.TankOrientation = 2;
                            break;

                        case 3:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            enemy.TankOrientation = 3;
                            break;
                    }
                }
                else if (enemy.TankOrientation == 1)
                {
                    switch (int.Parse(_direction))
                    {
                        case 0:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            enemy.TankOrientation = 0;
                            break;

                        case 1:
                            break;

                        case 2:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            enemy.TankOrientation = 2;
                            break;

                        case 3:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            enemy.TankOrientation = 3;
                            break;
                    }
                }
                else if (enemy.TankOrientation == 2)
                {
                    switch (int.Parse(_direction))
                    {
                        case 0:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            enemy.TankOrientation = 0;
                            break;

                        case 1:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            enemy.TankOrientation = 1;
                            break;

                        case 2:
                            break;

                        case 3:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            enemy.TankOrientation = 3;
                            break;
                    }
                }
                else
                {
                    switch (int.Parse(_direction))
                    {
                        case 0:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            enemy.TankOrientation = 0;
                            break;

                        case 1:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            enemy.TankOrientation = 1;
                            break;

                        case 2:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            enemy.TankOrientation = 2;
                            break;

                        case 3:
                            break;
                    }
                }
                cellList[int.Parse(y)][int.Parse(x)].Refresh();
            }

            //---------------------------------- enemyTank4 - Movements -----------------------------------------------------
            if (details.Length >= 7)
            {
                Enemy enemy = (Enemy)enemyArr[3];
                String coordinates = details[5].Split(';')[1];
                String _direction = details[5].Split(';')[2];
                String x = coordinates.Split(',')[0];
                String y = coordinates.Split(',')[1];

                Label currentPositionLabel = cellList[enemy.CurrentPositionX][enemy.CurrentPositionY];
                currentPositionLabel.BackColor = System.Drawing.Color.White;
                currentPositionLabel.Image = null;
                currentPositionLabel.Refresh();
                cellList[int.Parse(y)][int.Parse(x)].Image = new Bitmap("tank5.jpg");
                enemy.CurrentPositionX = int.Parse(y);
                enemy.CurrentPositionY = int.Parse(x);

                if (enemy.TankOrientation == 1)
                {
                    cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                }
                else if (enemy.TankOrientation == 2)
                {
                    cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                }
                else if (enemy.TankOrientation == 3)
                {
                    cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                }


                if (enemy.TankOrientation == 0)
                {
                    switch (int.Parse(_direction))
                    {
                        case 0:
                            break;

                        case 1:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            enemy.TankOrientation = 1;
                            break;

                        case 2:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            enemy.TankOrientation = 2;
                            break;

                        case 3:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            enemy.TankOrientation = 3;
                            break;
                    }
                }
                else if (enemy.TankOrientation == 1)
                {
                    switch (int.Parse(_direction))
                    {
                        case 0:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            enemy.TankOrientation = 0;
                            break;

                        case 1:
                            break;

                        case 2:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            enemy.TankOrientation = 2;
                            break;

                        case 3:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            enemy.TankOrientation = 3;
                            break;
                    }
                }
                else if (enemy.TankOrientation == 2)
                {
                    switch (int.Parse(_direction))
                    {
                        case 0:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            enemy.TankOrientation = 0;
                            break;

                        case 1:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            enemy.TankOrientation = 1;
                            break;

                        case 2:
                            break;

                        case 3:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            enemy.TankOrientation = 3;
                            break;
                    }
                }
                else
                {
                    switch (int.Parse(_direction))
                    {
                        case 0:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            enemy.TankOrientation = 0;
                            break;

                        case 1:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            enemy.TankOrientation = 1;
                            break;

                        case 2:
                            cellList[int.Parse(y)][int.Parse(x)].Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            enemy.TankOrientation = 2;
                            break;

                        case 3:
                            break;
                    }
                }
                cellList[int.Parse(y)][int.Parse(x)].Refresh();
            }
        }


        /**
        * This method will call the KeyPressEvents by arrow keys
        * */
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Left)
            {
                ArrowKeysPressed("LEFT#");
                return true;
            }
            else if (keyData == Keys.Right)
            {
                ArrowKeysPressed("RIGHT#");
                return true;
            }
            else if (keyData == Keys.Up)
            {
                ArrowKeysPressed("UP#");
                return true;
            }
            else if (keyData == Keys.Down)
            {
                ArrowKeysPressed("DOWN#");
                return true;
            }
            else if (keyData == Keys.Space)
            {
                ArrowKeysPressed("SHOOT#");
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /**
        * This method will stop threads when closing form
        * */
        private void btnStop_Click(object sender, EventArgs e)
        {
            if (connection != null)
                if (connection.Connected)
                {
                    isStop = true;
                    btnStop.Enabled = false;
                }
        }

        private void GUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Do you want to exit?", "Closing Tank Game", MessageBoxButtons.YesNo);

            if (dr == DialogResult.Yes)
            {
                if (connection != null)
                    if (connection.Connected)
                    {
                        isStop = true;
                        e.Cancel = false;
                    }

            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
