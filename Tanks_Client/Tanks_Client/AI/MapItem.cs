using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tanks_Client.beans;

namespace Tanks_Client.AI
{
    class MapItem : IComparable<MapItem>
    {
        public static String BLANK = DecodeOperations.blankSimbol;
        public static String STONE = DecodeOperations.stoneSimbol;
        public static String WATER = DecodeOperations.waterSimbol;
        public static String BRICK = DecodeOperations.brickSimbol;
        public static String COIN = DecodeOperations.coinSimbol;
        public static String LIFEPACK = DecodeOperations.lifePackSimbol;

        private String name;
        private String contain;
        private MapItem pre;
        private int dis;
        private int colour;
        private int dir;

        public int CompareTo(MapItem other)
        {
            return dis.CompareTo(other.Dis);
        }

        public int Dir
        {
            get { return dir; }
            set { dir = value; }
        }

        public int Colour
        {
            get { return colour; }
            set { colour = value; }
        }
        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        public String Contain
        {
            get { return contain; }
            set { contain = value; }
        }
        public MapItem Pre
        {
            get { return pre; }
            set { pre = value; }
        }
        public int Dis
        {
            get { return dis; }
            set { dis = value; }
        }
    }

    class DecodeOperations
    {
        private bool isInit = false;
        private int playerNo = -1;
        public static String brickSimbol = "▥";
        public static String stoneSimbol = "▦";
        public static String waterSimbol = "▩";
        public static String blankSimbol = "▢";
        public static String coinSimbol = "◉";
        public static String lifePackSimbol = "☩";

        public static String[] playerDir = { "▲", "►", "▼", "◄" };
        public static int GRID_SIZE = 10;

        private String[,] map;
        private List<Player> playerList;
        private List<Brick> brickList;
        private List<Coin> coinList;
        private List<LifePack> lifePackList;
        private List<Stone> stoneList;
        private List<Water> waterList;
        public static Clock clock = new Clock();

        private static DecodeOperations dec = new DecodeOperations();

        private DecodeOperations()
        {
            this.map = new String[GRID_SIZE, GRID_SIZE];
            this.playerList = new List<Player>();
            this.brickList = new List<Brick>();
            this.coinList = new List<Coin>();
            this.lifePackList = new List<LifePack>();
            this.stoneList = new List<Stone>();
            this.waterList = new List<Water>();
        }

        public static DecodeOperations GetInstance()
        {
            return dec;
        }

        internal List<Player> PlayerList
        {
            get { return playerList; }
        }

        internal List<Brick> BrickList
        {
            get { return brickList; }
        }

        internal List<Coin> CoinList
        {
            get { return coinList; }
        }

        internal List<Stone> StoneList
        {
            get { return stoneList; }
        }

        internal List<Water> WaterList
        {
            get { return waterList; }
        }

        public int PlayerNo
        {
            get { return playerNo; }
        }

        public Coin getCoinAt(int x, int y)
        {
            foreach (Coin c in coinList)
            {
                if (c.PosX == x && c.PosY == y)
                    return c;
            }
            return null;
        }
        public LifePack getLifePackAt(int x, int y)
        {
            foreach (LifePack lp in lifePackList)
            {
                if (lp.PosX == x && lp.PosY == y)
                    return lp;
            }
            return null;
        }

        public Player getPlayer(int no)
        {
            if (no <= this.playerList.Count)
            {
                return this.playerList[no];
            }
            else
            {
                return null;
            }
        }
        public void setMap(String msg)
        {

            if (!isInit)
            {
                initMap();
                isInit = true;
            }
            switch (msg[0])
            {
                case 'I':
                    {
                        setEnvironment(msg); break;
                    }
                case 'S':
                    {
                        DecodeOperations.clock.startClock();
                        setPlayer(msg); break;
                    }
                case 'G':
                    {
                        setPlayerGroup(msg); break;
                    }
                case 'C':
                    {
                        setCoins(msg); break;
                    }
                case 'L':
                    {
                        setLifePack(msg); break;
                    }
            }

        }

        public String[,] getMap()
        {
            if (!isInit)
            {
                initMap();
                isInit = true;
            }
            return map;
        }

        private void setEnvironment(String msg)
        {
            String[] things = msg.Split(':');
            playerNo = Int32.Parse(things[1][1].ToString());

            foreach (String bri in things[2].Split(';'))
            {
                int x = Int32.Parse(bri[0].ToString());
                int y = Int32.Parse(bri[2].ToString());
                this.brickList.Add(new Brick(x, y));
                map[y, x] = brickSimbol;
            }
            foreach (String sto in things[3].Split(';'))
            {
                int x = Int32.Parse(sto[0].ToString());
                int y = Int32.Parse(sto[2].ToString());
                this.stoneList.Add(new Stone(x, y));
                map[y, x] = stoneSimbol;
            }
            things[4] = things[4].Remove(things[4].Length - 2);
            foreach (String wat in things[4].Split(';'))
            {
                int x = Int32.Parse(wat[0].ToString());
                int y = Int32.Parse(wat[2].ToString());
                this.waterList.Add(new Water(x, y));
                map[y, x] = waterSimbol;
            }

        }

