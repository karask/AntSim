using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntSim
{
    public class Cell
    {
        public Location Location { get; private set; }
        public int AvailableFood { get; set; }
        public int FoodPheremone { get; set; }
        public int HomePheremone { get; set; }
        public bool IsHome { get; set; }
        public Ant Ant { get; set; }

        public Cell(Location location, int availableFood, int foodPheremone, int homePheremone)
        {
            Location = location;
            AvailableFood = availableFood;
            FoodPheremone = foodPheremone;
            HomePheremone = homePheremone;
            IsHome = false;
            Ant = null;
        }

        public bool hasFood()
        {
            return AvailableFood > 0 ? true : false;
        }
    }

    public class Location
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        //public static Location Empty()
        //{
        //    return new Location(0, 0);
        //}

        public static Location operator+ (Location loc1, Location loc2)
        {
            return new Location(loc1.X + loc2.X, loc1.Y + loc2.Y);
        }

        public override string ToString()
        {
            return "X=" + X + ", Y=" + Y;
        }
    }
}
