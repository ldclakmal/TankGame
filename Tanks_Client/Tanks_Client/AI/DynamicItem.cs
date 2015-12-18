using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks_Client.AI
{
    class DynamicItem
    {
        private int positionX;
        private int positionY;
        private int lifeTime = -1;
        private long appearTime = -1;


        public DynamicItem(int x, int y)
        {
            positionX = x;
            positionY = y;
        }


        public int LifeTime
        {
            get { return lifeTime; }
            set { lifeTime = value; }
        }

        public long AppearTime
        {
            get { return appearTime; }
            set { appearTime = value; }
        }

        public int PositionY
        {
            get { return positionY; }
            set { positionY = value; }
        }

        public int PositionX
        {
            get { return positionX; }
            set { positionX = value; }
        }

        public long getRemainingTime()
        {
            return lifeTime - (DecodeOperations.clock.getTime() - appearTime);
        }
    }
}
