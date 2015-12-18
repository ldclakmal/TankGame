using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks_Client.beans
{
    class LifePack
    {
        private int posX;
        private int posY;
        private int lifeTime = -1;
        private int appearTime = -1;
        private int disappearTime = -1;
        private int disappearBalance = -1;

        public LifePack(int x, int y, int time)
        {
            posX = x;
            posY = y;
            lifeTime = time;
        }

        public LifePack(int x, int y)
        {
            posX = x;
            posY = y;
        }

        public int DisappearBalance
        {
            get { return disappearBalance; }
            set { disappearBalance = value; }
        }

        public int LifeTime
        {
            get { return lifeTime; }
            set { lifeTime = value; }
        }

        public int AppearTime
        {
            get { return appearTime; }
            set { appearTime = value; }
        }

        public int DisappearTime
        {
            get { return disappearTime; }
            set { disappearTime = value; }
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
