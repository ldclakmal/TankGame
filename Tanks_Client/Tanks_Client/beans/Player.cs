using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks_Client.beans
{
    class Player
    {
        #region "Variables"
        private string name = "";
        private string ip = "";
        private int port = -1;
        private int startX;
        private int startY;
        private int currentX;
        private int currentY;
        private int direction = 0;
        private Boolean shot = false;
        private int pointsEarned = 0;
        private int coins = 0;
        private int health = 100;
        private bool isAlive = true;
        private bool invalidCell = false;
        private DateTime updatedTime;
        private int index = -1;


        public Player(string cName, string ipAdd, int cPort, int x, int y, int health)
        {
            name = cName;
            ip = ipAdd;
            port = cPort;
            startX = x;
            startY = y;
            this.health = health;
        }

        public Player(string cName, int x, int y, int direction)
        {
            name = cName;
            startX = x;
            startY = y;
            this.direction = direction;
        }

        public void setAll(int x, int y, int d, Boolean ws, int h, int c, int p)
        {
            this.startX = x;
            this.startY = y;
            this.direction = d;
            this.shot = ws;
            this.health = h;
            this.coins = c;
            this.pointsEarned = p;
        }

        public int Coins
        {
            get { return coins; }
            set { coins = value; }
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public Boolean Shot
        {
            get { return shot; }
            set { shot = value; }
        }

        public int Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        #endregion

        #region "Properties"
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string IP
        {
            get { return ip; }
            set { ip = value; }
        }

        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        public int StartX
        {
            get { return startX; }
            set { startX = value; }
        }

        public int StartY
        {
            get { return startY; }
            set { startY = value; }
        }

        public int CurrentX
        {
            get { return currentX; }
            set { currentX = value; }
        }

        public int CurrentY
        {
            get { return currentY; }
            set { currentY = value; }
        }

        public DateTime UpdatedTime
        {
            get { return updatedTime; }
            set { updatedTime = value; }
        }

        public int PointsEarned
        {
            get { return pointsEarned; }
            set { pointsEarned = value; }
        }

        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }

        public bool InvalidCell
        {
            get { return invalidCell; }
            set { invalidCell = value; }
        }
        #endregion
    }
}