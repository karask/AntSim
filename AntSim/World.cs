using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntSim
{
    public class World
    {
        private Cell[,] data;
        public int WorldDimensions { get; private set; }
        public int HomeDimensions { get; private set; }
        public Location HomeStart { get; private set; }


        public World(int worldDimensions, int homeDimensions, Location homeStart)
        {
            WorldDimensions = worldDimensions;
            HomeDimensions = homeDimensions;
            HomeStart = homeStart;
            this.data = new Cell[WorldDimensions, WorldDimensions];
            for (int x = 0; x < WorldDimensions; x++)
                for (int y = 0; y < WorldDimensions; y++)
                {
                    // initialize all
                    data[x, y] = new Cell(new Location(x, y), 0, 0, 0);
                    // set home cells
                    if (x >= HomeStart.X && x < HomeStart.X + HomeDimensions &&
                       y >= HomeStart.Y && y < HomeStart.Y + HomeDimensions)
                        data[x, y].IsHome = true;
                }
        }

        public Cell RandomCell(Random rnd)
        {
            return data[rnd.Next(0, WorldDimensions), rnd.Next(0, WorldDimensions)];
        }

        public Cell RandomHomeCell(Random rnd)
        {
            return data[rnd.Next(HomeStart.X, HomeStart.X + HomeDimensions), rnd.Next(HomeStart.Y, HomeStart.Y + HomeDimensions)];
        }


        public IEnumerable<Cell> AllCells
        {
            get
            {
                for (int x = 0; x < WorldDimensions; x++)
                    for (int y = 0; y < WorldDimensions; y++)
                        yield return data[x, y];
            }
        }

        public Cell GetCell(Location location)
        {
            return data[location.X, location.Y];
        }

    }
}
