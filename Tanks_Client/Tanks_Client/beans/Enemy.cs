using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks_Client.beans
{
    
    class Enemy
    {
        private int currentPositionX = -1;
        private int currentPositionY = -1;
        private int id;
        private int tankOrientation = 0;

        public int TankOrientation
        {
            get { return tankOrientation; }
            set { tankOrientation = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int CurrentPositionX
        {
            get { return currentPositionX; }
            set { currentPositionX = value; }
        }
        

        public int CurrentPositionY
        {
            get { return currentPositionY; }
            set { currentPositionY = value; }
        }

        
    }
}