        //S:P0;0,0;0#�
        private void setPlayer(String plDetails)
        {

            String[] players = plDetails.Split(':');

            int length = players.Length;

            for (int i = 1; i < length; i++)
            {
                String[] details = players[i].Split(';');

                String p = details[0][1].ToString();
                int x = Int32.Parse(details[1][0].ToString());
                int y = Int32.Parse(details[1][2].ToString());
                int d = Int32.Parse(details[2][0].ToString());

                Player player = new Player(p, x, y, d);
                this.playerList.Add(player);

                map[y, x] = playerDir[d];

            }



        }

        //G:P0;0,0;0;0;100;0;0:8,6,0;0,3,0;3,2,0;5,4,0;4,7,0;1,3,0;3,6,0#�

        //0,0 ; 0 ; 0; 100 ; 0 ; 0 
        //: 8,6,0 ; 0,3,0 ; 3,2,0 ; 5,4,0 ; 4,7,0 ; 1,3,0 ; 3,6,0#
        private void setPlayerGroup(String msg)
        {
            String[] players = msg.Split(':');

            int length = players.Length;
            String last = players[length - 1];
            players[length - 1] = last.Remove(last.Length - 2);

            for (int i = 1; i < length - 1; i++)
            {
                String[] playerDetails = players[i].Split(';');
                int x = Int32.Parse(playerDetails[1][0].ToString());
                int y = Int32.Parse(playerDetails[1][2].ToString());
                int d = Int32.Parse(playerDetails[2]);
                Boolean ws = Boolean.Parse(playerDetails[3]);
                int h = Int32.Parse(playerDetails[4]);
                int c = Int32.Parse(playerDetails[5]);
                int p = Int32.Parse(playerDetails[6]);

                map[playerList[i - 1].StartY, playerList[i - 1].StartX] = blankSimbol;

                playerList[i - 1].setAll(x, y, d, ws, h, c, p);
                if (h != 0)
                    map[y, x] = playerDir[d];
            }


            String[] bricksDetails = players[length - 1].Split(';');
            foreach (String br in bricksDetails)
            {

                String[] brd = br.Split(',');
                int x = Int32.Parse(brd[0]);
                int y = Int32.Parse(brd[1]);
                int d = Int32.Parse(brd[2]);
                if (d == 4)
                {
                    map[y, x] = blankSimbol;
                }
                setDamageLevel(y, x, d);
            }

        }

        private void setCoins(String coinDetails)
        {
            String[] coin = coinDetails.Split(':');

            int x = Int32.Parse(coin[1][0].ToString());
            int y = Int32.Parse(coin[1][2].ToString());
            int lt = Int32.Parse(coin[2]);
            int v = Int32.Parse(coin[3].Remove(coin[3].Length - 2));
            Coin c = new Coin(x, y, v);
            c.LifeTime = lt;
            c.AppearTime = int.Parse(clock.getTime().ToString());
            this.coinList.Add(c);
            map[y, x] = coinSimbol;
            //Thread coinThread = new Thread(() => disappearItem(c));
            //coinThread.Start();
        }

        private void setLifePack(String lifePackDetails)
        {
            String[] lifePack = lifePackDetails.Split(':');

            int x = Int32.Parse(lifePack[1][0].ToString());
            int y = Int32.Parse(lifePack[1][2].ToString());
            int lt = Int32.Parse(lifePack[2].Remove(lifePack[2].Length - 2));
            LifePack lp = new LifePack(x, y);
            lp.LifeTime = lt;
            lp.AppearTime = int.Parse(clock.getTime().ToString());
            this.lifePackList.Add(lp);
            map[y, x] = lifePackSimbol;
            //Thread lifePackThread = new Thread(() => disappearItem(lp));
            //lifePackThread.Start();
        }

        private void initMap()
        {

            for (int i = 0; i < GRID_SIZE; i++)
            {
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    map[i, j] = blankSimbol;
                }
            }

        }

        private void setDamageLevel(int x, int y, int dl)
        {
            foreach (Brick br in brickList)
            {
                if (br.PosX == x && br.PosY == y)
                {
                    br.Health = dl;
                    break;
                }

            }
        }

        //private void disappearItem(DynamicItem item)
        //{
        //    Thread.Sleep(item.LifeTime);
        //    map[item.PositionY, item.PositionX] = blankSimbol;
        //    if (item is Coin)
        //        this.coinList.Remove((Coin)item);
        //    else if (item is LifePack)
        //        this.lifePackList.Remove((LifePack)item);
        //}


    }
}
