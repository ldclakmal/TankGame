using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks_Client.beans
{
    class Brick : Obstacle
    {
        private int posX;
        private int posY;
        private int health;

        public Brick(int x, int y, int health)
        {
            posX = x;
            posY = y;
            this.health = health;
        }

        public int PosX
        {
            get { return posX; }
            set { posX = value; }
        }

        public int PosY
        {
            get { return posY; }
            set { posY = value; }
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }
    }
}
