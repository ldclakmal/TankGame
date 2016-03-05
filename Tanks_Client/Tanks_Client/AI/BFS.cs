using System.Collections;
using ClientApplication;

namespace Tanks_Client.AI
{
    class BFS
    {
        Cell healthPack = null;
        ArrayList path = new ArrayList();
        public ArrayList BFSsearch(Cell[][] Grid, Cell start)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Cell cell = Grid[i][j];
                    cell.Distance = 100000;
                    cell.Parent = null;
                }
            }
            Queue q = new Queue();
            start.Distance = 0;
            q.Enqueue(start);
            bool found = false;
            while (q.Count != 0)
            {
                Cell current = (Cell)q.Dequeue();
                ArrayList adjCells = new ArrayList();
                if (current.Col - 1 != -1)
                {
                    adjCells.Add(Grid[current.Row][current.Col - 1]);
                }
                if (current.Col + 1 != 10)
                {
                    adjCells.Add(Grid[current.Row][current.Col + 1]);
                }
                if (current.Row + 1 != 10)
                {
                    adjCells.Add(Grid[current.Row + 1][current.Col]);
                }
                if (current.Row - 1 != -1)
                {
                    adjCells.Add(Grid[current.Row - 1][current.Col]);
                }
                for (int j = 0; j < adjCells.Count; j++)
                {
                    Cell adj = (Cell)adjCells[j];
                    if (adj.Distance == 100000 && adj.Value.Equals("Movable"))
                    {
                        adj.Distance = current.Distance + 1;
                        adj.Parent = current;
                        q.Enqueue(adj);
                    }
                    if (adj.IsHealth)
                    {
                        healthPack = (Cell)adjCells[j];
                        found = true;
                    }
                    if (found)
                    {
                        break;
                    }
                }
                if (found)
                {
                    break;
                }
            }
            Cell destination = healthPack;

            while (destination != start)
            {
                path.Add(destination);
                destination = destination.Parent;
            }

            return path;

        }
    }
}
