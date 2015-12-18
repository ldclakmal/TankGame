using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks_Client.AI
{
    class Controller
    {
        private int WHITE = 0;
        private int GRAY = 1;
        private int BLACK = 2;

        private int GRIDE_SIZE = DecodeOperations.GRID_SIZE;
        private String whatToFind = null;
        private MapItem source = null;
        private MapItem destination = null;
        private List<MapItem> preList = null;
        private static Controller con = new Controller();

        private String[] blankList = new String[] { MapItem.BLANK, MapItem.COIN, MapItem.LIFEPACK };

        private Controller()
        { }

        public static Controller GetInstance()
        {
            return con;
        }
        /*
        * RELAX(u, v, w)
           1. if v.d> u.d+ w(u, v)
           2. v.d= u.d+ w(u, v)
           3. v.= u
            
        */

        private void relax(MapItem u, MapItem v)
        {
            if (v.Dis > u.Dis + 1)
            {
                v.Dis = u.Dis + 1;
                v.Pre = u;
            }
        }

        /* 
          DIJKSTRA( G, w, s)
            1. INITIALIZE-SINGLE-SOURCE(G, s)
            2. S = 
            3. Q = G.V
            4. while Q 
            5. u = EXTRACT-MIN(Q)
            6.S = S {u}
            7.for each v G.Adj[u]
            8.RELAX (u, v, w)
         
         */

        private void dijkstra(MapItem[,] map, MapItem s, MapItem d)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[j, i].Pre = null;
                    map[j, i].Dis = Int32.MaxValue - 1;
                }
            }
            s.Dis = 0;

            List<MapItem> S = new List<MapItem>();
            List<MapItem> Q = new List<MapItem>();

            for (int k = 0; k < map.GetLength(0); k++)
                for (int l = 0; l < map.GetLength(1); l++)
                    Q.Add(map[k, l]);

            while (Q.Count > 0)
            {
                MapItem u = Q.Min();
                Q.Remove(u);
                S.Add(u);
                int x = Int32.Parse(u.Name[0].ToString());
                int y = Int32.Parse(u.Name[2].ToString());

                try
                {
                    //if (map[y, x - 1].Contain.Equals(MapItem.BLANK))
                    if (blankList.Contains(map[y, x - 1].Contain, StringComparer.Ordinal))
                    {
                        relax(u, map[y, x - 1]);
                        if (map[y, x - 1].Name.Equals(d.Name))
                            return;
                    }
                }
                catch (Exception e)
                {
                }
                try
                {
                    //if (map[y - 1, x].Contain.Equals(MapItem.BLANK))
                    if (blankList.Contains(map[y - 1, x].Contain, StringComparer.Ordinal))
                    {
                        relax(u, map[y - 1, x]);
                        if (map[y - 1, x].Name.Equals(d.Name))
                            return;
                    }
                }
                catch (Exception e)
                {
                }
                try
                {
                    //if (map[y, x + 1].Contain.Equals(MapItem.BLANK))
                    if (blankList.Contains(map[y, x + 1].Contain, StringComparer.Ordinal))
                    {
                        relax(u, map[y, x + 1]);
                        if (map[y, x + 1].Name.Equals(d.Name))
                            return;

                    }
                }
                catch (Exception e)
                {
                }
                try
                {
                    //if (map[y + 1, x].Contain.Equals(MapItem.BLANK))
                    if (blankList.Contains(map[y + 1, x].Contain, StringComparer.Ordinal))
                    {
                        relax(u, map[y + 1, x]);
                        if (map[y + 1, x].Name.Equals(d.Name))
                            return;
                    }
                }
                catch (Exception e)
                {
                }
            }

        }

        /*
         * BFS(G,s)
            1   foreach vertex u in V[G] –{s}
            2       do color[u] <- white
            3           d[u] <- inf
            4           pre[u] <- null
            5   color[s] <- gray
            6   d[s] <- 0
            7   pre[s] <- null
            8   Q <- {}
            9   enqueue(Q,s)
            10  while Q!= null
            11      do u <- dequeue(Q)
            12      foreach v in Adj[u]
            13          do if color[v] = white
            14              thencolor[v] <- gray
            15                  d[v] <- d[u] + 1
            16                  pre[v] <- u
            17                  enqueue(Q,v)
            18      color[u] <- black
         */
        private MapItem bfs(MapItem[,] map, MapItem s, String whatToFind)
        {
            Console.WriteLine("WEWEWE: " + whatToFind);
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[j, i].Colour = this.WHITE;
                    map[j, i].Pre = null;
                    map[j, i].Dis = Int32.MaxValue - 1;
                }
            }
            s.Colour = this.GRAY;
            s.Dis = 0;


            Queue<MapItem> Q = new Queue<MapItem>();
            Q.Enqueue(s);
            while (Q.Count > 0)
            {
                MapItem u = Q.Dequeue();
                int x = Int32.Parse(u.Name[0].ToString());
                int y = Int32.Parse(u.Name[2].ToString());

                try
                {
                    //if (map[y, x - 1].Contain.Equals(MapItem.BLANK))
                    if (blankList.Contains(map[y, x - 1].Contain, StringComparer.Ordinal))
                    {
                        if (map[y, x - 1].Colour.Equals(this.WHITE))
                        {
                            map[y, x - 1].Colour = this.GRAY;
                            map[y, x - 1].Dis = u.Dis + 1;
                            map[y, x - 1].Pre = u;
                            Q.Enqueue(map[y, x - 1]);
                        }

                    }
                    if (map[y, x - 1].Contain.Equals(whatToFind))
                    {
                        map[y, x - 1].Pre = u;
                        return map[y, x - 1];
                    }
                }
                catch (Exception e)
                {
                }
                try
                {
                    //if (map[y - 1, x].Contain.Equals(MapItem.BLANK))
                    if (blankList.Contains(map[y - 1, x].Contain, StringComparer.Ordinal))
                    {
                        if (map[y - 1, x].Colour.Equals(this.WHITE))
                        {
                            map[y - 1, x].Colour = this.GRAY;
                            map[y - 1, x].Dis = u.Dis + 1;
                            map[y - 1, x].Pre = u;
                            Q.Enqueue(map[y - 1, x]);
                        }

                    }
                    if (map[y - 1, x].Contain.Equals(whatToFind))
                    {
                        map[y - 1, x].Pre = u;
                        return map[y - 1, x];
                    }

                }
                catch (Exception e)
                {
                }
                try
                {
                    //if (map[y, x + 1].Contain.Equals(MapItem.BLANK))
                    if (blankList.Contains(map[y, x + 1].Contain, StringComparer.Ordinal))
                    {
                        if (map[y, x + 1].Colour.Equals(this.WHITE))
                        {
                            map[y, x + 1].Colour = this.GRAY;
                            map[y, x + 1].Dis = u.Dis + 1;
                            map[y, x + 1].Pre = u;
                            Q.Enqueue(map[y, x + 1]);
                        }

                    }
                    if (map[y, x + 1].Contain.Equals(whatToFind))
                    {
                        map[y, x + 1].Pre = u;
                        return map[y, x + 1];
                    }

                }
                catch (Exception e)
                {
                }
                try
                {
                    //if (map[y + 1, x].Contain.Equals(MapItem.BLANK))
                    if (blankList.Contains(map[y + 1, x].Contain, StringComparer.Ordinal))
                    {
                        if (map[y + 1, x].Colour.Equals(this.WHITE))
                        {
                            map[y + 1, x].Colour = this.GRAY;
                            map[y + 1, x].Dis = u.Dis + 1;
                            map[y + 1, x].Pre = u;
                            Q.Enqueue(map[y + 1, x]);
                        }

                    }
                    if (map[y + 1, x].Contain.Equals(whatToFind))
                    {
                        map[y + 1, x].Pre = u;
                        return map[y + 1, x];
                    }

                }
                catch (Exception e)
                {
                }
                u.Colour = this.BLACK;
            }
            return null;
        }

        private MapItem bfsColl(MapItem[,] map, MapItem s, String[] whatToFind)
        {

            int playerX = Int32.Parse(s.Name[0].ToString());
            int playerY = Int32.Parse(s.Name[2].ToString());
            //Console.WriteLine("WEWEWE: " + whatToFind);
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[j, i].Colour = this.WHITE;
                    map[j, i].Pre = null;
                    map[j, i].Dis = Int32.MaxValue - 1;
                }
            }
            s.Colour = this.GRAY;
            s.Dis = 0;


            Queue<MapItem> Q = new Queue<MapItem>();
            Q.Enqueue(s);
            while (Q.Count > 0)
            {
                MapItem u = Q.Dequeue();
                int x = Int32.Parse(u.Name[0].ToString());
                int y = Int32.Parse(u.Name[2].ToString());

                try
                {
                    //if (map[y, x - 1].Contain.Equals(MapItem.BLANK))
                    if (blankList.Contains(map[y, x - 1].Contain, StringComparer.Ordinal))
                    {
                        if (map[y, x - 1].Colour.Equals(this.WHITE))
                        {
                            map[y, x - 1].Colour = this.GRAY;
                            map[y, x - 1].Dis = u.Dis + 1;
                            map[y, x - 1].Pre = u;
                            Q.Enqueue(map[y, x - 1]);
                        }

                    }
                    if (playerX != x - 1 && playerY != y && whatToFind.Contains(map[y, x - 1].Contain, StringComparer.Ordinal))
                    {
                        //Console.WriteLine(y+" " +(x-1)+" "+ map[y, x - 1].Contain);
                        map[y, x - 1].Pre = u;
                        return map[y, x - 1];
                    }
                }
                catch (Exception e)
                {
                }
                try
                {
                    //if (map[y - 1, x].Contain.Equals(MapItem.BLANK))
                    if (blankList.Contains(map[y - 1, x].Contain, StringComparer.Ordinal))
                    {
                        if (map[y - 1, x].Colour.Equals(this.WHITE))
                        {
                            map[y - 1, x].Colour = this.GRAY;
                            map[y - 1, x].Dis = u.Dis + 1;
                            map[y - 1, x].Pre = u;
                            Q.Enqueue(map[y - 1, x]);
                        }

                    }

                    if (playerX != x && playerY != y - 1 && whatToFind.Contains(map[y - 1, x].Contain, StringComparer.Ordinal))
                    {
                        //Console.WriteLine((y-1) + " " + x + " " + map[y, x - 1].Contain);
                        map[y - 1, x].Pre = u;
                        return map[y - 1, x];
                    }

                }
                catch (Exception e)
                {
                }
                try
                {
                    //if (map[y, x + 1].Contain.Equals(MapItem.BLANK))
                    if (blankList.Contains(map[y, x + 1].Contain, StringComparer.Ordinal))
                    {
                        if (map[y, x + 1].Colour.Equals(this.WHITE))
                        {
                            map[y, x + 1].Colour = this.GRAY;
                            map[y, x + 1].Dis = u.Dis + 1;
                            map[y, x + 1].Pre = u;
                            Q.Enqueue(map[y, x + 1]);
                        }

                    }
                    if (playerX != x + 1 && playerY != y && whatToFind.Contains(map[y, x + 1].Contain, StringComparer.Ordinal))
                    {
                        //Console.WriteLine(y + " " + (x+1) + " " + map[y, x - 1].Contain);
                        map[y, x + 1].Pre = u;
                        return map[y, x + 1];
                    }

                }
                catch (Exception e)
                {
                }
                try
                {
                    //if (map[y + 1, x].Contain.Equals(MapItem.BLANK))
                    if (blankList.Contains(map[y + 1, x].Contain, StringComparer.Ordinal))
                    {
                        if (map[y + 1, x].Colour.Equals(this.WHITE))
                        {
                            map[y + 1, x].Colour = this.GRAY;
                            map[y + 1, x].Dis = u.Dis + 1;
                            map[y + 1, x].Pre = u;
                            Q.Enqueue(map[y + 1, x]);
                        }

                    }
                    if (playerX != x && playerY != y + 1 && whatToFind.Contains(map[y + 1, x].Contain, StringComparer.Ordinal))
                    {
                        //Console.WriteLine((y+1) + " " + x + " " + map[y, x - 1].Contain);
                        map[y + 1, x].Pre = u;
                        return map[y + 1, x];
                    }

                }
                catch (Exception e)
                {
                }
                u.Colour = this.BLACK;
            }
            return null;
        }

        public List<MapItem> getPath(MapItem[,] map, MapItem s, MapItem d)
        {
            if (s.Equals(this.source) && d.Equals(this.destination) && this.preList != null)
                return preList;

            this.source = s;
            this.destination = d;
            preList = new List<MapItem>();

            if (s.Name.Equals(d.Name))
                return preList;

            this.dijkstra(map, s, d);

            preList.Add(d);
            MapItem pre = d.Pre;

            while (true)
            {
                preList.Add(pre);
                if (pre.Name.Equals(s.Name))
                    break;
                else
                    pre = pre.Pre;
            }

            preList.Reverse();
            return preList;
        }

        public List<MapItem> getPathTo(MapItem[,] map, MapItem s, String whatToFind)
        {
            if (s.Equals(this.source) && this.whatToFind == whatToFind && this.preList != null)
                return preList;

            this.source = s;
            this.whatToFind = whatToFind;
            preList = new List<MapItem>();

            if (s.Contain == whatToFind)
                return preList;

            MapItem d = this.bfs(map, s, whatToFind);

            if (d == null)
                return null;
            Console.WriteLine("SSSSSSS");
            preList.Add(d);
            MapItem pre = d.Pre;

            while (true)
            {
                preList.Add(pre);
                if (pre.Name.Equals(s.Name))
                    break;
                else
                    pre = pre.Pre;
            }

            preList.Reverse();
            return preList;
        }
        public List<MapItem> getPathToCol(MapItem[,] map, MapItem s, String[] whatToFind)
        {
            this.source = s;
            preList = new List<MapItem>();
            MapItem d = this.bfsColl(map, s, whatToFind);

            if (d == null)
                return null;
            Console.WriteLine("SSSSSSS");
            preList.Add(d);
            MapItem pre = d.Pre;

            while (true)
            {
                preList.Add(pre);
                if (pre.Name.Equals(s.Name))
                    break;
                else
                    pre = pre.Pre;
            }

            preList.Reverse();
            return preList;
        }

        public String next(List<MapItem> path)
        {
            if (path == null || path.Count() == 0)
                return "NOPATH";
            MapItem s = path[0];
            MapItem d = path[1];
            int x1 = Int32.Parse(s.Name[0].ToString());
            int y1 = Int32.Parse(s.Name[2].ToString());
            int x2 = Int32.Parse(d.Name[0].ToString());
            int y2 = Int32.Parse(d.Name[2].ToString());
            if (x1 < x2)
                return "RIGHT#";
            if (x1 > x2)
                return "LEFT#";
            if (y1 < y2)
                return "DOWN#";
            if (y1 > y2)
                return "UP#";
            return "DD";
        }

        public MapItem[,] createMapItemList(String[,] map)
        {
            int size = map.GetLength(0);
            MapItem[,] itList = new MapItem[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    itList[j, i] = new MapItem();
                    /*if (new String[] { MapItem.BLANK, MapItem.COIN, MapItem.LIFEPACK }.Contains(map[j, i], StringComparer.Ordinal))                    
                        itList[j, i].Contain = MapItem.BLANK;
                    else
                        itList[j, i].Contain = MapItem.STONE;*/
                    itList[j, i].Contain = map[j, i];
                    if (DecodeOperations.playerDir.Contains(map[j, i], StringComparer.Ordinal))
                    {
                        itList[j, i].Dir = Array.FindIndex(DecodeOperations.playerDir, pl => pl.Contains(map[j, i]));
                    }

                    itList[j, i].Name = i.ToString() + "," + j.ToString();
                    itList[j, i].Pre = null;
                    itList[j, i].Dis = Int32.MaxValue - 1;
                }
            }
            return itList;
        }

        public bool shouldShoot(MapItem[,] map, MapItem player)
        {
            int x = Int32.Parse(player.Name[0].ToString());
            int y = Int32.Parse(player.Name[2].ToString());
            String[] notShoot = { DecodeOperations.stoneSimbol };
            int dir = player.Dir;

            //Console.WriteLine(x+" :::: "+y);
            switch (dir)
            {
                case 0:
                    while (y > 0)
                    {
                        y--;
                        if (notShoot.Contains(map[y, x].Contain, StringComparer.Ordinal))
                            return false;
                        if (DecodeOperations.playerDir.Contains(map[y, x].Contain, StringComparer.Ordinal))
                            return true;
                    }
                    break;
                case 1:
                    while (x < map.GetLength(0) - 1)
                    {
                        x++;
                        if (notShoot.Contains(map[y, x].Contain, StringComparer.Ordinal))
                            return false;
                        if (DecodeOperations.playerDir.Contains(map[y, x].Contain, StringComparer.Ordinal))
                            return true;
                    }
                    break;
                case 2:
                    while (y < map.GetLength(0) - 1)
                    {
                        y++;
                        if (notShoot.Contains(map[y, x].Contain, StringComparer.Ordinal))
                            return false;
                        if (DecodeOperations.playerDir.Contains(map[y, x].Contain, StringComparer.Ordinal))
                            return true;
                    }
                    break;
                case 3:
                    while (x > 0)
                    {
                        x--;
                        if (notShoot.Contains(map[y, x].Contain, StringComparer.Ordinal))
                            return false;
                        if (DecodeOperations.playerDir.Contains(map[y, x].Contain, StringComparer.Ordinal))
                            return true;
                    }
                    break;
            }
            return false;
        }

        public bool removeFromDanger(MapItem[,] map, MapItem player)
        {


            int x = Int32.Parse(player.Name[0].ToString());
            int y = Int32.Parse(player.Name[2].ToString());
            String[] notShoot = { DecodeOperations.stoneSimbol, DecodeOperations.waterSimbol };
            int dir = player.Dir;
            //Console.WriteLine(x + " :::: " + y);
            switch (dir)
            {
                case 0:
                    while (x > 0)
                    {
                        x--;
                        if (notShoot.Contains(map[x, y].Contain, StringComparer.Ordinal))
                            return false;
                        if (DecodeOperations.playerDir.Contains(map[x, y].Contain, StringComparer.Ordinal))
                            return true;
                    }
                    break;
                case 1:
                    while (y < map.GetLength(0) - 1)
                    {
                        y++;
                        if (notShoot.Contains(map[x, y].Contain, StringComparer.Ordinal))
                            return false;
                        if (DecodeOperations.playerDir.Contains(map[x, y].Contain, StringComparer.Ordinal))
                            return true;
                    }
                    break;
                case 2:
                    while (x < map.GetLength(0) - 1)
                    {
                        x++;
                        if (notShoot.Contains(map[x, y].Contain, StringComparer.Ordinal))
                            return false;
                        if (DecodeOperations.playerDir.Contains(map[x, y].Contain, StringComparer.Ordinal))
                            return true;
                    }
                    break;
                case 3:
                    while (y > 0)
                    {
                        y--;
                        if (notShoot.Contains(map[x, y].Contain, StringComparer.Ordinal))
                            return false;
                        if (DecodeOperations.playerDir.Contains(map[x, y].Contain, StringComparer.Ordinal))
                            return true;
                    }
                    break;
            }
            return false;
        }
    }
}
