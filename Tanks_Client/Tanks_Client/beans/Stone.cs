using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks_Client.beans
{
    class Stone : Obstacle
    {
        private int posX;
        private int posY;

        public Stone(int x, int y)
        {
            posX = x;
            posY = y;
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
    }
}
